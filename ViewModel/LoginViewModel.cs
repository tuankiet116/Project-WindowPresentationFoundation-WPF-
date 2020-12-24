using MaterialDesignThemes.Wpf;
using MyProject.Model;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyProject.ViewModel
{
    class LoginViewModel:BaseViewModel
    {
        private string _UserName;
        private string _Password;

        public string userName { get { return _UserName; } set { _UserName = value; OnPropertyChanged(); } }
        public string password { get { return _Password; } set { _Password = value; OnPropertyChanged(); } }
        public Boolean isLogin { get; set; }
        private bool _IsDialogOpen;
        public bool IsDialogOpen
        {
            get { return _IsDialogOpen; }
            set {this._IsDialogOpen = value;  OnPropertyChanged(); }
        }

        public ICommand loginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand CloseDialogCommand { get; set; }
        public ICommand ExitCommand { get; set; }
  
        public LoginViewModel()
        {
            userName = "";
            password = "";
            loginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Login(p); });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { password = p.Password; } );
            CloseDialogCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { IsDialogOpen = false; });
            ExitCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
        }

        private void Login(Window p)
        {
            string passwordEncode = MD5Hash(Base64Encode(password));
            int count = DataProvider.Ins.Entities.UserTable.Where(x => x.UserName == userName && x.Password == passwordEncode).Count();
            if (p == null)
            {
                return;
            }
            if (count > 0)
            {
                isLogin = true;
                p.Close();
            }
            else
            {
                IsDialogOpen = true;
            }
        }

        private string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
