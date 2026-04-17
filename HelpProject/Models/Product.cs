using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpProject.Models
{
    public class Product
    {
        public string article {  get; set; }
        public string name { get; set; }
        public string unit { get; set; }
        public int price { get; set; }
        public string supplier { get; set; }
        public string manufacturer { get; set; }
        public string category { get; set; }
        public int discountPercent {  get; set; }
        public int stockQuantity { get; set; }
        public string description { get; set; }
        public string picture { get; set; }
    }
}
