using MaterialDesignThemes.Wpf;
using MyProject.Model;
using MyProject.Model.NotificationHelper;
using MyProject.ViewModel.HelperViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyProject.ViewModel
{
    public class SupplierViewModel : BaseViewModel
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
        private string _Message;
        private bool _IsActiveSnackBar;
        private string _SearchTerm;

        public string UserDisplayName { get { return _UserDisplayName; } set { _UserDisplayName = value; OnPropertyChanged(); } }

        public List<SupplierTable> ListSupplier { get { return _ListSupplier; } set { _ListSupplier = value; OnPropertyChanged(); } }
        public SupplierTable SelectedItems
        {
            get { return _SelectedItems; }
            set {
                _SelectedItems = value;
                if (SelectedItems != null)
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
        public string SearchTerm { get { return _SearchTerm; } set { _SearchTerm = value; OnPropertyChanged(); } }

        public string Message { get { return _Message; } set { _Message = value; OnPropertyChanged(); } }
        public bool IsActiveSnackBar { get { return _IsActiveSnackBar; } set { _IsActiveSnackBar = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; set; }
        public ICommand ConfirmEditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand LoadEditCommand { get; set; }

        public SupplierViewModel()
        {
            DisplayName = null;
            Address = null;
            Phone = null;
            ContractDay = DateTime.Today;

            loadUserCurrentLogin();
            ListSupplier = new List<SupplierTable>(DataProvider.Ins.Entities.SupplierTable);

            LoadEditCommand = new RelayCommand<object>((p) => { return true; }, (p) => { LoadDialogAccountEdit(); });

            AddCommand = new RelayCommand<object>((p) =>
            {
                var Supplier = DataProvider.Ins.Entities.SupplierTable.Where(x => x.DisplayName == DisplayName);

                if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Address) || ContractDay == null)
                    return false;

                if (Supplier.Count() != 0 || Supplier == null)
                    return false;

                return true;
            }, 
            (p) =>
            {
                var newSupplier = new SupplierTable();
                newSupplier.DisplayName = DisplayName;
                newSupplier.Address = Address;
                newSupplier.Phone = Phone;
                newSupplier.ContractDay = ContractDay;

                DataProvider.Ins.Entities.SupplierTable.Add(newSupplier);
                DataProvider.Ins.Entities.SaveChanges();
                ListSupplier = new List<SupplierTable>(DataProvider.Ins.Entities.SupplierTable);

                DisplayName = null;
                Address = null;
                Phone = null;
                ContractDay = DateTime.Today;
                SelectedItems = null;

                IsActiveSnackBar = true;
                Message = "Thêm Thành Công!";

                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 3000;
                timer.Enabled = true;
                timer.Elapsed += ShowSnackBar;
                timer.Start();
            });

            ConfirmEditCommand = new RelayCommand<Button>((p) =>
            {
                var Supplier = DataProvider.Ins.Entities.SupplierTable.Where(x => x.DisplayName == DisplayName);

                if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Address) || ContractDay == null)
                    return false;

                if (Supplier == null || SelectedItems == null)
                    return false;
                
                return true;
            },
            (p) =>
            {
                SupplierTable EditItem = DataProvider.Ins.Entities.SupplierTable.Where(x=>x.ID == SelectedItems.ID).SingleOrDefault();
                if(EditItem == null)
                {
                    return;
                }
                EditItem.DisplayName = DisplayName;
                EditItem.Address = Address;
                EditItem.ContractDay = ContractDay;
                EditItem.Phone = Phone;

                DataProvider.Ins.Entities.SaveChanges();
                ListSupplier = new List<SupplierTable>(DataProvider.Ins.Entities.SupplierTable);

                DisplayName = null;
                Address = null;
                Phone = null;
                ContractDay = DateTime.Today;
                SelectedItems = null;

                IsActiveSnackBar = true;
                Message = "Sửa Thành Công!";

                DialogHost.CloseDialogCommand.Execute(null, null);
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 3000;
                timer.Enabled = true;
                timer.Elapsed += ShowSnackBar;
                timer.Start();
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                var Supplier = DataProvider.Ins.Entities.SupplierTable.Where(x => x.DisplayName == DisplayName);

                if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Address) || ContractDay == null)
                    return false;

                if (Supplier == null || SelectedItems == null)
                    return false;

                return true;
            },
            (p) =>
            {
                SupplierTable DeleteItem = DataProvider.Ins.Entities.SupplierTable.Where(x => x.ID == SelectedItems.ID).SingleOrDefault();

                var listProduct = DataProvider.Ins.Entities.ProductTable.Where(x => x.ID_Supplier == SelectedItems.ID);

                if (DeleteItem == null)
                {
                    return;
                }

                if (listProduct.Count() > 0)
                {
                    DisplayName = null;
                    Address = null;
                    Phone = null;
                    ContractDay = DateTime.Today;
                    SelectedItems = null;
                    LoadDialogErrorDelete();
                    return;
                }

                DataProvider.Ins.Entities.SupplierTable.Remove(DeleteItem);
                DataProvider.Ins.Entities.SaveChanges();
               
                ListSupplier = new List<SupplierTable>(DataProvider.Ins.Entities.SupplierTable);

                DisplayName = null;
                Address = null;
                Phone = null;
                ContractDay = DateTime.Today;
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
                if(SearchTerm == null)
                {
                    return;
                }

                ListSupplier = new List<SupplierTable> (DataProvider.Ins.Entities.SupplierTable.Where(
                    x=>  x.DisplayName.ToLower().Contains(SearchTerm)
                        || x.Address.ToLower().Contains(SearchTerm) || x.ContractDay.ToString().ToLower().Contains(SearchTerm)
                        || x.Phone.ToLower().Contains(SearchTerm)
                    ));

            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                return true;
            },
            (p) =>
            {
                LoadDialogEdit();
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

        private void LoadDialogEdit()
        {
            EditSupplier dialog = new EditSupplier();
            if (SelectedItems == null)
                return;
            dialog.ID = SelectedItems.ID;
            dialog.ContractDay = SelectedItems.ContractDay;
            dialog.DisplayName = SelectedItems.DisplayName;
            dialog.Address = SelectedItems.Address;
            dialog.Phone = SelectedItems.Phone;
            dialog.ConfirmEdit = ConfirmEditCommand;
            DialogHost.Show(dialog, "SupplierDialog");
        }

        private void LoadDialogErrorDelete()
        {
            DeleteNotificationMessage msg = new DeleteNotificationMessage();
            msg.Message = "Cửa Hàng Đã Thực Hiện Giao Dịch Với Nhà Cung Cấp! Bạn Vui Lòng Xóa Thông Tin Ở Các Bản Ghi Liên Quan!";
            DialogHost.Show(msg, "SupplierDialog");
        }

        private void LoadDialogAccountEdit()
        {
            EditAccountViewModel account = new EditAccountViewModel();
            DialogHost.Show(account, "RootDialog");
        }
    }
}
