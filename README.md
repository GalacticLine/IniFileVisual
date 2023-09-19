# IniFileVisual

IniFileVisual 是我当初学习 C# 时所编写的 Ini 文件编辑器。它基于 HandyControl 的 UI 框架开发，使用 Avalonedit 作为文件编辑器内核，使用 ini-parser 对ini文件进行处理。

## 如何编译该项目
* 添加对 **Avalonedit** 项目的引用。项目地址： https://github.com/GalacticLine/AvalonEdit.git
  > 注：该项目不是原版的Avalonedit，是我在原项目基础上进行微小修改后的分支。 原版：https://github.com/icsharpcode/AvalonEdit

* 添加以下 **NuGet** 程序包：
  * **CommonServiceLocator** (版本2.0.7) GitHub: https://github.com/unitycontainer/commonservicelocator
  * **Dirkster.AvalonDock** (版本4.72.1) GitHub: https://github.com/Dirkster99/AvalonDock
  * **HandyControl** (版本3.4.0) GitHub: https://github.com/HandyOrg/HandyControl
  * **ini-parser-netstandard** (版本2.5.2) GitHub: https://github.com/rickyah/ini-parser
  * **Microsoft.Xaml.Behaviors.Wpf** (版本1.1.39) 
  * **MvvmLight** (版本5.4.1.1) Github: https://github.com/lbugnion/mvvmlight
  * **MvvmLightLibs** (版本5.4.1.1)

添加完成之后，您可以在 VS Studio 中正常编译该项目。

## 注意事项
** 项目目标框架为 **netcoreapp3.1**

## 演示
![IniFileVisual](https://github.com/GalacticLine/IniFileVisual/assets/128875843/797a7f36-00d5-46d4-965e-0a8f8bc7b9f7)
