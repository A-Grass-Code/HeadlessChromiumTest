using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessChromiumTest.Common
{
    public class ChromiumBrowser
    {

        /// <summary>
        /// <para>设置一个 Chromium 浏览器 启动选项 的对象，并返回这个对象；此方法兼容 Windows7 / Windows Server 2008</para>
        /// <para>异常可能：程序运行目录下 Chromium 浏览器不可用。</para>
        /// </summary>
        /// <param name="checkIsDownload">检查 是否下载 Chromium 浏览器；默认 false</param>
        /// <param name="isDisplay">Chromium 运行时 是否显示界面；默认 true</param>
        /// <param name="args">要传递给 Chromium 浏览器实例的其他参数。（ 此方法会自动传入 "--no-sandbox" 参数 ）
        /// <para>参考：https://peter.sh/experiments/chromium-command-line-switches/#no-sandbox </para>
        /// </param>
        /// <param name="ignoredDefaultArgs">如果给出数组，则过滤掉给定的 Puppeteer.DefaultArgs 默认参数。
        /// （ 此方法会自动设置 "--enable-automation" 过滤掉这个参数 ）
        /// <para>参考：https://peter.sh/experiments/chromium-command-line-switches/#no-sandbox </para>
        /// </param>
        /// <returns></returns>
        private static async Task<LaunchOptions> SetChromiumLaunchOptions(
            bool checkIsDownload = false,
            bool isDisplay = true,
            string[] args = null,
            string[] ignoredDefaultArgs = null)
        {
            return await Task.Run(async () =>
            {
                BrowserFetcher browserFetcher = Puppeteer.CreateBrowserFetcher(new BrowserFetcherOptions());
                RevisionInfo revisionInfo = browserFetcher.RevisionInfo(BrowserFetcher.DefaultRevision);

                #region 检查下载 Chromium
                if (!(revisionInfo.Downloaded && revisionInfo.Local))
                {
                    if (checkIsDownload)
                    {
                        // 检查 revisionInfo.Revision 这个版本的 Chromium 浏览器 是否 可下载
                        bool isCan = await browserFetcher.CanDownloadAsync(revisionInfo.Revision);
                        if (isCan)
                        {
                            // 下载 revisionInfo.Revision 这个版本的无头浏览器；可能需要等待一些时间
                            await browserFetcher.DownloadAsync(revisionInfo.Revision);
                        }
                        else
                        {
                            throw new Exception($"程序检测出 Chromium 浏览器（默认版本 {revisionInfo.Revision}）无法更新！");
                        }
                    }
                    else
                    {
                        throw new Exception("程序运行目录下 Chromium 浏览器不可用。请开发人员检查 程序运行目录下 是否正确安装 Chromium 浏览器。");
                    }
                }
                #endregion

                #region 兼容 Windows7 / Windows Server 2008
                LaunchOptions launchOptions = default(LaunchOptions);
                // 这个判断是为了兼容 Windows7 和 Windows Server 2008
                if (OSHelper.IsWin7Under())
                {
                    launchOptions = new LaunchOptions
                    {
                        WebSocketFactory = async (uri, socketOptions, cancellationToken) =>
                        {
                            WebSocket client = SystemClientWebSocket.CreateClientWebSocket();
                            if (client is System.Net.WebSockets.Managed.ClientWebSocket managed)
                            {
                                managed.Options.KeepAliveInterval = TimeSpan.FromSeconds(0);
                                await managed.ConnectAsync(uri, cancellationToken);
                            }
                            else
                            {
                                ClientWebSocket coreSocket = client as ClientWebSocket;
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
                #endregion

                #region 设置 Args 参数
                string[] argss = default(string[]);
                if (args != null && args.Length > 0)
                {
                    List<string> argsList = args.ToList<string>();
                    argsList.Add("--no-sandbox");
                    argss = argsList.ToArray();
                }
                else
                {
                    argss = new string[] { "--no-sandbox" };
                }
                launchOptions.Args = argss; //这些参数将会传递给 Chromium
                #endregion

                #region 设置 IgnoredDefaultArgs 参数
                string[] defaultArgs = default(string[]);
                if (ignoredDefaultArgs != null && ignoredDefaultArgs.Length > 0)
                {
                    List<string> ignoredDefaultArgsList = ignoredDefaultArgs.ToList<string>();
                    ignoredDefaultArgsList.Add("--enable-automation");
                    defaultArgs = ignoredDefaultArgsList.ToArray();
                }
                else
                {
                    defaultArgs = new string[] { "--enable-automation" };
                }
                launchOptions.IgnoredDefaultArgs = defaultArgs; //这些参数将被 Chromium 忽略
                #endregion

                launchOptions.Headless = !isDisplay; // Headless : true 是无头模式，无界面；false，有界面
                return launchOptions;
            });
        }


        /// <summary>
        /// <para>获取一个 Chromium 浏览器 启动选项 的对象，并返回这个对象；此方法兼容 Windows7 / Windows Server 2008</para>
        /// <para>检查不下载</para>
        /// <para>Chromium 运行时显示界面</para>
        /// <para>自动传入 "--no-sandbox" 参数</para>
        /// <para>自动过滤掉 "--enable-automation" 参数</para>
        /// <para>△ 异常可能：程序运行目录下 Chromium 浏览器不可用。</para>
        /// </summary>
        /// <returns></returns>
        public static async Task<LaunchOptions> ChromiumLaunchOptions()
        {
            return await SetChromiumLaunchOptions(false, true, null, null);
        }

        /// <summary>
        /// <para>获取一个 Chromium 浏览器 启动选项 的对象，并返回这个对象；此方法兼容 Windows7 / Windows Server 2008</para>
        /// <para>△ 异常可能：程序运行目录下 Chromium 浏览器不可用。</para>
        /// </summary>
        /// <param name="args">要传递给 Chromium 浏览器实例的其他参数。（ 此方法会自动传入 "--no-sandbox" 参数 ）</param>
        /// <param name="ignoredDefaultArgs">如果给出数组，则过滤掉给定的 Puppeteer.DefaultArgs 默认参数。
        /// （ 此方法会自动设置 "--enable-automation" 过滤掉这个参数 ）
        /// </param>
        /// <returns></returns>
        public static async Task<LaunchOptions> ChromiumLaunchOptions(string[] args,
            string[] ignoredDefaultArgs = null)
        {
            return await SetChromiumLaunchOptions(false, true, args, ignoredDefaultArgs);
        }

        /// <summary>
        /// <para>获取一个 Chromium 浏览器 启动选项 的对象，并返回这个对象；此方法兼容 Windows7 / Windows Server 2008</para>
        /// <para>△ 异常可能：程序运行目录下 Chromium 浏览器不可用。</para>
        /// </summary>
        /// <param name="isDisplay">Chromium 运行时 是否显示界面</param>
        /// <param name="args">要传递给 Chromium 浏览器实例的其他参数。（ 此方法会自动传入 "--no-sandbox" 参数 ）</param>
        /// <param name="ignoredDefaultArgs">如果给出数组，则过滤掉给定的 Puppeteer.DefaultArgs 默认参数。
        /// （ 此方法会自动设置 "--enable-automation" 过滤掉这个参数 ）
        /// </param>
        /// <returns></returns>
        public static async Task<LaunchOptions> ChromiumLaunchOptions(bool isDisplay,
            string[] args = null, string[] ignoredDefaultArgs = null)
        {
            return await SetChromiumLaunchOptions(false, isDisplay, args, ignoredDefaultArgs);
        }

        /// <summary>
        /// <para>获取一个 Chromium 浏览器 启动选项 的对象，并返回这个对象；此方法兼容 Windows7 / Windows Server 2008</para>
        /// <para>△ 异常可能：程序运行目录下 Chromium 浏览器不可用。</para>
        /// </summary>
        /// <param name="checkIsDownload">检查 是否下载 Chromium 浏览器</param>
        /// <param name="isDisplay">Chromium 运行时 是否显示界面</param>
        /// <param name="args">要传递给 Chromium 浏览器实例的其他参数。（ 此方法会自动传入 "--no-sandbox" 参数 ）</param>
        /// <param name="ignoredDefaultArgs">如果给出数组，则过滤掉给定的 Puppeteer.DefaultArgs 默认参数。
        /// （ 此方法会自动设置 "--enable-automation" 过滤掉这个参数 ）
        /// </param>
        /// <returns></returns>
        public static async Task<LaunchOptions> ChromiumLaunchOptions(bool checkIsDownload, bool isDisplay,
            string[] args = null, string[] ignoredDefaultArgs = null)
        {
            return await SetChromiumLaunchOptions(checkIsDownload, isDisplay, args, ignoredDefaultArgs);
        }



        /// <summary>
        /// 新建一个 Page（页面）并且初始化后再返回当前 Page 对象（页面）【避免js检测出 当前客户行为是无头浏览器自动化程序】
        /// </summary>
        /// <param name="browser"></param>
        /// <returns></returns>
        public static async Task<Page> NewPageAndInitAsync(Browser browser)
        {
            if (browser == null)
            {
                throw new Exception("传入了一个空的 Chromium 浏览器对象。Browser == null");
            }

            Page page = default(Page);
            Page[] pages = await browser.PagesAsync();
            if (pages != null && pages.Length == 1 && pages[0].Url == "about:blank")
            {
                page = pages[0];
            }
            else
            {
                page = await browser.NewPageAsync();
            }

            #region 定义浏览器页面属性（这里是为了绕过反爬虫的js检测）
            await page.SetUserAgentAsync("Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36");

            string navigator_languages = @"
            () => {
                Object.defineProperty(navigator, 'languages', {
                    get: () => ['zh-CN', 'en-US', 'en'],
                });
            }";

            string navigator_webdriver = @"
            () => {
                Object.defineProperty(navigator, 'webdriver', {
                    get: () => false,
                });
            }";

            string navigator_connection_rtt = @"
            () => {
                Object.defineProperty(navigator.connection, 'rtt', {
                    get: () => 50,
                });
            }";

            string navigator_plugins = @"
            () => {
                Object.defineProperty(navigator, 'plugins', {
                    get: () => {
                        var ChromiumPDFPlugin = {};
                        ChromiumPDFPlugin.__proto__ = Plugin.prototype;
                        var plugins = {
                            0: ChromiumPDFPlugin,
                            description: 'Portable Document Format',
                            filename: 'internal-pdf-viewer',
                            length: 1,
                            name: 'Chromium PDF Plugin',
                            __proto__: PluginArray.prototype,
                        };
                        return plugins;
                    },
                });
            }";

            string chrome = @"
            () => {
                chrome = { 'app': { 'isInstalled': false }, 'webstore': { 'onInstallStageChanged': {}, 'onDownloadProgress': {} }, 'runtime': { 'PlatformOs': { 'MAC': 'mac', 'WIN': 'win', 'ANDROID': 'android', 'CROS': 'cros', 'LINUX': 'linux', 'OPENBSD': 'openbsd' }, 'PlatformArch': { 'ARM': 'arm', 'X86_32': 'x86-32', 'X86_64': 'x86-64' }, 'PlatformNaclArch': { 'ARM': 'arm', 'X86_32': 'x86-32', 'X86_64': 'x86-64' }, 'RequestUpdateCheckStatus': { 'THROTTLED': 'throttled', 'NO_UPDATE': 'no_update', 'UPDATE_AVAILABLE': 'update_available' }, 'OnInstalledReason': { 'INSTALL': 'install', 'UPDATE': 'update', 'CHROME_UPDATE': 'chrome_update', 'SHARED_MODULE_UPDATE': 'shared_module_update' }, 'OnRestartRequiredReason': { 'APP_UPDATE': 'app_update', 'OS_UPDATE': 'os_update', 'PERIODIC': 'periodic' } } };
            }";

            string window_chrome = @"
            () => {
                window.chrome = { 'app': { 'isInstalled': false }, 'webstore': { 'onInstallStageChanged': {}, 'onDownloadProgress': {} }, 'runtime': { 'PlatformOs': { 'MAC': 'mac', 'WIN': 'win', 'ANDROID': 'android', 'CROS': 'cros', 'LINUX': 'linux', 'OPENBSD': 'openbsd' }, 'PlatformArch': { 'ARM': 'arm', 'X86_32': 'x86-32', 'X86_64': 'x86-64' }, 'PlatformNaclArch': { 'ARM': 'arm', 'X86_32': 'x86-32', 'X86_64': 'x86-64' }, 'RequestUpdateCheckStatus': { 'THROTTLED': 'throttled', 'NO_UPDATE': 'no_update', 'UPDATE_AVAILABLE': 'update_available' }, 'OnInstalledReason': { 'INSTALL': 'install', 'UPDATE': 'update', 'CHROME_UPDATE': 'chrome_update', 'SHARED_MODULE_UPDATE': 'shared_module_update' }, 'OnRestartRequiredReason': { 'APP_UPDATE': 'app_update', 'OS_UPDATE': 'os_update', 'PERIODIC': 'periodic' } } };
            }";

            string window_navigator_chrome = @"
            () => {
                window.navigator.chrome = { 'app': { 'isInstalled': false }, 'webstore': { 'onInstallStageChanged': {}, 'onDownloadProgress': {} }, 'runtime': { 'PlatformOs': { 'MAC': 'mac', 'WIN': 'win', 'ANDROID': 'android', 'CROS': 'cros', 'LINUX': 'linux', 'OPENBSD': 'openbsd' }, 'PlatformArch': { 'ARM': 'arm', 'X86_32': 'x86-32', 'X86_64': 'x86-64' }, 'PlatformNaclArch': { 'ARM': 'arm', 'X86_32': 'x86-32', 'X86_64': 'x86-64' }, 'RequestUpdateCheckStatus': { 'THROTTLED': 'throttled', 'NO_UPDATE': 'no_update', 'UPDATE_AVAILABLE': 'update_available' }, 'OnInstalledReason': { 'INSTALL': 'install', 'UPDATE': 'update', 'CHROME_UPDATE': 'chrome_update', 'SHARED_MODULE_UPDATE': 'shared_module_update' }, 'OnRestartRequiredReason': { 'APP_UPDATE': 'app_update', 'OS_UPDATE': 'os_update', 'PERIODIC': 'periodic' } } };
            }";

            string window_navigator_permissions_query = @"
            () => {
                const originalQuery = window.navigator.permissions.query;
                return window.navigator.permissions.query = (parameters) => (
                    parameters.name === 'notifications' ? Promise.resolve({ state: Notification.permission }) : originalQuery(parameters)
                );
            }";

            string webGLRenderingContext_getParameter = @"
            () => {
                const getParameter = WebGLRenderingContext.getParameter;
                WebGLRenderingContext.prototype.getParameter = function(parameter) {
                    // UNMASKED_VENDOR_WEBGL
                    if (parameter === 37445) {
                      return 'Intel Open Source Technology Center';
                    }
                    // UNMASKED_RENDERER_WEBGL
                    if (parameter === 37446) {
                      return 'Mesa DRI Intel(R) Ivybridge Mobile';
                    }
                    return getParameter(parameter);
                };
            }";

            await page.EvaluateOnNewDocumentAsync(navigator_languages);
            await page.EvaluateOnNewDocumentAsync(navigator_webdriver);
            await page.EvaluateOnNewDocumentAsync(navigator_connection_rtt);
            await page.EvaluateOnNewDocumentAsync(navigator_plugins);
            await page.EvaluateOnNewDocumentAsync(chrome);
            await page.EvaluateOnNewDocumentAsync(window_chrome);
            await page.EvaluateOnNewDocumentAsync(window_navigator_chrome);
            await page.EvaluateOnNewDocumentAsync(window_navigator_permissions_query);
            await page.EvaluateOnNewDocumentAsync(webGLRenderingContext_getParameter);
            #endregion

            await page.SetViewportAsync(new ViewPortOptions
            {
                Width = 820,
                Height = 820
            });

            return page;
        }



        /// <summary>
        /// 保存 Page 页面截图
        /// </summary>
        /// <param name="page">Chromium 的 Page 对象</param>
        /// <param name="isFullPage">true 获取整个可滚动页面的屏幕截图；false 获取页面可见部分的屏幕截图。默认值：false</param>
        /// <param name="path">保存截图的绝对路径 【 默认值：程序运行目录\SaveContent\{当前时间}.png 】</param>
        /// <returns></returns>
        public static async Task SavePageScreenshotAsync(Page page,
            bool isFullPage = false, string path = null)
        {
            if (page == null)
            {
                throw new Exception("传入了一个空的 Chromium Page 对象。Page == null");
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                path = SaveContent.SaveContentDirectory + $"{DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss.ffff")}.png";
            }
            else
            {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }

                if (Path.GetExtension(path).ToLower() != ".png")
                {
                    path += ".png";
                }
            }

            await page.ScreenshotAsync(path, new ScreenshotOptions() { FullPage = isFullPage });
        }

        /// <summary>
        /// 保存 Page 页面截图
        /// </summary>
        /// <param name="page">Chromium 的 Page 对象</param>
        /// <param name="screenshotOptions">ScreenshotOptions 截图选项 【 如果传入空值，则自动实例化一个 ScreenshotOptions 对象 】</param>
        /// <param name="path">保存截图的绝对路径 【 默认值：程序运行目录\SaveContent\{当前时间}.png 】</param>
        /// <returns></returns>
        public static async Task SavePageScreenshotAsync(Page page, ScreenshotOptions screenshotOptions,
            string path = null)
        {
            if (page == null)
            {
                throw new Exception("传入了一个空的 Chromium Page 对象。Page == null");
            }

            if (screenshotOptions == null)
            {
                screenshotOptions = new ScreenshotOptions();
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                path = SaveContent.SaveContentDirectory + $"{DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss.ffff")}.png";
            }
            else
            {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }

                if (Path.GetExtension(path).ToLower() != ".png")
                {
                    path += ".png";
                }
            }

            await page.ScreenshotAsync(path, screenshotOptions);
        }


    }
}
