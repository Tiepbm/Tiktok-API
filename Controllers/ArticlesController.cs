using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TikTokAPI.Database;
using TikTokAPI.Models;
using TikTokAPI.Utils;

namespace TikTokAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ArticlesController:ControllerBase
{
    private readonly AppDbContext _db;

    public ArticlesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _db.Articles.OrderByDescending(x => x.Id).ToListAsync());
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var item = await _db.Articles.FirstOrDefaultAsync(x => x.Slug == slug);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Article article)
    {
        if (string.IsNullOrWhiteSpace(article.Slug))
            article.Slug = SlugHelper.ToSlug(article.Title);
        else
            article.Slug = SlugHelper.ToSlug(article.Slug);

        _db.Articles.Add(article);
        await _db.SaveChangesAsync();
        return Ok(article);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Article input)
    {
        var article = await _db.Articles.FindAsync(id);
        if (article == null)
            return NotFound("Article not found");

        // Update fields
        article.Title = input.Title;
        article.Html = input.Html;

        // If slug not provided → regenerate from title
        if (string.IsNullOrWhiteSpace(input.Slug))
            article.Slug = SlugHelper.ToSlug(input.Title);
        else
            article.Slug = SlugHelper.ToSlug(input.Slug);

        await _db.SaveChangesAsync();

        return Ok(article);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var article = await _db.Articles.FindAsync(id);
        if (article == null)
            return NotFound("Article not found");

        _db.Articles.Remove(article);
        await _db.SaveChangesAsync();

        return Ok(new { success = true, message = "Deleted" });
    }
}