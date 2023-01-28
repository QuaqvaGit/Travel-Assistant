using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAssistant_.Models;
using Route = TravelAssistant_.Models.Route;

namespace TravelAssistant_.Controllers
{
    public class RoutesController : Controller
    {
        private TravelContext _context;
        public RoutesController(TravelContext context) => _context = context;

        public ActionResult Index()
        {
            ViewBag.PageName = "Список маршрутов";

            foreach (Route route in _context.Routes)
            {
                route.Departure = _context.Cities.Find(route.DepartureId);
                route.Destination = _context.Cities.Find(route.DestinationId);
            }
                
            var viewModels = _context.Routes
                .Select(route => new RouteViewModel(route)).ToList();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")//isAjax
                return PartialView("TablePartial", viewModels);
            return View(viewModels);
        }

        public ActionResult Details(int id)
        {
            try
            {
                ViewBag.PageName = "Подробности о маршруте";
                Route route = _context.Routes.Find(id);
                route.Departure = _context.Cities.Find(route.DepartureId);
                route.Destination = _context.Cities.Find(route.DestinationId);
                return PartialView(new RouteViewModel(route));
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int? id)
        {
            ViewBag.PageName = id == null ? "Добавление маршрута" : "Редактирование маршрута";
            ViewBag.IsEdit = id != null;

            if (!ViewBag.IsEdit)
                return PartialView();

            var route = _context.Routes.Find(id);
            route.Departure = _context.Cities.Find(route.DepartureId);
            route.Destination = _context.Cities.Find(route.DestinationId);
            return route == null ? NotFound() : PartialView(new RouteViewModel(route));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEdit(int id, RouteViewModel routeData)
        {
            if (_context.Cities.Find(routeData.DepartureId) != null && _context.Cities.Find(routeData.DestinationId) != null && ModelState.IsValid)
            {
                Route route = new Route()
                {
                    Id = id,
                    DestinationId = routeData.DestinationId,
                    DepartureId = routeData.DepartureId
                };
                Route oldRoute = _context.Routes.Find(id);

                if (oldRoute != null)
                {
                    _context.Entry(oldRoute).State = EntityState.Detached;
                    _context.Update(route);
                }
                else
                    _context.Add(route);

                _context.SaveChanges();
            }

            ViewBag.IsEdit = true;
            return PartialView(routeData);
        }


        [HttpGet]
        public ActionResult Delete(int? id)
        {
            try
            {
                ViewBag.PageName = "Подтверждение удаления";
                Route route = _context.Routes.Find(id);
                route.Departure = _context.Cities.Find(route.DepartureId);
                route.Destination = _context.Cities.Find(route.DestinationId);
                return PartialView(new RouteViewModel(route));
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
            _context.Routes.Remove(_context.Routes.Find(id));
            _context.SaveChanges();
            return PartialView();
        }
    }
}
