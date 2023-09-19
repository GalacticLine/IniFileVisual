using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HandyControl.Controls;
using HandyControl.Data;
using IniParser;
using IniParser.Exceptions;
using IniParser.Model;
using IniFileVisual.Model;
using IniFileVisual.Views;
using Microsoft.Win32;

namespace IniFileVisual.ViewModel
{
    public class IniTextEditorViewModel : ViewModelBase
    {
        #region  Private Fields
        private MvvmTextEditor selectedItem;
        private IniData data;
        private ObservableCollection<ParsingException> parserErrors;
        private ObservableHashSet<MvvmTextEditor> items;
        private RelayCommand<MvvmTextEditor> setSelectedItem;
        private RelayCommand addItems;
        private RelayCommand save;
        private RelayCommand saveall;
        private RelayCommand saveAs;
        private RelayCommand deleteAllKeys;
        private RelayCommand deleteAllComments;
        private RelayCommand iniDebug;
        private RelayCommand addComment;
        private RelayCommand deleteComment;
        private RelayCommand addTempItem;
        private RelayCommand closeCheck;
        private RelayCommand addStar;
        private RelayCommand openFlods;
        private RelayCommand foldFlods;
        private RelayCommand foldByCaret;
        private RelayCommand closeSelected;
        private RelayCommand<CancelRoutedEventArgs> closeByEvent;
        private RelayCommand updateItems;
        private RelayCommand updateFolding;
        private RelayCommand goToSameSection;
        private RelayCommand<object> goToLine;
        private RelayCommand<MouseWheelEventArgs> mouseWheelSetFontSize;
        private RelayCommand<KeyEventArgs> updateFoldingByKeyEvent;
        #endregion

        #region Public Properties
        public MvvmTextEditor SelectedItem
        {
            get => selectedItem;
            set
            {
                if (Set(nameof(SelectedItem), ref selectedItem, value))
                {
                    data = null;
                    parserErrors.Clear();
                }
            }
        }

        public IniData Data =>
            data ??= new FileIniDataParser().ReadFile(selectedItem.ToolTip.ToString());

        public ObservableCollection<ParsingException> ParserErrors =>
            parserErrors ??= new ObservableCollection<ParsingException>();

        public ObservableHashSet<MvvmTextEditor> Items =>
            items ??= new ObservableHashSet<MvvmTextEditor>(new ToolTipEqualityComparer());
        #endregion

        #region Public Commands
        //设置当前选中子项
        public RelayCommand<MvvmTextEditor> SetSelectedItem => setSelectedItem ??= new RelayCommand<MvvmTextEditor>((value) =>
        {
            selectedItem = value;
        });

        //添加一个或多个文件编辑器
        public RelayCommand AddItems => addItems ??= new RelayCommand(async () =>
        {
            GC.Collect();
            OpenFileDialog ofd = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Ini文件|*.ini",
                CheckFileExists = true,
                RestoreDirectory = true
            };
            if (ofd.ShowDialog() == true)
            {
                foreach (string fileName in ofd.FileNames)
                {
                    try
                    {
                        MvvmTextEditor editor = new MvvmTextEditor();
                        await editor.LoadAsync(fileName);
                        items.Add(editor);
                        UpdateStyle(editor);
                    }
                    catch
                    {
                        Growl.Error($"打开文件{fileName}失败！");
                    }
                }
            }
        });

        //添加临时的文件编辑器
        public RelayCommand AddTempItem => addTempItem ??= new RelayCommand(() =>
        {
            try
            {
                GC.Collect();
                int i = 0;
                string name;
                var names = items.Select(x => x.ToolTip.ToString());
                while (true)
                {
                    string newName = $"Unnamed-{i}.ini";
                    if (!names.Contains(newName))
                    {
                        name = newName;
                        break;
                    }
                    i++;
                }
                MvvmTextEditor item = new MvvmTextEditor
                {
                    FileName = name + "*",
                    ToolTip = name
                };
                items.Add(item);
                UpdateStyle(item);
            }
            catch
            {
                Growl.Warning("创建临时文件失败！");
            }
        });

        //保存当前文件
        public RelayCommand Save => save ??= new RelayCommand(() =>
        {
            try
            {
                selectedItem.SaveFile();
            }
            catch
            {
                Growl.Error("保存失败！");
            }
        }, () => HasSelected());

