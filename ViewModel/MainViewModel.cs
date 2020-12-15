using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyProject.ViewModel
{
    public class MainViewModel:BaseViewModel
    {
        private bool isLoaded = false;

        public MainViewModel()
        {
            if (!isLoaded)
            {
                this.isLoaded = true;
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
            }
        }
    }
}
