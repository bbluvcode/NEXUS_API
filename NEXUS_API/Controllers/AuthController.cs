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
using System.Collections.Generic;
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
        private string GenerateTokenEmployee(Employee employee)
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
                    new Claim(ClaimTypes.Role, employee.EmployeeRole.RoleName),
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        [HttpPost("employee/login")]
        public async Task<IActionResult> EmployeeLogin([FromBody] AuthLogin authLogin)
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
            var tokenString = GenerateTokenEmployee(employee);
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
        private string GenerateTokenCustomer(Customer customer)
        {
            var jwtSetting = _configuration.GetSection("JWT");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSetting["Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, customer.Email),
                    new Claim(ClaimTypes.Name, customer.FullName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        [HttpPost("customer/login")]
        public async Task<IActionResult> CustomerLogin([FromBody] AuthLogin authLogin)
        {
            object response = null;
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(u => u.Email == authLogin.Email);
            if (customer == null)
            {
                response = new ApiResponse(StatusCodes.Status401Unauthorized, "Email does not exist", null);
                return Unauthorized(response);
            }
            if (authLogin.Password != customer.Password)
            {
                customer.FailedLoginAttempts++;
                if (customer.FailedLoginAttempts >= 3)
                {
                    customer.ExpiredBan = DateTime.UtcNow.AddMinutes(5);
                }
                await _dbContext.SaveChangesAsync();
                response = new ApiResponse(StatusCodes.Status401Unauthorized, "Incorrect email or password", null);
                return Unauthorized(response);
            }
            if (customer.ExpiredBan.HasValue && customer.ExpiredBan > DateTime.UtcNow)
            {
                response = new ApiResponse(StatusCodes.Status400BadRequest, $"Account locked until {customer.ExpiredBan.Value}", null);
                return BadRequest(response);
            }
            customer.FailedLoginAttempts = 0;
            var tokenString = GenerateTokenCustomer(customer);
            customer.RefreshToken = Guid.NewGuid().ToString();
            customer.RefreshTokenExpried = DateTime.UtcNow.AddDays(7);
            await _dbContext.SaveChangesAsync();
            var data = new
            {
                token = tokenString,
                refreshToken = customer.RefreshToken
            };
            response = new ApiResponse(StatusCodes.Status200OK, "Login successfully", data);
            return Ok(response);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenRequest tokenRequest)
        {
            object response = null;
            var employee = _dbContext.Employees.FirstOrDefault(u => u.RefreshToken == tokenRequest.RefreshToken);
            IUserAuth user = employee;
            if (user == null)
            {
                var customer = await _dbContext.Customers.FirstOrDefaultAsync(u => u.RefreshToken == tokenRequest.RefreshToken);
                user = customer;
            }
            if (user == null || user.RefreshTokenExpried < DateTime.UtcNow)
            {
                response = new ApiResponse(StatusCodes.Status401Unauthorized, "Error", null);
                return Unauthorized(response);
            }
            string tokenString = user is Employee ? GenerateTokenEmployee((Employee)user) : GenerateTokenCustomer((Customer)user);
            user.RefreshToken = Guid.NewGuid().ToString();
            user.RefreshTokenExpried = DateTime.UtcNow.AddDays(7);
            await _dbContext.SaveChangesAsync();
            var data = new
            {
                token = tokenString,
                refreshToken = user.RefreshToken
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
            IUserAuth user = employee;
            if (user == null)
            {
                var customer = await _dbContext.Customers.FirstOrDefaultAsync(u => u.Email == authSendCode.Email);
                user = customer;
            }
            if (user == null)
            {
                response = new ApiResponse(StatusCodes.Status400BadRequest, "Email does not exist", null);
                return BadRequest(response);
            }

            if (user.LastSendCodeDate.HasValue && user.LastSendCodeDate.Value.Date < DateTime.UtcNow.Date)
            {
                user.SendCodeAttempts = 0;
                user.LastSendCodeDate = DateTime.UtcNow;
            }

            if (user.SendCodeAttempts >= maxAttemptsPerDay)
            {
                response = new ApiResponse(StatusCodes.Status429TooManyRequests, "You have reached the maximum number of requests for today. Please try again tomorrow.", null);
                return StatusCode(StatusCodes.Status429TooManyRequests, response);
            }

            if (user.LastSendCodeDate.HasValue && user.LastSendCodeDate.Value.Date < DateTime.UtcNow.Date)
            {
                user.SendCodeAttempts = 0;
                user.LastSendCodeDate = DateTime.UtcNow;
            }

            if (user.Code != null && user.ExpiredCode.HasValue && user.ExpiredCode > DateTime.UtcNow)
            {
                user.Code = null;
                user.ExpiredCode = null;
            }

            Random random = new Random();
            user.Code = random.Next(100000, 999999).ToString();
            user.ExpiredCode = DateTime.UtcNow.AddMinutes(expiryCodeTime);

            user.SendCodeAttempts++;
            user.LastSendCodeDate = DateTime.UtcNow;

            var emailRequest = new EmailRequest
            {
                ToMail = authSendCode.Email,
                Subject = isResend ? "Reset Password - Resend Code" : "Reset Password",
                HtmlContent = $"Your verification code is {user.Code}. It will expire in {expiryCodeTime} minutes."
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
            var user = await _dbContext.Customers.OfType<IUserAuth>().FirstOrDefaultAsync(u => u.Code == authCheckCode.Code && u.ExpiredCode > DateTime.UtcNow)
                    ?? await _dbContext.Employees.OfType<IUserAuth>().FirstOrDefaultAsync(u => u.Code == authCheckCode.Code && u.ExpiredCode > DateTime.UtcNow);
 
            if (user == null)
            {
                response = new ApiResponse(StatusCodes.Status400BadRequest, "VerifyCode is invalid", null);
                return BadRequest(response);
            }
            if (user.ExpiredCode < DateTime.UtcNow)
            {
                response = new ApiResponse(StatusCodes.Status400BadRequest, "VerifyCode is expried", null);
                return BadRequest(response);
            }
            response = new ApiResponse(StatusCodes.Status200OK, "Authentication successful redirect to next page", user.Email);
            return Ok(response);
        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] AuthUpdatePassword authUpdatePassword)
        {
            object response = null;
            var user = await _dbContext.Customers.OfType<IUserAuth>().FirstOrDefaultAsync(u => u.Email == authUpdatePassword.Email)
                    ?? await _dbContext.Employees.OfType<IUserAuth>().FirstOrDefaultAsync(u => u.Email == authUpdatePassword.Email);
            if (user == null)
            {
                response = new ApiResponse(StatusCodes.Status400BadRequest, "Not found", null);
                return NotFound(response);
            }
            user.Password = authUpdatePassword.Password;
            await _dbContext.SaveChangesAsync();
            response = new ApiResponse(StatusCodes.Status200OK, "Password updated successfully", null);
            return Ok(response);
        }
    }
}
