using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TravelAssistant_.Models
{
    public enum Criterias
    {
        [Display(Name = "Дешевизна")] Cheapest,
        [Display(Name = "Кратчайший по длине путь")] ShortestLen,
        [Display(Name = "Кратчайший по времени путь")] ShortestTime,
        [Display(Name = "Больше достопримечательностей")] MaxSights
    }

    public enum Values
    {
        [Display(Name = "Низкая")] Low,
        [Display(Name = "Средняя")] Medium,
        [Display(Name = "Высокая")] High
    }
    public static class Enums
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
    }
}
