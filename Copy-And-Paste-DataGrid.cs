
        private void PasteData_Method1(string[][] clipboardData, DataGrid dg )
        {

           
            int startRow = dg.SelectedIndex;
            int targetRowCount = clipboardData.Length;

            DataGridRow[] rows =
                  Enumerable.Range(startRow, targetRowCount)
                              .Select(rowIndex =>
                                dg.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow)
                                  .Where(row=>row!=null).ToArray();

            DataGridRow[] newRows = new DataGridRow[rows.Length];
            for (int i = 0; i < rows.Length; i++)
            {
                newRows[i] = rows[0];
            }
            
            DataGridColumn[] columns =
                dg.Columns.OrderBy(column => column.DisplayIndex)
                .SkipWhile(column => column != dg.CurrentCell.Column)
                .Take(clipboardData.Max(row => row.Length)).ToArray();
            

            for (int rowIndex = 0; rowIndex < newRows.Length; rowIndex++)
            {
                string[] rowContent = clipboardData[rowIndex];
                for (int colIndex = 0; colIndex < columns.Length; colIndex++)
                {
                    string cellContent = rowContent[colIndex];
                     columns[colIndex].OnPastingCellClipboardContent(rows[rowIndex].Item, cellContent);
                }
            }

        }

        
        private string[][] GetClipboardData()
        {
            return ((string)Clipboard.GetData(DataFormats.Text)).Split('\n')
                     .Select(row =>
                         row.Split('\t')
                         .Select(cell =>
                             cell.Length > 0 && cell[cell.Length - 1] == '\r' ?
                             cell.Substring(0, cell.Length - 1) : cell).ToArray())
                     .Where(a => a.Any(b => b.Length > 0)).ToArray();
        }




















  public static class ClipboardHelper
    {
        public delegate string[] ParseFormat(string value);

        public static List<string[]> ParseClipboardData()
        {
            List<string[]> clipboardData = null;
            object clipboardRawData = null;
            ParseFormat parseFormat = null;

            // get the data and set the parsing method based on the format
            // currently works with CSV and Text DataFormats            
            IDataObject dataObj = System.Windows.Clipboard.GetDataObject();
            if ((clipboardRawData = dataObj.GetData(DataFormats.CommaSeparatedValue)) != null)
            {
                parseFormat = ParseCsvFormat;
            }
            else if ((clipboardRawData = dataObj.GetData(DataFormats.Text)) != null)
            {
                parseFormat = ParseTextFormat;
            }

            if (parseFormat != null)
            {
                string rawDataStr = clipboardRawData as string;

                if (rawDataStr == null && clipboardRawData is MemoryStream)
                {
                    // cannot convert to a string so try a MemoryStream
                    MemoryStream ms = clipboardRawData as MemoryStream;
                    StreamReader sr = new StreamReader(ms);
                    rawDataStr = sr.ReadToEnd();
                }
                Debug.Assert(rawDataStr != null, string.Format("clipboardRawData: {0}, could not be converted to a string or memorystream.", clipboardRawData));

                string[] rows = rawDataStr.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (rows != null && rows.Length > 0)
                {
                    clipboardData = new List<string[]>();
                    foreach (string row in rows)
                    {
                        clipboardData.Add(parseFormat(row));
                    }
                }
                else
                {
                    Debug.WriteLine("unable to parse row data.  possibly null or contains zero rows.");
                }
            }

            return clipboardData;
        }

        public static string[] ParseCsvFormat(string value)
        {
            return ParseCsvOrTextFormat(value, true);
        }

        public static string[] ParseTextFormat(string value)
        {
            return ParseCsvOrTextFormat(value, false);
        }

        private static string[] ParseCsvOrTextFormat(string value, bool isCSV)
        {
            List<string> outputList = new List<string>();

            char separator = isCSV ? ',' : '\t';
            int startIndex = 0;
            int endIndex = 0;

            for (int i = 0; i < value.Length; i++)
            {
                char ch = value[i];
                if (ch == separator)
                {
                    outputList.Add(value.Substring(startIndex, endIndex - startIndex));

                    startIndex = endIndex + 1;
                    endIndex = startIndex;
                }
                else if (ch == '\"' && isCSV)
                {
                    // skip until the ending quotes
                    i++;
                    if (i >= value.Length)
                    {
                        throw new FormatException(string.Format("value: {0} had a format exception", value));
                    }
                    char tempCh = value[i];
                    while (tempCh != '\"' && i < value.Length)
                        i++;

                    endIndex = i;
                }
                else if (i + 1 == value.Length)
                {
                    // add the last value
                    outputList.Add(value.Substring(startIndex));
                    break;
                }
                else
                {
                    endIndex++;
                }
            }

            return outputList.ToArray();
        }
    }















