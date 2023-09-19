using System.Linq;
using System.Collections.Generic;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;

//Ini文件折叠策略
public class IniFoldingStrategy
{
    //创建Ini文件折叠
    private static IEnumerable<NewFolding> CreateIniFolding(TextDocument document, out int firstErrorOffset)
    {
        firstErrorOffset = -1;
        List<NewFolding> newFoldings = new List<NewFolding>();
        List<int> start = new List<int>();
        List<int> end = new List<int>();
        List<string> name = new List<string>();
        for (int i = 0; i < document.LineCount - 1; i++)
        {
            DocumentLine line = document.Lines[i];
            string text = document.GetText(line).Trim();
            if (!string.IsNullOrWhiteSpace(text))
            {
                bool isComment = text.StartsWith(';');
                bool isSection = text.StartsWith('[') && text.Contains("]");
                bool isKey = !isComment && !isSection;
                if (isSection)
                {
                    start.Add(line.Offset);
                    name.Add(text);
                    if (start.Count == 2)
                    {
                        if (end.Count > 0 && start[0] < end[0])
                        {
                            start.Add(line.Offset);
                            name.Add(text);
                            NewFolding folding = new NewFolding(start[0], end[0])
                            {
                                Name = name[0],
                                DefaultClosed = true
                            };
                            newFoldings.Add(folding);
                            start.RemoveRange(0, 2);
                            name.RemoveRange(0, 2);
                        }
                        else
                        {
                            start.RemoveAt(0);
                            name.RemoveAt(0);
                        }
                    }
                }
                if (isKey)
                {
                    end.Add(line.EndOffset);
                    if (end.Count == 2)
                    {
                        end.RemoveAt(0);
                    }
                }
                if (isComment)
                {
                    end.Add(line.EndOffset);
                    if (end.Count == 2)
                    {
                        end.RemoveAt(1);
                    }
                }
            }
        }
        if (start.Count > 0)
        {
            NewFolding folding = new NewFolding(start[0], document.TextLength)
            {
                Name = name[0],
                DefaultClosed = true
            };
            newFoldings.Add(folding);
        }
        return newFoldings;
    }

    //创建新折叠
    public IEnumerable<NewFolding> CreateNewFoldings(TextDocument document, out int firstErrorOffset)
    {
        try
        {
            return CreateIniFolding(document, out firstErrorOffset);
        }
        catch
        {
            firstErrorOffset = 0;
            return Enumerable.Empty<NewFolding>();
        }
    }

    //更新折叠
    public void UpdateFoldings(FoldingManager manager, TextDocument document)
    {
        int firstErrorOffset;
        IEnumerable<NewFolding> foldings = CreateNewFoldings(document, out firstErrorOffset);
        manager.UpdateFoldings(foldings, firstErrorOffset);
    }

}