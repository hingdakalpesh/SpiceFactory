using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceFactory.Extensions
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> items, int selectedValue)
        {
            return items.Select(p => new SelectListItem()
            {
                Text = p.GetPropertyValue("Name"),
                Value = p.GetPropertyValue("Id"),
                Selected = p.GetPropertyValue("Id").Equals(selectedValue.ToString())
            });
        }
    }
}
