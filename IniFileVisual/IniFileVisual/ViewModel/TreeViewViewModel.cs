using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace IniFileVisual.ViewModel
{
    public class TreeViewViewModel : ViewModelBase
    {
        #region Private Fields
        private Control selectedItem;
        private RelayCommand<Control> setSelectedItem;
        private RelayCommand copyToolTip;
        private RelayCommand showAtExplorer;
        #endregion

        public Control SelectedItem
        {
            get => selectedItem;
            set => Set(nameof(SelectedItem), ref selectedItem, value);
        }

        public RelayCommand<Control> SetSelectedItem => setSelectedItem ??= new RelayCommand<Control>((value) =>
        {
            selectedItem = value;
        });

        //复制文件路径
        public RelayCommand CopyToolTip => copyToolTip ??= new RelayCommand(() =>
        {
            if (HasSelected())
            {
                Clipboard.SetText(selectedItem.ToolTip.ToString());
            }
          
        });

        //打开文件所在目录
        public RelayCommand ShowAtExplorer => showAtExplorer ??= new RelayCommand(() =>
        {
            if (HasSelected())
            {
                Process.Start("explorer.exe", "/select," + selectedItem.ToolTip.ToString());
            }
        });

        //是否有选中的子项
        private bool HasSelected()
        {
            return SelectedItem != null;
        }

    }
}
