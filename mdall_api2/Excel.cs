using mdall_api2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdall_excel
{
    //public class File
    //{
    //    public Microsoft.Office.Interop.Excel.Application File { get; set; }

    //    private Microsoft.Office.Interop.Excel.Worksheet _currentWorkSheet { get; set; }
    //    private Microsoft.Office.Interop.Excel.Workbook _currentWorkBook { get; set; }

    //    public Excel()
    //    {
    //        File = new Microsoft.Office.Interop.Excel.Application();
    //    }

    //    public void AddCompany(Company company)
    //    {
    //        _currentWorkBook = File.Workbooks.Add();
    //        _currentWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)File.ActiveSheet;

    //        _currentWorkSheet.Cells[1, "A"] = company.company_name;

    //    }

    //    public void Save(string fileName)
    //    {
    //        _currentWorkBook.SaveAs(@$"C:\Desktop\{fileName}", Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook);
    //    }

    //}
}
