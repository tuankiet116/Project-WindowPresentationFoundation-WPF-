using MyProject.Model;
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
        private bool _IsDialogOpen;

        public string UserDisplayName { get { return _UserDisplayName; } set { _UserDisplayName = value; OnPropertyChanged(); } }
        public bool IsDialogOpen { get { return _IsDialogOpen; } set { _IsDialogOpen = value; OnPropertyChanged(); } }

        public ICommand LoadMainWindow { get; set; }
        public ICommand ImportWindow { get; set; }
        public ICommand SellWindow { get; set; }
        public ICommand CustomerWindow { get; set; }
        public ICommand ProductWindow { get; set; }
        public ICommand SupplierWindow { get; set; }
        public ICommand StatisticalWindow { get; set; }
        public ICommand CloseDialogCommand { get; set; }

        private bool isLoaded = false;
        public MainViewModel()
        {
            ImportWindow = new RelayCommand<Window>((p) => { return true; }, (p) => { if (_UserIDRole != 1) { IsDialogOpen = true; return; } ImportWindow wd = new ImportWindow(); wd.ShowDialog(); });
            SellWindow = new RelayCommand<Window>((p) => { return true; }, (p) => { SellWindow wd = new SellWindow(); wd.ShowDialog(); });
            CustomerWindow = new RelayCommand<Window>((p) => { return true; }, (p) => { if (_UserIDRole != 1) { IsDialogOpen = true; return; } CustomerWindow wd = new CustomerWindow(); wd.ShowDialog(); });
            ProductWindow = new RelayCommand<Window>((p) => { return true; }, (p) => { if (_UserIDRole != 1) { IsDialogOpen = true; return; } ProductWindow wd = new ProductWindow(); wd.ShowDialog(); });
            SupplierWindow = new RelayCommand<Window>((p) => { return true; }, (p) => { if (_UserIDRole != 1) { IsDialogOpen = true; return; } SupplierWindow wd = new SupplierWindow(); wd.ShowDialog(); });
            StatisticalWindow = new RelayCommand<Window>((p) => { return true; }, (p) => { if (_UserIDRole != 1) { IsDialogOpen = true; return; } StatisticalWindow wd = new StatisticalWindow(); wd.ShowDialog(); });
            CloseDialogCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { IsDialogOpen = false; });
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
    }
}
