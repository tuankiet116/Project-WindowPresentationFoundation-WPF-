using MyProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.ViewModel
{
    public class SupplierViewModel:BaseViewModel
    {
        private List<SupplierTable> _ListSupplier;
        private SupplierTable _SelectedItems;
        private int _ID_Supplier;
        private string _DisplayName;
        private string _Address;
        private string _Phone;
        private DateTime _ContractDay;
        private string _UserDisplayName;
        private int _UserIDRole;

        public string UserDisplayName { get { return _UserDisplayName; } set { _UserDisplayName = value; OnPropertyChanged(); } }

        public List<SupplierTable> ListSupplier { get { return _ListSupplier; } set { _ListSupplier = value; OnPropertyChanged(); } }
        public SupplierTable SelectedItems 
        { 
            get { return _SelectedItems; } 
            set { 
                _SelectedItems = value;
                if(SelectedItems != null)
                {
                    Address = SelectedItems.Address;
                    DisplayName = SelectedItems.DisplayName;
                    Phone = SelectedItems.Phone;
                    ContractDay = SelectedItems.ContractDay;
                    _ID_Supplier = SelectedItems.ID;
                    OnPropertyChanged();
                }
            } }

        public string DisplayName { get { return _DisplayName; } set { _DisplayName = value; OnPropertyChanged(); } }
        public string Address { get { return _Address; } set { _Address = value; OnPropertyChanged(); } }
        public string Phone { get { return _Phone; } set { _Phone = value; OnPropertyChanged(); } }
        public DateTime ContractDay { get { return _ContractDay; } set { _ContractDay = value; OnPropertyChanged(); } }

        public SupplierViewModel()
        {
            loadUserCurrentLogin();
            ListSupplier = new List<SupplierTable>(DataProvider.Ins.Entities.SupplierTable);
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
        }

        private void AddCommand()
        {

        }
    }
}
