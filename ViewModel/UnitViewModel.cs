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
    public class UnitViewModel:BaseViewModel
    {   
        private List<UnitTable> _ListUnit;
        private UnitTable _SelectedItems;
        private int _ID_Unit;
        private string _Descriptions;
        private string _Message;
        private bool _IsActiveSnackBar;
        private string _SearchTerm;

        private string _UserDisplayName;
        private int _UserIDRole;

        public string UserDisplayName { get { return _UserDisplayName; } set { _UserDisplayName = value; OnPropertyChanged(); } }
        public string Descriptions { get { return _Descriptions; } set { _Descriptions = value; OnPropertyChanged(); } }
        public string SearchTerm { get { return _SearchTerm; } set { _SearchTerm = value; OnPropertyChanged(); } }

        public string Message { get { return _Message; } set { _Message = value; OnPropertyChanged(); } }
        public bool IsActiveSnackBar { get { return _IsActiveSnackBar; } set { _IsActiveSnackBar = value; OnPropertyChanged(); } }

        public List<UnitTable> ListUnit { get { return _ListUnit; } set { _ListUnit = value; OnPropertyChanged(); } }
        public UnitTable SelectedItems
        {
            get { return _SelectedItems; }
            set
            {
                _SelectedItems = value;
                if (SelectedItems != null)
                {
                    Descriptions = SelectedItems.Descriptions;
                    _ID_Unit = SelectedItems.ID;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public UnitViewModel()
        {
            Descriptions = null;
            ListUnit = new List<UnitTable>(DataProvider.Ins.Entities.UnitTable);
            AddCommand = new RelayCommand<object>((p) =>
            {
                var Unit = DataProvider.Ins.Entities.UnitTable.Where(x => x.Descriptions == Descriptions);

                if (string.IsNullOrEmpty(Descriptions))
                    return false;

                if (Unit.Count() != 0 || Unit == null)
                    return false;

                return true;
            }, (p) =>
            {
                var newUnit = new UnitTable();
                newUnit.Descriptions = Descriptions;

                DataProvider.Ins.Entities.UnitTable.Add(newUnit);
                DataProvider.Ins.Entities.SaveChanges();
                ListUnit = new List<UnitTable>(DataProvider.Ins.Entities.UnitTable);

                Descriptions = null;
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
                var Unit = DataProvider.Ins.Entities.UnitTable.Where(x => x.Descriptions == Descriptions);

                if (string.IsNullOrEmpty(Descriptions))
                    return false;

                if (Unit == null || SelectedItems == null)
                    return false;

                return true;
            }, (p) =>
            {
                UnitTable EditItem = DataProvider.Ins.Entities.UnitTable.Where(x => x.ID == SelectedItems.ID).SingleOrDefault();
                if (EditItem == null)
                {
                    return;
                }
                EditItem.Descriptions = Descriptions;

                DataProvider.Ins.Entities.SaveChanges();
                ListUnit = new List<UnitTable>(DataProvider.Ins.Entities.UnitTable);

                Descriptions = null;
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
                var Unit = DataProvider.Ins.Entities.UnitTable.Where(x => x.Descriptions == Descriptions);

                if (string.IsNullOrEmpty(Descriptions))
                    return false;

                if (Unit == null || SelectedItems == null)
                    return false;

                return true;
            }, (p) =>
            {
                UnitTable DeleteItem = DataProvider.Ins.Entities.UnitTable.Where(x => x.ID == SelectedItems.ID).SingleOrDefault();

                var listProduct = DataProvider.Ins.Entities.ProductTable.Where(x => x.ID_Unit == SelectedItems.ID);

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

                DataProvider.Ins.Entities.UnitTable.Remove(DeleteItem);
                DataProvider.Ins.Entities.SaveChanges();

                ListUnit = new List<UnitTable>(DataProvider.Ins.Entities.UnitTable);

                Descriptions = null;
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

                ListUnit = new List<UnitTable>(DataProvider.Ins.Entities.UnitTable.Where(
                    x => x.Descriptions.ToLower().Contains(SearchTerm)));

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
            msg.Message = "Đơn Vị Đo Này Hiện Đang Được Sử Dụng Cho Các Sản Phẩm Khác ! Bạn Vui Lòng Xóa Thông Tin Ở Các Bản Ghi Liên Quan!";
            DialogHost.Show(msg, "UnitDialog");
        }
    }
}
