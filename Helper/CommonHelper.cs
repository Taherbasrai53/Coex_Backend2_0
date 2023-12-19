using Azure;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using Response = COeX_India1._2.Models.Response;

namespace COeX_India1._2.Helper
{
    public class CommonHelper
    {
        public static IConfiguration Configuration;

        public static string CurrentConnString;

        public static int DefaultRoleId = 2;

        public static Hashtable TrustedTable = new Hashtable();
        public static Hashtable EmergencyTable = new Hashtable();

        //public static AzureStorage _azureStorage;

        //public CommonHelper(AzureStorage azureStorage)
        //{
        //    _azureStorage = azureStorage;
        //}
        public static Response AddLog(string Descr)
        {
            return AddLog(Descr, "");
        }
        public static Response AddLog(string Descr, string Log)
        {
            try
            {
                DataHelper dataHelper = new DataHelper();
                string sqlExp = "Insert into Logs(Descr, LogData, InsertedAt) values('" + Descr.Replace("'", "''") + "', '" + Log.Replace("'", "''") + "', GetDate())";
                dataHelper.ExecuteNonQuery(sqlExp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed in create log, Error : " + ex.Message);
                return new Response(false, ex.Message);
            }

            return new Response(true, "Log Created successfully");
        }


        public static DataSet GetDataSetFromStringXml(string strXml)
        {
            //var strSchema = arrayOfXElement.Nodes[0].ToString();
            //var strData = arrayOfXElement.Nodes[1].ToString();
            //var strXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n\t<DataSet>";
            //strXml += strSchema + strData;
            //strXml += "</DataSet>";

            DataSet ds = new DataSet("TestDataSet");
            ds.ReadXml(new MemoryStream(Encoding.UTF8.GetBytes(strXml)));

            return ds;
        }

        public async static Task<string> DownloadString(string strFileUrlToDownload)

        {
            HttpClient client = new HttpClient();
            string downloadString = await client.GetStringAsync(new Uri(strFileUrlToDownload));
            return downloadString;

            //MemoryStream storeStream = new MemoryStream();

            //storeStream.SetLength(myDataBuffer.Length);

            //storeStream.Write(myDataBuffer, 0, (int)storeStream.Length);

            //storeStream.Flush();



            ////TO save into certain file must exist on Local

            //SaveMemoryStream(storeStream, "C:\\TestFile.txt");



            ////The below Getstring method to get data in raw format and manipulate it as per requirement

            //string download = Encoding.ASCII.GetString(myDataBuffer);



            //Console.WriteLine(download);

            //Console.ReadLine();

        }

        public static T Mapper<T>(Object source, Object destination) where T : class, new()
        {
            PropertyInfo[] cutieeProperties = typeof(T).GetProperties();

            foreach (PropertyInfo property in cutieeProperties)
            {
                try
                {

                    if (property.Name == "Password" || property.Name == "TempPassword") { continue; }
                    if (source.GetType().GetProperty(property.Name) != null)
                    {

                        property.SetValue(destination, source.GetType().GetProperty(property.Name).GetValue(source, null));
                    }
                    else
                    {
                        property.SetValue(destination, null);
                    }
                }
                catch (Exception ex)
                {

                    property.SetValue(destination, null);
                }
            }
            return (T)destination;
        }

        public static T Trimmer<T>(Object model) where T : class, new()
        {
            var properties = model.GetType().GetProperties()
                           .Where(prop => prop.PropertyType == typeof(string))
                           .Select(prop => prop.GetValue(model) as string);

            foreach (var propertyValue in properties)
            {
                if (propertyValue != null)
                {
                    var trimmedValue = propertyValue.Trim();
                    foreach (var prop in model.GetType().GetProperties()
                                                    .Where(p => p.PropertyType == typeof(string) &&
                                                                (string)p.GetValue(model) == propertyValue))
                    {
                        prop.SetValue(model, trimmedValue);
                    }
                }
            }

            return (T)model;
        }



        public static List<T> Fetch<T>(DataHelper dh) where T : class, new()
        {
            List<T> viewUsersResponse = new List<T>();
            T userResponse;
            if (dh.dataSet == null || dh.dataSet.Tables == null || dh.dataSet.Tables.Count == 0)
            {
                return null;
            }

            if (dh.dataSet.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow drUser in dh.dataSet.Tables[0].Rows)
                {

                    userResponse = new T();

                    foreach (DataColumn dc in dh.dataSet.Tables[0].Columns)
                    {
                        try
                        {
                            PropertyInfo propertyInfo = userResponse.GetType().GetProperty(dc.ColumnName);

                            propertyInfo.SetValue(userResponse, Convert.ChangeType(drUser[dc.ColumnName], propertyInfo.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }

                    viewUsersResponse.Add(userResponse);

                }
            }



            return viewUsersResponse;
        }

        //public static async Task<Response> SendCronNotification(List<string> deviceIds, string enContent)
        //{
        //    try
        //    {
        //        var client = new RestClient("https://onesignal.com/");
        //        var request = new RestRequest("api/v1/notifications", Method.Post);
        //        request.AddHeader("Authorization", Configuration.GetSection("OneSignal:AccessKey").Value);
        //        request.AddHeader("Content-Type", "application/json");

        //        Notification notif = new Models.Notification();
        //        notif.app_id = Configuration.GetSection("OneSignal:AppId").Value;
        //        notif.contents = new Content();
        //        notif.contents.en = enContent;
        //        notif.include_player_ids = deviceIds;
        //        var strBody = JsonSerializer.Serialize(notif);
        //        var body = strBody;

        //        request.AddParameter("application/json", body, ParameterType.RequestBody);
        //        RestResponse response = client.Execute(request);
        //        Console.WriteLine(response.Content);

        //        Console.WriteLine($"Notification created for {deviceIds} recipients");

        //        return new Response(true, "");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return new Response(false, ex.Message);
        //    }
        //}

        public static List<T> MapTableToModel<T>(DataTable SourceTable) where T : class, new()
        {
            List<T> ModelList = new List<T>();
            T Model;
            if (SourceTable == null)
            {
                return null;
            }

            if (SourceTable.Rows.Count > 0)
            {
                int rowNo = 1;
                SourceTable.Rows.Cast<DataRow>().All(DrSource =>
                {
                    if (rowNo % 100 == 0)
                    {
                        Console.WriteLine($"Row No : {rowNo}, Now : {DateTime.Now}");
                    }
                    Model = new T();
                    SourceTable.Columns.Cast<DataColumn>().All(dc =>
                    {
                        if (DrSource[dc.ColumnName] == null || DrSource[dc.ColumnName] == DBNull.Value)
                        {
                            return true;
                        }
                        PropertyInfo propertyInfo = null;
                        try
                        {
                            propertyInfo = Model.GetType().GetProperty(dc.ColumnName);
                            if (propertyInfo != null)
                            {
                                if (propertyInfo.PropertyType.IsEnum)
                                {
                                    int tmpInt;
                                    if (int.TryParse(DrSource[dc.ColumnName].ToString(), out tmpInt))
                                    {
                                        propertyInfo.SetValue(Model, tmpInt);
                                    }
                                    else
                                    {
                                        propertyInfo.SetValue(Model, Convert.ChangeType(DrSource[dc.ColumnName], propertyInfo.PropertyType));
                                    }
                                }
                                else
                                {
                                    propertyInfo.SetValue(Model, Convert.ChangeType(DrSource[dc.ColumnName], propertyInfo.PropertyType));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine($"Unable to set property : {propertyInfo.Name}");
                            //continue;
                        }
                        return true;
                    });
                    //foreach (DataColumn dc in SourceTable.Columns)
                    //{

                    //}
                    ModelList.Add(Model);
                    rowNo++;
                    return true;
                });
                //foreach (DataRow DrSource in SourceTable.Rows)
                //{

                //}
            }
            return ModelList;
        }

        public static async Task<List<T>> MapTableToModelNew<T>(DataTable SourceTable) where T : class, new()
        {
            List<T> ModelList = new List<T>();
            T Model;
            if (SourceTable == null)
            {
                return null;
            }

            if (SourceTable.Rows.Count > 0)
            {
                int rowNo = 1;
                SourceTable.Rows.Cast<DataRow>().All(DrSource =>
                {
                    if (rowNo % 100 == 0)
                    {
                        Console.WriteLine($"Row No : {rowNo}, Now : {DateTime.Now}");
                    }
                    Model = new T();

                    //PropertyInfo propertyInfo = null;
                    SourceTable.Columns.Cast<DataColumn>().All(dc =>
                    {
                        if (DrSource[dc.ColumnName] == null || DrSource[dc.ColumnName] == DBNull.Value)
                        {
                            return true;
                        }
                        PropertyInfo propertyInfo = null;
                        try
                        {
                            propertyInfo = Model.GetType().GetProperty(dc.ColumnName);
                            if (propertyInfo != null)
                            {
                                //if (propertyInfo.PropertyType.IsEnum)
                                //{
                                //    int tmpInt;
                                //    if (int.TryParse(DrSource[dc.ColumnName].ToString(), out tmpInt))
                                //    {
                                //        propertyInfo.SetValue(Model, tmpInt);
                                //    }
                                //    else
                                //    {
                                //        propertyInfo.SetValue(Model, Convert.ChangeType(DrSource[dc.ColumnName], propertyInfo.PropertyType));
                                //    }
                                //}
                                //else
                                //{
                                Console.WriteLine($"before change type : {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.ffffff")}");
                                object? val = null;
                                if (propertyInfo.PropertyType == typeof(int))
                                {
                                    int intVal;
                                    int.TryParse(DrSource[dc.ColumnName].ToString(), out intVal);
                                    propertyInfo.SetValue(Model, intVal);
                                }
                                //val = Convert.ChangeType(DrSource[dc.ColumnName], propertyInfo.PropertyType);
                                Console.WriteLine($"before set value : {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.ffffff")}");
                                propertyInfo.SetValue(Model, val);
                                Console.WriteLine($"after set value : {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.ffffff")}");
                                //}
                            }
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine($"Unable to set property : {propertyInfo.Name}");
                            //continue;
                        }
                        return true;
                    });

                    //SourceTable.Columns.Cast<DataColumn>().All(dc =>
                    //{
                    //    if (DrSource[dc.ColumnName] == null || DrSource[dc.ColumnName] == DBNull.Value)
                    //    {
                    //        return true;
                    //    }
                    //    PropertyInfo propertyInfo = null;
                    //    try
                    //    {
                    //        propertyInfo = Model.GetType().GetProperty(dc.ColumnName);
                    //        if (propertyInfo != null)
                    //        {
                    //            if (propertyInfo.PropertyType.IsEnum)
                    //            {
                    //                int tmpInt;
                    //                if (int.TryParse(DrSource[dc.ColumnName].ToString(), out tmpInt))
                    //                {
                    //                    propertyInfo.SetValue(Model, tmpInt);
                    //                }
                    //                else
                    //                {
                    //                    propertyInfo.SetValue(Model, Convert.ChangeType(DrSource[dc.ColumnName], propertyInfo.PropertyType));
                    //                }
                    //            }
                    //            else
                    //            {
                    //                propertyInfo.SetValue(Model, Convert.ChangeType(DrSource[dc.ColumnName], propertyInfo.PropertyType));
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        //Console.WriteLine($"Unable to set property : {propertyInfo.Name}");
                    //        //continue;
                    //    }
                    //    return true;
                    //});
                    //foreach (DataColumn dc in SourceTable.Columns)
                    //{

                    //}
                    ModelList.Add(Model);
                    rowNo++;
                    return true;
                });
                //foreach (DataRow DrSource in SourceTable.Rows)
                //{

                //}
            }
            return ModelList;
        }


        public static bool IsValidPhoneNumber(string temp, out string str)
        {
            str = "";

            str = temp.Replace(" ", "");

            string strRegex = @"^\+\d{1,3}\d{1,14}$";

            Regex re = new Regex(strRegex);
            if (re.IsMatch(str))
                return (true);
            else
                return (false);
        }
    }
}


