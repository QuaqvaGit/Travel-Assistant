#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAssistant_.Models
{
    public class Country//+CountryViewModel
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "ISO код"),Required,Range(0,10000,ErrorMessage = "Код должен быть полоительным")]
        public int iso_ID { get; set; }

        [Display(Name = "Название"),Required]
        public string Name { get; set; }

        public override string ToString() => $"ISO {iso_ID} | {Name}";
    }

    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
        public Country Country { get; set; }
        public int CountryId { get; set; }
    }

    public class Sight
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public TimeSpan OpenFrom { get; set; }
        public TimeSpan OpenTill { get; set; }
        public override string ToString()
        {
            return $"{Id} | {Name} {OpenFrom}-{OpenTill}";
        }
    }

    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Sight> Sights { get; set; }

        public int RegionId { get; set; }

        public Region Region { get; set; }
        public override string ToString()
        {
            return $"ID {Id} | {Name}";
        }
    }

    public class Route
    {
        public int Id { get; set; }
        public int DepartureId { get; set; }
        public City Departure { get; set; }
        public int DestinationId { get; set; }
        public City Destination { get; set; }
        public override string ToString()
        {
            return $"{Departure.Name} - {Destination.Name}";
        }
    }
    public class Way//+WayViewModel
    {
        [Key, Required, Display(Name = "ID")] public int Id { get; set; }
        [Required, Display(Name = "Способ передвижения")] public string way { get; set; }
        [Required, Display(Name = "Расстояние в км"), Range(0,100000)] public double Length { get; set; }
        [Required, Display(Name = "Стоимость")] public int Price { get; set; }
        [Required, Display(Name = "Время в пути")] public TimeSpan Time { get; set; }
        [Display(Name = "Валиден от")] public DateTime? ValidFrom { get; set; }
        [Display(Name = "Валиден до")] public DateTime? ValidTill { get; set; }
        public override string ToString()
        {
            return $"ID {Id} | {way} {Length} км";
        }
    }

    public class RouteToWay
    {
        public int Id { get; set; }
        public int WayId { get; set; }
        public Way Way { get; set; }
        public int RouteId { get; set; }
        public Route Route { get; set; }
    }

}
