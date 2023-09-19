using System;
using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using IniParser;
using IniParser.Model;

namespace IniFileVisual.ViewModel
{
    public class ToolViewModel : ViewModelBase
    {
        #region  Private Fields
        private string sourceFile;
        private string targetFile;
        private RelayCommand overWrite;
        private RelayCommand<string> openFile;
        #endregion

        public string SourceFile
        {
            get => sourceFile;
            set => Set(nameof(SourceFile), ref sourceFile, value);
        }
        public string TargetFile
        {
            get => targetFile;
            set => Set(nameof(TargetFile), ref targetFile, value);
        }

        //覆写功能
        public RelayCommand OverWrite => overWrite ??= new RelayCommand(() =>
        {

            if (sourceFile != null && targetFile != null)
            {
                FileIniDataParser parser = new FileIniDataParser();
                try
                {
                    IniData target = parser.ReadFile(targetFile);
                    target.Merge(parser.ReadFile(sourceFile));
                    parser.WriteFile(targetFile, target);
                    Growl.Success("覆写成功！");
                }
                catch(Exception ex)
                {
                    Growl.Error($"覆写失败！{ex.Message}");
                }
                finally
                {
                    sourceFile = null;
                    targetFile = null;
                    GC.Collect();
                }
            }
        });

        //OpenFileDialog打开指定文件
        public RelayCommand<string> OpenFile => openFile ??= new RelayCommand<string>((isSoure) =>
        {
            try
            {
                using OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Ini文件|*.ini";
                ofd.CheckFileExists = true;
                ofd.CheckPathExists = true;
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (isSoure == "0")
                    {
                        SourceFile = ofd.FileName;
                    }
                    else if (isSoure == "1")
                    {
                        TargetFile = ofd.FileName;
                    }
                }
            }
            catch
            {
                Growl.Error($"打开文件失败！");
            }

        });
    }

}
