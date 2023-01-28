using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAssistant_.Models;

namespace TravelAssistant_.Controllers
{
    public class SightsController : Controller
    {
        private TravelContext _context;
        public SightsController(TravelContext context) => _context = context;

        public ActionResult Index()
        {
            ViewBag.PageName = "Список достопримечательностей";

            foreach (Sight sight in _context.Sights)
                sight.City = _context.Cities.Find(sight.CityId);
            var viewModels = _context.Sights
                .Select(sight => new SightViewModel(sight)).ToList();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")//isAjax
                return PartialView("TablePartial", viewModels);
            return View(viewModels);
        }

        public ActionResult Details(int id)
        {
            try
            {
                ViewBag.PageName = "Подробности о достопримечательности";
                Sight sight = _context.Sights.Find(id);
                sight.City = _context.Cities.Find(sight.CityId);
                return PartialView(new SightViewModel(sight));
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int? id)
        {
            ViewBag.PageName = id == null ? "Добавление достопримечательности" : "Редактирование достопримечательности";
            ViewBag.IsEdit = id != null;

            if (!ViewBag.IsEdit)
                return PartialView();

            var sight = _context.Sights.Find(id);
            sight.City = _context.Cities.Find(sight.CityId);
            return sight == null ? NotFound() : PartialView(new SightViewModel(sight));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEdit(int id, SightViewModel sightData)
        {
            if (_context.Cities.Find(sightData.CityId) != null && ModelState.IsValid)
            {
                Sight sight = new Sight()
                {
                    Description = sightData.Description,
                    Id = id,
                    Name = sightData.Name,
                    CityId = sightData.CityId,
                    OpenFrom = sightData.OpenFrom,
                    OpenTill = sightData.OpenTill
                };
                Sight oldSight = _context.Sights.Find(id);

                if (oldSight != null)
                {
                    _context.Entry(oldSight).State = EntityState.Detached;
                    _context.Update(sight);
                }
                else
                    _context.Add(sight);

                _context.SaveChanges();
            }

            ViewBag.IsEdit = true;
            return PartialView(sightData);
        }


        [HttpGet]
        public ActionResult Delete(int? id)
        {
            try
            {
                ViewBag.PageName = "Подтверждение удаления";
                Sight sight = _context.Sights.Find(id);
                sight.City = _context.Cities.Find(sight.CityId);
                return PartialView(new SightViewModel(sight));
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
            _context.Sights.Remove(_context.Sights.Find(id));
            _context.SaveChanges();
            return PartialView();
        }
    }
}
