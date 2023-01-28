using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAssistant_.Models;

namespace TravelAssistant_.Controllers
{
    public class CountriesController:Controller
    {
        private TravelContext _context;
        public CountriesController(TravelContext context) => _context = context;

        public ActionResult Index()
        {
            var viewModels = _context.Countries.ToList();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")//isAjax
                return PartialView("TablePartial", viewModels);
            else
            {
                ViewBag.PageName = "Список стран";
                return View(viewModels);
            }
        }

        public ActionResult Details(int id)
        {
            try
            {
                ViewBag.PageName = "Подробности о стране";
                Country country = _context.Countries.Find(id);
                return PartialView(country);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.PageName = "Добавление страны";
            return PartialView("AddOrEdit");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.PageName = "Редактирование страны";
            return PartialView("AddOrEdit",_context.Countries.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Country countryData)
        {
            if (ModelState.IsValid)
            {
                if (_context.Countries.Find(countryData.iso_ID) == null)
                    _context.Database.ExecuteSqlRaw("set IDENTITY_INSERT dbo.Countries ON;" +
                                               $"insert into dbo.Countries(Name, iso_ID) values('{countryData.Name}', {countryData.iso_ID});" +
                                              "set IDENTITY_INSERT dbo.Countries OFF;");
                else return NotFound();
            }
            return PartialView("AddOrEdit",countryData);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Country countryData)
        {
            if(ModelState.IsValid)
            {
                if (_context.Countries.Find(countryData.iso_ID) != null)
                    _context.Database.ExecuteSqlRaw(
                        $"update dbo.Countries set Name = '{countryData.Name}' where iso_ID = {countryData.iso_ID};");
                else return NotFound();
            }
            return PartialView("AddOrEdit", countryData);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            try
            {
                ViewBag.PageName = "Удаление страны";
                return PartialView(_context.Countries.Find(id));
            }
            catch
            {
                return NotFound();
            }
        }

        // POST: CountriesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {

            _context.Countries.Remove(_context.Countries.Find(id));
            _context.SaveChanges();
            return PartialView();
        }

    }
}
