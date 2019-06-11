using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCAppRegister.Extensions
{
    public static class ICollectionExtensions
    {
        public static IEnumerable<SelectListItem>
            ToSelectListItems<T>
            (this ICollection<T> items, int selectdValue)
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("Title"),
                       Value = item.GetPropertyValue("Id"),
                       Selected = item.GetPropertyValue("Id").Equals(selectdValue.ToString())

                   };
        }

    }
}