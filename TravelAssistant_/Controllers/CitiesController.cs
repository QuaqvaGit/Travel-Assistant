using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAssistant_.Models;

namespace TravelAssistant_.Controllers
{
    public class CitiesController : Controller
    {
        private TravelContext _context;
        public CitiesController(TravelContext context) => _context = context;

        public ActionResult Index()
        {
            ViewBag.PageName = "Список городов";

            foreach (City city in _context.Cities)
            {
                city.Region = _context.Regions.Find(city.RegionId);
                city.Region.Country = _context.Countries.Find(city.Region.CountryId);
            }
            
                
            var viewModels = _context.Cities.Select(city => new CityViewModel(city));

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")//isAjax
                return PartialView("TablePartial", viewModels);
            return View(viewModels);
        }

        public ActionResult Details(int id)
        {
            try
            {
                ViewBag.PageName = "Подробности о городе";
                City city= _context.Cities.Find(id);
                city.Region = _context.Regions.Find(city.RegionId);
                city.Region.Country = _context.Countries.Find(city.Region.CountryId);
                city.Sights = _context.Sights.Where(sight => sight.CityId == id).ToList();
                return PartialView(new CityViewModel(city));
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int? id)
        {
            ViewBag.PageName = id == null ? "Добавление города" : "Редактирование города";
            ViewBag.IsEdit = id != null;

            if (!ViewBag.IsEdit)
                return PartialView();

            var city = _context.Cities.Find(id);
            city.Sights = _context.Sights.Where(sight => sight.CityId == id).ToList();
            return PartialView(new CityViewModel(city));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEdit(int id, CityViewModel cityData)
        {
            if (_context.Regions.Find(cityData.RegionId) != null && ModelState.IsValid)
            {
                City city = new City()
                {
                    Id = id,
                    RegionId = cityData.RegionId,
                    Name = cityData.Name,
                };
                City oldCity = _context.Cities.Find(id);

                if (oldCity != null)
                {
                    _context.Entry(oldCity).State = EntityState.Detached;
                    _context.Update(city);
                }
                else
                    _context.Add(city);

                _context.SaveChanges();
            }

            ViewBag.IsEdit = true;
            return PartialView(cityData);
        }


        [HttpGet]
        public ActionResult Delete(int? id)
        {
            try
            {
                ViewBag.PageName = "Подтверждение удаления";
                City city = _context.Cities.Find(id);
                city.Region = _context.Regions.Find(city.RegionId);
                city.Region.Country = _context.Countries.Find(city.Region.CountryId);
                city.Sights = _context.Sights.Where(sight => sight.CityId == id).ToList();
                return PartialView(new CityViewModel(city));
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {

            _context.Cities.Remove(_context.Cities.Find(id));
            _context.SaveChanges();
            return PartialView();
        }
    }
}
