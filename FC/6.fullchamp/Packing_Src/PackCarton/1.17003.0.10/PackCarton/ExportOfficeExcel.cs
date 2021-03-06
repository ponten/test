using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
//using System.Windows.Forms;
using Excel;
using MSForms = Microsoft.Vbe.Interop.Forms;

namespace ExportOfficeExcel
{
    public class CreateOfficeExcel
    {
        // 存檔路徑
        private string _SaveFilePath;
        public string SaveFilePath
        {
            get { return _SaveFilePath; }
            set { _SaveFilePath = value; }
        }
        //範本檔路徑
        private string _SampleFilePath;
        public string SampleFilePath
        {
            get { return _SampleFilePath; }
            set { _SampleFilePath = value; }
        }
        //SheetName
        private string _SheetName;
        public string SheetName
        {
            get { return _SheetName; }
            set { _SheetName = value; }
        }
        //是否須存檔
        private bool _Save;
        public bool Save
        {
            get { return _Save; }
            set { _Save = value; }
        }
        //巨集名稱
        private string _Macro;
        public string Macro
        {
            get { return _Macro; }
            set { _Macro = value; }
        }

        //是否須列印
        private bool _Print;
        public bool Print
        {
            get { return _Print; }
            set { _Print = value; }
        }
        //列印張數
        private int _PrintQty;
        public int PrintQty
        {
            get { return _PrintQty; }
            set { _PrintQty = value; }
        }

        public CreateOfficeExcel() : this(string.Empty, string.Empty) { }
        public CreateOfficeExcel(string SaveFilePath) : this(SaveFilePath, string.Empty) { }
        public CreateOfficeExcel(string SaveFilePath, string SampleFilePath)
        {
            this.SaveFilePath = SaveFilePath;
            this.SampleFilePath = SampleFilePath;

            _Save = true;
            _Print = false;
            _SheetName = "Sheet1";
            _PrintQty = 1;
            _Macro = string.Empty;
        }

        /*
        public bool ExportToOfficeExcel(DataGridView GridView)
        {
            DataSet dsGrid = new DataSet();
            dsGrid.Tables.Add();
            for (int i = 0; i <= GridView.Columns.Count - 1; i++)
            {
                dsGrid.Tables[0].Columns.Add(GridView.Columns[i].Name);
            }

            for (int i = 0; i <= GridView.Rows.Count - 1; i++)
            {
                dsGrid.Tables[0].Rows.Add();
                for (int j = 0; j <= GridView.Columns.Count - 1; j++)
                {
                    dsGrid.Tables[0].Rows[i][j] = GridView.Rows[i].Cells[j].Value;
                }
            }
            return ExportToOfficeExcel(dsGrid);
        }
         */
        public bool ExportToOfficeExcel(DataSet ds)
        {
            return ExportToOfficeExcel(ds.Tables[0]);
        }
        public bool ExportToOfficeExcel(System.Data.DataTable dt)
        {
            Excel.Application IExcelApp = null;
            Excel.Workbook IExcelWorkBook = null;
            Excel.Worksheet IExcelWorkSheet = null;
            Excel.Range IExcelRange = null;

            try
            {
                IExcelApp = new Excel.Application();
                IExcelApp.Visible = false;

                //產生新的 Excel Work Book
                if (string.IsNullOrEmpty(SampleFilePath))
                {
                    IExcelWorkBook = (Excel.Workbook)(IExcelApp.Workbooks.Add(Type.Missing));
                    IExcelWorkSheet = (Excel.Worksheet)IExcelWorkBook.ActiveSheet;
                    IExcelWorkSheet.Name = SheetName;
                }
                else //開啟範本檔
                {
                    IExcelWorkBook = (Excel.Workbook)(IExcelApp.Workbooks.Add(SampleFilePath));
                    IExcelWorkSheet = (Excel.Worksheet)IExcelWorkBook.Worksheets.get_Item(SheetName);
                }


                //製作表頭
                DataColumn dc;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dc = dt.Columns[i];
                    IExcelWorkSheet.Cells[1, i + 1] = dc.ColumnName;
                }

                //內容
                int int_Row = 2;
                DataRow dr;
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    dr = dt.Rows[j];

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dc = dt.Columns[i];
                        IExcelWorkSheet.Cells[int_Row, i + 1] = dr[i].ToString();
                    }
                    int_Row = int_Row + 1;
                }

