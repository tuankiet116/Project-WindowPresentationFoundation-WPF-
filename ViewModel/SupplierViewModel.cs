using MaterialDesignThemes.Wpf;
using MyProject.Model;
using MyProject.Model.NotificationHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
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

        public string Message { get { return _Message; } set { _Message = value; OnPropertyChanged(); } }
        public bool IsActiveSnackBar { get { return _IsActiveSnackBar; } set { _IsActiveSnackBar = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ConfirmDelete { get; set; }

        public SupplierViewModel()
        {
            DisplayName = null;
            Address = null;
            Phone = null;
            ContractDay = DateTime.Today;

            loadUserCurrentLogin();
            ListSupplier = new List<SupplierTable>(DataProvider.Ins.Entities.SupplierTable);

            AddCommand = new RelayCommand<object>((p) =>
            {
                var Supplier = DataProvider.Ins.Entities.SupplierTable.Where(x => x.DisplayName == DisplayName);

                if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Address) || ContractDay == null)
                    return false;

                if (Supplier.Count() != 0 || Supplier == null)
                    return false;

                return true;
            }, (p) =>
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
                var Supplier = DataProvider.Ins.Entities.SupplierTable.Where(x => x.DisplayName == DisplayName);

                if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Address) || ContractDay == null)
                    return false;

                if (Supplier == null || SelectedItems == null)
                    return false;

                return true;
            }, (p) =>
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
                var Supplier = DataProvider.Ins.Entities.SupplierTable.Where(x => x.DisplayName == DisplayName);

                if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Address) || ContractDay == null)
                    return false;

                if (Supplier == null || SelectedItems == null)
                    return false;

                return true;
            },(p) =>
            {
                SupplierTable DeleteItem = DataProvider.Ins.Entities.SupplierTable.Where(x => x.ID == SelectedItems.ID).SingleOrDefault();
                if (DeleteItem == null)
                {
                    return;
                }
                try
                {
                     DataProvider.Ins.Entities.SupplierTable.Remove(DeleteItem);
                     DataProvider.Ins.Entities.SaveChanges();
                }
                catch
                {
                    LoadDialogErrorConfirmDelete();
                    return;
                }
               
                ListSupplier = new List<SupplierTable>(DataProvider.Ins.Entities.SupplierTable);

                DisplayName = null;
                Address = null;
                Phone = null;
                ContractDay = DateTime.Today;

                IsActiveSnackBar = true;
                Message = "Xóa Thành Công!";

                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 3000;
                timer.Enabled = true;
                timer.Elapsed += ShowSnackBar;
                timer.Start();
            });

            ConfirmDelete = new RelayCommand<Window>((p) =>
            {
                return true;
            },
            (p) =>
            {
                var ListProduct = DataProvider.Ins.Entities.ProductTable.Where(x => x.ID_Supplier == SelectedItems.ID);
                if (ListProduct == null)
                    return;
                foreach(var item in ListProduct)
                {
                    
                    var ListSell = DataProvider.Ins.Entities.OutPutDetailTable.Where(x=>x.ID_Product == item.ID).ToList();
                    int IDSell = 0;
                    if (ListSell.Count() > 0)
                    {
                        foreach(var sell in ListSell)
                        {
                            IDSell = sell.ID_Output;
                            DataProvider.Ins.Entities.OutPutDetailTable.Remove(sell);
                            DataProvider.Ins.Entities.SaveChanges();
                        }
                    }
                    DataProvider.Ins.Entities.OutputTable.Remove(DataProvider.Ins.Entities.OutputTable.Where(x => x.ID == IDSell).SingleOrDefault());
                    DataProvider.Ins.Entities.SaveChanges();
                    var ListInput = DataProvider.Ins.Entities.InputDetailTable.Where(x => x.ID_Product == item.ID).ToList();
                    int IDInput = 0;
                    if (ListInput.Count() > 0)
                    {
                        foreach (var input in ListInput)
                        {
                            IDInput = input.ID_Input;
                            DataProvider.Ins.Entities.InputDetailTable.Remove(input);
                            DataProvider.Ins.Entities.SaveChanges();
                        }
                    }
                    DataProvider.Ins.Entities.InputTable.Remove(DataProvider.Ins.Entities.InputTable.Where(x => x.ID == IDInput).SingleOrDefault());
                    DataProvider.Ins.Entities.SaveChanges();
                }

                DataProvider.Ins.Entities.SupplierTable.Remove(DataProvider.Ins.Entities.SupplierTable.Where(x => x.ID == SelectedItems.ID).SingleOrDefault());
                DataProvider.Ins.Entities.SaveChanges();
                IsActiveSnackBar = true;
                Message = "Xóa Thành Công!";

                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 3000;
                timer.Enabled = true;
                timer.Elapsed += ShowSnackBar;
                timer.Start();
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

        private void LoadDialogErrorConfirmDelete()
        {
            DeleteNotificationMessage msg = new DeleteNotificationMessage();
            msg.Message = "Nếu Xóa Bản Ghi Này Sẽ Ảnh Hưởng Và Xóa Bỏ Những Bản Liên Quan! Bạn Có Chắc Chắn?";
            DialogHost.Show(msg, "SupplierDialog");
        }
    }
}
