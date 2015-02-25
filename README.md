#WGestures
WGestures鼠标手势 for Windows.
简单易用的全局鼠标手势, 支持Windows 7/8/10。官方网站 [www.yingDev.com/projects/WGestures](http://www.yingdev.com/projects/wgestures) <br/>
<br/>
![Alt text](http://ww1.sinaimg.cn/mw690/8cc88963gw1ekaujcoaf5g20a006yq7f.gif) <br/>
<br/>
by Ying Yuandong. 有问题请联系我: Ying@YingDev.com

_______________________
##如果你想改进WGestures...  
###环境
Visual Studio 2013, .Net Framework 3.5 SDK

###Build
首次Build请先运行 `/makeTestCert.bat` 并按提示生成测试证书（根证书和签名证书分别自动拷贝到了  `/WGestures.App/cert/YingDevCA.cer` 和 `/YingDevSPC.pfx` ), 该证书会在BuildEvent中用于对生成的exe进行签名。

###Todo
 * 单元测试
 * 解决内存占用偏高
 * ~~手势列表条目拖拽排序~~
 * ~~使用单独的AppDomain来完成IO和配置, 从而减小程序集占用的内存~~(放弃)
