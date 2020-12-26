using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.ViewModel
{
    class CustomerViewModel:BaseViewModel 
    {
        /*private List<CustomerTable> _ListCustomer;
        private CustomerTable _SelectedItems;
        private int _ID_Customer;
        private string _DisplayName;
        private string _Address;
        private string _Phone;
        private string _UserDisplayName;
        private int _UserIDRole;

        public string UserDisplayName { get { return _UserDisplayName; } set { _UserDisplayName = value; OnPropertyChanged(); } }

        public string DisplayName { get { return _DisplayName; } set { _DisplayName = value; OnPropertyChanged(); } }
        public string Address { get { return _Address; } set { _Address = value; OnPropertyChanged(); } }
        public string Phone { get { return _Phone; } set { _Phone = value; OnPropertyChanged(); } }

        public CustomerViewModel()
        {
            loadUserCurrentLogin();
            ListCustomer = new List<CustomerTable>(DataProvider.Ins.Entities.SupplierTable);
        }

        private void loadUserCurrentLogin()
        {
            Console.WriteLine((int)App.Current.Properties["UserID"]);
            int userID = (int)App.Current.Properties["UserID"];
            var UserInformation = DataProvider.Ins.Entities.UserTable.Where(p => p.ID == userID);
            foreach (var item in UserInformation)
            {
                UserDisplayName = item.DisplayName;
                _UserIDRole = (int)item.ID_Role;
            }
        }*/
    }
}
