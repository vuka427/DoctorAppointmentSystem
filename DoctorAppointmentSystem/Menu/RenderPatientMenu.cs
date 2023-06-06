using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Menu
{
    public class RenderPatientMenu
    {
        public List<MenuItem> RenderMenu(string idActive)
        {
            List<MenuItem> menu = new List<MenuItem>();
            menu.Add(new MenuItem("home", "Home", "fas fa-home", false, "", "", ""));
            menu.Add(new MenuItem("viewDoctor", "All Doctor", "far fa-hospital-user", false, "", "", ""));
            menu.Add(new MenuItem("scheduleSessions", "Home", "fas fa-calendar-plus", false, "", "", ""));
            menu.Add(new MenuItem("booking", "My Booking", "far fa-bookmark", false, "", "", ""));

            foreach(MenuItem item in menu)
            {
                if (item.id.Equals(idActive))
                {
                    item.active = true;
                    break;
                }
            }
            return menu;
        }
    }
}