        //保存所有的文件
        public RelayCommand SaveAll => saveall ??= new RelayCommand(() =>
        {
            foreach (MvvmTextEditor item in items)
            {
                try
                {
                    item.SaveFile();
                }
                catch
                {
                    Growl.Error($"保存{item.ToolTip}失败！");
                }
            }
        }, () => HasSelected());

        //当前文件另存为
        public RelayCommand SaveAs => saveAs ??= new RelayCommand(() =>
        {
            try
            {
                selectedItem.SaveFile(true);
            }
            catch
            {
                Growl.Error("另存为失败！");
            }
        }, () => HasSelected());

        //调试当前文件
        public RelayCommand IniDebug => iniDebug ??= new RelayCommand(async () =>
        {
            try
            {
                selectedItem.SaveFile();
                parserErrors.Clear();

                FileIniDataParser parser = new FileIniDataParser();
                parser.Parser.Configuration.ThrowExceptionsOnError = false;
                ParserModel.Instance.MapProperties(parser.Parser.Configuration);

                Growl.Info("正在解析...");
                string file = selectedItem.ToolTip.ToString();
                await Task.Run(() =>
                {
                    parser.ReadFile(file);
                });
                foreach (ParsingException item in parser.Parser.Errors)
                {
                    parserErrors.Add(item);
                }
                Growl.Success("解析成功!");
            }
            catch (Exception ex)
            {
                Growl.Error($"解析失败！{ex.Message}");
            }
        }, () => HasSelected());

        //删除当前文件中的所有键值
        public RelayCommand DeleteAllKeys => deleteAllKeys ??= new RelayCommand(() =>
        {
            try
            {
                if (Data.Clone() is IniData varData)
                {
                    foreach (SectionData item in varData.Sections)
                    {
                        item.ClearKeyData();
                    }
                    selectedItem.Document.Text = varData.ToString();
                }
            }
            catch
            {
                Growl.Error("删除全部键值过程中出现错误！");
            }
        }, () => HasSelected());

        //删除当前文件中的所有注释
        public RelayCommand DeleteAllComments => deleteAllComments ??= new RelayCommand(() =>
        {
            try
            {
                if (Data.Clone() is IniData varData)
                {
                    varData.ClearAllComments();
                    selectedItem.Document.Text = varData.ToString();
                }
            }
            catch
            {
                Growl.Error("删除全部注释过程中出现错误！");
            }
        }, () => HasSelected());

        //注释选中文本
        public RelayCommand AddComment => addComment ??= new RelayCommand(() =>
        {
            try
            {
                string text = selectedItem.SelectedText.Replace("\n", "\n" + ";").Insert(0, ";");
                selectedItem.SelectedText = text;
            }
            catch
            {
                Growl.Error("添加注释失败！");
            }
        });

        //删除选中文本的注释
        public RelayCommand DeleteComment => deleteComment ??= new RelayCommand(() =>
        {
            try
            {
                selectedItem.SelectedText = selectedItem.SelectedText.Replace(';', char.MinValue);
            }
            catch
            {
                Growl.Error("删除注释失败！");
            }
        });

        //检查是否可以关闭所有文件编辑器
        public RelayCommand CloseCheck => closeCheck ??= new RelayCommand(() =>
        {
            foreach (MvvmTextEditor item in items)
            {
                try
                {
                    item.CanbeClosed();
                }
                catch
                {
                    Growl.Error($"{item.ToolTip}未能正确关闭！");
                }
            }
        });

        //当前文件添加未保存的标志“*”
        public RelayCommand AddStar => addStar ??= new RelayCommand(() =>
        {
            selectedItem.AddStar();
        });

        //展开当前文件的全部折叠
        public RelayCommand OpenFlods => openFlods ??= new RelayCommand(() =>
        {
            try
            {
                selectedItem.OpenOrCloseAllFoldings(false);
            }
            catch
            {
                Growl.Error("展开全部折叠失败！");
            }
        });

        //折闭当前文件的全部折叠
        public RelayCommand FoldFlods => foldFlods ??= new RelayCommand(() =>
        {
            try
            {
                selectedItem.OpenOrCloseAllFoldings(true);
            }
            catch
            {
                Growl.Error("折闭全部折叠失败！");
            }
        });

