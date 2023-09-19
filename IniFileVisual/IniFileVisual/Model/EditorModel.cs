using System.ComponentModel;
using HandyControl.Controls;
using IniFileVisual.Views;

namespace IniFileVisual.Model
{
    //编辑器配置
    public class EditorModel : ConfigModel
    {
        #region Private Fields
        private static EditorModel instance;
        private string font;
        private double fontSize;
        private bool showSpaces;
        private bool showLineNumbers;
        private bool showEndOfLine;
        private bool showTabs;
        private bool allowScrollBelowDocument;
        private bool allowToggleOverstrikeMode;
        private bool cutCopyWholeLine;
        private bool convertTabsToSpaces;
        private bool enableEmailHyperlinks;
        private bool enableHyperlinks;
        private bool enableImeSupport;
        private bool enableTextDragDrop;
        private bool enableVirtualSpace;
        private bool highlightCurrentLine;
        private int indentationSize;
        private bool hideCursorWhileTyping;
        private bool inheritWordWrapIndentation;
        private bool requireControlModifierForHyperlinkClick;
        private bool showColumnRuler;
        private string hightLightFile;
        private double lineSpacing;
        private string encodingName;
        private bool useFolding;
        #endregion

        #region Public Properties
        [DisplayName("字体")]
        [Editor(typeof(FontPropertyEditor), typeof(PropertyEditorBase))]
        public string Font
        {
            get => font;
            set => Set(nameof(Font), ref font, value);
        }

        [DisplayName("默认字体大小(像素)")]
        public double FontSize
        {
            get => fontSize;
            set => Set(nameof(FontSize), ref fontSize, (value < 1) ? 1 : (value > 100) ? 100 : value);
        }

        [DisplayName("是否显示空格符")]
        public bool ShowSpaces
        {
            get => showSpaces;
            set => Set(nameof(ShowSpaces), ref showSpaces, value);
        }

        [DisplayName("是否显示行号")]
        public bool ShowLineNumbers
        {
            get => showLineNumbers;
            set => Set(nameof(ShowLineNumbers), ref showLineNumbers, value);
        }

        [DisplayName("是否显示换行符")]
        public bool ShowEndOfLine
        {
            get => showEndOfLine;
            set => Set(nameof(ShowEndOfLine), ref showEndOfLine, value);
        }

        [DisplayName("是否显示制表符")]
        public bool ShowTabs
        {
            get => showTabs;
            set => Set(nameof(ShowTabs), ref showTabs, value);
        }

        [DisplayName("是否允许文档滚动到底部以下")]
        [Description("默认值为False，使用折叠时，最好将此属性设置为True.")]
        public bool AllowScrollBelowDocument
        {
            get => allowScrollBelowDocument;
            set => Set(nameof(AllowScrollBelowDocument), ref allowScrollBelowDocument, value);
        }

        [DisplayName("是否允许切换到插入模式")]
        [Description("控制是否可以使用键盘上的Insert键。")]
        public bool AllowToggleOverstrikeMode
        {
            get => allowToggleOverstrikeMode;
            set => Set(nameof(AllowToggleOverstrikeMode), ref allowToggleOverstrikeMode, value);
        }

        [DisplayName("是否剪切/复制整行")]
        public bool CutCopyWholeLine
        {
            get => cutCopyWholeLine;
            set => Set(nameof(CutCopyWholeLine), ref cutCopyWholeLine, value);
        }

        [DisplayName("制表符换成空格")]
        public bool ConvertTabsToSpaces
        {
            get => convertTabsToSpaces;
            set => Set(nameof(ConvertTabsToSpaces), ref convertTabsToSpaces, value);
        }

        [DisplayName("是否启用电子邮件链接")]
        public bool EnableEmailHyperlinks
        {
            get => enableEmailHyperlinks;
            set => Set(nameof(EnableEmailHyperlinks), ref enableEmailHyperlinks, value);
        }

