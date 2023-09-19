namespace ICSharpCode.AvalonEdit.Search
{
    //SearchPanel中文本地化
    public sealed class ChineseLocalization : Localization
    {
        public override string MatchCaseText => "区分大小写";

        public override string MatchWholeWordsText => "匹配整个单词";

        public override string UseRegexText => "使用正则表达式";

        public override string FindNextText => "查找下一个(F3)";

        public override string FindPreviousText => "查找上一个(Shift+F3)";

        public override string ErrorText => "错误: ";

        public override string NoMatchesFoundText => "未找到匹配项!";
    }

}




