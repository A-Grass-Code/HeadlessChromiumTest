# HeadlessChromiumTest
**Chromium 无头浏览器测试 Demo**

### 说明：（ 请参考最新的 *更新 Log* ）

1. 开发（完成）平台：**PuppeteerSharp 1.17.2**；**Windows 10 x64 / Windows 7 x64 / Windows Server 2008 R2 x64**；**.NET Framework 4.7** ；**VS 2017**。（ 其他环境未测试 ）

2. 参考：

   ① https://www.cnblogs.com/TTonly/p/10920294.html?tdsourcetag=s_pcqq_aiomsg

   ② https://www.cnblogs.com/shanyou/archive/2019/03/09/10500049.html?tdsourcetag=s_pcqq_aiomsg

   ③ https://www.cnblogs.com/VAllen/p/9234675.html?tdsourcetag=s_pcqq_aiomsg

   ④ https://github.com/kblok/puppeteer-sharp 【puppeteer-sharp GitHub官网】

   ⑤ http://www.puppeteersharp.com/api/index.html 【puppeteer-sharp 官网API】

   ⑥ https://github.com/kblok/puppeteer-sharp/issues/1162 【兼容 Win7 环境下启动 Chromium】

   ⑦ https://zhaoqize.github.io/puppeteer-api-zh_CN/ 【puppeteer node.js API】

3. 可能会遇到 Chromium 无头浏览器 下载不动的情况，不用慌。将 **`chromium-Win64-662092.zip`** 这个压缩包解压后，把里面的 **`.local-chromium`** 文件夹，整体复制到程序的运行目录下（ 也就是 **`bin\Debug`** 或 **`bin\Release`** ）。

   - **`chromium-Win64-662092.zip`** ***百度云盘分享地址***  如下：
   - 链接：https://pan.baidu.com/s/156G-3tC9hohuBM8TgTeuWQ
   - 提取码：l83n
   
4. 从 **GitHub** 上获取代码后，可用 **VS 2017** 直接打开；还原 **NuGet** 包，并重新生成 **解决方案** 或 **项目**，即可调试运行。





### ***更新 Log* :**



##### ***2021-04-27  ==***

1. 更新时环境：Windows 10 x64，VS 2019，.NET Framework 4.8；
2. `PuppeteerSharp` 更新至 `NuGet` 上的最新稳定版 `4.0.0` ；
3. 优化已有的代码；
4. `PuppeteerSharp 4.0.0` 对应的 Chromium 浏览器
   - **`chromium-Win64-848005.zip`** ***百度云盘分享地址***  如下：
   - 链接：https://pan.baidu.com/s/1CSlM7kep6RxIDa89zjj7jA
   - 提取码：ujq1
5. 注意：之前的更新Log可能并不适用于本次更新。



##### ***2020-03-24  ==***

1. `PuppeteerSharp` 更新至 `NuGet` 上的最新稳定版 `2.0.2` ；

2. 对应的 `chromium` 更新：

   - **`chromium-Win64-706915`** ***百度云盘分享地址***  如下：
   - 链接：https://pan.baidu.com/s/1FcmtNQbztADcCa0VXwnpZg
   - 提取码：yj42

3. 更新参考：https://www.nuget.org/packages/PuppeteerSharp/

4. 说明一下，这次的代码更新，是在 **Windows 10 x64** 系统、 **VS 2019** 中完成的！

5. 代码中增加 chromium 下载地址解析注释；

   因为下载过程中可能 ***网络不行*** 导致卡死，所以研究了源代码的下载地址；

   下面是在研究后的整理，在代码中已注释

   ```c#
   #region 下载地址 解析
       // 参考于源代码：https://github.com/hardkoded/puppeteer-sharp/blob/37ea56934281209830254df3ec3ffe37c57cfac2/lib/PuppeteerSharp/BrowserFetcher.cs
   
       // https://storage.googleapis.com/chromium-browser-snapshots/Win_x64/706915/chrome-win.zip 下载地址（ 样例 ）
   
       // const string DefaultDownloadHost = "https://storage.googleapis.com";
       // const int DefaultRevision = 706915;
   
       // [Platform.Linux] = "{0}/chromium-browser-snapshots/Linux_x64/{1}/{2}.zip",
       // [Platform.MacOS] = "{0}/chromium-browser-snapshots/Mac/{1}/{2}.zip",
       // [Platform.Win32] = "{0}/chromium-browser-snapshots/Win/{1}/{2}.zip",
       // [Platform.Win64] = "{0}/chromium-browser-snapshots/Win_x64/{1}/{2}.zip"
   
       // case Platform.Linux:
       //     return "chrome-linux";
       // case Platform.MacOS:
       //     return "chrome-mac";
       // case Platform.Win32:
       // case Platform.Win64:
       //     return revision > 591479 ? "chrome-win" : "chrome-win32";
   #endregion
   ```

   下载后得到的压缩包，解压，然后按照下图创建目录，之后的你应该已经会了......

   截图链接： https://share.weiyun.com/5yemhoI 

6. 因为已经不再做这个相关的项目了，所以更新的很不及时，后面可能会更不及时！哈哈 ^_^ ...

   其实这个 Demo 就是想分享给大家，也算是指一条路去走吧，至于走不走，走成什么样，那就看你的喽 ......



##### ***2019-07-21  ==***

1. `PuppeteerSharp` 更新至 `NuGet` 上的最新稳定版 `1.18.0` ；
2. 对应的 `chromium` 更新：
   - **`chromium-Win64-672088.zip`** ***百度云盘分享地址***  如下：
   - 链接：https://pan.baidu.com/s/1xoCEwmJf-6HcJfdaVm2ntQ
   - 提取码：q2ei
3. 更新参考：https://www.nuget.org/packages/PuppeteerSharp/

