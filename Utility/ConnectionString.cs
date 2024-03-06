using Microsoft.EntityFrameworkCore;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Event.Utility
{
    //public class ConnectionString 
   
        //private static string cName = "Data Source=DESKTOP-8RADP4C\\SQLEXPRESS;Initial Catalog=Event;Integrated Security=True;Trust Server Certificate=True";
        //public static string Connection { get => cName; }
        public class ConnectionString
    {
            private static IConfiguration configuration;
            static ConnectionString()
            {
                var builder = new ConfigurationBuilder()
                   .SetBasePath(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location))
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                configuration = builder.Build();
            }

            public static string Get(string name)
            {
                string appSettings = configuration[name];
                return appSettings;
            }

        }
    }
