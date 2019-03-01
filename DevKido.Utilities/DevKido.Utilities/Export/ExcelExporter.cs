using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;


namespace DevKido.Utilities.Export
{
    public class ExcelExporter
    {
        public ExcelExporter()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        /// <summary>
        /// Exports Data from Gridview  to Excel 2007/2010/2013 format
        /// </summary>
        /// <param name="Title">Title to be shown on Top of Exported Excel File</param>
        /// <param name="HeaderBackgroundColor">Background Color of Title</param>
        /// <param name="HeaderForeColor">Fore Color of Title</param>
        /// <param name="HeaderFont">Font size of Title</param>
        /// <param name="DateRange">Specify if Date Range is to be shown or not.</param>
        /// <param name="FromDate">Value to be stored in From Date of Date Range</param>
        /// <param name="ToDate">Value to be stored in To Date of Date Range</param>
        /// <param name="DateRangeBackgroundColor">Background Color of Date Range</param>
        /// <param name="DateRangeForeColor">Fore Color of Date Range</param>
        /// <param name="DateRangeFont">Font Size of Date Range</param>
        /// <param name="gv">GridView Containing Data. Should not be a templated Gridview</param>
        /// <param name="ColumnBackgroundColor">Background Color of Columns</param>
        /// <param name="ColumnForeColor">Fore Color of Columns</param>
        /// <param name="SheetName">Name of Excel WorkSheet</param>
        /// <param name="FileName">Name of Excel File to be Created</param>
        /// <param name="response">Page Response of the Calling Page</param>


        public string Export2Excel(string Title, XLColor HeaderBackgroundColor, XLColor HeaderForeColor, int HeaderFont,

                                 bool DateRange, string FromDate, string ToDate, XLColor DateRangeBackgroundColor,
                                 XLColor DateRangeForeColor, int DateRangeFont, List<ModelDataList> DataList, XLColor ColumnBackgroundColor,
                                 XLColor ColumnForeColor, string SheetName, string FileName, HttpResponse response)
        {
            var wb = new XLWorkbook();
            foreach (var dataTable in DataList)
            {
                wb = GenerateExcelSheet(wb, dataTable.DataListName, dataTable.SheetName, Title, ColumnBackgroundColor, ColumnForeColor);
            }

            //Code to save the file
            HttpResponse httpResponse = response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=" + FileName);

            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();
            return "Ok";
            //}
            //else
            //{
            //    return "Invalid GridView. It is null";
            //}


        }

        public string ExportToExcel(List<ModelDataList> DataList, string Title, string SheetName, string FileName, HttpResponse response)
        { 
            XLColor ColumnBackgroundColor = XLColor.Orange;
            XLColor ColumnForeColor = XLColor.White;
            var wb = new XLWorkbook();
            foreach (var dataTable in DataList)
            {
                wb = GenerateExcelSheet(wb, dataTable.DataListName, dataTable.SheetName, Title, ColumnBackgroundColor, ColumnForeColor);
            }

            //Code to save the file
            HttpResponse httpResponse = response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=" + FileName);
            
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();
            return "Ok";
        }

