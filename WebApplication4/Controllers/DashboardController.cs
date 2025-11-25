using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.EF;

namespace WebApplication4.Controllers
{
    public class DashboardController : Controller
    {
        NGOEntities db = new NGOEntities();
        // GET: Dashboard
        [HttpGet]
        public ActionResult ViewRequest()
        {
            var data = db.CollectRequests.ToList();

            ViewBag.Employees = db.Employees.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult ViewRequest(CollectRequest cr,string btn)
        {
            var data = db.CollectRequests.Find(cr.creq_id);
            data.creq_e_id = cr.creq_e_id;

            if(btn == "Accept")
                data.creq_status = "Accepted";
            else if(btn == "Confirm")
                data.creq_status = "Delivered";

            db.SaveChanges();

                return RedirectToAction("ViewRequest");
        }

        [HttpGet]
        public ActionResult CreateRequest(int id)
        {
            var rdata = db.Resturants.Find(id);

            var data = db.Foods.ToList();
            ViewBag.Foods = data;

            return View(rdata);
        }
        [HttpPost]
        public ActionResult CreateRequest(CollectRequest cr)
        {
            db.CollectRequests.Add(cr);
            db.SaveChanges();
            TempData["Msg"] = "Request Added";

            
            return RedirectToAction("CreateRequest");
        }
    }
}