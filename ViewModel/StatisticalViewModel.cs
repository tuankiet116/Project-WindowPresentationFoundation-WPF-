using MyProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.ViewModel
{
    public class StatisticalViewModel: BaseViewModel
    {
        private ObservableCollection<StatiscalModel> _ListInventory;
        private int _AmountInventory =  0;
        private int _AmountEmployee = 0;
        private int _AmountCustomer = 0;
        private int _AmountSale = 0;
        private string _UserDisplayName;
        private int _UserIDRole;

        public string UserDisplayName { get { return _UserDisplayName; } set { _UserDisplayName = value; OnPropertyChanged(); } }

        public ObservableCollection<StatiscalModel> ListInventory { 
            get { return _ListInventory; } 
            set { _ListInventory = value;
                OnPropertyChanged(); 
            } }
        public int AmountInventory { get { return _AmountInventory; } set { _AmountInventory = value; OnPropertyChanged(); } }
        public int AmountEmployee  { get { return _AmountEmployee; } set { _AmountEmployee = value; OnPropertyChanged(); } }
        public int AmountCustomer  { get { return _AmountCustomer; } set { _AmountCustomer = value; OnPropertyChanged(); } }
        public int AmountSale { get { return _AmountSale; } set { _AmountSale = value; OnPropertyChanged();} }

        public StatisticalViewModel()
        {
            loadUserCurrentLogin();
            loadCustomerData();
            loadEmployeeData();
            loadDataInventory();
        }

        private void loadDataInventory()
        {
            ListInventory = new ObservableCollection<StatiscalModel>();
            var ProductList = DataProvider.Ins.Entities.ProductTable;
            foreach(var item in ProductList)
            {
                var InputList = DataProvider.Ins.Entities.InputDetailTable.Where(p=>p.ID_Product == item.ID);
                var OutputList = DataProvider.Ins.Entities.OutPutDetailTable.Where(p => p.ID_Product == item.ID);

                int sumOutput = 0;
                int sumInput = 0;

                if(InputList.Count() > 0)
                {
                    sumInput = InputList.Sum(p => p.Amount);
                }

                if(OutputList.Count() > 0)
                {
                    sumOutput = OutputList.Sum(p => p.Count);
                }

                AmountInventory += sumInput;
                AmountSale += sumOutput;

                StatiscalModel statiscal = new StatiscalModel();
                statiscal.CountInventory = sumInput - sumOutput;
                statiscal.CountSaled = sumOutput;
                statiscal.Product = item;

                ListInventory.Add(statiscal);
            }
        }

        private void loadCustomerData()
        {
            var CustomerList = DataProvider.Ins.Entities.CustomerTable;
            if(CustomerList != null)
            {
                AmountCustomer = CustomerList.Count();
            }
            
        }

        private void loadEmployeeData()
        {
            var EmployeeList = DataProvider.Ins.Entities.UserTable;
            if (EmployeeList != null)
            {
                AmountEmployee = EmployeeList.Count();
            }
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
    }
}
