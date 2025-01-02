using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NEXUS_API.Data;
using NEXUS_API.DTOs;
using NEXUS_API.Helpers;
using NEXUS_API.Models;
using NEXUS_API.Repository;
using NEXUS_API.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NEXUS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        public AuthController(DatabaseContext databaseContext, IConfiguration configuration, EmailService emailService)
        {
            _dbContext = databaseContext;
            _configuration = configuration;
            _emailService = emailService;
        }
        private string GenerateToken(Employee employee)
        {
            var jwtSetting = _configuration.GetSection("JWT");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSetting["Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, employee.Email),
                    new Claim(ClaimTypes.Name, employee.FullName),
                    new Claim(ClaimTypes.Role, employee.Role),
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthLogin authLogin)
        {
            object response = null;
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(u => u.Email == authLogin.Email);
            if (employee == null)
            {
                response = new ApiResponse(StatusCodes.Status401Unauthorized, "Email does not exist", null);
                return Unauthorized(response);
            }
            if (authLogin.Password != employee.Password)
            {
                employee.FailedLoginAttempts++;
                if (employee.FailedLoginAttempts >= 3)
                {
                    employee.ExpiredBan = DateTime.UtcNow.AddMinutes(5);
                }
                await _dbContext.SaveChangesAsync();
                response = new ApiResponse(StatusCodes.Status401Unauthorized, "Incorrect email or password", null);
                return Unauthorized(response);
            }
            if (employee.ExpiredBan.HasValue && employee.ExpiredBan > DateTime.UtcNow)
            {
                response = new ApiResponse(StatusCodes.Status400BadRequest, $"Account locked until {employee.ExpiredBan.Value}", null);
                return BadRequest(response);
            }
            employee.FailedLoginAttempts = 0;
            var tokenString = GenerateToken(employee);
            employee.RefreshToken = Guid.NewGuid().ToString();
            employee.RefreshTokenExpried = DateTime.UtcNow.AddDays(7);
            await _dbContext.SaveChangesAsync();
            var data = new
            {
                token = tokenString,
                refreshToken = employee.RefreshToken
            };
            response = new ApiResponse(StatusCodes.Status200OK, "Login successfully", data);
            return Ok(response);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenRequest tokenRequest)
        {
            object response = null;
            var employee = _dbContext.Employees.FirstOrDefault(u => u.RefreshToken == tokenRequest.RefreshToken)!;
            if (employee == null || employee.RefreshTokenExpried < DateTime.UtcNow)
            {
                response = new ApiResponse(StatusCodes.Status401Unauthorized, "Error", null);
                return Unauthorized(response);
            }
            var tokenString = GenerateToken(employee);
            employee.RefreshToken = Guid.NewGuid().ToString();
            employee.RefreshTokenExpried = DateTime.UtcNow.AddDays(7);
            await _dbContext.SaveChangesAsync();
            var data = new
            {
                token = tokenString,
                refreshToken = employee.RefreshToken
            };
            response = new ApiResponse(StatusCodes.Status200OK, "Refresh successfully", data);
            return Ok(response);
        }
        [HttpPost("send-code")]
        public async Task<IActionResult> SendCode([FromBody] AuthSendCode authSendCode, bool isResend = false)
        {
            object response = null;
            int expiryCodeTime = 2;
            int maxAttemptsPerDay = 3;

            var employee = await _dbContext.Employees.FirstOrDefaultAsync(u => u.Email == authSendCode.Email);
            if (employee == null)
            {
                response = new ApiResponse(StatusCodes.Status400BadRequest, "Email does not exist", null);
                return BadRequest(response);
            }

            if (employee.LastSendCodeDate.HasValue && employee.LastSendCodeDate.Value.Date < DateTime.UtcNow.Date)
            {
                employee.SendCodeAttempts = 0;
                employee.LastSendCodeDate = DateTime.UtcNow;
            }

            if (employee.SendCodeAttempts >= maxAttemptsPerDay)
            {
                response = new ApiResponse(StatusCodes.Status429TooManyRequests, "You have reached the maximum number of requests for today. Please try again tomorrow.", null);
                return StatusCode(StatusCodes.Status429TooManyRequests, response);
            }

            if (employee.LastSendCodeDate.HasValue && employee.LastSendCodeDate.Value.Date < DateTime.UtcNow.Date)
            {
                employee.SendCodeAttempts = 0;
                employee.LastSendCodeDate = DateTime.UtcNow;
            }

            if (employee.Code != null && employee.ExpiredCode.HasValue && employee.ExpiredCode > DateTime.UtcNow)
            {
                employee.Code = null;
                employee.ExpiredCode = null;
            }

            Random random = new Random();
            employee.Code = random.Next(100000, 999999).ToString();
            employee.ExpiredCode = DateTime.UtcNow.AddMinutes(expiryCodeTime);

            employee.SendCodeAttempts++;
            employee.LastSendCodeDate = DateTime.UtcNow;

            var emailRequest = new EmailRequest
            {
                ToMail = authSendCode.Email,
                Subject = isResend ? "Reset Password - Resend Code" : "Reset Password",
                HtmlContent = $"Your verification code is {employee.Code}. It will expire in {expiryCodeTime} minutes."
            };
            try
            {
                await _emailService.SendMailAsync(emailRequest);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "Error sending email", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            await _dbContext.SaveChangesAsync();
            response = new ApiResponse(StatusCodes.Status200OK, isResend
                ? "A new verification code has been sent to your email"
                : "A verification code has been sent to your email",
                expiryCodeTime);
            return Ok(response);
        }

        [HttpPost("check-code")]
        public async Task<IActionResult> CheckCode([FromBody] AuthCheckCode authCheckCode)
        {
            object response = null;
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(u => u.Code == authCheckCode.Code && u.ExpiredCode > DateTime.UtcNow);
            if (employee == null)
            {
                response = new ApiResponse(StatusCodes.Status400BadRequest, "VerifyCode is invalid", null);
                return BadRequest(response);
            }
            if (employee.ExpiredCode < DateTime.UtcNow)
            {
                response = new ApiResponse(StatusCodes.Status400BadRequest, "VerifyCode is expried", null);
                return BadRequest(response);
            }
            response = new ApiResponse(StatusCodes.Status200OK, "Authentication successful redirect to next page", employee.EmployeeId);
            return Ok(response);
        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] AuthUpdatePassword authUpdatePassword)
        {
            object response = null;
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(u => u.EmployeeId == authUpdatePassword.UserId);
            if (employee == null)
            {
                response = new ApiResponse(StatusCodes.Status400BadRequest, "Employee not found", null);
                return NotFound(response);
            }
            employee.Password = authUpdatePassword.Password;
            await _dbContext.SaveChangesAsync();
            response = new ApiResponse(StatusCodes.Status200OK, "Password updated successfully", null);
            return Ok(response);
        }
    }
}
