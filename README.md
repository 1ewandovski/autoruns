# 环境说明

我使用的操作系统是`windows 10`，基于`.NET Framework 4.6.1`开发，程序需要以管理员权限运行。

我没有将整个程序及其所有依赖打包成`exe`，编译生成的`exe`文件路径为`autoruns/WpfApp1.exe`，点击即可运行。

使用`visual studio`打开`WpfApp1.sln`就是整个项目源代码。

#使用说明

点击`Logon` `services` `drivers` `tasks` `InternetExplorer` `Winlogon`可以分别查看对应的自启动项

点击`FilterEmpty`之后再点击那几个自启动项的按钮后，会刷新是否过滤掉空键

点击`FilterWindows`之后再点击`services`后，会刷新是否过滤掉发布者`windows`的服务，对于driver的过滤由于未能实现识别windows签名就没有做

默认进去是过滤掉空键和发布者为windows的记录的

点击一条记录，将鼠标移至窗口外再移到这条记录上，会提示这条记录的完整信息

# 作者

姚逸洲 邮箱 yaoyizhou0620@sjtu.edu.cn



