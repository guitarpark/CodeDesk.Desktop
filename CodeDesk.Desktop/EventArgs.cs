using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesk.Desktop
{
    public class SizeChangeEventArgs:System.EventArgs
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
