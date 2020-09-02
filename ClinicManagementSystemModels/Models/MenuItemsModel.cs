using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicManagementSystemModels.Models
{
    public class MenuItemsModel
    {
        public MenuItemsModel(string text, string icon, string link)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Icon = icon ?? throw new ArgumentNullException(nameof(icon));
            Link = link ?? throw new ArgumentNullException(nameof(link));
        }

        public string Text { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }

        public List<MenuItemsModel> ChildMenuItems { get; set; }

    }
}