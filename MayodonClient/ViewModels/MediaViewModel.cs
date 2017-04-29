using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mastonet.Entities;

namespace MayodonClient.ViewModels
{
    public class MediaViewModel
    {
        public string Url { get; private set; }
        public int Column { get; private set; }
        public int Row { get; private set; }
        public int ColumnSpan { get; private set; }
        public int RowSpan { get; private set; }

        public MediaViewModel(Attachment attachment, int index, int count)
        {
            Url = attachment.PreviewUrl;

            CalcGridCell(index, count);
        }

        private void CalcGridCell(int index, int count)
        {
            switch (count)
            {
                case 1:
                    Column = 0;
                    Row = 0;
                    ColumnSpan = 2;
                    RowSpan = 2;
                    break;
                case 2:
                    ColumnSpan = 1;
                    RowSpan = 2;
                    Row = 0;
                    Column = index;
                    break;
                case 3:
                    ColumnSpan = 1;
                    switch (index)
                    {
                        case 0:
                            Row = 0;
                            Column = 0;
                            RowSpan = 2;
                            break;
                        case 1:
                            Row = 0;
                            Column = 1;
                            RowSpan = 1;
                            break;
                        case 2:
                            Row = 1;
                            Column = 1;
                            RowSpan = 1;
                            break;
                    }
                    break;
                case 4:
                    ColumnSpan = 1;
                    RowSpan = 1;
                    switch (index)
                    {
                        case 0:
                            Row = 0;
                            Column = 0;
                            break;
                        case 1:
                            Row = 0;
                            Column = 1;
                            break;
                        case 2:
                            Row = 1;
                            Column = 0;
                            break;
                        case 3:
                            Row = 1;
                            Column = 1;
                            break;
                    }
                    break;
            }
        }
    }
}
