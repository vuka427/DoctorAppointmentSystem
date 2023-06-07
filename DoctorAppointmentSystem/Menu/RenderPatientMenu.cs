using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Menu
{
    public class RenderPatientMenu
    {
        public void RenderAvatar(string username) 
        {
            
        }
        public List<MenuItem> RenderMenu(string idActive)
        {
            List<MenuItem> menu = new List<MenuItem>();
            menu.Add(new MenuItem("Home", "fas fa-home", false, "", "Home", "Index"));
            menu.Add(new MenuItem("Make Appointment", "fas fa-calendar-plus", false, "", "", ""));
            menu.Add(new MenuItem("Appointment History", "fas fa-calendar-alt", false, "", "", ""));
            menu.Add(new MenuItem("Log out", "fas fa-sign-out-alt", false, "", "", ""));

            foreach(MenuItem item in menu)
            {
                if (item.title.Equals(idActive))
                {
                    item.active = true;
                    break;
                }
            }
            return menu;
        }
    }
}