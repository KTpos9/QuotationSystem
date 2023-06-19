using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Zero.Core.Mvc.Models.Select2;

namespace Zero.Core.Mvc.Extensions
{
    public static class SelectListItemExtension
    {
        public static List<SelectListItem> ToSelectListItem(this List<Select2Model> sources)
        {
            if (sources == null)
            {
                return new List<SelectListItem>();
            }

            var destination = new List<SelectListItem>();
            foreach (var source in sources)
            {
                destination.Add(new SelectListItem
                {
                    Text = source.Text,
                    Value = source.Id,
                    Selected = source.Selected,
                    Disabled = source.Disabled
                });
            }
            return destination;
        }
    }
}