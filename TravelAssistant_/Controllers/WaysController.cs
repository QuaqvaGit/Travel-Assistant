using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAssistant_.Models;

namespace TravelAssistant_.Controllers
{
    public class WaysController : Controller
    {
        private TravelContext _context;
        public WaysController(TravelContext context) => _context = context;

        public ActionResult Index()
        {
            ViewBag.PageName = "Список способов передвижения";
            var viewModels = _context.Ways.ToList();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")//isAjax
                return PartialView("TablePartial", viewModels);
            return View(viewModels);
        }

        public ActionResult Details(int id)
        {
            try
            {
                ViewBag.PageName = "Подробности о способе передвижения";
                Way way = _context.Ways.Find(id);
                return PartialView(way);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int? id)
        {
            ViewBag.PageName = id == null ? "Добавление способа" : "Редактирование способа";
            ViewBag.IsEdit = id != null;

            if (!ViewBag.IsEdit)
                return PartialView();

            var way = _context.Ways.Find(id);
            return way == null ? NotFound() : PartialView(way);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEdit(int id, Way wayData)
        {
            if (ModelState.IsValid)
            {
                wayData.Id = id;
                Way oldWay = _context.Ways.Find(id);

                if (oldWay != null)
                {
                    _context.Entry(oldWay).State = EntityState.Detached;
                    _context.Update(wayData);
                }
                else
                    _context.Add(wayData);

                _context.SaveChanges();
            }

            ViewBag.IsEdit = true;
            return PartialView(wayData);
        }


        [HttpGet]
        public ActionResult Delete(int? id)
        {
            try
            {
                ViewBag.PageName = "Подтверждение удаления";
                Way way = _context.Ways.Find(id);
                return PartialView(way);
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
            _context.Ways.Remove(_context.Ways.Find(id));
            _context.SaveChanges();
            return PartialView();
        }
    }
}