using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CypherCrescent.PvtRms.Resource.Custom_Control
{
    public class CustomDataGrid : DataGrid
    {
        public CustomDataGrid()
        {
            LoadingRow += CustomDataGrid_LoadingRow;
            MouseDown += CustomDataGrid_MouseDown;
           KeyDown += CustomDataGrid_KeyDown;

            //CommandManager.RegisterClassCommandBinding(
            //    typeof(CustomDataGrid),
            //    new CommandBinding(ApplicationCommands.Paste,
            //        new ExecutedRoutedEventHandler(OnExecutedPaste),
            //        new CanExecuteRoutedEventHandler(OnCanExecutePaste)));
        }

        private void CustomDataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V && Keyboard.Modifiers == ModifierKeys.Control)
            {
                PasteData_Method1();
            }
        }

        private void CustomDataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                SetupRightClickMenu();

            }
        }

        private void SetupRightClickMenu()
        {
            ContextMenu contextMenu = new ContextMenu();

            MenuItem pasteItem = new MenuItem
            {
                Header = "Paste         Ctrl+V"

            };

            pasteItem.Click += delegate { /*Invoke a Paste method; */};

            contextMenu.Items.Add(pasteItem);

            ContextMenu = contextMenu;
        }


        private void CustomDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }


        //private static void OnCanExecutePaste(object target, CanExecuteRoutedEventArgs args)
        //{
        //    ((CustomDataGrid)target).OnCanExecutePaste(args);
        //}


        //protected virtual void OnCanExecutePaste(CanExecuteRoutedEventArgs args)
        //{
        //    args.CanExecute = CurrentCell != null;
        //    args.Handled = true;
        //}

        //private static void OnExecutedPaste(object target, ExecutedRoutedEventArgs args)
        //{
        //    ((CustomDataGrid)target).OnExecutedPaste(args);
        //}


        //protected virtual void OnExecutedPaste(ExecutedRoutedEventArgs args)
        //{
        //    try
        //    {

        //        var rowData = GetClipboardData();

        //        bool hasAddedNewRow = false;

        //        int minRowIndex = Items.IndexOf(CurrentItem);
        //        int maxRowIndex = Items.Count - 1;
        //        int minColumnDisplayIndex = CurrentColumn.DisplayIndex;
        //        int maxColumnDisplayIndex = Columns.Count - 1;
        //        int rowDataIndex = 0;
        //        for (int i = minRowIndex; i <= maxRowIndex && rowDataIndex < rowData.Length; i++, rowDataIndex++)
        //        {
        //            CurrentItem = Items[i];

        //            BeginEditCommand.Execute(null, this);

        //            int columnDataIndex = 0;
        //            for (int j = minColumnDisplayIndex; j <= maxColumnDisplayIndex && columnDataIndex < rowData[rowDataIndex].Length; j++, columnDataIndex++)
        //            {
        //                DataGridColumn column = ColumnFromDisplayIndex(j);
        //                column.OnPastingCellClipboardContent(Items[i], rowData[rowDataIndex][columnDataIndex]);
        //            }

        //            CommitEditCommand.Execute(this, this);
        //            if (i == maxRowIndex)
        //            {
        //                maxRowIndex++;
        //                hasAddedNewRow = true;
        //            }
        //        }


        //        if (hasAddedNewRow)
        //        {
        //            UnselectAll();
        //            UnselectAllCells();

        //            CurrentItem = Items[minRowIndex];

        //            if (SelectionUnit == DataGridSelectionUnit.FullRow)
        //            {
        //                SelectedItem = Items[minRowIndex];
        //            }
        //            else if (SelectionUnit == DataGridSelectionUnit.CellOrRowHeader ||
        //                     SelectionUnit == DataGridSelectionUnit.Cell)
        //            {
        //                SelectedCells.Add(new DataGridCellInfo(Items[minRowIndex], Columns[minColumnDisplayIndex]));

        //            }

        //        }
        //    }
        //    catch (Exception)
        //    {

        //        CustomMessageBox.Show("Waring", "Double-click the component column to select component.",
        //                 MessageBoxButton.OK,
        //                    MessageBoxImage.Warning);
        //    }
        //}

        private string[][] GetClipboardData()
        {
            return ((string)Clipboard.GetData(DataFormats.Text)).Split('\n')
                     .Select(row =>
                         row.Split('\t')
                         .Select(cell =>
                             cell.Length > 0 && cell[cell.Length - 1] == '\r' ?
                             cell.Substring(0, cell.Length - 1) : cell).ToArray())
                     .Where(a => a.Any(b => b.Length > 0)).ToArray();
        }



        private void PasteData_Method1()
        {
            try
            {

                var rowData = GetClipboardData();

                bool hasAddedNewRow = false;

                int minRowIndex = Items.IndexOf(CurrentItem);
                int maxRowIndex = Items.Count - 1;
                int minColumnDisplayIndex = CurrentColumn.DisplayIndex;
                int maxColumnDisplayIndex = Columns.Count - 1;
                int rowDataIndex = 0;
                for (int i = minRowIndex; i <= maxRowIndex && rowDataIndex < rowData.Length; i++, rowDataIndex++)
                {
                    CurrentItem = Items[i];

                    BeginEditCommand.Execute(null, this);

                    int columnDataIndex = 0;
                    for (int j = minColumnDisplayIndex; j <= maxColumnDisplayIndex && columnDataIndex < rowData[rowDataIndex].Length; j++, columnDataIndex++)
                    {
                        DataGridColumn column = ColumnFromDisplayIndex(j);
                        column.OnPastingCellClipboardContent(Items[i], rowData[rowDataIndex][columnDataIndex]);
                    }

                    CommitEditCommand.Execute(this, this);
                    if (i == maxRowIndex)
                    {
                        maxRowIndex++;
                        hasAddedNewRow = true;
                    }
                }


                if (hasAddedNewRow)
                {
                    UnselectAll();
                    UnselectAllCells();

                    CurrentItem = Items[minRowIndex];

                    if (SelectionUnit == DataGridSelectionUnit.FullRow)
                    {
                        SelectedItem = Items[minRowIndex];
                    }
                    else if (SelectionUnit == DataGridSelectionUnit.CellOrRowHeader ||
                             SelectionUnit == DataGridSelectionUnit.Cell)
                    {
                        SelectedCells.Add(new DataGridCellInfo(Items[minRowIndex], Columns[minColumnDisplayIndex]));

                    }

                }
            }
            catch (Exception)
            {

                CustomMessageBox.Show("Waring", "Double-click the component column to select component.",
                         MessageBoxButton.OK,
                            MessageBoxImage.Warning);
            }


        }

    }
}

   