        [DisplayName("是否启用超链接")]
        public bool EnableHyperlinks
        {
            get => enableHyperlinks;
            set => Set(nameof(EnableHyperlinks), ref enableHyperlinks, value);
        }

        [DisplayName("是否支持输入法编辑")]
        [Description("控制是否可以切换输入法。")]
        public bool EnableImeSupport
        {
            get => enableImeSupport;
            set => Set(nameof(EnableImeSupport), ref enableImeSupport, value);
        }

        [DisplayName("是否启用文本拖放")]
        public bool EnableTextDragDrop
        {
            get => enableTextDragDrop;
            set => Set(nameof(EnableTextDragDrop), ref enableTextDragDrop, value);
        }

        [DisplayName("是否启用虚拟空间")]
        public bool EnableVirtualSpace
        {
            get => enableVirtualSpace;
            set => Set(nameof(EnableVirtualSpace), ref enableVirtualSpace, value);
        }

        [DisplayName("是否高亮当前行")]
        public bool HighlightCurrentLine
        {
            get => highlightCurrentLine;
            set => Set(nameof(HighlightCurrentLine), ref highlightCurrentLine, value);
        }

        [DisplayName("Tab缩进大小")]
        public int IndentationSize
        {
            get => indentationSize;
            set => Set(nameof(IndentationSize), ref indentationSize, (value < 1) ? 1 : (value > 100) ? 100 : value);
        }

        [DisplayName("是否输入时隐藏光标")]
        public bool HideCursorWhileTyping
        {
            get => hideCursorWhileTyping;
            set => Set(nameof(HideCursorWhileTyping), ref hideCursorWhileTyping, value);
        }

        [DisplayName("是否按上一行缩进")]
        public bool InheritWordWrapIndentation
        {
            get => inheritWordWrapIndentation;
            set => Set(nameof(InheritWordWrapIndentation), ref inheritWordWrapIndentation, value);
        }

        [DisplayName("是否点击超链接时需要按 Ctrl 键")]
        [Description("如果设置为true，则必须按下Ctrl键才能单击超链接。")]
        public bool RequireControlModifierForHyperlinkClick
        {
            get => requireControlModifierForHyperlinkClick;
            set => Set(nameof(RequireControlModifierForHyperlinkClick), ref requireControlModifierForHyperlinkClick, value);
        }

        [DisplayName("是否显示列标尺")]
        public bool ShowColumnRuler
        {
            get => showColumnRuler;
            set => Set(nameof(ShowColumnRuler), ref showColumnRuler, value);
        }

        [DisplayName("默认代码高亮文件")]
        [Editor(typeof(HighlightPropertyEditor), typeof(PropertyEditorBase))]
        public string HightLightFile
        {
            get => hightLightFile;
            set => Set(nameof(HightLightFile), ref hightLightFile, value);
        }

        [DisplayName("默认行距(倍数)")]
        public double LineSpacing
        {
            get => lineSpacing;
            set => Set(nameof(LineSpacing), ref lineSpacing, (value < 1) ? 1 : (value > 100) ? 100 : value);
        }

        [DisplayName("编码格式")]
        [Editor(typeof(EncodingPropertyEditor), typeof(PropertyEditorBase))]
        public string EncodingName
        {
            get => encodingName;
            set => Set(nameof(EncodingName), ref encodingName, value);
        }

        [DisplayName("是否启用代码折叠")]
        [Description("编辑大型文档时关闭，以获得更好的性能。")]
        public bool UseFolding
        {
            get => useFolding;
            set => Set(nameof(UseFolding), ref useFolding, value);
        }
        #endregion

        public static EditorModel Instance => instance ??= new EditorModel();

        private EditorModel()
        {
            Read();
        }

        public void Read()
        {
            Configure("Editor", true);
        }

        public void Write()
        {
            Configure("Editor", false);
        }
    }

}
