using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web;
using System.Xml;

namespace DevKido.Utilities.Error
{
    /// <summary>
    ///  <appSettings>
    ///    <add key = "ErrorCon" value="data source=servername;initial catalog=dbname;user id=dbusername;password=dbbpassword;"></add>
    ///  </appSettings>
    /// </summary>
    public class ErrorLog
    {
        /// <summary>
        /// Add exception to database. sepecify connection string in ErrorConnection key..this will automatically create tables if there not exists.
        /// </summary>
        /// <param name="ex">Exception</param>
        public static void LogInsert(Exception ex, string fileName = "", string methodName = "")
        {
            ErrorModel errorModel = new ErrorModel
            {
                Active = true,
                CommandType = "",
                ErrorId = 0,
                ErrorType = "General",
                Exception = ex.Message,
                FileName = fileName,
                MethodName = methodName,
                InnerException = Convert.ToString(ex.InnerException),
                PageName = Convert.ToString(ex.StackTrace),
                RequestId = 0,
                RequestURL = Convert.ToString(HttpContext.Current?.Request?.Url),
                SqlParam = "",
                SqlQuery = "",
                TotalSeconds = 0,
                UserAgent = HttpContext.Current?.Request?.UserAgent,
                UserName = ""
            };
            InsertError(errorModel);
        }
        
        /// <summary>
        /// Add exception to database with Custom Type. sepecify connection string in ErrorConnection key..this will automatically create tables if there not exists.
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="type">Exception Type</param>
        public static void LogInsert(Exception ex, string type, string userName, string fileName = "", string methodName = "")
        {
            ErrorModel errorModel = new ErrorModel
            {
                Active = true,
                CommandType = "",
                ErrorId = 0,
                ErrorType = type,
                Exception = ex.Message,
                FileName = fileName,
                MethodName = methodName,
                InnerException = Convert.ToString(ex.InnerException),
                PageName = Convert.ToString(ex.StackTrace), 
                RequestId = 0,
                RequestURL = Convert.ToString(HttpContext.Current.Request.Url),
                SqlParam = "",
                SqlQuery = "",
                TotalSeconds = 0,
                UserAgent = HttpContext.Current.Request.UserAgent,
                UserName = userName
            };
            InsertError(errorModel);
        }

        /// <summary>
        /// <add key="writelog" value="true" />
        /// <add key = "logpath" value="D:\ErrorLog\" />
        /// </summary>
        /// <param name="LogValues"></param>
        public static string WriteLog(string LogValues)
        {
            try
            {
                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["writelog"].ToString()))
                    return "writelog = false";
                // for web application
                var folder = HttpRuntime.AppDomainAppPath //System.Web.HttpContext.Current.Request.PhysicalApplicationPath.ToString() 
                    +  "\\ErrorLog\\"; 
                //For Static path
               // var folder = ConfigurationManager.AppSettings["LogPath"].ToString();

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var fileName = DateTime.Today.ToString("dd-MMM-yy", CultureInfo.InvariantCulture) + ".txt";
                var serverPath = folder + fileName;
                if (!File.Exists(serverPath))
                {
                    File.Create(serverPath).Close();
                }
                var w = File.AppendText(serverPath);
                try
                {
                    w.WriteLine("\n" + System.DateTime.Now + "\t" + LogValues);

                    w.Flush();
                    w.Close();
                }
                catch
                {
                }
                finally
                {
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "success";
        }

