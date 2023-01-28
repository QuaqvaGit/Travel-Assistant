using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TravelAssistant_.Models;

namespace TravelAssistant_.Controllers
{
    public class HomeController : Controller
    {
        TravelContext _travelContext;
        public HomeController(TravelContext travelContext)
        {
            _travelContext = travelContext;
        }

        public IActionResult Index()
        {
            _travelContext.AttachEntities("Cities", true);
            ViewBag.Options = _travelContext.Countries
                .Select(country => new Tuple<string, List<(string, List<string>)>>
                (country.Name, _travelContext.Regions
                .Where(region => region.Country == country)
                .Select(region => new Tuple<string,List<string>>(region.Name, _travelContext.Cities.Where(city => city.Region == region).Select(city => city.Name).ToList()).ToValueTuple())
                .ToList()).ToValueTuple()).ToList();
            return View();
        }

        public ActionResult Proccess(HomeViewModel homeInfo)
        {
            //RouteProcessor processor = new();
            //return View("Results",processor.BuildRoute());
            return View(homeInfo);

        }
    }
}