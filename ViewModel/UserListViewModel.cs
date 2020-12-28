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
    public class UserListViewModel:BaseViewModel
    {
        private List<UserTable> _ListUser;
        private UserTable _SelectedItems;
        private int _ID_Unit;
        private string _DisplayName;
        private string _Message;
        private bool _IsActiveSnackBar;
        private string _SearchTerm;

        private string _UserDisplayName;
        private string _UserRole;
        private RoleTable _Role;
        private List<RoleTable> _ListRole;

        public string UserDisplayName { get { return _UserDisplayName; } set { _UserDisplayName = value; OnPropertyChanged(); } }
        public string DisplayName { get { return _DisplayName; } set { _DisplayName = value; OnPropertyChanged(); } }
        public string SearchTerm { get { return _SearchTerm; } set { _SearchTerm = value; OnPropertyChanged(); } }
        public RoleTable Role { get { return _Role; } set { _Role = value; OnPropertyChanged(); } }
        public List<RoleTable> ListRole { get { return _ListRole; } set { _ListRole = value; OnPropertyChanged(); } }
        public string UserRole { get { return _UserRole; } set { _UserRole = value; OnPropertyChanged(); } }

        public string Message { get { return _Message; } set { _Message = value; OnPropertyChanged(); } }
        public bool IsActiveSnackBar { get { return _IsActiveSnackBar; } set { _IsActiveSnackBar = value; OnPropertyChanged(); } }

        public List<UserTable> ListUser { get { return _ListUser; } set { _ListUser = value; OnPropertyChanged(); } }
        public UserTable SelectedItems
        {
            get { return _SelectedItems; }
            set
            {
                _SelectedItems = value;
                if (SelectedItems != null)
                {
                    DisplayName = SelectedItems.DisplayName;
                    _ID_Unit = SelectedItems.ID;
                    Role = DataProvider.Ins.Entities.RoleTable.Where(x => x.ID == SelectedItems.ID_Role).SingleOrDefault();
                    OnPropertyChanged();
                }
            }
        }

        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand LoadEditCommand { get; set; }

        public UserListViewModel()
        {
            DisplayName = null;
            ListUser = new List<UserTable>(DataProvider.Ins.Entities.UserTable);
            ListRole = new List<RoleTable>(DataProvider.Ins.Entities.RoleTable);

            loadUserCurrentLogin();

            LoadEditCommand = new RelayCommand<object>((p) => { return true; }, (p) => { LoadDialogAccountEdit(); });

            EditCommand = new RelayCommand<object>((p) =>
            {
                var USer = DataProvider.Ins.Entities.UserTable.Where(x => x.UserName == SelectedItems.UserName);

                if (string.IsNullOrEmpty(DisplayName) || Role == null)
                    return false;

                if (USer == null || SelectedItems == null)
                    return false;

                return true;
            }, (p) =>
            {
                UserTable EditItem = DataProvider.Ins.Entities.UserTable.Where(x => x.ID == SelectedItems.ID).SingleOrDefault();
                if (EditItem == null)
                {
                    return;
                }
                EditItem.DisplayName = DisplayName;
                EditItem.ID_Role = Role.ID;

                DataProvider.Ins.Entities.SaveChanges();
                ListUser = new List<UserTable>(DataProvider.Ins.Entities.UserTable);

                DisplayName = null;
                SelectedItems = null;
                Role = null;

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
                var Unit = DataProvider.Ins.Entities.UserTable.Where(x => x.UserName == SelectedItems.UserName);

                if (string.IsNullOrEmpty(DisplayName) || Role == null)
                    return false;

                if (Unit == null || SelectedItems == null)
                    return false;

                return true;
            }, (p) =>
            {
                UserTable DeleteItem = DataProvider.Ins.Entities.UserTable.Where(x => x.ID == SelectedItems.ID).SingleOrDefault();

                var listUserInput = DataProvider.Ins.Entities.InputTable.Where(x => x.ID_User == SelectedItems.ID);
                var listUserOutput = DataProvider.Ins.Entities.OutputTable.Where(x => x.ID_User == SelectedItems.ID);

                if (DeleteItem == null)
                {
                    return;
                }

                if (listUserInput.Count() > 0 || listUserOutput.Count()>0)
                {
                    SelectedItems = null;
                    LoadDialogErrorDelete();
                    return;
                }

                DataProvider.Ins.Entities.UserTable.Remove(DeleteItem);
                DataProvider.Ins.Entities.SaveChanges();

                ListUser = new List<UserTable>(DataProvider.Ins.Entities.UserTable);

                Role = null;
                DisplayName = null;
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

                ListUser = new List<UserTable>(DataProvider.Ins.Entities.UserTable.Where(
                    x => x.DisplayName.ToLower().Contains(SearchTerm) && x.UserName.ToLower().Contains(SearchTerm) 
                    && x.RoleTable.Role.ToLower().Contains(SearchTerm)));

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
                _UserRole = item.RoleTable.Role;
            }
        }

        private void ShowSnackBar(Object source, System.Timers.ElapsedEventArgs e)
        {
            IsActiveSnackBar = false;
        }

        private void LoadDialogErrorDelete()
        {
            DeleteNotificationMessage msg = new DeleteNotificationMessage();
            msg.Message = "Nhân Viên Đã Có Hoạt ĐỘng ! Bạn Vui Lòng Xóa Thông Tin Ở Các Bản Ghi Liên Quan!";
            DialogHost.Show(msg, "UserDialog");
        }

        private void LoadDialogAccountEdit()
        {
            EditAccountViewModel account = new EditAccountViewModel();
            DialogHost.Show(account, "RootDialog");
        }
    }
}
