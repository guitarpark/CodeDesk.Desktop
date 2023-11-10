using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesk.Desktop.CefGlue
{
    public enum CEF_VERSION
    {
        CEF_VERSION_MAJOR = 0,
        CEF_VERSION_MINOR = 1,
        CEF_VERSION_PATCH = 2,
        CEF_COMMIT_NUMBER = 3,

        CHROME_VERSION_MAJOR = 4,
        CHROME_VERSION_MINOR = 5,
        CHROME_VERSION_BUILD = 6,
        CHROME_COMMIT_PATCH = 7
    }
}