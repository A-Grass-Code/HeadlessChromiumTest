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
        /// 保存页面的目录
        /// </summary>
        private string _savePageDirectory
        {
            get
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "SavePage\\";
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                return path;
            }
        }


        /// <summary>
        /// 测试无头浏览器
        /// </summary>
        /// <param name="isDisplay">是否显示界面</param>
        /// <returns></returns>
        private async Task TestHeadlessChromiumAsync(bool isDisplay = true)
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

                    string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss");
                    // 保存截图
                    await page.ScreenshotAsync($"{_savePageDirectory}{fileName}.png");
                    // 获取并返回页面的 Html 内容
                    htmlContent = await page.GetContentAsync();
                    // 保存 Html 内容
                    WriteCreate($"{_savePageDirectory}{fileName}.html", htmlContent);

                    this.Invoke(new Action(() =>
                    {
                        this.rTxt_log.AppendText($"测试页面已保存成功。目录 ==> {_savePageDirectory} \n");
                    }));

                    // 界面停留，给开发人员看
                    await Task.Delay(14 * 1000);
                }
            }
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


        // 无头浏览器测试
        private void btn_chromiumTest_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                this.btn_chromiumTest.Enabled = false;
                this.rTxt_log.Clear();
            }));

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

                await TestHeadlessChromiumAsync();

                this.Invoke(new Action(() =>
                {
                    this.rTxt_log.AppendText("无头浏览器测试完成。 \n");
                    this.btn_chromiumTest.Enabled = true;
                }));
            });
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }



        // 无头浏览器测试，封装版
        private void btn_chromiumTest_Encapsulated_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                this.btn_chromiumTest_Encapsulated.Enabled = false;
            }));

            Task.Run(async () =>
            {
                LaunchOptions launchOptions = await ChromiumBrowser.ChromiumLaunchOptions(true, true);
                using (Browser browser = await Puppeteer.LaunchAsync(launchOptions))
                {
                    using (Page page = await ChromiumBrowser.NewPageAndInitAsync(browser))
                    {
                        // 导航到 url 页
                        await page.GoToAsync(_testUrl);

                        string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss");
                        // 保存截图 1
                        await ChromiumBrowser.SavePageScreenshotAsync(page, true, $"{SaveContent.SaveContentDirectory}{fileName}-1.png");
                        // 保存截图 2
                        await ChromiumBrowser.SavePageScreenshotAsync(page, null, $"{SaveContent.SaveContentDirectory}{fileName}-2.png");

                        // 获取并保存页面的 Html 内容
                        string htmlContent = await page.GetContentAsync();
                        SaveContent.SaveContentByCreate(htmlContent, $"{SaveContent.SaveContentDirectory}{fileName}.html");
                    }
                }
            }).ContinueWith(t =>
            {
                this.Invoke(new Action(() =>
                {
                    this.btn_chromiumTest_Encapsulated.Enabled = true;
                }));
            });
        }


    }
}
