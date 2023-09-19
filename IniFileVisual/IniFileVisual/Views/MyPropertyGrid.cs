using System.Windows;
using HandyControl.Controls;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Collections.Generic;
using System.Reflection;
using System.Drawing;


namespace IniFileVisual.Views
{
    //编码类属性编辑器
    internal class EncodingPropertyEditor : PropertyEditorBase
    {  
        public override FrameworkElement CreateElement(PropertyItem propertyItem)
        {
            return new System.Windows.Controls.ComboBox
            {
                IsEnabled = !propertyItem.IsReadOnly,
                ItemsSource = Encoding.GetEncodings().Select(x => x.Name)
            };
        }

        public override DependencyProperty GetDependencyProperty()
        {
            return Selector.SelectedValueProperty;
        }
    }

    //高亮文件属性编辑器
    internal class HighlightPropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem)
        {
            return new System.Windows.Controls.ComboBox
            {
                IsEnabled = !propertyItem.IsReadOnly,
                ItemsSource = GetHighlightResources()
            };
        }

        public override DependencyProperty GetDependencyProperty()
        {
            return Selector.SelectedValueProperty;
        }

        public IEnumerable<string> GetHighlightResources()
        {
            string resource = "XshdSyntaxDefinition.";
            foreach (string item in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {

                if (item.Contains(resource))
                {
                    yield return item.Split(resource)[1];
                }
            }
        }
    }

    //字体属性编辑器
    internal class FontPropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem)
        {
            return new System.Windows.Controls.ComboBox
            {
                IsEnabled = !propertyItem.IsReadOnly,
                ItemsSource = FontFamily.Families.Select(x => x.Name)
            };
        }

        public override DependencyProperty GetDependencyProperty()
        {
            return Selector.SelectedValueProperty;
        }
    }

}