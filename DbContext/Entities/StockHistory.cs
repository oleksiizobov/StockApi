using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockData.Objects.Entities
{

    public class StockHistory
    {
        public int Id { get; set; }
        public string StockId { get; set; }
        public DateTime DateTime { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public DateTime DateCreated { get; set; }
    }

}
