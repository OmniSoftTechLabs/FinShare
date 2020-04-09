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
    public class ClientDAL
    {
        string cnstr;
        string userid;
        public ClientDAL()
        {
            cnstr = Connection.ConnectionStr();
            userid = Connection.userid();
        }
        public string InsertUpdate(Client objEn, Int32 OpType)
        {
            string msg = string.Empty;
            DbCommand DbCommand = default(DbCommand);
            try
            {
                Database ds = default(Database);

                ds = DatabaseFactory.CreateDatabase(cnstr);

                DbCommand = ds.GetStoredProcCommand("USP_tblMstClient_InsertUpdate");

                ds.AddInParameter(DbCommand, "ClientID", DbType.Guid, objEn.ClientID);
                ds.AddInParameter(DbCommand, "ClientName", DbType.String, objEn.ClientName);
                ds.AddInParameter(DbCommand, "ClientAlieas", DbType.String, objEn.ClientAlieas);
                ds.AddInParameter(DbCommand, "Address", DbType.String, objEn.Address);
                ds.AddInParameter(DbCommand, "City", DbType.String, objEn.City);
                ds.AddInParameter(DbCommand, "State", DbType.String, objEn.State);
                ds.AddInParameter(DbCommand, "PinCode", DbType.Decimal, objEn.PinCode);
                ds.AddInParameter(DbCommand, "PANNo", DbType.String, objEn.PANNo);
                ds.AddInParameter(DbCommand, "BankName", DbType.String, objEn.BankName);
                ds.AddInParameter(DbCommand, "AccountNo", DbType.String, objEn.AccountNo);
                ds.AddInParameter(DbCommand, "IFSC", DbType.String, objEn.IFSC);
                ds.AddInParameter(DbCommand, "MICR", DbType.String, objEn.MICR);
                ds.AddInParameter(DbCommand, "EmailID", DbType.String, objEn.EmailID);
                ds.AddInParameter(DbCommand, "ContactNo1", DbType.String, objEn.ContactNo1);
                ds.AddInParameter(DbCommand, "ContactNo2", DbType.String, objEn.ContactNo2);
                ds.AddInParameter(DbCommand, "LG1", DbType.Guid, objEn.LG1);
                ds.AddInParameter(DbCommand, "LG2", DbType.Guid, objEn.LG2);
                ds.AddInParameter(DbCommand, "LG1Share", DbType.Decimal, objEn.LG1Share);
                ds.AddInParameter(DbCommand, "LG2Share", DbType.Decimal, objEn.LG2Share);
                ds.AddInParameter(DbCommand, "BirthDate", DbType.DateTime, objEn.BirthDate);
                ds.AddInParameter(DbCommand, "Nominee", DbType.String, objEn.Nominee);
                ds.AddInParameter(DbCommand, "EntBy", DbType.Guid, new Guid(userid));
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

        public int Delete(Guid ClientId)
        {
            int i = new int();
            DbCommand DbCommand = default(DbCommand);
            try
            {
                Database ds = default(Database);
                ds = DatabaseFactory.CreateDatabase(cnstr);
                DbCommand = ds.GetStoredProcCommand("USP_tblMstClient_Delete");
                ds.AddInParameter(DbCommand, "ClientId", DbType.Guid, ClientId);
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

        public IEnumerable<Client> GetAll()
        {
            List<Client> enobjlst = new List<Client>();
            DbCommand DbCommand = default(DbCommand);

            try
            {
                Database ds = DatabaseFactory.CreateDatabase(cnstr);
                DbCommand = ds.GetStoredProcCommand("USP_tblMstClient_GetAll");

                using (IDataReader dataReader = ds.ExecuteReader(DbCommand))
                {
                    while (dataReader.Read())
                    {

                        Client objen = new Client();
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

        public Client GetSingle(Guid ClientId)
        {
            Client entobj1 = new Client();
            DbCommand DbCommand = default(DbCommand);
            try
            {
                Database ds = DatabaseFactory.CreateDatabase(cnstr);
                DbCommand = ds.GetStoredProcCommand("USP_tblMstClient_GetByID");
                ds.AddInParameter(DbCommand, "ClientId", DbType.Guid, ClientId);
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

        public List<ClientAccMapper> GetAcountNumberByClinetId(Guid? ClientId)
        {
            List<ClientAccMapper> enobjlst = new List<ClientAccMapper>();

            DbCommand DbCommand = default(DbCommand);

            try
            {
                Database ds = DatabaseFactory.CreateDatabase(cnstr);
                DbCommand = ds.GetStoredProcCommand("USP_tblClientAccMapper_GetByID");
                ds.AddInParameter(DbCommand, "ClientId", DbType.Guid, ClientId);
                using (IDataReader dataReader = ds.ExecuteReader(DbCommand))
                {
                    while (dataReader.Read())
                    {

                        ClientAccMapper objen = new ClientAccMapper();
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

        public string InsertAccountNumber(ClientAccMapper objEn)
        {
            string msg = string.Empty;
            DbCommand DbCommand = default(DbCommand);
            try
            {
                Database ds = default(Database);

                ds = DatabaseFactory.CreateDatabase(cnstr);

                DbCommand = ds.GetStoredProcCommand("USP_tblClinetAccMapper_Insert");

                ds.AddInParameter(DbCommand, "ClientID", DbType.Guid, objEn.ClientID);
                ds.AddInParameter(DbCommand, "AccNumber", DbType.String, objEn.AccountNumber);
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

        public int DeleteAccountNumber(int Uid)
        {
            int i = new int();
            DbCommand DbCommand = default(DbCommand);
            try
            {
                Database ds = default(Database);
                ds = DatabaseFactory.CreateDatabase(cnstr);
                DbCommand = ds.GetStoredProcCommand("USP_tblClientAccMapper_Delete");
                ds.AddInParameter(DbCommand, "Uid", DbType.Int32, Uid);
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
