using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeadlessChromiumTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 测试 URL
        /// </summary>
        private const string _url = "https://hanyu.baidu.com/s?wd=%E8%85%BE&ptype=zici";

        /// <summary>
        /// 程序根目录
        /// </summary>
        private static readonly string _programRoot = AppDomain.CurrentDomain.BaseDirectory;


        /// <summary>
        /// 测试无头浏览器
        /// </summary>
        /// <returns></returns>
        private static async Task<string> TestHeadlessChromiumAsync()
        {
            // Headless true 是无头模式；false 是开发者模式，有界面
            LaunchOptions launchOptions = new LaunchOptions { Headless = false };

            // 启动 Chromium 浏览器
            Browser browser = await Puppeteer.LaunchAsync(launchOptions);

            // 新建一个标签页
            Page page = await browser.NewPageAsync();

            // 导航到 url 页
            await page.GoToAsync(_url);

            // 获取并返回页面的HTML内容
            string htmlContent = await page.GetContentAsync();


            // 停留 20 秒钟，给开发人员看
            await Task.Delay(20 * 1000);


            await page.CloseAsync();    // 关闭标签页

            await browser.CloseAsync(); // 关闭浏览器

            return htmlContent;
        }

        /// <summary>
        /// Create 【新建】 方式写入文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="result"></param>
        private static void WriteCreate(string path, string result)
        {
            Task.Run(() =>
            {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.Write(result);
                    }
                }
            });
        }



        private void button1_Click(object sender, EventArgs e)
        {
            this.button1.Enabled = false;
            this.richTextBox1.Clear();

            Task.Run(async () =>
            {
                BrowserFetcher browserFetcher = new BrowserFetcher();

                this.Invoke(new Action(() =>
                {
                    this.richTextBox1.AppendText("正在更新无头浏览器... \n\n");
                }));

                // 下载 BrowserFetcher.DefaultRevision 这个版本的无头浏览器；可能需要等待一些时间
                await browserFetcher.DownloadAsync(BrowserFetcher.DefaultRevision);

                this.Invoke(new Action(() =>
                {
                    this.richTextBox1.AppendText("无头浏览器更新完成。 \n\n");
                }));

                this.Invoke(new Action(() =>
                {
                    this.richTextBox1.AppendText("开始测试无头浏览器... \n\n");
                }));

                string htmlContent = await TestHeadlessChromiumAsync();

                this.Invoke(new Action(() =>
                {
                    this.richTextBox1.AppendText("无头浏览器测试完成。 \n\n");

                    this.button1.Enabled = true;
                }));

                WriteCreate(_programRoot + $"SaveHtmlPage\\{DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss")}.html", htmlContent);
            });
        }


    }
}
