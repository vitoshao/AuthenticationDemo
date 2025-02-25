using System.Diagnostics;
using Demo.DAL;
using Demo2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly TestDbContext _dbContext;

    public HomeController(ILogger<HomeController> logger, TestDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
