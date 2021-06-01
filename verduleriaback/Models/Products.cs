using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace verduleriaback.Models
{
    public class Products
    {
        public int IDPRODUCT { get; set; }
        public string NAMEPRODUCT { get; set; }
        public decimal PRICEPRODUCT { get; set; }
        public int STOCKPRODUCT { get; set; }
        public int STATUSPRODUCT { get; set; }
        public string IMAGEPRODUCT { get; set; }
        public int IDCATEGORY { get; set; }
    }
}
