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
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 测试 URL
        /// </summary>
        private const string _testUrl = "https://hanyu.baidu.com/s?wd=%E8%85%BE&ptype=zici";

        /// <summary>
        /// 程序根目录
        /// </summary>
        private static readonly string _programRoot = AppDomain.CurrentDomain.BaseDirectory;



        /// <summary>
        /// 测试无头浏览器
        /// </summary>
        /// <param name="isDisplay">是否显示界面</param>
        /// <returns></returns>
        private async Task<string> TestHeadlessChromiumAsync(bool isDisplay = true)
        {
            this.Invoke(new Action(() =>
            {
                this.rTxt_log.AppendText("正在启动无头浏览器... \n");
            }));

            #region 兼容 Win7 及以下系统 （包含 Win7）

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
            }
            launchOptions.Headless = !isDisplay; // Headless true 是无头模式；false 是开发者模式，有界面

            #endregion

            string htmlContent = string.Empty;

            // 启动 Chromium 浏览器
            using (Browser browser = await Puppeteer.LaunchAsync(launchOptions))
            {
                this.Invoke(new Action(() =>
                {
                    this.rTxt_log.AppendText("无头浏览器已启动。 \n");
                }));

                // 新建一个标签页
                using (Page page = await browser.NewPageAsync())
                {
                    // 导航到 url 页
                    await page.GoToAsync(_testUrl);

                    // 获取并返回页面的HTML内容
                    htmlContent = await page.GetContentAsync();

                    // 界面停留，给开发人员看
                    await Task.Delay(14 * 1000);
                }
            }

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



        private void btn_chromiumTest_Click(object sender, EventArgs e)
        {
            this.btn_chromiumTest.Enabled = false;
            this.rTxt_log.Clear();

            Task.Run(async () =>
            {
                #region 无头浏览器下载更新

                BrowserFetcher browserFetcher = Puppeteer.CreateBrowserFetcher(new BrowserFetcherOptions());
                RevisionInfo revisionInfo = browserFetcher.RevisionInfo(BrowserFetcher.DefaultRevision);

                if (revisionInfo.Downloaded && revisionInfo.Local)
                {
                    this.Invoke(new Action(() =>
                    {
                        this.rTxt_log.AppendText("无头浏览器检查完成，无需更新！ \n");
                    }));
                }
                else
                {
                    this.Invoke(new Action(() =>
                    {
                        this.rTxt_log.AppendText("无头浏览器检查完成，需要更新！ \n");
                        this.rTxt_log.AppendText("正在检查更新... \n");
                    }));

                    // 检查 revisionInfo.Revision 这个版本的 Chromium 浏览器 是否 可用\可下载
                    bool isCan = await browserFetcher.CanDownloadAsync(revisionInfo.Revision);
                    if (!isCan)
                    {
                        this.Invoke(new Action(() =>
                        {
                            this.rTxt_log.AppendText($"程序检测出无头浏览器（默认版本 {revisionInfo.Revision}）无法更新！ \n");
                            this.btn_chromiumTest.Enabled = true;
                        }));
                        return;
                    }

                    this.Invoke(new Action(() =>
                    {
                        this.rTxt_log.AppendText("正在更新无头浏览器... \n");
                    }));

                    // 下载 revisionInfo.Revision 这个版本的无头浏览器；可能需要等待一些时间
                    await browserFetcher.DownloadAsync(revisionInfo.Revision);

                    this.Invoke(new Action(() =>
                    {
                        this.rTxt_log.AppendText("无头浏览器更新完成。 \n");
                    }));
                }

                #endregion

                this.Invoke(new Action(() =>
                {
                    this.rTxt_log.AppendText("开始测试无头浏览器... \n");
                }));

                string htmlContent = await TestHeadlessChromiumAsync();

                this.Invoke(new Action(() =>
                {
                    this.rTxt_log.AppendText("无头浏览器测试完成。 \n");
                    this.btn_chromiumTest.Enabled = true;
                }));

                string path = _programRoot + $"SaveHtmlPage\\{DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss")}.html";
                WriteCreate(path, htmlContent);

                this.Invoke(new Action(() =>
                {
                    this.rTxt_log.AppendText($"测试页面已保存成功。位置 ==> {path} \n");
                }));
            });
        }


        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }


    }
}
