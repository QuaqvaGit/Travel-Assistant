using System.ComponentModel.DataAnnotations;

namespace TravelAssistant_.Models
{
    public class RegionViewModel
    {
        [Required,Display(Name = "ID")]public int Id { get; init; }
        [Required, Display(Name = "Название")] public string Name { get; set; }
        [Required, Display(Name = "Код")] public int Code { get; set; }
        [Required, Display(Name = "ID страны")] public int CountryIsoId { get; set; }
        [Display(Name = "Страна")] public string CountryName { get; set; }

        [Display(Name = "О стране")]
        public string CountryFullDesc
        {
            get => $"{CountryName}, ISO код {CountryIsoId}";
        }

        public RegionViewModel() {}

        public RegionViewModel(Region region)
        {
            if (region.Country == null) throw new NullReferenceException("Country is null");
            CountryName = region.Country.Name;
            Id = region.Id;
            Name = region.Name;
            Code = region.Code;
            CountryIsoId = region.CountryId;
        }
    }

    public class CityViewModel
    {
        [Key,Required,Display(Name = "ID")] public int Id { get; init; }
        [Required,Display(Name = "Название")] public string Name { get; set; }
        [Display(Name = "Достоприм-ти")] public string Sights { get; init; }
        [Required, Display(Name = "ID региона")] public int RegionId { get; set; }
        [Display(Name = "Регион")] public string RegionName { get; set; }
        [Display(Name = "О регионе")] public string RegionFullDesc { get => $"ID {RegionId} | {RegionName} в стране {CountryName}"; }
        [Display(Name = "Страна")] public string CountryName { get; set; }
        public CityViewModel() {}
        public CityViewModel(City city)
        {
            Id = city.Id;
            Name = city.Name;
            RegionId = city.RegionId;
            if (city.Region!=null)
            {
                RegionName = city.Region.Name;
                CountryName = city.Region.Country.Name;
            }
            if (city.Sights != null)
                Sights = String.Join(",\n", city.Sights);
        }
    }

    public class SightViewModel
    {
        [Key,Required, Display(Name = "ID")] public int Id { get; set; }
        [Required,Display(Name = "Название")] public string Name { get; set; }
        [Display(Name = "Описание")] public string Description { get; set; }
        [Required,Display(Name = "ID города")] public int CityId { get; set; }
        [Display(Name = "Город")] public string CityName { get; set; }
        [Display(Name = "О городе")] public string CityDesc { get => $"ID = {CityId} | {CityName}"; }
        [Display(Name = "Открыт от")] public TimeSpan OpenFrom { get; set; }
        [Display(Name = "Открыт до")] public TimeSpan OpenTill { get; set; }

        public SightViewModel() { }
        public SightViewModel(Sight sight)
        {
            Id = sight.Id;
            Name = sight.Name;
            Description = sight.Description;
            CityId = sight.CityId;
            if (sight.City != null) CityName = sight.City.Name;
            OpenFrom = sight.OpenFrom;
            OpenTill = sight.OpenTill;
        }
    }

    public class RouteViewModel
    {
        [Key,Required,Display(Name = "ID")] public int Id { get; set; }

        [Required,Display(Name = "ID Точки отправления")] public int DepartureId { get; set; }

        [Display(Name = "Точка отправления")] public string DepartureName { get; set; }

        [Required,Display(Name = "ID Точки назначения")] public int DestinationId { get; set; }

        [Display(Name = "Точка назначения")] public string DestinationName { get; set; }
        public RouteViewModel() { }
        public RouteViewModel(Route route)
        {
            Id = route.Id;
            DepartureId = route.DepartureId;
            DestinationId = route.DestinationId;
            DepartureName = route.Departure.ToString();
            DestinationName = route.Destination.ToString();
        }
    }

    public class RouteToWayViewModel
    {
        [Key, Required, Display(Name = "ID")] public int Id { get; set; }
        [Required,Display(Name = "ID способа передвижения")] public int WayId { get; set; }
        [Display(Name = "Способ передвижения")] public string Way { get; init; }
        [Required,Display(Name = "ID маршрута")] public int RouteId { get; set; }
        [Display(Name = "Маршрут")] public string Route { get; init; }
        public RouteToWayViewModel() { }
        public RouteToWayViewModel(RouteToWay routeToWay)
        {
            Id = routeToWay.Id;
            RouteId = routeToWay.RouteId;
            Route = routeToWay.Route.ToString();
            WayId = routeToWay.WayId;
            Way = routeToWay.Way.ToString();
        }
    }

    public class HomeViewModel
    {
        //план:
        //1.Получить строки городов через asp-for , впоследствии парсить их в города
        //2. Для критериев создавать отдельные поля HiddenInput, на submit заносить выбранные в select-ах значения в эти поля, считывать их в посте, формируя словарь
        //3. Передавать словарь в обработчик маршрута и плясать дальше там
        public string Departure { get; set; }
        public string Destination { get; set; }
        public DateTime TicketsValidTill { get; set; }
    }


}
