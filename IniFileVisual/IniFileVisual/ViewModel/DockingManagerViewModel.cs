using System;
using System.IO;
using AvalonDock;
using AvalonDock.Layout;
using AvalonDock.Layout.Serialization;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;


namespace IniFileVisual.ViewModel
{
    public class DockingManagerViewModel : ViewModelBase
    {
        #region Private Fields
        private DockingManager manager;
        private RelayCommand<string> readLayout;
        private RelayCommand<string> writeLayout;
        private RelayCommand<LayoutAnchorable> layoutToggle;
        private RelayCommand<DockingManager> setManager;
        #endregion

        #region Public Commands
        //设置 DockingManager
        public RelayCommand<DockingManager> SetManager => setManager ??= new RelayCommand<DockingManager>((m) =>
        {
            manager = m;
        });

        //读取布局文件
        public RelayCommand<string> ReadLayout => readLayout ??= new RelayCommand<string>((file) =>
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + @"Resources\Layout\" + file;
                using StreamReader stream = new StreamReader(path);
                new XmlLayoutSerializer(manager).Deserialize(stream);
                Growl.Success($"读取{file}成功!");
            }
            catch
            {
                Growl.Error($"读取{file}失败!");
            }
        });

        //写入布局文件
        public RelayCommand<string> WriteLayout => writeLayout ??= new RelayCommand<string>((file) =>
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + @"Resources\Layout\" + file;
                using StreamWriter stream = new StreamWriter(path);
                new XmlLayoutSerializer(manager).Serialize(stream);
                Growl.Success($"写入{file}成功!");
            }
            catch
            {
                Growl.Error($"写入{file}失败!");
            }
        });

        //Layout显示隐藏开关
        public RelayCommand<LayoutAnchorable> LayoutToggle => layoutToggle ??= new RelayCommand<LayoutAnchorable>((anchorable) =>
        {
            if (anchorable.IsHidden)
            {
                anchorable.Show();
            }
            else
            {
                anchorable.Hide();
            }
        });
        #endregion
    }

}


