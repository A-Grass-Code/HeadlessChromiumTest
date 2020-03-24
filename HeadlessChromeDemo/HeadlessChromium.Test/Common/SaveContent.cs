using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessChromium.Test.Common
{
    public class SaveContent
    {

        /// <summary>
        /// <para>保存内容的根目录</para>
        /// <para>位置：程序运行目录\SaveContent\</para>
        /// </summary>
        public static string SaveContentDirectory
        {
            get
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "SaveContent\\";
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                return path;
            }
        }

        /// <summary>
        /// 保存内容，通过 FileMode.Create 方式
        /// </summary>
        /// <param name="content">内容字符串</param>
        /// <param name="path">保存文件的绝对路径 【 默认值：程序运行目录\SaveContent\{当前时间}.html 】</param>
        public static void SaveContentByCreate(string content, string path = null)
        {
            Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    path = SaveContentDirectory + $"{DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss.ffff")}.html";
                }

                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.Write(content);
                    }
                }
            });
        }


    }
}
