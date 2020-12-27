using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyProject.Model.NotificationHelper
{
    public class EditSupplier:NotificationMessage
    {
        public int ID { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string DisplayName { get; set; }
        public DateTime ContractDay { get; set; }
        public ICommand ConfirmEdit { get; set; }
        public EditSupplier()
        {
            Title = "Edit";
        }
    }
}
