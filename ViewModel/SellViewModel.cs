using MaterialDesignThemes.Wpf;
using MyProject.Model;
using MyProject.Model.NotificationHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MyProject.ViewModel
{
    public class SellViewModel: BaseViewModel
    {
        private List<BillModel> _ListTemp;
        private List<BillModel> _ListBill;
        private BillModel _SelectedItemsBill;
        private int _TotalPrice;
        private int _PhoneCustomer;
        private string _NameCustomer;
        private bool _IsEnableName;

        public List<BillModel> ListBill { get { return _ListBill; } set { _ListBill = value; OnPropertyChanged(); }}
        public BillModel SelectedItemsBill { get { return _SelectedItemsBill; } set { _SelectedItemsBill = value; OnPropertyChanged(); } }
        public int TotalPrice { get { return _TotalPrice; } set { _TotalPrice = value; OnPropertyChanged(); } }
        public int PhoneCustomer { get { return _PhoneCustomer; } set { _PhoneCustomer = value; OnPropertyChanged(); } }
        public string NameCustomer { get { return _NameCustomer; } set { _NameCustomer = value; OnPropertyChanged(); } }
        public bool IsEnableName { get { return _IsEnableName; } set { _IsEnableName = value; OnPropertyChanged(); } }

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
        private List<UnitTable> _ListUnit;
        private List<SupplierTable> _ListSupplier;
        private ProductTable _SelectedItems;

        public List<ProductTable> List { get { return _List; } set { _List = value; OnPropertyChanged(); } }
        public List<SupplierTable> ListSupplier { get { return _ListSupplier; } set { _ListSupplier = value; OnPropertyChanged(); } }
        public List<UnitTable> ListUnit { get { return _ListUnit; } set { _ListUnit = value; OnPropertyChanged(); } }

        public ProductTable SelectedItems
        {
            get { return _SelectedItems; }
            set
            {
                _SelectedItems = value;
            }
        }
        public ICommand SearchCommand { get; set; }
        public ICommand AddBillCommand { get; set; }
        public ICommand DeleteItemInBill { get; set; }
        public ICommand AmountChange { get; set; }
        public ICommand CreateBill { get; set; }
        public ICommand PhoneCustomerChange { get; set; }
        public ICommand DestroyCommand { get; set; }

        public SellViewModel()
        {
            loadUserCurrentLogin();
            TotalPrice = 0;
            IsEnableName = false;
            List = new List<ProductTable>(DataProvider.Ins.Entities.ProductTable);
            ListSupplier = new List<SupplierTable>(DataProvider.Ins.Entities.SupplierTable);
            ListUnit = new List<UnitTable>(DataProvider.Ins.Entities.UnitTable);
            ListBill = new List<BillModel>();
            _ListTemp = new List<BillModel>();

            AddBillCommand = new RelayCommand<object>((p) =>
            {
                if(SelectedItems == null)
                {
                    return false;
                }
                SelectedItemsBill = null;
                return true;
            },
            (p) =>
            {
                BillModel bill = new BillModel();
                ProductTable billProduct = new ProductTable();
                billProduct.ID = SelectedItems.ID;
                billProduct.DisplayName = SelectedItems.DisplayName;
                billProduct.SupplierTable = SelectedItems.SupplierTable;
                billProduct.UnitTable = SelectedItems.UnitTable;
                billProduct.Image = SelectedItems.Image;
                int Price = 0;
                var listPrice = DataProvider.Ins.Entities.InputDetailTable.Where(x => x.ID_Product == SelectedItems.ID).Select(x=>x.PriceSale);

                foreach(var item in listPrice)
                {
                    Price = item;
                }

                bill.product = billProduct;
                bill.Price = Price;
                bill.Amount = 1;
                bill.PriceItems = Price;
                bill.AmountChange = AmountChange;

                if (LoadInventory(bill.product.ID) <= 0)
                {
                    LoadDialogError();
                    return;
                }

                TotalPrice += Price;

                var listCheckDuplicate = _ListTemp.Where(x => x.product.ID == bill.product.ID);
                if (listCheckDuplicate.Count()!=0)
                {
                    int DuplicateProduct = _ListTemp.Where(x => x.product.ID == bill.product.ID).FirstOrDefault().Amount;
                    _ListTemp.Where(x => x.product.ID == bill.product.ID).FirstOrDefault().Amount = DuplicateProduct + 1;
                    _ListTemp.Where(x => x.product.ID == bill.product.ID).FirstOrDefault().PriceItems += Price;
                    ListBill = new List<BillModel>(_ListTemp);
                    SelectedItems = null;
                    return;
                }
                _ListTemp.Add(bill);
                ListBill = new List<BillModel>(_ListTemp);
                SelectedItems = null;
            });

            DeleteItemInBill = new RelayCommand<object>((p) =>
            {
                if (SelectedItemsBill == null)
                    return false;
                SelectedItems = null;
                return true;
            },
            (p) =>
            {
                var FindToDelete = _ListTemp.Where(x => x.product.ID == SelectedItemsBill.product.ID).FirstOrDefault();
                TotalPrice -= FindToDelete.PriceItems;
                _ListTemp.Remove(FindToDelete);
                ListBill = new List<BillModel>(_ListTemp);
                SelectedItemsBill = null;
            });

            AmountChange = new RelayCommand<object>((p) =>
            {
                return true;
            },
            (p) =>
            {
                try
                {
                    var FindToEdit = _ListTemp.Where(x => x.product.ID == SelectedItemsBill.product.ID).FirstOrDefault();
                    TotalPrice -= FindToEdit.PriceItems;
                    FindToEdit.Amount = SelectedItemsBill.Amount;
                    FindToEdit.PriceItems = FindToEdit.Amount * FindToEdit.Price;
                    TotalPrice += FindToEdit.PriceItems;
                    ListBill = new List<BillModel>(_ListTemp);
                }
                catch
                {
                    MessageBox.Show(Message);
                }
            });

            PhoneCustomerChange = new RelayCommand<object>((p) =>
            {
                if (PhoneCustomer.ToString().Length < 9)
                    return false;
                return true;
            },
            (p) =>
            {
                var Customer = DataProvider.Ins.Entities.CustomerTable.Where(x => x.Phone.ToLower().Contains(PhoneCustomer.ToString())).FirstOrDefault();
                if(Customer != null)
                {
                    NameCustomer = Customer.FullName;
                    IsEnableName = false;
                    return;
                }
                NameCustomer = "";
                IsEnableName = true;
            });

            CreateBill = new RelayCommand<object>((p) =>
            {
                if (TotalPrice <= 0 || NameCustomer == null || PhoneCustomer.ToString().Length < 9)
                    return false;
                return true;
            },
            (p) =>
            {
                if (IsEnableName)
                {
                    CustomerTable customer = new CustomerTable();
                    customer.FullName = NameCustomer;
                    customer.Phone = "0" + PhoneCustomer.ToString();
                    customer.Address = "";
                    DataProvider.Ins.Entities.CustomerTable.Add(customer);
                    DataProvider.Ins.Entities.SaveChanges();
                }
                var ID_Customer = DataProvider.Ins.Entities.CustomerTable.Where(x => x.Phone.ToLower().Contains(PhoneCustomer.ToString())).FirstOrDefault();

                OutputTable output = new OutputTable();
                output.ID_User = (int)App.Current.Properties["UserID"];
                output.DateOutput = DateTime.Now;
                output.ID_Customer = ID_Customer.ID;
                DataProvider.Ins.Entities.OutputTable.Add(output);
                DataProvider.Ins.Entities.SaveChanges();
                var ID_Output = DataProvider.Ins.Entities.OutputTable.Where(x => x.ID_User == output.ID_User
                && x.ID_Customer == output.ID_Customer && x.DateOutput == output.DateOutput.Date).FirstOrDefault();

                int ID = ID_Output.ID;
                OutPutDetailTable outPutDetail = new OutPutDetailTable();
                foreach(var item in _ListTemp)
                {
                    outPutDetail.Count = item.Amount;
                    outPutDetail.ID_Product = item.product.ID;
                    outPutDetail.ID_Output = ID;
                }
                LoadDialogSuccessSale();
                TotalPrice = 0;
                _ListBill = null;
                ListBill = null;
                NameCustomer = null;
                PhoneCustomer = 0;
                IsEnableName = false;
            });

            DestroyCommand = new RelayCommand<object>((p) =>
            {
                return true;
            },
            (p) =>
            {
                TotalPrice = 0;
                ListBill = null;
                _ListBill = null;
                NameCustomer = null;
                PhoneCustomer = 0;
                IsEnableName = false;
                LoadDialogDeleted();
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

        private int LoadInventory(int ID_Product)
        {
            var InputList = DataProvider.Ins.Entities.InputDetailTable.Where(p => p.ID_Product == ID_Product);
            var OutputList = DataProvider.Ins.Entities.OutPutDetailTable.Where(p => p.ID_Product == ID_Product);
            int sumOutput = 0;
            int sumInput = 0;

            if (InputList.Count() > 0)
            {
                sumInput = InputList.Sum(p => p.Amount);
            }

            if (OutputList.Count() > 0)
            {
                sumOutput = OutputList.Sum(p => p.Count);
            }

            return sumInput - sumOutput;
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

        private void LoadDialogSuccessSale()
        {
            InforNotificationMessage msg = new InforNotificationMessage();
            msg.Title = "Thành Công";
            msg.Message = "Thanh Toán Thành Công";

            DialogHost.Show(msg, "SellDialog");
        }

        private void LoadDialogDeleted()
        {
            DeleteNotificationMessage msg = new DeleteNotificationMessage();
            msg.Title = "Thành Công";
            msg.Message = "Xóa Hóa Đơn Thành Công!";

            DialogHost.Show(msg, "SellDialog");
        }

        private void LoadDialogError()
        {
            ErrorNotificationMessage msg = new ErrorNotificationMessage();
            msg.Title = "Lỗi";
            msg.Message = "Sản Phẩm Không Còn Trong Kho! Vui Lòng Liên Hệ Quản Trị Viên!";

            DialogHost.Show(msg, "SellDialog");
        }
    }
}
