using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace IniFileVisual.Views
{
    /// <summary>
    /// Thank.xaml 的交互逻辑
    /// </summary>
    public partial class Thank : UserControl
    {
        public Thank()
        {
            InitializeComponent();
        }

        //单机超链接跳转
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(((Hyperlink)sender).NavigateUri.ToString())
                {
                    UseShellExecute = true
                });
            }
            catch
            {

            }
        }

    }
}
