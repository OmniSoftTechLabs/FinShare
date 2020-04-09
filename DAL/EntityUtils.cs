using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;

namespace DAL
{
   public class EntityUtils
    {
       public static void PopulateEntity<T>(T entity, IDataRecord record)
       {
           if (record != null && record.FieldCount > 0)
           {
               Type type = entity.GetType();

               for (int i = 0; i < record.FieldCount; i++)
               {
                   if (DBNull.Value != record[i])
                   {
                       try
                       {
                           PropertyInfo property = type.GetProperty(record.GetName(i), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                           if (property != null)
                           {
                               property.SetValue(entity, record[property.Name], null);
                           }
                       }
                       catch (Exception ex)
                       {
                           throw ex;
                       }
                   }
               }
           }
       }
    }
}
