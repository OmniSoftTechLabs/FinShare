using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
namespace DAL
{
    public class ReportDAL
    {
        string cnstr;
        string userid;
        public ReportDAL()
        {
            cnstr = Connection.ConnectionStr();
            userid = Connection.userid();
        }

        public IEnumerable<ReportEn> GetAll()
        {
            List<ReportEn> enobjlst = new List<ReportEn>();
            DbCommand DbCommand = default(DbCommand);

            try
            {
                Database ds = DatabaseFactory.CreateDatabase(cnstr);
                DbCommand = ds.GetStoredProcCommand("USP_tbltrnImport_GetAll");

                using (IDataReader dataReader = ds.ExecuteReader(DbCommand))
                {
                    while (dataReader.Read())
                    {

                        ReportEn objen = new ReportEn();
                        DAL.EntityUtils.PopulateEntity(objen, dataReader);
                        enobjlst.Add(objen);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbCommand.Dispose();
            }
            return enobjlst;
        }

        public DataTable GetReportData(int Month, int Year)
        {
            DataTable dt = new DataTable();
            Database ds = default(Database);
            ds = DatabaseFactory.CreateDatabase("CoreBase");
            DbCommand DbCommand = default(DbCommand);
            try
            {

                DbCommand = ds.GetStoredProcCommand("USP_Rpt_ClientwiseLGShare");
                ds.AddInParameter(DbCommand, "FromMonth", DbType.Int32, Month);
                ds.AddInParameter(DbCommand, "FromYear", DbType.Int32, Year);
                DbCommand.CommandTimeout = 0;
                StringBuilder readerData = new StringBuilder();
                using (IDataReader dataReader = ds.ExecuteReader(DbCommand))
                {
                    dt.Load(dataReader);
                }
                DbCommand.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbCommand.Parameters.Clear();

            }
            return dt;
        }

        public int DeleteReportDate(int Month, int year)
        {
            int i = new int();
            DbCommand DbCommand = default(DbCommand);
            try
            {
                Database ds = default(Database);
                ds = DatabaseFactory.CreateDatabase(cnstr);
                DbCommand = ds.GetStoredProcCommand("USP_Rpt_Monthwise_Delete");
                ds.AddInParameter(DbCommand, "month", DbType.Int32, Month);
                ds.AddInParameter(DbCommand, "year", DbType.Int32, year);
                i = ds.ExecuteNonQuery(DbCommand);
                DbCommand.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbCommand.Dispose();
            }
            return i;
        }
    }
}
