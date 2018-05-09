using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace a3
{
    class Excel
    {
        string path = "";
        _Application excel = new _Excel.Application();
        Workbook wb;
        Worksheet ws;
        Range wr;
        public int _userCount = 0;
        public Excel(string path, int Sheet)
        {
            this.path = path;
            wb = excel.Workbooks.Open(path);
            ws = wb.Worksheets[Sheet];
            wr = ws.UsedRange;
        }
        public List<_App_User> ReadCell()
        {
            _userCount = 0;
            List<_App_User> _items = new List<_App_User>();
            int rowCount = wr.Rows.Count;
            int colCount = wr.Columns.Count;

            for (int i = 1; i <= rowCount; i++)
            {
                _userCount++;
                _App_User appUser = new _App_User();

                string hold_id = (wr.Cells[i, 1].Value2.ToString());
                try
                {
                    int id = Int32.Parse(hold_id);
                    appUser.ID = id;
                }
                catch (FormatException) { throw new FormatException();}

                appUser.accountNumber = wr.Cells[i, 2].Value2.ToString();
                appUser.college = wr.Cells[i, 3].Value2.ToString();
                appUser.course = wr.Cells[i, 4].Value2.ToString();
                appUser.firstName = wr.Cells[i, 5].Value2.ToString();
                appUser.lastName = wr.Cells[i, 6].Value2.ToString();
                appUser.middleName = wr.Cells[i, 7].Value2.ToString();
                appUser.status = wr.Cells[i, 8].Value2.ToString();
                appUser.year = wr.Cells[i, 9].Value2.ToString();
                appUser.password = wr.Cells[i, 10].Value2.ToString();
                _items.Add(appUser);


            }
            return _items;
        }
        public void cleanCOMobjects()
        {
            Marshal.ReleaseComObject(wr);
            Marshal.ReleaseComObject(ws);
            wb.Close();
            Marshal.ReleaseComObject(wb);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Console.WriteLine("Cleaning COM Objects finished.");
        }

    }
}
