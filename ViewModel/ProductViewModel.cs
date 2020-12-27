using MaterialDesignThemes.Wpf;
using MyProject.Model;
using MyProject.Model.NotificationHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyProject.ViewModel
{
    public class ProductViewModel:BaseViewModel
    {
        private string _UserDisplayName;
        private int _UserIDRole;
        private string _Message;
        private bool _IsActiveSnackBar;
        private string _SearchTerm;

        public string SearchTerm { get { return _SearchTerm; } set { _SearchTerm = value; OnPropertyChanged(); } }
        public string Message { get { return _Message; } set { _Message = value; OnPropertyChanged(); } }
        public bool IsActiveSnackBar { get { return _IsActiveSnackBar; } set { _IsActiveSnackBar = value; OnPropertyChanged(); } }
        public string UserDisplayName { get { return _UserDisplayName; } set { _UserDisplayName = value; OnPropertyChanged(); } }

        private List<ProductTable> _List;
        private int _ID;
        private string _DisplayName;
        private UnitTable _Unit;
        private List<UnitTable> _ListUnit;
        private SupplierTable _Supplier;
        private List<SupplierTable> _ListSupplier;
        private ProductTable _SelectedItems;

        public List<ProductTable> List { get { return _List; } set { _List = value; OnPropertyChanged(); } }
        public int ID { get { return _ID; } set { _ID = value; OnPropertyChanged(); } }
        public string DisplayName { get { return _DisplayName; } set { _DisplayName = value; OnPropertyChanged(); } }
        public UnitTable Unit { get { return _Unit; } set { _Unit = value; OnPropertyChanged(); } }
        public SupplierTable Supplier { get { return _Supplier; } set { _Supplier = value; OnPropertyChanged(); } }
        public List<SupplierTable> ListSupplier { get { return _ListSupplier; } set { _ListSupplier = value; OnPropertyChanged(); } }
        public List<UnitTable> ListUnit { get { return _ListUnit; } set { _ListUnit = value; OnPropertyChanged(); } }
        public ProductTable SelectedItems { get { return _SelectedItems; } set { 
                _SelectedItems = value;
                if(SelectedItems != null)
                {
                    ID = SelectedItems.ID;
                    DisplayName = SelectedItems.DisplayName;
                    Unit = SelectedItems.UnitTable;
                    Supplier = SelectedItems.SupplierTable;
                    OnPropertyChanged();
                }
            } 
        }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public ProductViewModel()
        {
            loadUserCurrentLogin();
            List = new List<ProductTable>(DataProvider.Ins.Entities.ProductTable);
            ListSupplier = new List<SupplierTable>(DataProvider.Ins.Entities.SupplierTable);
            ListUnit = new List<UnitTable>(DataProvider.Ins.Entities.UnitTable);

            AddCommand = new RelayCommand<object>((p) =>
            {
                var products = DataProvider.Ins.Entities.ProductTable.Where(x => x.DisplayName == DisplayName);

                if (string.IsNullOrEmpty(DisplayName) || Unit == null || Supplier == null)
                    return false;

                if (products.Count() != 0 || products == null)
                    return false;

                return true;
            }, (p) =>
            {
                var newProduct = new ProductTable();
                newProduct.DisplayName = DisplayName;
                newProduct.ID_Unit = Unit.ID;
                newProduct.ID_Supplier = Supplier.ID;
                newProduct.Image = "none";

                DataProvider.Ins.Entities.ProductTable.Add(newProduct);
                DataProvider.Ins.Entities.SaveChanges();
                List = new List<ProductTable>(DataProvider.Ins.Entities.ProductTable);

                DisplayName = null;
                Unit = null;
                Supplier = null;
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
                var product = DataProvider.Ins.Entities.ProductTable.Where(x => x.DisplayName == DisplayName);

                if (string.IsNullOrEmpty(DisplayName) || Unit == null || Supplier == null)
                    return false;

                if (product.Count() == 0 || SelectedItems == null)
                    return false;

                return true;
            }, (p) =>
            {
                ProductTable EditItem = DataProvider.Ins.Entities.ProductTable.Where(x => x.ID == SelectedItems.ID).SingleOrDefault();
                if (EditItem == null)
                {
                    return;
                }
                EditItem.DisplayName = DisplayName;
                EditItem.ID_Supplier = Supplier.ID;
                EditItem.ID_Unit = Unit.ID;

                DataProvider.Ins.Entities.SaveChanges();
                List = new List<ProductTable>(DataProvider.Ins.Entities.ProductTable);

                DisplayName = null;
                Unit = null;
                Supplier = null;
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
                var product = DataProvider.Ins.Entities.ProductTable.Where(x => x.DisplayName == DisplayName && x.SupplierTable == Supplier);

                if (string.IsNullOrEmpty(DisplayName) || Unit == null || Supplier == null)
                    return false;

                if (product.Count() == 0 || SelectedItems == null)
                    return false;

                return true;
            }, (p) =>
            {
                if(SelectedItems == null)
                {
                    return;
                }
                ProductTable DeleteItem = DataProvider.Ins.Entities.ProductTable.Where(x => x.ID == SelectedItems.ID).SingleOrDefault();

                var input = DataProvider.Ins.Entities.InputDetailTable.Where(x => x.ID_Product == SelectedItems.ID);
                var output = DataProvider.Ins.Entities.OutPutDetailTable.Where(x => x.ID_Product == SelectedItems.ID);

                if (DeleteItem == null)
                {
                    return;
                }

                if (input.Count() > 0 || output.Count()>0)
                {
                    SelectedItems = null;
                    
                    DisplayName = null;
                    Unit = null;
                    Supplier = null;
                    SelectedItems = null;
                    LoadDialogErrorDelete();
                    return;
                }



                DataProvider.Ins.Entities.ProductTable.Remove(DeleteItem);
                DataProvider.Ins.Entities.SaveChanges();

                List = new List<ProductTable>(DataProvider.Ins.Entities.ProductTable);

                DisplayName = null;
                Unit = null;
                Supplier = null;
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

                List = new List<ProductTable>(DataProvider.Ins.Entities.ProductTable.Where(
                    x => x.DisplayName.ToLower().Contains(SearchTerm)
                        || x.SupplierTable.DisplayName.ToLower().Contains(SearchTerm) || x.UnitTable.Descriptions.ToLower().Contains(SearchTerm)));
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
            msg.Message = "Sản Phẩm Này Đã Được Thực Hiện Giao Dịch! Bạn Vui Lòng Xóa Thông Tin Ở Các Bản Ghi Liên Quan!";
            DialogHost.Show(msg, "ProductDialog");
        }
    }
}
