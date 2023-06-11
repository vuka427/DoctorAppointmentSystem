using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Menu
{
    public class RenderAdminMenu
    {
        public List<MenuItem> RenderMenu(string idActive)
        {
            List<MenuItem> menu = new List<MenuItem>();
            menu.Add(new MenuItem("Home", "fa-solid fa-house", false, "Admin", "Manage", "Index"));
            menu.Add(new MenuItem("User management", "fa-solid fa-user", false, "Admin", "UserManage", "Index"));
            menu.Add(new MenuItem("Doctor management", "fa-solid fa-user-doctor", false, "Admin", "DoctorManage", "Index"));
            menu.Add(new MenuItem("Patient management", "fa-solid fa-bed-pulse", false, "Admin", "PatientManage", "Index"));
            menu.Add(new MenuItem("Admin management", "fa-solid fa-person-military-pointing", false, "Admin", "AdminUserManage", "Index"));
            menu.Add(new MenuItem("Log out", "fas fa-sign-out-alt", false, "", "", ""));

            foreach (MenuItem item in menu)
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