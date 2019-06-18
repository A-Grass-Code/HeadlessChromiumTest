# HeadlessChromiumTest
Chromium 无头浏览器测试 Demo

说明：

1. 开发（完成）平台：**Windows 10 x64 / Windows 7 x64 / Windows Server 2008 R2 x64**；

   **.NET Framework 4.7** ；**VS 2017**。（ 其他环境未测试 ）

2. 参考：

   ① https://www.cnblogs.com/TTonly/p/10920294.html?tdsourcetag=s_pcqq_aiomsg

   ② https://www.cnblogs.com/shanyou/archive/2019/03/09/10500049.html?tdsourcetag=s_pcqq_aiomsg

   ③ https://www.cnblogs.com/VAllen/p/9234675.html?tdsourcetag=s_pcqq_aiomsg

   ④ https://github.com/kblok/puppeteer-sharp 【puppeteer-sharp GitHub官网】

   ⑤ http://www.puppeteersharp.com/api/index.html 【puppeteer-sharp 官网API】

   ⑥ https://github.com/kblok/puppeteer-sharp/issues/1162 【兼容 Win7 环境下启动 Chromium】

   ⑦ https://zhaoqize.github.io/puppeteer-api-zh_CN/#?product=Puppeteer&version=v1.17.0&show=api-class-puppeteer 【puppeteer node.js API】

3. 可能会遇到 Chromium 无头浏览器 下载不动的情况，不用慌。将 **`chromium-Win64-662092.zip`** 这个压缩包解压后，把里面的 **`.local-chromium`** 文件夹，整体复制到程序的运行目录下（ 也就是 **`bin\Debug`** 或 **`bin\Release`** ）。

   - **`chromium-Win64-662092.zip`** ***百度云盘分享地址***  如下：
- 链接：https://pan.baidu.com/s/156G-3tC9hohuBM8TgTeuWQ
   - 提取码：l83n

4. 从 **GitHub** 上获取代码后，可用 **VS 2017** 直接打开；还原 **NuGet** 包，并重新生成 **解决方案** 或 **项目**，即可调试运行。