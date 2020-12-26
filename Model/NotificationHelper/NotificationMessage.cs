using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Model.NotificationHelper
{
    public class NotificationMessage
    {
        private string _message = "";
        private string _title = "";

        public string Message { get { return _message; }  set { _message = value; } }
        public string Title { get { return _title; } set { _title = value; } }
    }
}
