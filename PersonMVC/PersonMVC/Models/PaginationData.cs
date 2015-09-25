using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonMVC.Models
{
    public class PaginationData
    {
        public int totalcount { get; set; }
        public int per_num { get; set; }
        public int now_page { get; set; }
    }
}