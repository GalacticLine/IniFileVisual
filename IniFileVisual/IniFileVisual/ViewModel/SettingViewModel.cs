using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using IniFileVisual.Model;

namespace IniFileVisual.ViewModel
{
    public class SettingViewModel : ViewModelBase
    {
        #region  Private Fields
        private SimpleText selectedItem;
        private RelayCommand setAll;
        private RelayCommand reSetAll;
        #endregion

        #region Public Properties
        public EditorModel Editor { get; set; } = EditorModel.Instance;

        public ParserModel Parser { get; set; } = ParserModel.Instance;

        public SimpleText SelectedItem
        {
            get => selectedItem;
            set => Set(nameof(selectedItem), ref selectedItem, value);
        }
        #endregion

        #region Public Commands
        //设置所有配置
        public RelayCommand SetAll => setAll ??= new RelayCommand(() =>
        {
            try
            {
                Editor.Write();
                Parser.Write();
                Growl.Success("修改配置成功！");
            }
            catch
            {
                Growl.Error("修改配置失败！");
            }
        });

        //重置所有配置
        public RelayCommand ReSetAll => reSetAll ??= new RelayCommand(() =>
        {
            try
            {
                Editor.Read();
                Parser.Read();
            }
            catch
            {
                Growl.Error("配置重置失败！");
            }
        });
        #endregion
    }

}
