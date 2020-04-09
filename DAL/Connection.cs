using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace DAL
{
   public class Connection
    {
       public static string ConnectionStr()
       {
           string basestr = "CoreBase";
           return basestr;
       }
       public static string userid()
       {
           string basestr = ConfigurationSettings.AppSettings["userid"];
           return basestr;
       }
    }
}
