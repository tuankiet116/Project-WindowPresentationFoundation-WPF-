using MaterialDesignThemes.Wpf;
using MyProject.Model;
using MyProject.Model.NotificationHelper;
using MyProject.ViewModel.HelperViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyProject.ViewModel
{
    class CustomerViewModel:BaseViewModel 
    {
        private string _UserDisplayName;
        private int _UserIDRole;

        public string UserDisplayName { get { return _UserDisplayName; } set { _UserDisplayName = value; OnPropertyChanged(); } }

        private List<CustomerTable> _ListCustomer;
        private CustomerTable _SelectedItems;
        private string _FullName;
        private string _Address;
        private string _Phone;
        private bool _IsActiveSnackBar = false;
        private string _Message;
        private string _SearchTerm;

        public List<CustomerTable> ListCustomer { get { return _ListCustomer; } set { _ListCustomer = value; OnPropertyChanged(); } }
        public string FullName { get { return _FullName; } set { _FullName = value; OnPropertyChanged(); } }
        public string Address { get { return _Address; } set { _Address = value; OnPropertyChanged(); } }
        public string Phone { get { return _Phone; } set { _Phone = value; OnPropertyChanged(); } }
        public CustomerTable SelectedItems { get { return _SelectedItems; } set { 
                _SelectedItems = value;
                if (SelectedItems != null)
                {
                    Address = SelectedItems.Address;
                    FullName = SelectedItems.FullName;
                    Phone = SelectedItems.Phone;
                    OnPropertyChanged();
                }
            } }
        public bool IsActiveSnackBar { get { return _IsActiveSnackBar; } set { _IsActiveSnackBar = value; OnPropertyChanged(); } }
        public string SearchTerm { get { return _SearchTerm; } set { _SearchTerm = value; OnPropertyChanged(); } }
        public string Message { get { return _Message; } set { _Message = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ConfirmDelete { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand LoadEditCommand { get; set; }

        public CustomerViewModel()
        {
            FullName = null;
            Address = null;
            Phone = null;
            loadUserCurrentLogin();
            ListCustomer = new List<CustomerTable>(DataProvider.Ins.Entities.CustomerTable);

            LoadEditCommand = new RelayCommand<object>((p) => { return true; }, (p) => { LoadDialogAccountEdit(); });

            AddCommand = new RelayCommand<object>((p) =>
            {
                var Customer = DataProvider.Ins.Entities.CustomerTable.Where(x => x.Phone == Phone);

                if (string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Address))
                    return false;

                if (Customer.Count() != 0 || Customer == null)
                    return false;

                return true;
            },
            (p) =>
            {
                var newCustomer = new CustomerTable();
                newCustomer.FullName = FullName;
                newCustomer.Address = Address;
                newCustomer.Phone = Phone;

                DataProvider.Ins.Entities.CustomerTable.Add(newCustomer);
                DataProvider.Ins.Entities.SaveChanges();
                ListCustomer = new List<CustomerTable>(DataProvider.Ins.Entities.CustomerTable);

                FullName = null;
                Address = null;
                Phone = null;
                SelectedItems = null;

                IsActiveSnackBar = true;
                Message = "Thêm Thành Công!";

                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 3000;
                timer.Enabled = true;
                timer.Elapsed += ShowSnackBar;
                timer.Start();
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                var Customer = DataProvider.Ins.Entities.CustomerTable.Where(x => x.FullName == FullName);

                if (string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Address))
                    return false;

                if (Customer == null || SelectedItems == null)
                    return false;

                return true;
            }, (p) =>
            {
                CustomerTable EditItem = DataProvider.Ins.Entities.CustomerTable.Where(x => x.ID == SelectedItems.ID).SingleOrDefault();
                if (EditItem == null)
                {
                    return;
                }
                EditItem.FullName = FullName;
                EditItem.Address = Address;
                EditItem.Phone = Phone;

                DataProvider.Ins.Entities.SaveChanges();
                ListCustomer = new List<CustomerTable>(DataProvider.Ins.Entities.CustomerTable);

                FullName = null;
                Address = null;
                Phone = null;
                SelectedItems = null;

                IsActiveSnackBar = true;
                Message = "Sửa Thành Công!";

                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 3000;
                timer.Enabled = true;
                timer.Elapsed += ShowSnackBar;
                timer.Start();
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                var Customer = DataProvider.Ins.Entities.CustomerTable.Where(x => x.Phone == Phone);

                if (string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Address))
                    return false;

                if (Customer == null || SelectedItems == null)
                    return false;

                return true;
            }, (p) =>
            {
                CustomerTable DeleteItem = DataProvider.Ins.Entities.CustomerTable.Where(x => x.ID == SelectedItems.ID).SingleOrDefault();

                var listProduct = DataProvider.Ins.Entities.OutputTable.Where(x => x.ID_Customer == SelectedItems.ID);

                if (DeleteItem == null)
                {
                    return;
                }

                if (listProduct.Count() > 0)
                {
                    SelectedItems = null;
                    LoadDialogErrorDelete();
                    return;
                }

                DataProvider.Ins.Entities.CustomerTable.Remove(DeleteItem);
                DataProvider.Ins.Entities.SaveChanges();

                ListCustomer = new List<CustomerTable>(DataProvider.Ins.Entities.CustomerTable);

                FullName = null;
                Address = null;
                Phone = null;
                SelectedItems = null;

                IsActiveSnackBar = true;
                Message = "Xóa Thành Công!";

                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 3000;
                timer.Enabled = true;
                timer.Elapsed += ShowSnackBar;
                timer.Start();
            });

            SearchCommand = new RelayCommand<object>((p) =>
            {
                return true;
            },
            (p) =>
            {
                if (SearchTerm == null)
                {
                    return;
                }

                ListCustomer = new List<CustomerTable>(DataProvider.Ins.Entities.CustomerTable.Where(
                    x => x.FullName.ToLower().Contains(SearchTerm)
                        || x.Address.ToLower().Contains(SearchTerm) || x.Phone.ToLower().Contains(SearchTerm)
                    ));

            });
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

        private void ShowSnackBar(Object source, System.Timers.ElapsedEventArgs e)
        {
            IsActiveSnackBar = false;
        }

        private void LoadDialogErrorDelete()
        {
            DeleteNotificationMessage msg = new DeleteNotificationMessage();
            msg.Message = "Cửa Hàng Đã Thực Hiện Giao Dịch Với Khách Hàng Này! Bạn Vui Lòng Xóa Thông Tin Ở Các Bản Ghi Liên Quan!";
            DialogHost.Show(msg, "CustomerDialog");
        }

        private void LoadDialogAccountEdit()
        {
            EditAccountViewModel account = new EditAccountViewModel();
            DialogHost.Show(account, "RootDialog");
        }
    }
}
