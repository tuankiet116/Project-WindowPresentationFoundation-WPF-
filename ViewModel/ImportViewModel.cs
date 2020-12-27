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
    public class ImportViewModel:BaseViewModel
    {
        private string _UserDisplayName;
        private int _UserIDRole;
        private List<ImportModel> _ListInput;
        private List<ProductTable> _ListProduct;
        private ImportModel _SelectedItemInput;
        private ImportDetailModel _SelectedItemInputDetail;
        private bool _IsActiveSnackBar;
        private string _Message;
        private List<ImportDetailModel> _ListInputDetail;
        private ProductTable _SelectedProduct;
        private int _InputPrice;
        private int _Price;
        private int _Amount;
        private string _Status;
        private string _SearchTermInput;
        private string _SearchTermInputDetail;


        public string Message { get { return _Message; } set { _Message = value; OnPropertyChanged(); } }
        public bool IsActiveSnackBar { get { return _IsActiveSnackBar; } set { _IsActiveSnackBar = value; OnPropertyChanged(); } }
        public string UserDisplayName { get { return _UserDisplayName; } set { _UserDisplayName = value; OnPropertyChanged(); } }
        public List<ImportModel> ListImport { get { return _ListInput; } set { _ListInput = value; OnPropertyChanged(); } }
        public List<ProductTable> ListProduct { get { return _ListProduct; } set { _ListProduct = value; OnPropertyChanged(); } }
        public ImportModel SelectedItemInput { get { return _SelectedItemInput; } set { 
                _SelectedItemInput = value; 
                if(SelectedItemInput != null)
                {
                    SelectedItemInputDetail = null;
                    InputPrice = 0;
                    Price = 0;
                    Status = null;
                    Amount = 0;
                    SelectedProduct = null;
                    ListInputDetail = new List<ImportDetailModel>();
                    var List = DataProvider.Ins.Entities.InputDetailTable.Where(x => x.ID_Input == SelectedItemInput.input.ID);
                    foreach(var item in List)
                    {
                        ImportDetailModel detail = new ImportDetailModel();
                        ProductTable product = new ProductTable();
                        product = DataProvider.Ins.Entities.ProductTable.Where(x => x.ID == item.ID_Product).FirstOrDefault();
                        detail.InputDetail = item;
                        detail.Product = product;
                        ListInputDetail.Add(detail);
                    }
                    OnPropertyChanged(); 
                }
            } }
        public ImportDetailModel SelectedItemInputDetail { get { return _SelectedItemInputDetail; } set { 
                _SelectedItemInputDetail = value; 
                if(SelectedItemInputDetail != null)
                {
                    InputPrice = SelectedItemInputDetail.InputDetail.PriceInput;
                    Price = SelectedItemInputDetail.InputDetail.PriceSale;
                    Amount = SelectedItemInputDetail.InputDetail.Amount;
                    Status = SelectedItemInputDetail.InputDetail.Status;
                    SelectedProduct = SelectedItemInputDetail.Product;
                    OnPropertyChanged();
                }
            } }
        public List<ImportDetailModel> ListInputDetail { get { return _ListInputDetail; } set { _ListInputDetail = value; OnPropertyChanged(); } }
        public string SearchTermInput { get { return _SearchTermInput; } set { _SearchTermInput = value; OnPropertyChanged(); } }
        public string SearchTermInputDetail { get { return _SearchTermInputDetail; } set { _SearchTermInputDetail = value; OnPropertyChanged(); } }
        public ProductTable SelectedProduct { get { return _SelectedProduct; } set { _SelectedProduct = value; OnPropertyChanged(); } }
        public int InputPrice { get { return _InputPrice; } set { _InputPrice = value; OnPropertyChanged(); } }
        public int Price { get { return _Price; } set { _Price = value; OnPropertyChanged(); } }
        public int Amount { get { return _Amount; } set { _Amount = value; OnPropertyChanged(); } }
        public string Status { get { return _Status; } set { _Status = value; OnPropertyChanged(); } }

        public ICommand CreateNewImportCommand { get; set; }
        public ICommand DeleteImportCommand { get; set; }

        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand DeleteConfirmCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public ICommand SearchCommand { get; set; }
        public ICommand SearchDetailCommand { get; set; }

        private int ID_CurrentUser = (int)App.Current.Properties["UserID"];

        public ImportViewModel()
        {
            loadUserCurrentLogin();
            LoadImportData();

            ListProduct = new List<ProductTable> (DataProvider.Ins.Entities.ProductTable);
            CreateNewImportCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                InputTable newInput = new InputTable();
                newInput.DateInput = DateTime.Now;
                newInput.ID_User = ID_CurrentUser;
                if (newInput == null)
                {
                    return;
                }

                DataProvider.Ins.Entities.InputTable.Add(newInput);
                DataProvider.Ins.Entities.SaveChanges();

                IsActiveSnackBar = true;
                Message = "Thêm Mới Phiếu Nhập Thành Công!";

                LoadImportData();

                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 3000;
                timer.Enabled = true;
                timer.Elapsed += ShowSnackBar;
                timer.Start();

            });

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (Amount == 0 || Price == 0 || InputPrice == 0 || string.IsNullOrEmpty(Status))
                {
                    return false;
                }
                    

                var Product = DataProvider.Ins.Entities.InputDetailTable.Where(x => x.ID_Product == SelectedProduct.ID && x.ID_Input == SelectedItemInput.input.ID );

                if (Product.Count() != 0 || Product == null)
                {
                    return false;
                }

                return true;
            }, 
            (p) =>
            {
                InputDetailTable detailItem = new InputDetailTable();
                detailItem.ID_Input = SelectedItemInput.input.ID;
                detailItem.ID_Product = SelectedProduct.ID;
                detailItem.PriceInput = InputPrice;
                detailItem.PriceSale = Price;
                detailItem.Status = Status;
                detailItem.Amount = Amount;
                DataProvider.Ins.Entities.InputDetailTable.Add(detailItem);
                DataProvider.Ins.Entities.SaveChanges();

                ListInputDetail = new List<ImportDetailModel>();
                var List = DataProvider.Ins.Entities.InputDetailTable.Where(x => x.ID_Input == SelectedItemInput.input.ID);
                foreach (var item in List)
                {
                    ImportDetailModel detail = new ImportDetailModel();
                    ProductTable product = new ProductTable();
                    product = DataProvider.Ins.Entities.ProductTable.Where(x => x.ID == item.ID_Product).FirstOrDefault();
                    detail.InputDetail = item;
                    detail.Product = product;
                    ListInputDetail.Add(detail);
                }

                SelectedItemInputDetail = null;
                InputPrice = 0;
                Price = 0;
                Status = null;
                Amount = 0;
                SelectedProduct = null;

                IsActiveSnackBar = true;
                Message = "Thêm Mới Chi Tiết Thành Công!";
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 5000;
                timer.Enabled = true;
                timer.Elapsed += ShowSnackBar;
                timer.Start();

            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (Amount == 0 || Price == 0 || InputPrice == 0 || string.IsNullOrEmpty(Status) || SelectedProduct == null)
                {
                    return false;
                }

                var Product = DataProvider.Ins.Entities.InputDetailTable.Where(x =>x.ID != SelectedItemInputDetail.Product.ID && x.ID_Product == SelectedProduct.ID && x.ID_Input == SelectedItemInput.input.ID);

                if (Product.Count() != 0 || Product == null)
                {
                    return false;
                }

                return true;
            }, 
            (p) =>
            {
                InputDetailTable detailItem = DataProvider.Ins.Entities.InputDetailTable.Where(x => x.ID == SelectedItemInputDetail.InputDetail.ID).FirstOrDefault();
                detailItem.ID_Product = SelectedProduct.ID;
                detailItem.PriceInput = InputPrice;
                detailItem.PriceSale = Price;
                detailItem.Status = Status;
                detailItem.Amount = Amount;
                DataProvider.Ins.Entities.SaveChanges();

                ListInputDetail = new List<ImportDetailModel>();
                var List = DataProvider.Ins.Entities.InputDetailTable.Where(x => x.ID_Input == SelectedItemInput.input.ID);
                foreach (var item in List)
                {
                    ImportDetailModel detail = new ImportDetailModel();
                    ProductTable product = new ProductTable();
                    product = DataProvider.Ins.Entities.ProductTable.Where(x => x.ID == item.ID_Product).FirstOrDefault();
                    detail.InputDetail = item;
                    detail.Product = product;
                    ListInputDetail.Add(detail);
                }

                SelectedItemInputDetail = null;
                InputPrice = 0;
                Price = 0;
                Status = null;
                Amount = 0;
                SelectedProduct = null;

                IsActiveSnackBar = true;
                Message = "Sửa Chi Tiết Thành Công!";
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 5000;
                timer.Enabled = true;
                timer.Elapsed += ShowSnackBar;
                timer.Start();
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (Amount == 0 || Price == 0 || InputPrice == 0 || string.IsNullOrEmpty(Status) || SelectedProduct == null)
                {
                    return false;
                }

                return true;
            }, (p) =>
            {
                DeleteNotificationMessage msg = new DeleteNotificationMessage();
                Message = "Xóa Thông Tin Có Thể Dẫn Đến Sai Lệch Dữ Liệu! Bạn Chắc Chắn Chứ?";
                DialogHost.Show(msg, "ImportDialog");
            });

            DeleteConfirmCommand = new RelayCommand<object>((p) =>
            {
                if (Amount == 0 || Price == 0 || InputPrice == 0 || string.IsNullOrEmpty(Status) || SelectedProduct == null)
                {
                    return false;
                }

                return true;
            }, (p) =>
            {
                InputDetailTable detailItem = DataProvider.Ins.Entities.InputDetailTable.Where(x => x.ID == SelectedItemInputDetail.InputDetail.ID).FirstOrDefault();
                detailItem.ID_Product = SelectedProduct.ID;
                detailItem.PriceInput = InputPrice;
                detailItem.PriceSale = Price;
                detailItem.Status = Status;
                detailItem.Amount = Amount;
                DataProvider.Ins.Entities.InputDetailTable.Remove(detailItem);
                DataProvider.Ins.Entities.SaveChanges();

                ListInputDetail = new List<ImportDetailModel>();
                var List = DataProvider.Ins.Entities.InputDetailTable.Where(x => x.ID_Input == SelectedItemInput.input.ID);
                foreach (var item in List)
                {
                    ImportDetailModel detail = new ImportDetailModel();
                    ProductTable product = new ProductTable();
                    product = DataProvider.Ins.Entities.ProductTable.Where(x => x.ID == item.ID_Product).FirstOrDefault();
                    detail.InputDetail = item;
                    detail.Product = product;
                    ListInputDetail.Add(detail);
                }
                DialogHost.CloseDialogCommand.Execute(null, null);

                SelectedItemInputDetail = null;
                InputPrice = 0;
                Price = 0;
                Status = null;
                Amount = 0;
                SelectedProduct = null;

                IsActiveSnackBar = true;
                Message = "Xóa Chi Tiết Thành Công!";
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 5000;
                timer.Enabled = true;
                timer.Elapsed += ShowSnackBar;
                timer.Start();
            });

            DeleteImportCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                var List = DataProvider.Ins.Entities.InputDetailTable.Where(x => x.ID_Input == SelectedItemInput.input.ID);
                if(List.Count() != 0)
                {
                    ErrorNotificationMessage msg = new ErrorNotificationMessage();
                    Message = "Phiếu Nhập Này Đang Có Chi Tiết! Bạn Vui Lòng Xóa Bỏ Các Thông Tin Liên Quan!";
                    DialogHost.Show(msg, "ImportDialog");
                    return;
                }

                DataProvider.Ins.Entities.InputTable.Remove(SelectedItemInput.input);
                DataProvider.Ins.Entities.SaveChanges();

                LoadImportData();
                DialogHost.CloseDialogCommand.Execute(null, null);

                SelectedItemInput = null;
                SelectedItemInputDetail = null;
                InputPrice = 0;
                Price = 0;
                Status = null;
                Amount = 0;
                SelectedProduct = null;

                IsActiveSnackBar = true;
                Message = "Xóa Phiếu Nhập Thành Công!";
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 5000;
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
                if (SearchTermInput == null)
                {
                    return;
                }

                
                var ListInput = DataProvider.Ins.Entities.InputTable;
                List<ImportModel> Result = new List<ImportModel>();
                foreach (var item in ListInput)
                {
                    ImportModel import = new ImportModel();
                    import.input = item;
                    import.User = DataProvider.Ins.Entities.UserTable.Where(x => x.ID == item.ID_User).FirstOrDefault();
                    Result.Add(import);
                }

                ListImport = new List<ImportModel>(Result.Where(
                    x => x.input.DateInput.ToString().ToLower().Contains(SearchTermInput)
                        || x.User.DisplayName.ToLower().Contains(SearchTermInput)
                    ));

            });

            SearchDetailCommand = new RelayCommand<object>((p) =>
            {
                return true;
            },
            (p) =>
            {
                if (SearchTermInputDetail == null || SelectedItemInput == null)
                {
                    return;
                }
                List<ImportDetailModel> Result = new List<ImportDetailModel>();

                var List = DataProvider.Ins.Entities.InputDetailTable.Where(x => x.ID_Input == SelectedItemInput.input.ID);
                foreach (var item in List)
                {
                    ImportDetailModel detail = new ImportDetailModel();
                    ProductTable product = new ProductTable();
                    product = DataProvider.Ins.Entities.ProductTable.Where(x => x.ID == item.ID_Product).FirstOrDefault();
                    detail.InputDetail = item;
                    detail.Product = product;
                    Result.Add(detail);
                }
                ListInputDetail = new List<ImportDetailModel>(Result.Where(
                    x => x.Product.DisplayName.ToLower().Contains(SearchTermInputDetail)
                        || x.InputDetail.Amount.ToString().ToLower().Contains(SearchTermInputDetail) || x.InputDetail.PriceInput.ToString().ToLower().Contains(SearchTermInputDetail)
                        || x.InputDetail.PriceSale.ToString().ToLower().Contains(SearchTermInputDetail) || x.InputDetail.Status.Contains(SearchTermInputDetail)
                    ));

            });
        }

        private void LoadImportData()
        {
            ListImport = new List<ImportModel>();
            
            var ListInput = DataProvider.Ins.Entities.InputTable;
            
            foreach(var item in ListInput)
            {
                ImportModel import = new ImportModel();
                import.input = item;
                import.User = DataProvider.Ins.Entities.UserTable.Where(x => x.ID == item.ID_User).FirstOrDefault();
                ListImport.Add(import);
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

        private void ShowSnackBar(Object source, System.Timers.ElapsedEventArgs e)
        {
            IsActiveSnackBar = false;
        }
    }
}
