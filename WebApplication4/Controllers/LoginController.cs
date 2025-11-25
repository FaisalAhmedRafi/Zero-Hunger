using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            return View(new Login());
        }
        [HttpPost]
        public ActionResult Login(String userid, string password, string status)
        {
            if (status == "Employee")
            {

                if (userid == "2" && password == "1234")
                {
                    return RedirectToAction("ViewRequest", "Dashboard");
                }
            }

            if (status == "Resturant")
            {

                if (userid == "4" && password == "1234")
                {
                    return RedirectToAction("CreateRequest", "Dashboard");
                }
            }

            return View();
        }
    }
}