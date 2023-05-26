using DoctorAppointmentSystem.Models;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Controllers
{
    public class AccountController : Controller
    {
        private string salt = "dungvu";
        private readonly UserIO db;

        public AccountController(UserIO db)
        {
            this.db = db;
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            USER user = db.GetUser(username);
            if(user != null && VerifyPassword(password, user.PASSWORDHASH))
            {
                RedirectToAction("Index", "Home");
            }
            return Json(new { error = 1, message = "Login failed! Username or password is incorrect!" });

        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [NonAction]
        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                password = password + salt;
                // Convert the password string to byte array
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Compute the hash value of the password
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Convert the hash bytes to a hexadecimal string
                string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "");

                return hashedPassword;
            }
        }
        [NonAction]
        public bool VerifyPassword(string password, string hashedpassword)
        {
            bool verified = false;
            if (HashPassword(password).Equals(hashedpassword))
            {
                verified = true;
            }
            return verified;
        }
    }
}