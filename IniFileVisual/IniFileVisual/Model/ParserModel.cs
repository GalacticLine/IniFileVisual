using System.ComponentModel;

namespace IniFileVisual.Model
{
    //解析器类
    public class ParserModel : ConfigModel
    {
        #region  Private Fields
        private static ParserModel instance;
        private bool caseInsensitive;
        private bool allowKeysWithoutSection;
        private bool allowDuplicateKeys;
        private bool allowDuplicateSections;
        private bool skipInvalidLines;
        private bool overrideDuplicateKeys;
        #endregion

        #region Public Property
        [DisplayName("是否忽略大小写")]
        public bool CaseInsensitive
        {
            get => caseInsensitive;
            set => Set(nameof(CaseInsensitive), ref caseInsensitive, value);
        }

        [DisplayName("是否允许无节键")]
        public bool AllowKeysWithoutSection
        {
            get => allowKeysWithoutSection;
            set => Set(nameof(AllowKeysWithoutSection), ref allowKeysWithoutSection, value);
        }

        [DisplayName("是否允许重复键")]
        public bool AllowDuplicateKeys
        {
            get => allowDuplicateKeys;
            set => Set(nameof(AllowDuplicateKeys), ref allowDuplicateKeys, value);
        }

        [DisplayName("是否允许重复节")]
        public bool AllowDuplicateSections
        {
            get => allowDuplicateSections;
            set => Set(nameof(AllowDuplicateSections), ref allowDuplicateSections, value);
        }

        [DisplayName("是否跳过无效行")]
        public bool SkipInvalidLines
        {
            get => skipInvalidLines;
            set => Set(nameof(SkipInvalidLines), ref skipInvalidLines, value);
        }

        [DisplayName("是否覆盖重复键")]
        [Description("如果为“是”，当有重复的键时，解析器只读取的最后一个键。")]
        public bool OverrideDuplicateKeys
        {
            get => overrideDuplicateKeys;
            set => Set(nameof(OverrideDuplicateKeys), ref overrideDuplicateKeys, value);
        }
        #endregion

        public static ParserModel Instance => instance ??= new ParserModel();

        private ParserModel()
        {
            Read();
        }

        public void Read()
        {
            try
            {
                Configure("Parser", true);
            }
            catch
            {
                throw;
            }
        }

        public void Write()
        {
            try
            {
                Configure("Parser", false);
            }
            catch
            {
                throw;
            }
        }
    }

}
