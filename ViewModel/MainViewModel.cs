using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MyProject.ViewModel
{
    public class MainViewModel:BaseViewModel
    {
        public ICommand CustomerWindow { get; set; }
        public MainViewModel()
        {
            CustomerWindow = new RelayCommand<object>((p) => { return true; }, (p) => { CustomerWindow wd = new CustomerWindow(); wd.ShowDialog(); });
        }
    }
}
