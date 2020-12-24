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
        public ICommand LoadMainWindow { get; set; }
        public ICommand CustomerWindow { get; set; }
        public ICommand ProductWindow { get; set; }
        public ICommand SupplierWindow { get; set; }

        private bool isLoaded = false;
        public MainViewModel()
        {
            CustomerWindow = new RelayCommand<object>((p) => { return true; }, (p) => { CustomerWindow wd = new CustomerWindow(); openWorkForm(wd); });
            ProductWindow = new RelayCommand<object>((p) => { return true; }, (p) => { ProductWindow wd = new ProductWindow(); openWorkForm(wd); });
            SupplierWindow = new RelayCommand<object>((p) => { return true; }, (p) => { SupplierWindow wd = new SupplierWindow(); openWorkForm(wd); });
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
                        p.Show();
                    }
                    else
                    {
                        p.Close();
                    }
                }
            });
        }

        private void openWorkForm(Window wd)
        {
            wd.ShowInTaskbar = false;
            wd.Owner = Application.Current.MainWindow;
            wd.Show();
        }
    }
}