        //折闭鼠标所在的折叠
        public RelayCommand FoldByCaret => foldByCaret ??= new RelayCommand(() =>
        {
            try
            {
                selectedItem.FoldedFoldingAtCaret();
            }
            catch
            {
                Growl.Error("折闭当前折叠失败！");
            }
        });

        //关闭当前文件
        public RelayCommand CloseSelected => closeSelected ??= new RelayCommand(() =>
        {
            try
            {
                if (selectedItem.CanbeClosed())
                {
                    items.Remove(selectedItem);
                    GC.Collect();
                }
            }
            catch
            {
                Growl.Error("当前编辑器未能正确关闭！");
            }
        }, () => HasSelected());

        //关闭当前触发关闭事件的文件
        public RelayCommand<CancelRoutedEventArgs> CloseByEvent => closeByEvent ??= new RelayCommand<CancelRoutedEventArgs>(e =>
        {
            try
            {
                MvvmTextEditor item = items.FirstOrDefault(x => x == (e.OriginalSource as MvvmTextEditor));
                if (item != null)
                {
                    bool canClose = item.CanbeClosed();
                    e.Cancel = !canClose;
                    if (canClose)
                    {
                        items.Remove(item);
                        GC.Collect();
                    }
                }
            }
            catch
            {
                Growl.Error("当前编辑器未能正确关闭！");
            }
        });

        //更新所有编辑器设置样式
        public RelayCommand UpdateItems => updateItems ??= new RelayCommand(() =>
        {
            foreach (MvvmTextEditor item in items)
            {
                UpdateStyle(item);
            }
        });

        //当前编辑器更新折叠
        public RelayCommand UpdateFolding => updateFolding ??= new RelayCommand(() =>
        {
            selectedItem.UpdateFolding();
        }, () => { return selectedItem.UseFolding; });

        //转到同名的节
        public RelayCommand GoToSameSection => goToSameSection ??= new RelayCommand(() =>
        {
            try
            {
                selectedItem.GotoSameSection();
            }
            catch
            {
                Growl.Error("转到同名节失败！");
            }
        });

        //转到当前文件的指定行
        public RelayCommand<object> GoToLine => goToLine ??= new RelayCommand<object>((arg) =>
        {
            try
            {
                if (int.TryParse(arg.ToString(), out int i))
                {
                    selectedItem.GoToLine(i);
                }
            }
            catch
            {
                Growl.Error("转到指定行失败！");
            }
        }, (x) => HasSelected());

        //Ctrl+鼠标滚轮临时修改字体大小
        public RelayCommand<MouseWheelEventArgs> MouseWheelSetFontSize => mouseWheelSetFontSize ??= new RelayCommand<MouseWheelEventArgs>((e) =>
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (e.Delta > 0)
                {
                    if (selectedItem.FontSize < 100)
                    {
                        selectedItem.FontSize++;
                    }
                }
                else
                {
                    if (selectedItem.FontSize > 2)
                    {
                        selectedItem.FontSize--;
                    }
                }
            }
        });

        //触发键盘事件的编辑器更新折叠
        public RelayCommand<KeyEventArgs> UpdateFoldingByKeyEvent => updateFoldingByKeyEvent ??= new RelayCommand<KeyEventArgs>((e) =>
        {
            selectedItem.UpdateFolding();
        }, e =>
        {
            return (e.Key == Key.Back
                || e.Key == Key.Enter
                || e.Key == Key.Oem4
                || e.Key == Key.Oem6
                || e.Key == Key.OemPlus
                || e.Key == Key.LeftCtrl
                || e.Key == Key.RightCtrl)
                && selectedItem.UseFolding; ;
        });
        #endregion

        #region Private Functions
        //判断是否有选中的编辑器
        private bool HasSelected()
        {
            return selectedItem != null;
        }

        //更新编辑器配置样式
        private void UpdateStyle(MvvmTextEditor item)
        {
            try
            {
                EditorModel.Instance.MapProperties(item);
                EditorModel.Instance.MapProperties(item.Options);
            }
            catch
            {
                Growl.Error("映射配置时出现错误！");
            }
        }
        #endregion

    }

}
