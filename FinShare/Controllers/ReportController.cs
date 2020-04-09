using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using PagedList;
using PagedList.Mvc;
using DAL;
using System.Text;
namespace FinShare.Controllers
{
    public class ReportController : Controller
    {
        
        // GET: /Report/
        public ActionResult Index(string month, string year ,int? page)
        {

            ReportDAL objdal = new ReportDAL();
            List<DAL.ReportEn> objen = new List<ReportEn>();
            objen = objdal.GetAll().ToList();
            
            if (!string.IsNullOrEmpty(month))
                objen = objen.Where(x => x.Month.ToString().Equals(month)).ToList();
            if (!string.IsNullOrEmpty(year))
                objen = objen.Where(x => x.Year.ToString().Equals(year)).ToList();
            
            return View(objen.ToPagedList(page ?? 1, 10));
            
             
        }
        [HttpGet]
        public ActionResult GetReport(int Month,int Year, string ReportName )
        {
            //Fill dataset with records
           // DataSet dataSet = GetRecordsFromDatabase();
            ReportDAL objdal = new ReportDAL();
            
            DataTable dt = objdal.GetReportData(Month,Year);
            StringBuilder sb = new StringBuilder();

            //Response.ContentType = "application/vnd.ms-excel";
            //Response.AddHeader("content-disposition", "attachment;filename=" + ReportName.ToString() + ".xls");

           

            sb.Append("<table border='1'>");

            //LINQ to get Column names
            var columnName = dt.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();
            sb.Append("<tr>");
            //Looping through the column names
            foreach (var col in columnName)
                sb.Append("<th>" + col + "</th>");
            sb.Append("</tr>");
            int colcnt= dt.Columns.Count-2;
            decimal[] intArray;
            intArray = new decimal[colcnt];  
            //Looping through the records
            int cnt = 0;
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("<tr>");
                cnt = 0;
                foreach (DataColumn dc in dt.Columns)
                {
                    sb.Append("<td>" + dr[dc] + "</td>");
                    if (cnt >= 2)
                        intArray[cnt - 2] = string.IsNullOrEmpty(intArray[cnt - 2].ToString()) ? 0 : intArray[cnt - 2] + Convert.ToDecimal(DBNull.Value ==dr[dc] ? 0 : dr[dc]);
                    cnt++;
                }
                sb.Append("</tr>");
            }
            sb.Append("<tfoot  style='font:bold'><tr>");
            sb.Append("<th></th>");
            sb.Append("<th>Total</th>");
            for (int i = 0; i < colcnt ; i++)
            {
                sb.Append("<th>"+intArray[i]  +"</th>");
            }
            sb.Append("</tr></tfoot>");
            sb.Append("</table>");

            //Writing StringBuilder content to an excel file.
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + ReportName.ToString() + ".xls");

            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.AppendHeader("content-disposition", "attachment; filename=" + ReportName.ToString() + ".xlsx");


//            ---------------------------------------------------
            Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
            Response.Write("<head>");
            Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            Response.Write("<!--[if gte mso 9]><xml>");
            Response.Write("<x:ExcelWorkbook>");
            Response.Write("<x:ExcelWorksheets>");
            Response.Write("<x:ExcelWorksheet>");
            Response.Write("<x:Name>Report Data</x:Name>");
            Response.Write("<x:WorksheetOptions>");
            Response.Write("<x:Print>");
            Response.Write("<x:ValidPrinterInfo/>");
            Response.Write("</x:Print>");
            Response.Write("</x:WorksheetOptions>");
            Response.Write("</x:ExcelWorksheet>");
            Response.Write("</x:ExcelWorksheets>");
            Response.Write("</x:ExcelWorkbook>");
            Response.Write("</xml>");
            Response.Write("<![endif]--> ");

            //            ---------------------------------------------------
                               
          //  Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.AppendHeader("content-disposition", "attachment; filename=" + ReportName.ToString() + ".xlsx");
          //  Response.AppendHeader("content-disposition", "attachment; filename=\"Name.xlsx\"");
            Response.Write(sb.ToString());
            Response.Flush();
            Response.End();
            //Response.Close();

            return View();
        }

	}
}