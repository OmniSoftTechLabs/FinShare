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
    public class ImportExcelDAL
    {
        string cnstr;
        string userid;
        public ImportExcelDAL()
        {
            cnstr = Connection.ConnectionStr();
            userid = Connection.userid();
        }

        public string InsertUpdate(DataTable dt, string AMCCode, ImportExcel objen, int optype)
        {
            string msg = string.Empty;
            //   DbCommand DbCommand = default(DbCommand);
            try
            {
                Database ds = default(Database);

                ds = DatabaseFactory.CreateDatabase(cnstr);
                DataTable dttemp = new DataTable();

                dttemp.Columns.Add("ID", typeof(System.Guid));
                dttemp.Columns.Add("AccNumber", typeof(System.String));
                dttemp.Columns.Add("Amount", typeof(System.Decimal));
                Guid TempID = Guid.NewGuid();
                if (AMCCode.ToUpper() == "CAMS") //CAMS
                {

                    DataRow dr;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[i]["folio_no"].ToString()) && !string.IsNullOrEmpty(dt.Rows[i]["brkage_amt"].ToString()))
                        {
                            decimal decValue;
                            if (decimal.TryParse(dt.Rows[i]["brkage_amt"].ToString(), out decValue))
                            {

                                dr = dttemp.NewRow();
                                dr["ID"] = TempID;
                                dr["AccNumber"] = dt.Rows[i]["folio_no"];
                                dr["Amount"] = dt.Rows[i]["brkage_amt"];
                                dttemp.Rows.Add(dr);
                            }
                        }
                    }
                }
                else if (AMCCode.ToUpper() == "KARVY")//Karvy
                {

                    DataRow dr;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[i]["Account Number"].ToString()) && !string.IsNullOrEmpty(dt.Rows[i]["Gross Brokerage"].ToString()))
                        {
                            decimal decValue;
                            if (decimal.TryParse(dt.Rows[i]["Gross Brokerage"].ToString(), out decValue))
                            {
                                dr = dttemp.NewRow();
                                dr["ID"] = TempID;
                                dr["AccNumber"] = dt.Rows[i]["Account Number"];
                                dr["Amount"] = dt.Rows[i]["Gross Brokerage"];
                                dttemp.Rows.Add(dr);
                            }
                        }
                    }
                }
                else if (AMCCode.ToUpper() == "FRANKLINE")//Frankline
                {

                    DataRow dr;
                    for (int i = 2; i < dt.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[i][3].ToString()) && !string.IsNullOrEmpty(dt.Rows[i][18].ToString()))
                        {
                            decimal decValue;
                            if (decimal.TryParse(dt.Rows[i][18].ToString(), out decValue))
                            {
                                dr = dttemp.NewRow();
                                dr["ID"] = TempID;
                                dr["AccNumber"] = dt.Rows[i][3];//["ACCOUNTNO"];
                                dr["Amount"] = dt.Rows[i][18];//["BROKERAGE"];
                                dttemp.Rows.Add(dr);
                            }
                        }
                    }
                }
                if (dttemp.Rows.Count > 0)
                {
                    SqlTransaction tr;

                    SqlConnection connection;
                    connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[cnstr].ConnectionString);
                    connection.Open();
                    tr = connection.BeginTransaction();
                    try
                    {
                        using (var command = new SqlCommand("USP_tbl_Temp_Import_Type_Insert") { CommandType = CommandType.StoredProcedure })//USP_Trn_Emp_Attendance_Summary_InsertTable
                        {
                            command.CommandTimeout = 5000;
                            command.Connection = connection;
                            command.Parameters.Add(new SqlParameter("@tbltype", dttemp));
                            command.Transaction = tr;
                            object obj = command.ExecuteNonQuery();

                            command.Parameters.Clear();

                            command.ResetCommandTimeout();// = 5000;
                            command.Connection = connection;
                            command.CommandText = "USP_tbltrnImport_Insert";
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Add(new SqlParameter("@Month", objen.Month));
                            command.Parameters.Add(new SqlParameter("@Year", objen.Year));
                            command.Parameters.Add(new SqlParameter("@AMCID", new Guid(Convert.ToString(objen.AMCID.ToString().Split('$')[0]))));
                            command.Parameters.Add(new SqlParameter("@EntBy", userid));
                            command.Parameters.Add(new SqlParameter("@TempID", TempID));
                            command.Parameters.Add(new SqlParameter("@optype", optype));
                            
                            command.Transaction = tr;

                            object obj1 = command.ExecuteNonQuery();
                            tr.Commit();

                        }
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        throw ex;
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // DbCommand.Dispose();
            }
            return msg;
        }

        public List<AMC> GetAMCList()
        {
            List<AMC> enobjlst = new List<AMC>();
            DbCommand DbCommand = default(DbCommand);

            try
            {
                Database ds = DatabaseFactory.CreateDatabase(cnstr);
                DbCommand = ds.GetStoredProcCommand("USP_tblMstAMC_GetAll");

                using (IDataReader dataReader = ds.ExecuteReader(DbCommand))
                {
                    while (dataReader.Read())
                    {

                        AMC objen = new AMC();
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

        public int ConfirmUpload(ImportExcel objen)
        {
            Int32 uploadcount = 0;
            DbCommand DbCommand = default(DbCommand);
            try
            {
                Database ds = DatabaseFactory.CreateDatabase(cnstr);
                DbCommand = ds.GetStoredProcCommand("USP_tbltrnImport_ConfirmUpload");
                ds.AddInParameter(DbCommand, "AMCID", DbType.Guid, new Guid(objen.AMCID.ToString().Split('$')[0]));
                ds.AddInParameter(DbCommand, "Month", DbType.Int32, objen.Month);
                ds.AddInParameter(DbCommand, "Year", DbType.Int32, objen.Year);
                uploadcount = Convert.ToInt32(Convert.ToString((ds.ExecuteScalar(DbCommand))));
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
            return uploadcount;
        }

    }
}
