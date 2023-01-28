using Microsoft.EntityFrameworkCore;

namespace TravelAssistant_.Models
{
    public class TravelContext:DbContext
    {
        public DbSet<Country> Countries {get;set;}
        public DbSet<Way> Ways{get;set;}
        public DbSet<Route> Routes {get;set;}
        public DbSet<RouteToWay> RoutesToWays {get;set;}
        public DbSet<Sight> Sights {get;set;}
        public DbSet<Region> Regions { get; set; }
        public DbSet<City> Cities { get; set; }
        public TravelContext():base()
        {
            Database.EnsureCreated();
        }
        public TravelContext(DbContextOptions<TravelContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public void AttachEntities(string listName,bool attachParents)
        {
            switch (listName)
            {
                case "Regions":
                    {
                        foreach (Region region in Regions)
                            region.Country = Countries.Find(region.CountryId);
                        break;
                    }
                case "Cities":
                    {
                        if (attachParents) AttachEntities("Regions", false);
                        foreach(City city in Cities)
                        {
                            city.Region = Regions.Find(city.RegionId);
                            city.Region.Country = Countries.Find(city.Region.CountryId);
                            city.Sights = Sights.Where(sight => sight.CityId == city.Id).ToList();
                        }
                        break;
                    }
                case "Sights":
                    {
                        foreach (Sight sight in Sights)
                            sight.City = Cities.Find(sight.CityId);
                        break;
                    }
                case "Routes":
                    {
                        foreach (Route route in Routes)
                        {
                            route.Departure = Cities.Find(route.DepartureId);
                            route.Destination = Cities.Find(route.DestinationId);
                        }
                        break;
                    }
                case "Ways":
                    {
                        if (attachParents) AttachEntities("Routes", false);
                        foreach (RouteToWay routeToWay in RoutesToWays)
                        {
                            routeToWay.Route = Routes.Find(routeToWay.RouteId);
                            routeToWay.Way = Ways.Find(routeToWay.WayId);
                        }
                        break;
                    }
            }

        }
    }
}
