using EnvDTE;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace DeleteHistory
{
    internal class DeleteHistoryEntryValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DeleteHistoryEntry entry)
            {

                TextBlock textBlock = new TextBlock()
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                };

                this.AddFileNameText(textBlock, entry.FileName);
                this.AddDeleteTimeStampText(textBlock, entry.DeleteTime);
                this.AddNewLine(textBlock);
                this.AddDeleteHistoryText(textBlock, entry.DeletedText);

                Button button = new Button()
                {
                    Command = new PasteCommand(entry.DeletedText),
                    Content = textBlock,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                };

                return button;
            }
            throw new Exception("Unknown type sent to DeleteHistoryEntryValueConverter");
        }

        private TextBlock AddFileNameText(TextBlock textBlock, string filePath)
        {
            if (DeleteHistoryOptions.Instance.ShowSourceFileName)
            {
                var fileName = Path.GetFileName(filePath);
                textBlock.Inlines.Add(new Run(fileName) { FontWeight = FontWeights.Bold });
            }

            return textBlock;
        }

        private TextBlock AddDeleteTimeStampText(TextBlock textBlock, DateTime time)
        {
            if (DeleteHistoryOptions.Instance.ShowDeleteTime)
            {
                string dateText;
                if(time.Date == DateTime.Today)
                {
                    dateText = $" - {time.ToString("T")}";
                }
                else
                {
                    dateText = time.ToString("F");
                    
                    // if the date is really long, put it on a new line
                    this.AddNewLine(textBlock);
                }
                textBlock.Inlines.Add(new Run(dateText) { FontWeight = FontWeights.Light });
            }

            return textBlock;
        }

        private TextBlock AddNewLine(TextBlock textBlock)
        {
            textBlock.Inlines.Add(new Run(Environment.NewLine));
            return textBlock;
        }

        private TextBlock AddDeleteHistoryText(TextBlock textBlock, string deleteHistoryText)
        {
            if (DeleteHistoryOptions.Instance.TruncateLines)
            {
                var splitString = deleteHistoryText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                if (splitString.Count() > DeleteHistoryOptions.Instance.TruncateLinesCount)
                {
                    textBlock.Inlines.Add(new Run(string.Join(Environment.NewLine, splitString.Take(DeleteHistoryOptions.Instance.TruncateLinesCount))));
                    var missingLineCount = splitString.Count() - DeleteHistoryOptions.Instance.TruncateLinesCount;
                    this.AddNewLine(textBlock);
                    textBlock.Inlines.Add(new Run($"Plus {missingLineCount} additional lines.") { FontWeight = FontWeights.Bold });

                    return textBlock;
                }
            }
            textBlock.Inlines.Add(new Run(deleteHistoryText));

            return textBlock;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
