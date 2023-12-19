using Microsoft.Data.SqlClient;
using System.Data;

namespace COeX_India1._2.Helper
{
    public class DataHelper
    {

        private DataSet _dataSet;
        public DataSet dataSet { get { return _dataSet; } }

        private string _connString;
        public string ConnString { get { return _connString; } }

        private SqlCommand _sqlCommand;
        public SqlCommand sqlCommand { get { return _sqlCommand; } }

        public DataHelper() : this(CommonHelper.CurrentConnString)
        {
        }

        public DataHelper(string connString)
        {
            _connString = connString;
        }

        public int ExecuteScalarQuery(string query, List<SqlPara> Paras, CommandType commandType = CommandType.Text)
        {
            SqlConnection sqlConn = null;
            object result = null;
            try
            {
                sqlConn = new SqlConnection(_connString);

                sqlConn.Open();
                SqlCommand sqlComm = sqlConn.CreateCommand();
                if (Paras != null)
                {
                    foreach (SqlPara para in Paras)
                    {
                        SqlParameter sqlPara = sqlComm.Parameters.AddWithValue(para.Name, para.Value);
                        sqlPara.Direction = para.Direction;

                    }
                }
                sqlComm.CommandText = query;
                sqlComm.CommandType = commandType;
                _sqlCommand = sqlComm;
                result = sqlComm.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (sqlConn != null)
                {
                    try
                    {
                        sqlConn.Close();
                    }
                    catch { }
                }
            }

            if (result != null && result != DBNull.Value)
            {
                return Convert.ToInt32(result);
            }

            return 0;

        }
        public ExecuteNonQueryResult ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(commandText, null);
        }

        public ExecuteNonQueryResult ExecuteNonQuery(string commandText, List<SqlPara> Paras, CommandType commandType = CommandType.Text, int commandTimeout = 30)
        {
            ExecuteNonQueryResult result = new ExecuteNonQueryResult();
            SqlConnection sqlConn = null;
            try
            {
                sqlConn = new SqlConnection(_connString);

                sqlConn.Open();

                SqlCommand sqlComm = sqlConn.CreateCommand();
                if (Paras != null)
                {
                    foreach (SqlPara para in Paras)
                    {
                        SqlParameter sqlPara = sqlComm.Parameters.AddWithValue(para.Name, para.Value);
                        sqlPara.Direction = para.Direction;
                    }
                }
                sqlComm.CommandText = commandText;
                sqlComm.CommandType = commandType;
                sqlComm.CommandTimeout = commandTimeout;
                _sqlCommand = sqlComm;

                result.NoOfRowsAffected = sqlComm.ExecuteNonQuery();
                result.flag = true;
            }
            catch (Exception ex)
            {
                result.flag = false;
                result.Message = ex.Message;
                throw ex;
                //Console.WriteLine(ex.Message);
            }
            finally
            {
                if (sqlConn != null)
                {
                    try
                    {
                        sqlConn.Close();
                    }
                    catch { }
                }
            }
            return result;
        }

        public class ExecuteNonQueryResult
        {
            public bool flag { get; set; }
            public int NoOfRowsAffected { get; set; }
            public string Message { get; set; }
        }

        public void Select(string commandText)
        {
            Select(commandText, null);
        }
        public void Select(string commandText, List<SqlPara> Paras, CommandType commandType = CommandType.Text, int commandTimeout = 30)
        {
            //Console.WriteLine("In User Sync");
            bool flag = false;
            SqlConnection sqlConn = null;
            try
            {
                sqlConn = new SqlConnection(_connString);
                sqlConn.Open();

                SqlCommand sqlComm = sqlConn.CreateCommand();
                if (Paras != null)
                {
                    foreach (SqlPara par in Paras)
                    {
                        sqlComm.Parameters.AddWithValue(par.Name, par.Value);
                    }
                }
                sqlComm.CommandText = commandText;
                sqlComm.CommandType = commandType;
                sqlComm.CommandTimeout = commandTimeout;
                _sqlCommand = sqlComm;
                //Console.WriteLine("Before Execute Nonquery");
                var adapter = new SqlDataAdapter(sqlComm);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                _dataSet = ds;
                flag = true;

                //Console.WriteLine("After Execute Nonquery");
            }
            catch (Exception ex)
            {
                throw ex;
                //Console.WriteLine(ex.Message);
                //return new Response(false, ex.ToString());
            }
            finally
            {
                if (sqlConn != null)
                {
                    try
                    {
                        sqlConn.Close();
                    }
                    catch { }
                }
            }
            //Console.WriteLine("Out Sync user");
        }
    }

    public class SqlPara
    {
        public SqlPara()
        {
        }
        public SqlPara(string Name, object Value, ParameterDirection Direction = ParameterDirection.Input)
        {
            this.Name = Name;
            this.Value = Value;
            this.Direction = Direction;
        }
        public string Name { get; set; }
        public object Value { get; set; }
        public ParameterDirection Direction { get; set; } = ParameterDirection.Input;
    }
}

