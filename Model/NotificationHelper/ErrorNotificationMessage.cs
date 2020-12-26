using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Model.NotificationHelper
{
    public class ErrorNotificationMessage:NotificationMessage
    {
        public ErrorNotificationMessage()
        {
            Title = "Error";
        }
    }
}
