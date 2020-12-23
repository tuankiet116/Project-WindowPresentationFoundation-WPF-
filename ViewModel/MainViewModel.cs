﻿using System;
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
        public ICommand LoadMainWindow { get; set; }
        public ICommand CustomerWindow { get; set; }
        public ICommand ProductWindow { get; set; }
        public ICommand SupplierWindow { get; set; }

        private bool isLoaded = false;
        public MainViewModel()
        {
            CustomerWindow = new RelayCommand<object>((p) => { return true; }, (p) => { CustomerWindow wd = new CustomerWindow(); wd.ShowDialog(); });
            ProductWindow = new RelayCommand<object>((p) => { return true; }, (p) => { ProductWindow wd = new ProductWindow(); wd.ShowDialog(); });
            SupplierWindow = new RelayCommand<object>((p) => { return true; }, (p) => { SupplierWindow wd = new SupplierWindow(); wd.ShowDialog(); });
            LoadMainWindow = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (!isLoaded)
                {
                    isLoaded = true;
                    LoginWindow wd = new LoginWindow();
                    wd.ShowDialog();
                }
            });
        }
    }
}
