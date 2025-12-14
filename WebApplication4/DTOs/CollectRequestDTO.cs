using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication4.EF;

namespace WebApplication4.DTOs
{
    public class CollectRequestDTO
    {
        public int creq_id { get; set; }
        public int creq_r_id { get; set; }
        public int creq_f_id { get; set; }
        public System.DateTime creq_max_time { get; set; }
        public string creq_status { get; set; }
        public Nullable<int> creq_e_id { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Resturant Resturant { get; set; }
        public virtual Food Food { get; set; }
    }
}