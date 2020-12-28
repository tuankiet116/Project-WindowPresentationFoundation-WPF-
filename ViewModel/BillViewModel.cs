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
    public class BillViewModel:BaseViewModel
    {
        private string _UserDisplayName;
        private int _UserIDRole;
        private OutputModel _SelectedItemOutput;
        private OutputDetailModel _SelectedItemOutputDetail;
        private bool _IsActiveSnackBar;
        private string _Message;
        private List<OutputDetailModel> _ListOutputDetail;
        private int _TotalPrice;
        private int _Price;
        private int _Amount;
        private string _SearchTermOutput;
        private string _SearchTermOutputDetail;
        private List<OutputModel> _ListOutput;
        private List<ProductTable> _ListProduct;


        public string Message { get { return _Message; } set { _Message = value; OnPropertyChanged(); } }
        public bool IsActiveSnackBar { get { return _IsActiveSnackBar; } set { _IsActiveSnackBar = value; OnPropertyChanged(); } }
        public string UserDisplayName { get { return _UserDisplayName; } set { _UserDisplayName = value; OnPropertyChanged(); } }

        public List<ProductTable> ListProduct { get { return _ListProduct; } set { _ListProduct = value; } }
        public List<OutputModel> ListOutput { get { return _ListOutput; } set { _ListOutput = value; OnPropertyChanged(); } }
        public OutputModel SelectedItemOutput
        {
            get { return _SelectedItemOutput; }
            set
            {
                _SelectedItemOutput = value;
                if (SelectedItemOutput != null)
                {
                    SelectedItemOutputDetail = null;
                    TotalPrice = 0;
                    Price = 0;
                    Amount = 0;
                    ListOutputDetail = new List<OutputDetailModel>();
                    var List = DataProvider.Ins.Entities.OutPutDetailTable.Where(x => x.ID_Output == SelectedItemOutput.Output.ID);
                    foreach (var item in List)
                    {
                        ProductTable product = new ProductTable();
                        OutputDetailModel detail = new OutputDetailModel();
                        product = DataProvider.Ins.Entities.ProductTable.Where(x => x.ID == item.ID_Product).FirstOrDefault();
                        detail.Output = item;
                        detail.Product = product;
                        ListOutputDetail.Add(detail);
                    }
                    OnPropertyChanged();
                }
            }
        }
        public OutputDetailModel SelectedItemOutputDetail
        {
            get { return _SelectedItemOutputDetail; }
            set
            {
                _SelectedItemOutputDetail = value;
                if (SelectedItemOutputDetail != null)
                {
                    TotalPrice = SelectedItemOutputDetail.Output.TotalPrice;
                    Price = SelectedItemOutputDetail.Output.Price;
                    Amount = SelectedItemOutputDetail.Output.Count;
                    OnPropertyChanged();
                }
            }
        }
        public List<OutputDetailModel> ListOutputDetail { get { return _ListOutputDetail; } set { _ListOutputDetail = value; OnPropertyChanged(); } }
        public string SearchTermOutput { get { return _SearchTermOutput; } set { _SearchTermOutput = value; OnPropertyChanged(); } }
        public string SearchTermOutputDetail { get { return _SearchTermOutputDetail; } set { _SearchTermOutputDetail = value; OnPropertyChanged(); } }
        public int TotalPrice { get { return _TotalPrice; } set { _TotalPrice = value; OnPropertyChanged(); } }
        public int Price { get { return _Price; } set { _Price = value; OnPropertyChanged(); } }
        public int Amount { get { return _Amount; } set { _Amount = value; OnPropertyChanged(); } }

        public ICommand DeleteOutputCommand { get; set; }
        public ICommand DeleteDetailCommand { get; set; }
        public ICommand DeleteConfirmCommand { get; set; }
        public ICommand LoadEditCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        private int ID_CurrentUser = (int)App.Current.Properties["UserID"];

        public BillViewModel()
        {
            LoadOutputData();
            loadUserCurrentLogin();

            LoadEditCommand = new RelayCommand<object>((p) => { return true; }, (p) => { LoadDialogAccountEdit(); });

            DeleteDetailCommand = new RelayCommand<object>((p) =>
            {
                if(SelectedItemOutputDetail == null)
                {
                    return false;
                }
                return true;
            }, (p) =>
            {
                DeleteNotificationMessage msg = new DeleteNotificationMessage();
                Message = "Xóa Thông Tin Có Thể Dẫn Đến Sai Lệch Dữ Liệu! Bạn Chắc Chắn Chứ?";
                DialogHost.Show(msg, "BillDialog");
            });

            DeleteConfirmCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                OutPutDetailTable detailItem = DataProvider.Ins.Entities.OutPutDetailTable.Where(x => x.ID == SelectedItemOutputDetail.Output.ID).FirstOrDefault();
                DataProvider.Ins.Entities.OutPutDetailTable.Remove(detailItem);
                DataProvider.Ins.Entities.SaveChanges();

                ListOutputDetail = new List<OutputDetailModel>();
                var List = DataProvider.Ins.Entities.OutPutDetailTable.Where(x => x.ID_Output == SelectedItemOutput.Output.ID);
                foreach (var item in List)
                {
                    OutputDetailModel detail = new OutputDetailModel();
                    ProductTable product = new ProductTable();
                    product = DataProvider.Ins.Entities.ProductTable.Where(x => x.ID == item.ID_Product).FirstOrDefault();
                    detail.Output = item;
                    detail.Product = product;
                    ListOutputDetail.Add(detail);
                }
                DialogHost.CloseDialogCommand.Execute(null, null);

                SelectedItemOutputDetail = null;

                IsActiveSnackBar = true;
                Message = "Xóa Chi Tiết Thành Công!";
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 5000;
                timer.Enabled = true;
                timer.Elapsed += ShowSnackBar;
                timer.Start();
            });

            DeleteOutputCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                var List = DataProvider.Ins.Entities.OutPutDetailTable.Where(x => x.ID_Output == SelectedItemOutput.Output.ID);
                if (List.Count() != 0)
                {
                    ErrorNotificationMessage msg = new ErrorNotificationMessage();
                    Message = "Phiếu Nhập Này Đang Có Chi Tiết! Bạn Vui Lòng Xóa Bỏ Các Thông Tin Liên Quan!";
                    DialogHost.Show(msg, "BillDialog");
                    return;
                }

                DataProvider.Ins.Entities.OutputTable.Remove(SelectedItemOutput.Output);
                DataProvider.Ins.Entities.SaveChanges();

                LoadOutputData();
                DialogHost.CloseDialogCommand.Execute(null, null);

                SelectedItemOutput = null;

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
                if (SearchTermOutput == null)
                {
                    return;
                }


                var ListInput = DataProvider.Ins.Entities.OutputTable;
                List<OutputModel> Result = new List<OutputModel>();
                foreach (var item in ListInput)
                {
                    OutputModel output = new OutputModel();
                    output.Output = item;
                    output.User = DataProvider.Ins.Entities.UserTable.Where(x => x.ID == item.ID_User).FirstOrDefault();
                    output.Customer = DataProvider.Ins.Entities.CustomerTable.Where(x => x.ID == item.ID_Customer).FirstOrDefault();
                    Result.Add(output);
                }

                ListOutput = new List<OutputModel>(Result.Where(
                    x => x.Output.DateOutput.ToString().ToLower().Contains(SearchTermOutput)
                        || x.User.DisplayName.ToLower().Contains(SearchTermOutput) || x.Customer.FullName.ToLower().Contains(SearchTermOutput)
                    ));

            });
        }

        private void LoadOutputData()
        {
            ListOutput= new List<OutputModel>();

            var List = DataProvider.Ins.Entities.OutputTable;

            foreach (var item in List)
            {
                OutputModel output = new OutputModel();
                output.Output = item;
                output.User = DataProvider.Ins.Entities.UserTable.Where(x => x.ID == item.ID_User).FirstOrDefault();
                output.Customer = DataProvider.Ins.Entities.CustomerTable.Where(x => x.ID == item.ID_Customer).FirstOrDefault();
                ListOutput.Add(output);
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

        private void LoadDialogAccountEdit()
        {
            EditAccountViewModel account = new EditAccountViewModel();
            DialogHost.Show(account, "RootDialog");
        }

        private void ShowSnackBar(Object source, System.Timers.ElapsedEventArgs e)
        {
            IsActiveSnackBar = false;
        }
    }
}

