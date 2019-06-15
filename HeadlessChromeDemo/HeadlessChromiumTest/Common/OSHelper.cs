using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessChromiumTest.Common
{
    public static class OSHelper
    {

        /// <summary>
        /// 判断当前系统是否是 Win7 以下（包含 Win7）
        /// </summary>
        /// <returns></returns>
        public static bool IsWin7Under()
        {
            var p = Environment.OSVersion.Platform;
            var a = Environment.OSVersion.Version.Major;
            var i = Environment.OSVersion.Version.Minor;
            if ((p == PlatformID.Win32NT) && (Convert.ToDouble($"{a}.{i}") <= 6.1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
