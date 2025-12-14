using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication4.EF;

namespace WebApplication4.DTOs
{
    public class EmployeeDTO
    {

        public int e_id { get; set; }
        public string e_name { get; set; }
        public string e_location { get; set; }
        public string e_designation { get; set; }

    }
}