using System.Windows;
using System.Windows.Media;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Win32;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Search;
using ICSharpCode.AvalonEdit.Utils;

namespace IniFileVisual.Views
{
    /// <summary>
    /// MvvmTextEditor.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class MvvmTextEditor : TextEditor, INotifyPropertyChanged
    {
        #region  Private Fields
        private bool useFolding;
        private string hightLightFile;
        private FoldingManager manager;
        private IniFoldingStrategy strategy;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        #region Public Properties
        public double LineSpacing
        {
            get => TextArea.TextView.LineSpacing;
            set => TextArea.TextView.LineSpacing = value;
        }

        public string Font
        {
            get => FontFamily.Source;
            set
            {
                if (FontFamily.Source != value)
                {
                    FontFamily = new FontFamily(value);
                }
            }
        }

        public string HightLightFile
        {
            get
            {
                return hightLightFile;
            }
            set
            {
                if (hightLightFile != value)
                {
                    hightLightFile = value;

                    Assembly assembly = Assembly.GetExecutingAssembly();
                    string name = $"{assembly.GetName().Name}.Resources.{nameof(XshdSyntaxDefinition)}.{value}";
                    using Stream stream = assembly.GetManifestResourceStream(name);
                    SyntaxHighlighting = HighlightingLoader.LoadHighlighting(stream);
                }
            }
        }

        public string EncodingName
        {
            get
            {
                return Encoding.WebName;
            }
            set
            {
                if (Encoding == null || Encoding.WebName != value)
                {
                    Encoding = System.Text.Encoding.GetEncoding(value);
                }
            }
        }

        public string FileName
        {
            get
            {
                return Document.FileName;
            }
            set
            {
                if (Document.FileName != value)
                {
                    Document.FileName = value;
                    RaisePropertyChanged(nameof(FileName));
                }
            }
        }

        public bool UseFolding
        {
            get => useFolding;
            set
            {
                useFolding = value;
                if (!useFolding)
                {
                    FoldingManager.Uninstall(Manager);
                    Manager = null;
                }
            }
        }

        public FoldingManager Manager
        {
            get
            {
                if (manager == null)
                {
                    manager = FoldingManager.Install(TextArea);
                }
                return manager;
            }
            set
            {
                manager = value;
            }
        }

        public IniFoldingStrategy Strategy =>
            strategy ??= new IniFoldingStrategy();
        #endregion

        public MvvmTextEditor() : this(new MvvmTextArea())
        {
            SearchPanel.Install(this).Localization = new ChineseLocalization();
        }

        public MvvmTextEditor(TextArea mvvmTextArea) : base(mvvmTextArea)
        {
            InitializeComponent();
        }

        #region Public Functions
        //异步加载文件
        public async Task<string> LoadAsync(string fileName)
        {
            try
            {
                FileName = Path.GetFileName(fileName);
                ToolTip = fileName;

                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (StreamReader reader = FileReader.OpenStream(fs, Encoding ?? System.Text.Encoding.UTF8))
                    {
                        Text = await reader.ReadToEndAsync();
                        SetCurrentValue(EncodingProperty, reader.CurrentEncoding);
                        SetCurrentValue(IsModifiedProperty, false);
                    }
                }
                return Text;
            }
            catch
            {
                throw;
            }
        }

        //添加未保存标志“*”
        public void AddStar()
        {
            if (!FileName.EndsWith('*'))
            {
                FileName = $"{FileName}*";
            }
        }

        //删除未保存标志“*”
        public void DeleteStar()
        {
            if (FileName.EndsWith('*'))
            {
                FileName = FileName[0..^1];
            }
        }

        //保存或另存为文件
        public void SaveFile(bool isAs = false)
        {
            try
            {
                if (isAs || !Path.IsPathFullyQualified(ToolTip.ToString()))
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.DefaultExt = ".ini";
                    if (sfd.ShowDialog() == true)
                    {
                        Save(sfd.FileName);
                        ToolTip = sfd.FileName;
                        FileName = Path.GetFileName(sfd.FileName);
                        DeleteStar();
                    }
                }
                else
                {
                    Save(ToolTip.ToString());
                    DeleteStar();
                }
            }
            catch
            {
                throw;
            }

        }

        //打开或折闭折叠
        public void OpenOrCloseAllFoldings(bool isFold)
        {
            try
            {
                foreach (FoldingSection newFolding in Manager.AllFoldings)
                {
                    newFolding.IsFolded = isFold;
                }
            }
            catch
            {
                throw;
            }
        }

        //折叠光标所在位置
        public void FoldedFoldingAtCaret()
        {
            try
            {
                var containing = Manager.GetFoldingsContaining(CaretOffset);
                if (containing.Count > 0)
                {
                    containing[0].IsFolded = true;
                }
            }
            catch
            {
                throw;
            }
        }

        //跳转指定行
        public void GoToLine(int i)
        {
            try
            {
                if (i <= LineCount && i > 0)
                {
                    ScrollToLine(i);
                    var line = Document.GetLineByNumber(i);
                    Select(line.Offset, line.Length);
                }
            }
            catch
            {
                throw;
            }
        }

        //跳转到同名节
        public void GotoSameSection()
        {
            try
            {
                string pattern = @"\[" + Regex.Escape(SelectedText) + @"\]";
                Match match = Regex.Match(Text, pattern);
                int line = match.Success switch
                {
                    true => Document.GetLineByOffset(match.Index).LineNumber,
                    false => TextArea.Caret.Line,
                };

                GoToLine(line);
            }
            catch
            {
                throw;
            }
        }

        //更新折叠
        public void UpdateFolding()
        {
            Strategy.UpdateFoldings(Manager, Document);
        }

        //编辑器是否可以被关闭
        public bool CanbeClosed()
        {
            try
            {
                bool canClose;
                if (FileName.EndsWith("*"))
                {
                    MessageBoxResult result = MessageBox.Show("当前文件未保存，是否保存后再关闭?", "警告", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        SaveFile();
                        canClose = true;
                    }
                    else if (result == MessageBoxResult.No)
                    {
                        canClose = true;
                    }
                    else
                    {
                        canClose = false;
                    }
                }
                else
                {
                    canClose = true;
                }
                return canClose;
            }
            catch
            {
                throw;
            }
        }

        public virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

}