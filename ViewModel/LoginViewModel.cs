using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyProject.ViewModel
{
    class LoginViewModel
    {
        public Boolean isLoaded { get; set; } 
        public ICommand loginCommand { get; set; }
        public LoginViewModel()
        {
            loginCommand = new RelayCommand<object>((p) => { return true; }, (p) => { isLoaded = true; });
        }
    }
}