                //Run巨集
                if (!string.IsNullOrEmpty(Macro))
                {
                    RunMacro(IExcelApp, new object[] { Macro });
                }

                //存入 
                if (Save)
                    IExcelWorkBook.SaveAs(SaveFilePath, Excel.XlFileFormat.xlWorkbookNormal, null, null, false, false, Excel.XlSaveAsAccessMode.xlExclusive, false, false, null, null, null);
                //列印
                if (Print)
                {
                    IExcelWorkBook.PrintOut(1, Type.Missing, PrintQty, false, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
            finally
            {
                //釋放所有物件及資源
                if (IExcelWorkBook != null)
                    IExcelWorkBook.Close(false, null, null);
                if (IExcelApp != null)
                {
                    IExcelApp.Workbooks.Close();
                    IExcelApp.Quit();
                }

                NAR(IExcelRange);
                NAR(IExcelWorkSheet);
                NAR(IExcelWorkBook);
                NAR(IExcelApp);

                IExcelWorkSheet = null;
                IExcelWorkBook = null;
                IExcelApp = null;
                GC.Collect();
            }
        }

        private void NAR(object o)
        {
            //為了解決記憶體無法釋放
            try
            {
                if (o != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
            }
            finally
            {
                o = null;
            }
        }

        private void RunMacro(object oApp, object[] oRunArgs)
        {
            oApp.GetType().InvokeMember("Run", System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.InvokeMethod, null, oApp, oRunArgs);
        }
    }

    public class ExcelEdit
    {
        public string mFilename;
        public Excel.Application app;
        public Excel.Workbooks wbs;
        public Excel.Workbook wb;
        public Excel.Worksheets wss;
        public Excel.Worksheet ws;
        public ExcelEdit()
        {

        }

        public void Create()//建立Excel
        {
            app = new Excel.Application();
            wbs = app.Workbooks;
            wb = wbs.Add(true);
        }
        public void Open(string FileName)//打開Excel範本
        {
            app = new Excel.Application();
            wbs = app.Workbooks;
            wb = wbs.Add(FileName);
            mFilename = FileName;
        }

        public Excel.Worksheet GetSheet(string SheetName)
        //獲取一個工作表
        {
            Excel.Worksheet s = (Excel.Worksheet)wb.Worksheets[SheetName];
            return s;
        }

        public Excel.Worksheet AddSheet(string SheetName)
        //加入一個工作表
        {
            Excel.Worksheet s = (Excel.Worksheet)wb.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            s.Name = SheetName;
            return s;
        }

        //列印指定的工作簿
        public void Print(string SheetName)
        {
            Excel.Worksheet s = (Excel.Worksheet)wb.Worksheets[SheetName];
            s.Activate();
            s.PageSetup.PrintGridlines = false;
            s.PageSetup.CenterHorizontally = true;
            s.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4;
            s.PrintOut();
        }

        public void DelSheet(string SheetName)
        //刪除一個工作表
        {
            app.DisplayAlerts = false; //刪除時不出現提示框
            ((Excel.Worksheet)wb.Worksheets[SheetName]).Delete();
            app.DisplayAlerts = true;
        }

        public Excel.Worksheet ReNameSheet(string OldSheetName, string NewSheetName)
        //重命名一個工作表一
        {
            Excel.Worksheet s = (Excel.Worksheet)wb.Worksheets[OldSheetName];
            s.Name = NewSheetName;
            return s;
        }
        public Excel.Worksheet ReNameSheet(Excel.Worksheet Sheet, string NewSheetName)
        //重命名一個工作表二
        {
            Sheet.Name = NewSheetName;
            return Sheet;
        }
        //隐藏工作簿
        public void HiddenSheet(string SheetName)
        {
            Excel.Worksheet Sheet = (Excel.Worksheet)wb.Worksheets[SheetName];
            Sheet.Visible = XlSheetVisibility.xlSheetHidden;
            //return Sheet;
        }

        public void ControlEdit(string sSheetName, string sActiveControlName, bool bflag)
        {
            ws = (Excel.Worksheet)wb.Worksheets[sSheetName];
            Excel.OLEObject c = (Excel.OLEObject)ws.OLEObjects(sActiveControlName);
            MSForms.CheckBox d = (MSForms.CheckBox)c.Object;
            object o = bflag;
            d.set_Value(o);
        }

        public bool ControlGet(string sSheetName, string sActiveControlName)
        {
            ws = (Excel.Worksheet)wb.Worksheets[sSheetName];
            Excel.OLEObject c = (Excel.OLEObject)ws.OLEObjects(sActiveControlName);
            MSForms.CheckBox d = (MSForms.CheckBox)c.Object;
            return (bool)d.get_Value();
        }

        //ws：要設值的工作表  X行Y列 value值 
        public void SetCellValue(Excel.Worksheet ws, int x, int y, object value)
        {
            ws.Cells[y, x] = value;
        }
        public void SetCellValue(string SheetName, int x, int y, object value)
        {
            SetCellValue(GetSheet(SheetName), x, y, value);
        }

        //ws：要讀值的工作表  X行Y列 value值 
        public string ReadCellValue(Excel.Worksheet ws, int x, int y)
        {
            Excel.Range r = (Excel.Range)ws.Cells[y, x];
            return r.Text.ToString();
        }
        public string ReadCellValue(string SheetName, int x, int y)
        {
            return ReadCellValue(GetSheet(SheetName), x, y);
        }

        //設置一個單元格的字型(字體,大小,粗體)
        public void SetCellFont(Excel.Worksheet ws, int Startx, int Starty, int Endx, int Endy, int FontSize, string FontName, bool FontBold)
        {
            //name = "新細明體";
            //size = 12; 
            if (!string.IsNullOrEmpty(FontName))
                ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).Font.Name = FontName;     //字型
            ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).Font.Size = FontSize;         //大小
            ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).Font.Bold = FontBold;         //粗體    
        }
        public void SetCellFont(string SheetName, int Startx, int Starty, int Endx, int Endy, int FontSize, string FontName, bool FontBold)
        {
            Excel.Worksheet ws = GetSheet(SheetName);
            SetCellFont(ws, Startx, Starty, Endx, Endy, FontSize, FontName, FontBold);
        }

        //設置單元格的屬性(對齊方式)
        public void SetCellAlignment(Excel.Worksheet ws, int Startx, int Starty, int Endx, int Endy, string Vertical, string Horizontal)
        {
            //垂直對齊
            if (!string.IsNullOrEmpty(Vertical))
            {
                if (Vertical == "T")
                    ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
                else if (Vertical == "C")
                    ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                else if (Vertical == "B")
                    ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).VerticalAlignment = Excel.XlVAlign.xlVAlignBottom;
            }
            //水平對齊 
            if (!string.IsNullOrEmpty(Horizontal))
            {
                if (Horizontal == "L")
                    ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                else if (Horizontal == "C")
                    ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                else if (Horizontal == "R")
                    ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            }
        }
        /// <summary>
        /// 設置單元格的屬性(對齊方式)
        /// </summary>
        /// <param name="SheetName">SheetName</param>
        /// <param name="Startx">起始X</param>
        /// <param name="Starty">超始Y</param>
        /// <param name="Endx">結束X</param>
        /// <param name="Endy">結束Y</param>
        /// <param name="Vertical">垂直對齊 "T" "C" "B"</param>
        /// <param name="Horizontal">水平對齊 "L" "C" "R"</param>
        public void SetCellAlignment(string SheetName, int Startx, int Starty, int Endx, int Endy, string Vertical, string Horizontal)
        {
            Excel.Worksheet ws = GetSheet(SheetName);
            SetCellAlignment(ws, Startx, Starty, Endx, Endy, Vertical, Horizontal);
        }

        //設置單元格的屬性(自動調整列高)
        public void SetCellAutoFit(Excel.Worksheet ws, int Startx, int Starty, int Endx, int Endy, bool EntireRow, bool EntireColumn)
        {
            //自動調整列高 
            if (EntireRow)
                ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).EntireRow.AutoFit();

            //自動調整欄寬
            if (EntireColumn)
                ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).EntireColumn.AutoFit();
        }
        public void SetCellAutoFit(string SheetName, int Startx, int Starty, int Endx, int Endy, bool EntireRow, bool EntireColumn)
        {
            Excel.Worksheet ws = GetSheet(SheetName);
            SetCellAutoFit(ws, Startx, Starty, Endx, Endy, EntireRow, EntireColumn);
        }


        //設置單元格的字型或儲存格顏色
        public void SetCellColor(Excel.Worksheet ws, int Startx, int Starty, int Endx, int Endy, object FontColor, object CellColor)
        {
            //FontColor=Color.Red;
            if (FontColor != null)
                ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).Font.Color = System.Drawing.ColorTranslator.ToOle((System.Drawing.Color)FontColor);
            if (CellColor != null)
                ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).Interior.Color = System.Drawing.ColorTranslator.ToOle((System.Drawing.Color)CellColor);
        }
        public void SetCellColor(string SheetName, int Startx, int Starty, int Endx, int Endy, object FontColor, object CellColor)
        {
            Excel.Worksheet ws = GetSheet(SheetName);
            SetCellColor(ws, Startx, Starty, Endx, Endy, FontColor, CellColor);
        }

        //調整儲存格格式(設為文字格式)
        public void SetCellNumberFormat(Excel.Worksheet ws, int Startx, int Starty, int Endx, int Endy)
        {
            ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).NumberFormatLocal = "@";
        }
        public void SetCellNumberFormat(string SheetName, int Startx, int Starty, int Endx, int Endy)
        {
            Excel.Worksheet ws = GetSheet(SheetName);
            SetCellNumberFormat(ws, Startx, Starty, Endx, Endy);
        }

        //合倂單元格
        public void MergeCells(Excel.Worksheet ws, int x1, int y1, int x2, int y2)
        {
            ws.get_Range(ws.Cells[y1, x1], ws.Cells[y2, x2]).Merge(Type.Missing);
        }
        public void MergeCells(string SheetName, int x1, int y1, int x2, int y2)
        {
            MergeCells(GetSheet(SheetName), x1, y1, x2, y2);
        }

        //屏蔽报警
        public void ControlAlerts(bool b_flag)
        {
            app.DisplayAlerts = b_flag;
        }


        //將DataTable加到Excel指定工作表的指定位置
        public void AddTable(System.Data.DataTable dt, string SheetName, int startX, int startY)
        {
            AddTable(dt, GetSheet(SheetName), startX, startY);
        }
        public void AddTable(System.Data.DataTable dt, Excel.Worksheet ws, int startX, int startY)
        {
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {
                    ws.Cells[j + startY, i + startX] = dt.Rows[i][j];
                }
            }
        }

        //儲存格框線
        public void SetCellLineStyle(Excel.Worksheet ws, int Startx, int Starty, int Endx, int Endy, int LineStyle)
        {
            //LineStyle = 1;
            ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).Borders.LineStyle = LineStyle;
        }
        public void SetCellLineStyle(string SheetName, int Startx, int Starty, int Endx, int Endy, int LineStyle)
        {
            Excel.Worksheet ws = GetSheet(SheetName);
            SetCellLineStyle(ws, Startx, Starty, Endx, Endy, LineStyle);
        }

        //儲存格列高
        public void SetCellRowHeight(Excel.Worksheet ws, int Startx, int Starty, int Endx, int Endy, int LineStyle)
        {
            //LineStyle = 1;
            ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).RowHeight = LineStyle;
        }
        public void SetCellRowHeight(string SheetName, int Startx, int Starty, int Endx, int Endy, int LineStyle)
        {
            Excel.Worksheet ws = GetSheet(SheetName);
            SetCellRowHeight(ws, Startx, Starty, Endx, Endy, LineStyle);
        }

        //複製範圍並貼上
        public void CopyRange(Excel.Worksheet ws, int Startx, int Starty, int Endx, int Endy, Excel.Worksheet wp, int Pasetx, int Pasety)
        {
            ws.get_Range(ws.Cells[Starty, Startx], ws.Cells[Endy, Endx]).Copy(Type.Missing);
            wp.get_Range(wp.Cells[Pasety, Pasetx], wp.Cells[Pasety, Pasetx]).Select();
            wp.Paste(Type.Missing, Type.Missing);
        }
        public void CopyRange(string SheetName, int Startx, int Starty, int Endx, int Endy, string PasteSheetName, int Pasetx, int Pasety)
        {
            Excel.Worksheet ws = GetSheet(SheetName);
            Excel.Worksheet wp = GetSheet(PasteSheetName);
            CopyRange(ws, Startx, Starty, Endx, Endy, wp, Pasetx, Pasety);
        }
        //插入一行
        public void InsertRange(string SheetName, int iRowIndex)
        {
            Excel.Worksheet ws = GetSheet(SheetName);
            InsertRange(ws, iRowIndex);
        }
        public void InsertRange(Excel.Worksheet ws, int iRowIndex)
        {
            //for (int i = 1; i < iRows; i++)
            //{
            Excel.Range CopyInsert_Range = (Excel.Range)ws.Rows.get_Item(iRowIndex, Type.Missing);
            CopyInsert_Range.Copy(CopyInsert_Range);
            CopyInsert_Range.Rows.Insert(XlInsertShiftDirection.xlShiftDown);
            //}
        }

        //巨集
        private void RunMacro(object oApp, object[] oRunArgs)
        {
            oApp.GetType().InvokeMember("Run", System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.InvokeMethod, null, oApp, oRunArgs);
        }
        public void RunMacro(string sMacroName)
        {
            RunMacro(app, new object[] { sMacroName });
        }

        public void InsertActiveChart(Excel.XlChartType ChartType, string SheetName, int DataSourcesX1, int DataSourcesY1, int DataSourcesX2, int DataSourcesY2, Excel.XlRowCol ChartDataType)
        //插入圖片操作
        {
            ChartDataType = Excel.XlRowCol.xlColumns;
            wb.Charts.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            {
                wb.ActiveChart.ChartType = ChartType;
                wb.ActiveChart.SetSourceData(GetSheet(SheetName).get_Range(GetSheet(SheetName).Cells[DataSourcesY1, DataSourcesX1], GetSheet(SheetName).Cells[DataSourcesX2, DataSourcesY2]), ChartDataType);
                wb.ActiveChart.Location(Excel.XlChartLocation.xlLocationAsObject, SheetName);
            }
        }

        public bool Save()
        //儲存文件
        {
            if (mFilename == "")
            {
                return false;
            }

            try
            {
                wb.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool SaveAs(object FileName)
        //文件另存為
        {
            if (mFilename == "")
            {
                return false;
            }

            try
            {
                wb.SaveAs(FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                // wb.PrintOut(Type.Missing, Type.Missing, 1, false, Type.Missing, false, false, Type.Missing);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Close()
        //關閉Excel,釋放Excel
        {
            wb.Close(false, Type.Missing, Type.Missing);
            wbs.Close();
            app.Quit();

            NAR(wbs);
            NAR(wb);
            NAR(app);

            wb = null;
            wbs = null;
            app = null;
            GC.Collect();
        }
        private void NAR(object o)
        {
            //為了解決記憶體無法釋放
            try
            {
                if (o != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
            }
            finally
            {
                o = null;
            }
        }
    }
}
