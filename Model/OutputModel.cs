using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Model
{
    public class OutputModel
    {
        public UserTable User { get; set; }
        public CustomerTable Customer { get; set; }
        public OutputTable Output { get; set; }
    }
}
