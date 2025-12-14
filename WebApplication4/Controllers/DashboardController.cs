using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApplication4.DTOs;
using WebApplication4.EF;

namespace WebApplication4.Controllers
{
    public class DashboardController : Controller
    {
        NGOEntities db = new NGOEntities();

        public static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CollectRequest, CollectRequestDTO>().ReverseMap();
                cfg.CreateMap<Food, FoodDTO>().ReverseMap();
                cfg.CreateMap<Resturant, ResturantDTO>().ReverseMap();
                cfg.CreateMap<Employee, EmployeeDTO>().ReverseMap();
            });
            return new Mapper(config);
        }

        // GET: Dashboard
        [HttpGet]
        public ActionResult ViewRequest()
        {
            var data = db.CollectRequests.ToList();
            var mdata = GetMapper().Map<List<CollectRequestDTO>>(data);

            var edata = db.Employees.ToList();
            ViewBag.Employees = GetMapper().Map<List<EmployeeDTO>>(edata);

            return View(mdata);
        }
        [HttpPost]
        public ActionResult ViewRequest(CollectRequestDTO cr,string btn)
        {
            var creq = GetMapper().Map<CollectRequest>(cr);
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
            //
            var rdata = db.Resturants.Find(id);
            var rs = GetMapper().Map<ResturantDTO>(rdata);

            var data = db.Foods.ToList();
            ViewBag.Foods =GetMapper().Map<List<FoodDTO>>(data);

            return View(rs);
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