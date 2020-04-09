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
    public class LeadGeneratorDAL
    {
        string cnstr;
        string userid;
        public LeadGeneratorDAL()
        {
            cnstr = Connection.ConnectionStr();
            userid = Connection.userid();
        }
        /// <summary>
        /// This Method is used for Insert and Update of LeadGeneator Master
        /// </summary>
        /// <param name="objEn"></param>
        /// <param name="OpType"></param>
        /// <returns></returns>
        public string InsertUpdate(LeadGenerator objEn, Int32 OpType)
        {
            string msg = string.Empty;
            DbCommand DbCommand = default(DbCommand);
            try
            {
                Database ds = default(Database);

                ds = DatabaseFactory.CreateDatabase(cnstr);

                DbCommand = ds.GetStoredProcCommand("USP_tblMstLeadGenerator_InsertUpdate");

                ds.AddInParameter(DbCommand, "LeadGeneratorID", DbType.Guid, objEn.LeadGeneratorID);
                ds.AddInParameter(DbCommand, "LeadGeneratorName", DbType.String, objEn.LeadGeneratorName);
                ds.AddInParameter(DbCommand, "ContactNo1", DbType.String, objEn.ContactNo1);
                ds.AddInParameter(DbCommand, "ContactNo2", DbType.String, objEn.ContactNo2);
                ds.AddInParameter(DbCommand, "EmailID", DbType.String, objEn.EmailID);
                //ds.AddInParameter(DbCommand, "Share", DbType.Decimal, objEn.Share);
                ds.AddInParameter(DbCommand, "EntBy", DbType.Guid, new Guid( userid));
                ds.AddInParameter(DbCommand, "Change", DbType.Int32, OpType);
                msg = ds.ExecuteNonQuery(DbCommand).ToString();
                DbCommand.Parameters.Clear();
                msg = "success";

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbCommand.Dispose();
            }
            return msg;
        }
        /// <summary>
        /// This Method is used to delete LeadGenerator
        /// </summary>
        /// <param name="LeadGeneratorID"></param>
        /// <returns></returns>
        public int Delete(Guid LeadGeneratorID)
        {
            int i = new int();
            DbCommand DbCommand = default(DbCommand);
            try
            {
                Database ds = default(Database);
                ds = DatabaseFactory.CreateDatabase(cnstr);
                DbCommand = ds.GetStoredProcCommand("USP_tblMstLeadGenerator_Delete");
                ds.AddInParameter(DbCommand, "LeadGeneratorID", DbType.Guid, LeadGeneratorID);
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
        /// <summary>
        /// This Method is used to Retrive all the entries of LeadGenerator
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LeadGenerator> GetAll()
        {
            List<LeadGenerator> enobjlst = new List<LeadGenerator>();
            DbCommand DbCommand = default(DbCommand);

            try
            {
                Database ds = DatabaseFactory.CreateDatabase(cnstr);
                DbCommand = ds.GetStoredProcCommand("USP_tblMstLeadGenerator_GetAll");

                using (IDataReader dataReader = ds.ExecuteReader(DbCommand))
                {
                    while (dataReader.Read())
                    {

                        LeadGenerator objen = new LeadGenerator();
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
        /// <summary>
        /// This Method is used to get Single Entry of Lead Generator By LeadGeneratorID
        /// </summary>
        /// <param name="LGId"></param>
        /// <returns></returns>
        public LeadGenerator GetSingle(Guid LGId)
        {
            LeadGenerator entobj1 = new LeadGenerator();
            DbCommand DbCommand = default(DbCommand);
            try
            {
                Database ds=DatabaseFactory.CreateDatabase(cnstr);
                DbCommand = ds.GetStoredProcCommand("USP_tblMstLeadGenerator_GetByID");
                ds.AddInParameter(DbCommand, "LeadGeneratorID", DbType.Guid, LGId);
                IDataReader reader = (ds.ExecuteReader(DbCommand));
                DbCommand.Parameters.Clear();

                if (reader.Read())
                {
                    DAL.EntityUtils.PopulateEntity(entobj1, reader);
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
            return entobj1;
        }
    }
}
