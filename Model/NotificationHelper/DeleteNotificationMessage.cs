using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyProject.Model.NotificationHelper
{
    public class DeleteNotificationMessage:NotificationMessage
    {
        public ICommand DeleteConfirmCommand; 
        public DeleteNotificationMessage()
        {
            Title = "Xác Nhận Xóa!";
        }
    }
}
