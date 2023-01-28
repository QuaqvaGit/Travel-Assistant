using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAssistant_.Models;

namespace TravelAssistant_.Controllers
{
    public class RoutesToWaysController : Controller
    {
        private TravelContext _context;
        public RoutesToWaysController(TravelContext context) => _context = context;

        public ActionResult Index()
        {
            ViewBag.PageName = "Список соответствий";

            foreach (RouteToWay routeToWay in _context.RoutesToWays)
            {
                routeToWay.Route = _context.Routes.Find(routeToWay.RouteId);
                routeToWay.Route.Departure = _context.Cities.Find(routeToWay.Route.DepartureId);
                routeToWay.Route.Destination = _context.Cities.Find(routeToWay.Route.DestinationId);
                routeToWay.Way = _context.Ways.Find(routeToWay.WayId);
            }

            var viewModels = _context.RoutesToWays
                .Select(routeToWay => new RouteToWayViewModel(routeToWay)).ToList();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")//isAjax
                return PartialView("TablePartial", viewModels);
            return View(viewModels);
        }

        public ActionResult Details(int id)
        {
            try
            {
                ViewBag.PageName = "Подробности о маршруте";
                RouteToWay routeToWay = _context.RoutesToWays.Find(id);
                routeToWay.Route = _context.Routes.Find(routeToWay.RouteId);
                routeToWay.Route.Departure = _context.Cities.Find(routeToWay.Route.DepartureId);
                routeToWay.Route.Destination = _context.Cities.Find(routeToWay.Route.DestinationId);
                routeToWay.Way = _context.Ways.Find(routeToWay.WayId);
                return PartialView(new RouteToWayViewModel(routeToWay));
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

            var routeToWay = _context.RoutesToWays.Find(id);
            routeToWay.Route = _context.Routes.Find(routeToWay.RouteId);
            routeToWay.Route.Departure = _context.Cities.Find(routeToWay.Route.DepartureId);
            routeToWay.Route.Destination = _context.Cities.Find(routeToWay.Route.DestinationId);
            routeToWay.Way = _context.Ways.Find(routeToWay.WayId);
            return routeToWay == null ? NotFound() : PartialView(new RouteToWayViewModel(routeToWay));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEdit(int id, RouteToWayViewModel routeData)
        {
            if (_context.Routes.Find(routeData.RouteId) != null && _context.Ways.Find(routeData.WayId) != null && ModelState.IsValid)
            {
                RouteToWay routeToWay = new RouteToWay()
                {
                    Id = id,
                    RouteId = routeData.RouteId,
                    WayId = routeData.WayId
                };
                RouteToWay oldRoute = _context.RoutesToWays.Find(id);

                if (oldRoute != null)
                {
                    _context.Entry(oldRoute).State = EntityState.Detached;
                    _context.Update(routeToWay);
                }
                else
                    _context.Add(routeToWay);

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
                RouteToWay routeToWay = _context.RoutesToWays.Find(id);
                routeToWay.Route = _context.Routes.Find(routeToWay.RouteId);
                routeToWay.Route.Departure = _context.Cities.Find(routeToWay.Route.DepartureId);
                routeToWay.Route.Destination = _context.Cities.Find(routeToWay.Route.DestinationId);
                routeToWay.Way = _context.Ways.Find(routeToWay.WayId);
                return PartialView(new RouteToWayViewModel(routeToWay));
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
            _context.RoutesToWays.Remove(_context.RoutesToWays.Find(id));
            _context.SaveChanges();
            return PartialView();
        }
    }
}