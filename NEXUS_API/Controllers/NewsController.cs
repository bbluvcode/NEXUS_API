using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXUS_API.Data;
using NEXUS_API.Models;
using System.Net;

[Route("api/news")]
[ApiController]
public class NewsController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<NewsController> _logger;

    public NewsController(DatabaseContext context, IWebHostEnvironment env, ILogger<NewsController> logger)
    {
        _context = context;
        _env = env;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> AddNews([FromForm] News newsDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid model state", status = HttpStatusCode.BadRequest });

            var news = new News
            {
                Title = newsDto.Title,
                EmployeeId = newsDto.EmployeeId,
                Content = newsDto.Content,
                CreateDate = DateTime.UtcNow,
                Status = newsDto.Status
            };

            // Lưu bài viết vào cơ sở dữ liệu
            _context.NewsTB.Add(news);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNewsById), new { id = news.NewsId }, news);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating news");
            return StatusCode(500, new { message = ex.Message, status = HttpStatusCode.InternalServerError });
        }
    }

    // 📌 API: Lấy danh sách bài viết
    [HttpGet]
    public async Task<IActionResult> GetNews()
    {
        var newsList = await _context.NewsTB.ToListAsync();
        return Ok(newsList);
    }

    // 📌 API: Lấy chi tiết bài viết
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNewsById(int id)
    {
        var news = await _context.NewsTB.FindAsync(id);
        if (news == null)
            return NotFound(new { message = "News not found", status = HttpStatusCode.NotFound });

        return Ok(news);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNews(int id, [FromForm] News newsDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid model state", status = HttpStatusCode.BadRequest });

            // Tìm bài viết cần cập nhật
            var existingNews = await _context.NewsTB.FindAsync(id);
            if (existingNews == null)
                return NotFound(new { message = "News not found", status = HttpStatusCode.NotFound });

            // Cập nhật các trường trong bài viết
            existingNews.Title = newsDto.Title;
            existingNews.EmployeeId = newsDto.EmployeeId;
            existingNews.Content = newsDto.Content;
            existingNews.Status = newsDto.Status;
            existingNews.CreateDate = newsDto.CreateDate; // nếu bạn cần cập nhật ngày tạo, bạn có thể thay đổi điều này theo yêu cầu

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.NewsTB.Update(existingNews);
            await _context.SaveChangesAsync();

            return Ok(existingNews);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating news");
            return StatusCode(500, new { message = ex.Message, status = HttpStatusCode.InternalServerError });
        }
    }

}
