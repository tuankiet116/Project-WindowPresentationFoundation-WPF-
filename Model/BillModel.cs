using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyProject.Model
{
    public class BillModel
    {
        public ProductTable product { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        public int PriceItems { get; set; }
        public ICommand AmountChange { get; set; }
    }
}
