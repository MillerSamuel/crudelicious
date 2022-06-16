using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using crudelicious.Models;
namespace crudelicious.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private MyContext _context;

    public HomeController(ILogger<HomeController> logger,MyContext context)
    {
        _context=context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewBag.allDishes=_context.Dishes.ToList();
        return View();
    }

    [HttpGet("/new")]
    public IActionResult New()
    {
        return View();
    }

    [HttpPost("/add")]
    public IActionResult Add(Dish newDish)
    {
        if(ModelState.IsValid){
            _context.Add(newDish);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        else{
            return View("New");
        }
    }

    [HttpGet("{DishId}")]
    public IActionResult One(int DishId)
    {
        ViewBag.oneDish=_context.Dishes.FirstOrDefault(d=>d.DishId==DishId);
        return View("One");
    }

    [HttpGet("delete/{DishId}")]
    public IActionResult Delete(int DishId)
    {
        Dish dishDelete=_context.Dishes.SingleOrDefault(d=>d.DishId==DishId);
        _context.Dishes.Remove(dishDelete);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpGet("edit/{DishId}")]
    public IActionResult Edit(int DishId)
    {
        Dish oneDish=_context.Dishes.FirstOrDefault(d=>d.DishId==DishId);
        return View(oneDish);
    }

    [HttpPost("update/{DishId}")]
    public IActionResult Update(int DishId,Dish newDish)
    {
        Dish updateDish=_context.Dishes.FirstOrDefault(d=>d.DishId==DishId);
        updateDish.Name=newDish.Name;
        updateDish.Chef=newDish.Chef;
        updateDish.Calories=newDish.Calories;
        updateDish.Tastiness=newDish.Tastiness;
        updateDish.Description=newDish.Description;
        updateDish.UpdatedAt=DateTime.Now;
        _context.SaveChanges();
        return RedirectToAction("Index");
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
