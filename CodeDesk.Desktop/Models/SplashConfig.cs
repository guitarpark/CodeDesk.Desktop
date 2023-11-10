using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesk.Desktop.Models
{
    public class SplashConfig
    {
        /// <summary>
        ///启动尺寸
        /// </summary>
        public Size Size { get; set; } = new Size(500, 400);
        public string Splash { get; set; } = "splash.png";

        public LoadingOption Loading { get; set; } = new LoadingOption();


        public class LoadingOption
        {
            /// <summary>
            /// 启动画面延时（毫秒）
            /// </summary>
            public int Delayed { get; set; } = 1000;
            public int Left { get; set; } = 0;
            public int Top { get; set; } = 320;
            public int Width { get; set; } = 500;
            public int Height { get; set; } = 20;
            public string Color { get; set; } = "#1ab394";
        }

    }
}
