#WGestures
WGestures鼠标手势 for Windows.
一个更简单, 更现代的全局鼠标手势, 支持Windows 7/8/10。
官方网站 [www.yingDev.com/projects/wgestures](http://www.yingdev.com/projects/wgestures) 

![Alt text](http://ww1.sinaimg.cn/mw690/8cc88963gw1ekaujcoaf5g20a006yq7f.gif) 

![Alt text](http://ww3.sinaimg.cn/large/8cc88963gw1epn68m6qfsg20a006yacc.gif) 

by Ying Yuandong. 有问题请联系我: Ying@YingDev.com

_______________________
##如果你想改进WGestures...  
###环境
* Visual Studio 2013, .Net Framework 3.5 SDK
* WGestures.Install 项目(安装程序)需要Wix: http://wixtoolset.org/releases/

###Build
首次Build请先运行 `/makeTestCert.bat` 并按提示生成测试证书（根证书和签名证书分别自动拷贝到了  `/WGestures.App/cert/YingDevCA.cer` 和 `/YingDevSPC.pfx` ), 该证书会在BuildEvent中用于对生成的exe进行签名。要正确运行此脚本,请打开"VS开发人员命令提示" > cd `WGestures所在目录` > makeTestCert.bat