        private XLWorkbook GenerateExcelSheet(XLWorkbook wb, System.Data.DataTable gv, string SheetName, string Title,
            XLColor ColumnBackgroundColor, XLColor ColumnForeColor)
        {
            System.Data.DataTable table = gv;
            if (gv != null)
            {
                //CreateTable(gv, table);
                //creating a new Workbook

                // adding a new sheet in workbook
                var ws = wb.Worksheets.Add(SheetName);
                //adding content
                //Title
                ws.Cell("A1").Value = Title;
                //if (DateRange)
                //{
                //    ws.Cell("A2").Value = "Date Range :" + FromDate + " - " + ToDate;
                //}
                //else
                //{
                //    ws.Cell("A2").Value = "";
                //}

                //add columns
                string[] cols = new string[table.Columns.Count];
                for (int c = 0; c < table.Columns.Count; c++)
                {
                    var a = table.Columns[c].ToString();
                    cols[c] = table.Columns[c].ToString().Replace('_', ' ');
                }


                char StartCharCols = 'A';
                int StartIndexCols = 1;
                #region CreatingColumnHeaders
                for (int i = 1; i <= cols.Length; i++)
                {
                    if (i == cols.Length)
                    {
                        string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                        ws.Cell(DataCell).Value = cols[i - 1];
                        ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                        ws.Cell(DataCell).Style.Font.Bold = true;
                        ws.Cell(DataCell).Style.Fill.BackgroundColor = ColumnBackgroundColor;
                        ws.Cell(DataCell).Style.Font.FontColor = ColumnForeColor;
                    }
                    else
                    {
                        string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                        ws.Cell(DataCell).Value = cols[i - 1];
                        ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                        ws.Cell(DataCell).Style.Font.Bold = true;
                        ws.Cell(DataCell).Style.Fill.BackgroundColor = ColumnBackgroundColor;
                        ws.Cell(DataCell).Style.Font.FontColor = ColumnForeColor;
                        StartCharCols++;
                    }
                }
                #endregion

                //Merging Header

                string Range = "A1:" + StartCharCols.ToString() + "1";

                //ws.Range(Range).Merge();
                //ws.Range(Range).Style.Font.FontSize = HeaderFont;
                //ws.Range(Range).Style.Font.Bold = true;

                //ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                //ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //if (HeaderBackgroundColor != null && HeaderForeColor != null)
                //{
                //    ws.Range(Range).Style.Fill.BackgroundColor = HeaderBackgroundColor;
                //    ws.Range(Range).Style.Font.FontColor = HeaderForeColor;
                //}


                //Style definitions for Date range
                //Range = "A2:" + StartCharCols.ToString() + "2";

                //ws.Range(Range).Merge();
                //ws.Range(Range).Style.Font.FontSize = 10;
                //ws.Range(Range).Style.Font.Bold = true;
                //ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
                //ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                //border definitions for Columns
                Range = "A3:" + StartCharCols.ToString() + "3";
                ws.Range(Range).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Range(Range).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Range(Range).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Range(Range).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                char StartCharData = 'A';
                int StartIndexData = 2;

                char StartCharDataCol = char.MinValue;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int j = 0; j < table.Columns.Count; j++)
                    {

                        string DataCell = StartCharData.ToString() + StartIndexData;
                        var a = table.Rows[i][j].ToString();
                        a = a.Replace("&nbsp;", " ");
                        a = a.Replace("&amp;", "&");
                        //check if value is of integer type
                        int val = 0;
                        DateTime dt = DateTime.Now;
                        if (int.TryParse(a, out val))
                        {
                            //    ws.Cell(DataCell).Style.NumberFormat.NumberFormatId = 15;
                            ws.Cell(DataCell).Value = val;
                        }
                        //check if datetime type
                        else if (DateTime.TryParse(a, out dt))
                        {
                            ws.Cell(DataCell).Value = dt.ToShortDateString();
                        }
                        ws.Cell(DataCell).SetValue(a);
                        StartCharData++;
                    }
                    StartCharData = 'A';
                    StartIndexData++;
                }

                char LastChar = Convert.ToChar(StartCharData + table.Columns.Count - 1);
                int TotalRows = table.Rows.Count + 1;
                Range = "A4:" + LastChar + TotalRows;
                ws.Range(Range).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Range(Range).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Range(Range).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Range(Range).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            return wb;
        }

        public static System.Data.DataTable CopyGenericToDataTable<T>(IEnumerable<T> items)
        {
            var properties = typeof(T).GetProperties();
            var result = new System.Data.DataTable();

            //Build the columns
            foreach (var prop in properties)
            {
                if (prop.ToString().Contains("Nullable"))
                {
                    try
                    {
                        result.Columns.Add(prop.Name, typeof(string));
                    }
                    catch (Exception)
                    {
                        try
                        {
                            result.Columns.Add(prop.Name, typeof(Int32));
                        }
                        catch (Exception)
                        {
                            result.Columns.Add(prop.Name, typeof(Guid));
                        }
                    } 
                }
                else
                {
                    result.Columns.Add(prop.Name, prop.PropertyType);
                }
            }

            //Fill the DataTable
            foreach (var item in items)
            {
                var row = result.NewRow();

                foreach (var prop in properties)
                {
                    var itemValue = prop.GetValue(item, new object[] { });
                    row[prop.Name] = itemValue;
                }

                result.Rows.Add(row);
            }

            return result;
        }
    }
}
