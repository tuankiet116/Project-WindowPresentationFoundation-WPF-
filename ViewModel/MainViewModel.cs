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
        public ICommand ImportWindow { get; set; }
        public ICommand SellWindow { get; set; }
        public ICommand CustomerWindow { get; set; }
        public ICommand ProductWindow { get; set; } 
        public ICommand SupplierWindow { get; set; }
        public ICommand StatisticalWindow { get; set; }

        private bool isLoaded = false;
        public MainViewModel()
        {
            ImportWindow = new RelayCommand<Window>((p) => { return true; }, (p) => { ImportWindow wd = new ImportWindow(); wd.ShowDialog(); });
            SellWindow = new RelayCommand<Window>((p) => { return true; }, (p) => { SellWindow wd = new SellWindow(); wd.ShowDialog(); });
            CustomerWindow = new RelayCommand<Window>((p) => { return true; }, (p) => { CustomerWindow wd = new CustomerWindow(); wd.ShowDialog(); });
            ProductWindow = new RelayCommand<Window>((p) => { return true; }, (p) => { ProductWindow wd = new ProductWindow(); wd.ShowDialog(); });
            SupplierWindow = new RelayCommand<Window>((p) => { return true; }, (p) => { SupplierWindow wd = new SupplierWindow(); wd.ShowDialog(); });
            StatisticalWindow = new RelayCommand<Window>((p) => { return true; }, (p) => { StatisticalWindow wd = new StatisticalWindow(); wd.ShowDialog(); });
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
    }
}
