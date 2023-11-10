using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesk.Desktop.Models
{
    internal class SchemeConfig
    {
        public string AppScheme { get; set; }
        public Uri AppOriginUri { get; set; }
        public string AppOrigin { get; set; }
        public string HomePage { get; set; }
        public string HomePagePath { get; set; }
    }
}
