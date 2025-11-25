using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace WirtualnaUczelnia.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            // Pobiera atrybut [Display(Name="...")] z enuma
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .FirstOrDefault()?
                            .GetCustomAttribute<DisplayAttribute>()?
                            .Name ?? enumValue.ToString();
        }
    }
}