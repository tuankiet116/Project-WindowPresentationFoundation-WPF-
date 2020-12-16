using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyProject.ViewModel
{
    public class ControlBarViewModel:BaseViewModel
    {
        public ICommand CloseWindow { get; set; }
        public ICommand MinimizeWindow { get; set; }
        public ICommand MaximizeWindow { get; set; }
        public ICommand DragUserControlBar { get; set; }

        public ControlBarViewModel()
        {
            CloseWindow = new RelayCommand<UserControl>((p) => { return p == null ? false : true; },
                (p) =>
                {
                    FrameworkElement window = GetParent(p);
                    Window w = window as Window;
                    if (w != null)
                    {
                        w.Close();
                    }
                });

            MinimizeWindow = new RelayCommand<UserControl>((p) => { return p == null ? false : true; },
                (p) =>
                {
                    FrameworkElement window = GetParent(p);
                    Window w = window as Window;
                    if(w != null)
                    {
                        if(w.WindowState == WindowState.Minimized)
                        {
                            w.WindowState = WindowState.Maximized;
                        }
                        else
                        {
                            w.WindowState = WindowState.Minimized;
                        }                    
                    }
                });
            MaximizeWindow = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                FrameworkElement window = GetParent(p);
                Window w = window as Window;
                if (w != null)
                {
                    if(w.WindowState != WindowState.Maximized)
                    {
                        w.WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        w.WindowState = WindowState.Normal;
                    }
                }
            });

            DragUserControlBar = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                Window w = GetParent(p) as Window;
                if (w != null)
                {
                    if(w.WindowState == WindowState.Maximized)
                    {
                        w.WindowState = WindowState.Normal;
                    }
                    w.DragMove();
                }
            });
        }

        private FrameworkElement GetParent(UserControl p)
        {
            FrameworkElement parent = p;
            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }
            return parent;
        }
    }
}
