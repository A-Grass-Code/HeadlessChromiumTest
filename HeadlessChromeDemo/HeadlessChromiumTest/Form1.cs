using HeadlessChromiumTest.Common;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
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
        /// <param name="isDisplay">是否显示界面</param>
        /// <param name="executablePath">Chromium 或 Chrome 可执行文件的路径</param>
        /// <returns></returns>
        private async Task<string> TestHeadlessChromiumAsync(bool isDisplay = true, string executablePath = null)
        {
            this.Invoke(new Action(() =>
            {
                this.richTextBox1.AppendText("正在启动无头浏览器... \n");
            }));

            #region 兼容 Win7 及以下系统

            LaunchOptions launchOptions;
            if (OSHelper.IsWin7Under())
            {
                launchOptions = new LaunchOptions
                {
                    WebSocketFactory = async (uri, socketOptions, cancellationToken) =>
                    {
                        var client = SystemClientWebSocket.CreateClientWebSocket();
                        if (client is System.Net.WebSockets.Managed.ClientWebSocket managed)
                        {
                            managed.Options.KeepAliveInterval = TimeSpan.FromSeconds(0);
                            await managed.ConnectAsync(uri, cancellationToken);
                        }
                        else
                        {
                            var coreSocket = client as ClientWebSocket;
                            coreSocket.Options.KeepAliveInterval = TimeSpan.FromSeconds(0);
                            await coreSocket.ConnectAsync(uri, cancellationToken);
                        }
                        return client;
                    }
                };
            }
            else
            {
                launchOptions = new LaunchOptions();
                if (executablePath != null)
                {
                    launchOptions.ExecutablePath = executablePath;
                }
            }
            launchOptions.Headless = !isDisplay; // Headless true 是无头模式；false 是开发者模式，有界面

            #endregion

            // 启动 Chromium 浏览器
            Browser browser = await Puppeteer.LaunchAsync(launchOptions);

            this.Invoke(new Action(() =>
            {
                this.richTextBox1.AppendText("无头浏览器已启动。 \n");
            }));

            // 新建一个标签页
            Page page = await browser.NewPageAsync();

            // 导航到 url 页
            await page.GoToAsync(_url);

            // 获取并返回页面的HTML内容
            string htmlContent = await page.GetContentAsync();


            // 界面停留，给开发人员看
            await Task.Delay(14 * 1000);


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
                #region 无头浏览器下载更新

                BrowserFetcher browserFetcher = Puppeteer.CreateBrowserFetcher(new BrowserFetcherOptions());
                // 533271 、 BrowserFetcher.DefaultRevision
                RevisionInfo revisionInfo = browserFetcher.RevisionInfo(BrowserFetcher.DefaultRevision);

                if (revisionInfo.Downloaded && revisionInfo.Local)
                {
                    this.Invoke(new Action(() =>
                    {
                        this.richTextBox1.AppendText("无头浏览器检查完成，无需更新！ \n");
                    }));
                }
                else
                {
                    this.Invoke(new Action(() =>
                    {
                        this.richTextBox1.AppendText("无头浏览器检查完成，需要更新！ \n");
                        this.richTextBox1.AppendText("正在检查更新... \n");
                    }));

                    // 检查 revisionInfo.Revision 这个版本的 Chromium 浏览器 是否 可用\可下载
                    bool isCan = await browserFetcher.CanDownloadAsync(revisionInfo.Revision);
                    if (!isCan)
                    {
                        this.Invoke(new Action(() =>
                        {
                            this.richTextBox1.AppendText($"程序检测出无头浏览器（默认版本 {revisionInfo.Revision}）无法更新！ \n");
                            this.button1.Enabled = true;
                        }));
                        return;
                    }

                    this.Invoke(new Action(() =>
                    {
                        this.richTextBox1.AppendText("正在更新无头浏览器... \n");
                    }));

                    // 下载 revisionInfo.Revision 这个版本的无头浏览器；可能需要等待一些时间
                    await browserFetcher.DownloadAsync(revisionInfo.Revision);

                    this.Invoke(new Action(() =>
                    {
                        this.richTextBox1.AppendText("无头浏览器更新完成。 \n");
                    }));
                }

                #endregion

                this.Invoke(new Action(() =>
                {
                    this.richTextBox1.AppendText("开始测试无头浏览器... \n");
                }));

                string htmlContent = await TestHeadlessChromiumAsync(true, revisionInfo.ExecutablePath);

                this.Invoke(new Action(() =>
                {
                    this.richTextBox1.AppendText("无头浏览器测试完成。 \n");
                    this.button1.Enabled = true;
                }));

                WriteCreate(_programRoot + $"SaveHtmlPage\\{DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss")}.html", htmlContent);
            });
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }


    }
}
