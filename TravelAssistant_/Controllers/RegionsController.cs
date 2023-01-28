using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAssistant_.Models;

namespace TravelAssistant_.Controllers
{
    public class RegionsController:Controller
    {
        private TravelContext _context;
        public RegionsController(TravelContext context) => _context = context;

        public ActionResult Index()
        {
            ViewBag.PageName = "Список регионов";

            foreach (Region region in _context.Regions)
                region.Country = _context.Countries.Find(region.CountryId);
            var viewModels = _context.Regions
                .Select(region => new RegionViewModel(region)).ToList();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")//isAjax
                return PartialView("TablePartial", viewModels);
            return View(viewModels);
        }

        public ActionResult Details(int id)
        {
            try
            {
                ViewBag.PageName = "Подробности о регионе";
                Region region = _context.Regions.Find(id);
                region.Country = _context.Countries.Find(region.CountryId);
                return PartialView(new RegionViewModel(region));
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int? id)
        {
            ViewBag.PageName = id == null ? "Добавление региона" : "Редактирование региона";
            ViewBag.IsEdit = id != null;

            if (!ViewBag.IsEdit)
                return PartialView();

            var region = _context.Regions.Find(id);
            region.Country = _context.Countries.Find(region.CountryId);
            return region == null ? NotFound() : PartialView(new RegionViewModel(region));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEdit(int id, RegionViewModel regionData)
        {
            if (_context.Countries.Find(regionData.CountryIsoId)!=null && ModelState.IsValid )
            {
                Region region = new Region()
                {
                    Code = regionData.Code, 
                    Id = id, 
                    Name = regionData.Name,
                    CountryId = regionData.CountryIsoId
                };
                Region oldRegion = _context.Regions.Find(id);

                if (oldRegion != null)
                {
                    _context.Entry(oldRegion).State = EntityState.Detached;
                    _context.Update(region);
                }
                else
                    _context.Add(region);

                _context.SaveChanges();
            }

            ViewBag.IsEdit = true;
            return PartialView(regionData);
        }


        [HttpGet]
        public ActionResult Delete(int? id)
        {
            try
            {
                ViewBag.PageName = "Подтверждение удаления";
                Region region = _context.Regions.Find(id);
                region.Country = _context.Countries.Find(region.CountryId);
                return PartialView(new RegionViewModel(region));
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
            _context.Regions.Remove(_context.Regions.Find(id));
            _context.SaveChanges();
            return PartialView();
        }
    }
}
