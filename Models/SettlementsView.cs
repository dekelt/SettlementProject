using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SettlementProject.Models
{
    public class SettlementsView
    {
        public int CurrentPageIndex { get; set; }
        public int PageCount { get; set; }
        public List<Settlement> SettlementList { get; set; }



    }
}
