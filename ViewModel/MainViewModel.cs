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
        public ICommand ProductWindow { get; set; }
        public MainViewModel()
        {
            ProductWindow = new RelayCommand<object>((p) => { return true; }, (p) => { ProductWindow wd = new ProductWindow(); wd.ShowDialog(); });
        }

    }
}
