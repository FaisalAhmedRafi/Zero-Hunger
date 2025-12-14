using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication4.EF;

namespace WebApplication4.DTOs
{
    public class ResturantDTO
    {
        public int r_id { get; set; }
        public string r_name { get; set; }
        public string r_address { get; set; }
        public int food_id { get; set; }
    }
}