        private static void CreateTableIfNotExists()
        {
            ExecuteQuery("IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Error' AND xtype='U')Create TABLE [dbo].[Error]( [ErrorId] [int] IDENTITY(1,1) NOT NULL, [SqlQuery] [ntext] NULL, [SqlParam] [ntext] NULL, [CommandType] [ntext] NULL, [TotalSeconds] [decimal](18, 2) NOT NULL, [Exception] [ntext] NULL, [InnerException] [ntext] NULL, [RequestId] [int] NOT NULL, [FileName] [ntext] NULL,  [MethodName] [nvarchar](100) NULL, [CreateDate] [datetime] NOT NULL, [Active] [bit] NOT NULL, [UserIPAddress] [nvarchar](20) NULL, [UserOS] [ntext] NULL, [UserLocation] [ntext] NULL, [PageName] [ntext] NULL, [ErrorType] [nvarchar](100) NULL, [RequestURL] [nvarchar](500) NULL, [UserAgent] [ntext] NULL, [UserName] [nvarchar](100) NULL, CONSTRAINT [PK_dbo.Error] PRIMARY KEY CLUSTERED ( [ErrorId] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");// +
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           // "IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('dbo.ErrorInsert')) BEGIN exec('create PROCEDURE [dbo].[ErrorInsert] @SqlQuery nvarchar(max), @SqlParam nvarchar(max), @CommandType nvarchar(max), @TotalSeconds decimal(18,2), @Exception nvarchar(max), @InnerException nvarchar(max), @RequestId int, @FileName nvarchar(max), @Active bit, @UserIPAddress nvarchar(500), @UserOS nvarchar(200), @UserLocation nvarchar( max), @PageName nvarchar(max), @CreateDate datetime, @ErrorType nvarchar(100), @RequestURL nvarchar(500), @UserAgent nvarchar(200), @UserName nvarchar(100) AS BEGIN INSERT INTO dbo.Error values ( @SqlQuery, @SqlParam ,@CommandType ,@TotalSeconds ,@Exception ,@InnerException ,@RequestId ,@FileName ,@CreateDate ,@Active ,@UserIPAddress ,@UserOS ,@UserLocation ,@PageName ,@ErrorType ,@RequestURL ,@UserAgent ,@UserName) END') END");
        }
        private static void ExecuteQuery(string sql)
        {
            using (SqlConnection theConnection = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ErrorCon"].ToString()))
            using (SqlCommand theCommand = new SqlCommand(sql, theConnection))
            {
                theConnection.Open();
                theCommand.CommandType = CommandType.Text;
                theCommand.ExecuteNonQuery();
            }
        }
        private static void InsertError(ErrorModel errorModel)
        {
            CreateTableIfNotExists();
            // define INSERT query with parameters

            // create connection and command
            using (SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ErrorCon"].ToString()))
            using (SqlCommand cmd = new SqlCommand("ErrorInsert", cn))
            {
                var ipAddress = GetIpAddress();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO dbo.Error values ( @SqlQuery, @SqlParam ,@CommandType ,@TotalSeconds ,@Exception ,@InnerException ,@RequestId ,@FileName, @MethodName ,@CreateDate ,@Active ,@UserIPAddress ,@UserOS ,@UserLocation ,@PageName ,@ErrorType ,@RequestURL ,@UserAgent ,@UserName) ";
                // define parameters and their values
                cmd.Parameters.AddWithValue("@SqlQuery", errorModel.SqlQuery);
                cmd.Parameters.AddWithValue("@SqlParam", errorModel.SqlParam);
                cmd.Parameters.AddWithValue("@CommandType", errorModel.CommandType);
                cmd.Parameters.AddWithValue("@TotalSeconds", errorModel.TotalSeconds);
                cmd.Parameters.AddWithValue("@Exception", errorModel.Exception);
                cmd.Parameters.AddWithValue("@InnerException", errorModel.InnerException);
                cmd.Parameters.AddWithValue("@RequestId", errorModel.RequestId);
                cmd.Parameters.AddWithValue("@FileName", Convert.ToString(errorModel.FileName) ?? "");
                cmd.Parameters.AddWithValue("@MethodName", errorModel.MethodName);
                cmd.Parameters.AddWithValue("@CreateDate", errorModel.CreateDate);
                cmd.Parameters.AddWithValue("@Active", errorModel.Active);
                cmd.Parameters.AddWithValue("@UserIPAddress", ipAddress);
                cmd.Parameters.AddWithValue("@UserOS", GetOsName());
                cmd.Parameters.AddWithValue("@UserLocation", GetInfo(ipAddress));
                cmd.Parameters.AddWithValue("@PageName", errorModel.PageName ?? "");

                cmd.Parameters.AddWithValue("@ErrorType", errorModel.ErrorType);
                cmd.Parameters.AddWithValue("@RequestURL", Convert.ToString(HttpContext.Current?.Request?.Url));
                cmd.Parameters.AddWithValue("@UserAgent", Convert.ToString(HttpContext.Current?.Request?.UserAgent ?? ""));
                cmd.Parameters.AddWithValue("@UserName", errorModel.UserName);

                // open connection, execute INSERT, close connection
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        private static string GetIpAddress()
        {
            try
            {
                string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
                // Get the IP
                 return Dns.GetHostEntry(hostName).AddressList[2].ToString() != "" ? Dns.GetHostEntry(hostName).AddressList[2].ToString() : HttpContext.Current.Request.UserHostAddress; 
            }
            catch
            {
                return "";
            }
        }
        private static string GetOsName()
        {
            return Environment.OSVersion.ToString();
        }
        private static string GetInfo(string ipAddress)
        {
            string strReturnVal = "";
            try
            {
                string ipResponse = IpRequestHelper("http://ip-api.com/xml/" + ipAddress);

                //return ipResponse;
                XmlDocument ipInfoXML = new XmlDocument();
                ipInfoXML.LoadXml(ipResponse);
                XmlNodeList responseXML = ipInfoXML.GetElementsByTagName("query");

                NameValueCollection dataXML = new NameValueCollection
                {
                    {responseXML.Item(0).ChildNodes[2].InnerText, responseXML.Item(0).ChildNodes[2].Value}
                };

                strReturnVal = responseXML.Item(0).ChildNodes[1].InnerText; // Contry
                strReturnVal += "(" + responseXML.Item(0).ChildNodes[2].InnerText + ")";  // Contry Code 
            }
            catch
            {
                // ignored
            }
            return strReturnVal;
        }
        private static string IpRequestHelper(string url)
        {
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();

            StreamReader responseStream = new StreamReader(objResponse.GetResponseStream());
            string responseRead = responseStream.ReadToEnd();

            responseStream.Close();
            responseStream.Dispose();

            return responseRead;
        }
    }
}
