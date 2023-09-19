using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace IniFileVisual.ViewModel
{
    public class ViewModelLocator
    {
        #region  Public Properties
        public SettingViewModel Setting => ServiceLocator.Current.GetInstance<SettingViewModel>();
        public IniTextEditorViewModel Tab => ServiceLocator.Current.GetInstance<IniTextEditorViewModel>();
        public MainWindowViewModel Main => ServiceLocator.Current.GetInstance<MainWindowViewModel>();
        public TreeViewViewModel Tree => ServiceLocator.Current.GetInstance<TreeViewViewModel>();
        public DockingManagerViewModel Docking => ServiceLocator.Current.GetInstance<DockingManagerViewModel>();
        public ToolViewModel Tool => ServiceLocator.Current.GetInstance<ToolViewModel>();
        #endregion

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            RegisterViewModels(SimpleIoc.Default);
        }

        public static void Cleanup()
        {
            SimpleIoc.Default.Reset();
        }

        //注册ViewModels
        private void RegisterViewModels(SimpleIoc ioc)
        {
            ioc.Register<SettingViewModel>();
            ioc.Register<IniTextEditorViewModel>();
            ioc.Register<MainWindowViewModel>();
            ioc.Register<TreeViewViewModel>();
            ioc.Register<DockingManagerViewModel>();
            ioc.Register<ToolViewModel>();
        }
    }
}