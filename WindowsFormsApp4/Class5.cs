using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using excel = Microsoft.Office.Interop.Excel;

namespace WindowsFormsApp4
{
    class ExcelReaderWriter
    {
        excel.Application application;
        excel.Workbook workbook;
        excel.Sheets sheets;
        excel.Worksheet worksheet;
        bool opened;

        string file;
        public ExcelReaderWriter(string filepath)
        {
            if (File.Exists(filepath))
            {
                try
                {
                    application = new excel.Application();
                    workbook = application.Workbooks.Open(filepath);
                    sheets = workbook.Sheets;
                    worksheet = sheets[0];
                    opened = true;
                    file = filepath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ExcelReaderWriter(): " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    application = new excel.Application();
                    workbook = application.Workbooks.Add();
                    worksheet = workbook.ActiveSheet;
                    worksheet.Name = "Multiple YAxis Data";
                    opened = true;
                    file = filepath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ExcelReaderWriter(): " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// find next nonexistent filename like "TestExcel#.xlsx"
        /// </summary>
        /// <returns></returns>
        public static string FindNextFileName()
        {
            string ret = string.Empty;
            int i = 0;
            bool cont = true;

            while (cont)
            {
                string path = @"C:\Users\dominik.schneider\Documents\YAxisAppFiles\" + "TestExcel" + i + ".xlsx";
                if (!File.Exists(path))
                {
                    ret = path;
                    cont = false;
                }
                else
                {
                    i++;
                }
            }

            return ret;
        }

        /// <summary>
        /// quit app.
        /// set opened to false.
        /// </summary>
        public void Quit()
        {
            workbook.Close(0);

            application.Quit();

            opened = false;
        }

        /// <summary>
        /// return value and color of input cell.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public Tuple<string, Color> ReadCell(string cell)
        {
            string value = string.Empty;
            Color color = Color.Transparent;

            if (opened)
            {
                try
                {
                    value = worksheet.Cells[cell].Value2.ToString();
                    color = worksheet.Cells[cell].Style.Fill.BackgroundColor;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ReadCell(): " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("App closed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Tuple<string, Color> ret = new Tuple<string, Color>(value, color);

            return ret;
        }

        /// <summary>
        /// return value and color of input cell.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public Tuple<string, Color> ReadCell(int row, int column)
        {
            string value = string.Empty;
            Color color = Color.Transparent;

            if (opened)
            {
                try
                {
                    value = worksheet.Cells[row, column].Value2.ToString();
                    color = worksheet.Cells[row, column].Style.Fill.BackgroundColor;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ReadCell(): " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("App closed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Tuple<string, Color> ret = new Tuple<string, Color>(value, color);

            return ret;
        }

        /// <summary>
        /// write string to input cell.
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="value"></param>
        public void WriteCell(string cell, string value)
        {

            if (opened)
            {
                try
                {
                    excel.Range range = worksheet.Cells[cell];
                    range.Value2 = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("WriteCell(): " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("App closed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// write double value to input cell.
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="value"></param>
        public void WriteCell(string cell, double value)
        {

            if (opened)
            {
                try
                {
                    excel.Range range = worksheet.Cells[cell];
                    range.Value2 = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("WriteCell(): " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("App closed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// write string to input cell.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        public void WriteCell(int row, int column, string value)
        {

            if (opened)
            {
                try
                {
                    excel.Range range = worksheet.Cells[row, column];
                    range.Value2 = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("WriteCell(): " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("App closed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// write double value to input cell.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        public void WriteCell(int row, int column, double value)
        {

            if (opened)
            {
                try
                {
                    excel.Range range = worksheet.Cells[row, column];
                    range.Value2 = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("WriteCell(): " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("App closed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// write int value to input cell.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        public void WriteCell(int row, int column, int value)
        {
            if (opened)
            {
                try
                {
                    excel.Range range = worksheet.Cells[row, column];
                    range.Value2 = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("WriteCell(): " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("App closed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// set background color of input cell.
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="color"></param>
        public void SetColor(string cell, Color color)
        {
            if (opened)
            {
                try
                {
                    worksheet.Cells[cell].Style.Fill.SetCellsColor(color);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SetColor(): " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("App closed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// set background color of input cell.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="color"></param>
        public void SetColor(int row, int column, Color color)
        {
            if (opened)
            {
                try
                {
                    worksheet.Cells[row, column].Style.Fill.SetCellsColor(color);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SetColor(): " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("App closed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// save changes to filepath.
        /// </summary>
        public void SaveChanges()
        {            
            try
            {
                if (File.Exists(file))
                {
                    workbook.Save();
                }
                else
                {
                    workbook.SaveAs(file);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("SaveFile(): " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
