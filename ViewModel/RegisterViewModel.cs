using MyProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyProject.ViewModel
{
    public class RegisterViewModel:BaseViewModel
    {
        private string _UserName;
        private string _DisplayName;
        private string _Password;
        private string _ConfirmPassword;
        private RoleTable _Role;
        private List<RoleTable> _ListRole;
        private bool _IsActiveSnackBar;
        private string _Message;

        public string Message { get { return _Message; } set { _Message = value; OnPropertyChanged(); } }
        public bool IsActiveSnackBar { get { return _IsActiveSnackBar; } set { _IsActiveSnackBar = value; OnPropertyChanged(); } }
        public string UserName { get { return _UserName; } set { _UserName = value; OnPropertyChanged(); } }
        public string DisplayName { get { return _DisplayName; } set { _DisplayName = value; OnPropertyChanged(); } }
        public string Password { get { return _Password; } set { _Password = value; OnPropertyChanged(); } }
        public string ConfirmPassword { get { return _ConfirmPassword; } set { _ConfirmPassword = value; OnPropertyChanged(); } }
        public RoleTable Role { get { return _Role; } set { _Role = value; OnPropertyChanged(); } }
        public List<RoleTable> ListRole { get { return _ListRole; } set { _ListRole = value; OnPropertyChanged(); } }
        public string Title { get; set; }

        public ICommand Register { get; set; }
        public ICommand PasswordConfirmChangedCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public RegisterViewModel()
        {
            ListRole = new List<RoleTable>(DataProvider.Ins.Entities.RoleTable);
            Title = "Edit Account";
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) =>
            {
                return true;
            }, (p) =>
            {
                Password = p.Password;
            });

            PasswordConfirmChangedCommand = new RelayCommand<PasswordBox>((p) =>
            {
                return true;
            }, (p) =>
            {
                ConfirmPassword = p.Password;
            });

            Register = new RelayCommand<object>((p) =>
            {
                if(string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword) || string.IsNullOrEmpty(UserName))
                    return false;
                var User = DataProvider.Ins.Entities.UserTable.Where(x => x.UserName == UserName);
                if(User.Count() >0)
                {
                    IsActiveSnackBar = true;
                    Message = "Tài Khoản Đã Tồn Tại!";
                    return false;
                }
                if (Role == null)
                    return false;
                if(Password != ConfirmPassword)
                {
                    IsActiveSnackBar = true;
                    Message = "Mật Khẩu Không Khớp!";
                    return false;
                }
                IsActiveSnackBar = false;
                return true;
            },
            (p)=>{
                UserTable newUser = new UserTable();
                newUser.ID_Role = Role.ID;
                newUser.DisplayName = DisplayName;
                newUser.Password = MD5Hash(Base64Encode(Password));
                newUser.UserName = UserName;

                if (newUser != null)
                {
                    DataProvider.Ins.Entities.UserTable.Add(newUser);
                    DataProvider.Ins.Entities.SaveChanges();
                    DisplayName = null;
                    UserName = null;
                    Role = null;
                    Password = null;
                    ConfirmPassword = null;
                    IsActiveSnackBar = true;
                    Message = "Tạo Tài Khoản Thành Công";

                    System.Timers.Timer timer = new System.Timers.Timer();
                    timer.Interval = 3000;
                    timer.Enabled = true;
                    timer.Elapsed += ShowSnackBar;
                    timer.Start();
                }
            });

            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
        }

        private void ShowSnackBar(Object source, System.Timers.ElapsedEventArgs e)
        {
            IsActiveSnackBar = false;
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
