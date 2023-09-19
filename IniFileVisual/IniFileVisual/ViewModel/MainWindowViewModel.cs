using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ICSharpCode.AvalonEdit;

namespace IniFileVisual.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region  Private Fields
        private RelayCommand exit;
        private RelayCommand<object> windowShowDialog;
        private RelayCommand<Window> closeWindow;
        private RelayCommand<MenuItem> openMenuItemSubmenu;
        #endregion

        #region Public Commands
        //关闭主程序
        public RelayCommand Exit => exit ??= new RelayCommand(() =>
        {
            System.Windows.Application.Current.Shutdown();
        });

        //打开指定窗口
        public RelayCommand<object> WindowShowDialog => windowShowDialog ??= new RelayCommand<object>((obj) =>
        {
            if (Activator.CreateInstance(obj as Type) is Window window)
            {
                window.ShowDialog();
            }
        });

        //关闭指定窗口
        public RelayCommand<Window> CloseWindow => closeWindow ??= new RelayCommand<Window>((window) =>
        {
            window.Close();
            GC.Collect();
        });

        //根据键盘输入打开MenuItem
        public RelayCommand<MenuItem> OpenMenuItemSubmenu => openMenuItemSubmenu ??= new RelayCommand<MenuItem>((item) =>
        {
            if (Keyboard.FocusedElement is TextBoxBase || Keyboard.FocusedElement is ITextEditorComponent)
            {
                return;
            }
            if (item != null)
            {
                item.IsSubmenuOpen = true;
            }
        });
        #endregion

    }

}
