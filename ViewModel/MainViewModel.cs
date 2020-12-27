using MaterialDesignThemes.Wpf;
using MyProject.Model;
using MyProject.Model.NotificationHelper;
using MyProject.ViewModel.HelperViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace MyProject.ViewModel
{
    public class MainViewModel:BaseViewModel
    {
        private string _UserDisplayName;
        private int _UserIDRole;

        public string UserDisplayName { get { return _UserDisplayName; } set { _UserDisplayName = value; OnPropertyChanged(); } }

        public ICommand LoadMainWindow { get; set; }
        public ICommand ImportWindow { get; set; }
        public ICommand SellWindow { get; set; }
        public ICommand CustomerWindow { get; set; }
        public ICommand ProductWindow { get; set; } 
        public ICommand SupplierWindow { get; set; }
        public ICommand StatisticalWindow { get; set; }
        public ICommand UnitWindow { get; set; }
        public ICommand UserListWindow { get; set; }
        public ICommand AccountCreateWindow { get; set; }
        public ICommand CloseDialogCommand { get; set; }
        public ICommand AccountWindowCommand { get; set; }
        public ICommand LoadEditCommand { get; set; }

        private bool isLoaded = false;
        public MainViewModel()
        {
            ImportWindow = new RelayCommand<Window>((p) => { return true; }, (p) => 
            { 
                if (_UserIDRole != 1) {
                    LoadDialogErrorNotPermission();
                    return;
                } 
                ImportWindow wd = new ImportWindow(); 
                wd.ShowDialog(); 
            });
            SellWindow = new RelayCommand<Window>((p) => { return true; }, (p) => { SellWindow wd = new SellWindow(); wd.ShowDialog(); });
            CustomerWindow = new RelayCommand<Window>((p) => { return true; }, (p) => { if (_UserIDRole != 1) { LoadDialogErrorNotPermission(); return; } CustomerWindow wd = new CustomerWindow(); wd.ShowDialog(); });
            ProductWindow = new RelayCommand<Window>((p) => { return true; }, (p) => { if (_UserIDRole != 1) { LoadDialogErrorNotPermission(); return; } ProductWindow wd = new ProductWindow(); wd.ShowDialog(); });
            SupplierWindow = new RelayCommand<Window>((p) => { return true; }, (p) => { if (_UserIDRole != 1) { LoadDialogErrorNotPermission(); return; } SupplierWindow wd = new SupplierWindow(); wd.ShowDialog(); });
            StatisticalWindow = new RelayCommand<Window>((p) => { return true; }, (p) => { if (_UserIDRole != 1) { LoadDialogErrorNotPermission(); return; } StatisticalWindow wd = new StatisticalWindow(); wd.ShowDialog(); });
            UnitWindow = new RelayCommand<Window>((p) => { return true; }, (p) => { if (_UserIDRole != 1) { LoadDialogErrorNotPermission(); return; } UnitWindow wd = new UnitWindow(); wd.ShowDialog(); });
            UserListWindow = new RelayCommand<Window>((p) => { return true; }, (p) => { if (_UserIDRole != 1) { LoadDialogErrorNotPermission(); return; } UserListWindow wd = new UserListWindow(); wd.ShowDialog(); });
            AccountCreateWindow = new RelayCommand<Window>((p) => { return true; }, (p) => { if (_UserIDRole != 1) { LoadDialogErrorNotPermission(); return; } AccountCreateWindow wd = new AccountCreateWindow(); wd.ShowDialog(); });
            LoadMainWindow = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (!isLoaded)
                {
                    isLoaded = true;
                    if (p == null)
                        return;

                    p.Hide();
                    LoginWindow wd = new LoginWindow();
                    wd.ShowDialog();
                    var loginViewModel = wd.DataContext as LoginViewModel;
                    if (loginViewModel == null)
                        return;
                    if (loginViewModel.isLogin)
                    {
                        loadUserCurrentLogin();
                        p.Show();
                    }
                    else
                    {
                        p.Close();
                    }
                }
            });

            LoadEditCommand = new RelayCommand<object>((p) => { return true; }, (p) => { LoadDialogAccountEdit(); });
        }

        private void loadUserCurrentLogin()
        {
            Console.WriteLine((int)App.Current.Properties["UserID"]);
            int userID = (int)App.Current.Properties["UserID"];
            var UserInformation = DataProvider.Ins.Entities.UserTable.Where(p => p.ID == userID);
            foreach(var item in UserInformation)
            {
                UserDisplayName = item.DisplayName;
                _UserIDRole = (int)item.ID_Role;
            }
        }

        private void LoadDialogErrorNotPermission()
        {
            ErrorNotificationMessage msg = new ErrorNotificationMessage();
            msg.Message = "Bạn Không Có Quyền Truy Cập! Vui Lòng Liên Hệ Quản Trị Viên!";

            DialogHost.Show(msg, "RootDialog");
        }

        private void LoadDialogAccountEdit()
        {
            EditAccountViewModel account = new EditAccountViewModel();
            DialogHost.Show(account, "RootDialog");
        }
    }
}
