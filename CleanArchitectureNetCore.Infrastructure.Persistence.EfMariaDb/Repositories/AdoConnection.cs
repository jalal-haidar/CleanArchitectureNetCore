using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace CleanArchitectureNetCore.Infrastructure.Persistence.EfMariaDb.Repositories
{
    public class AdoConnection
    {
        SqlConnection _Connection;
        string _ConnectionString;

        public AdoConnection(IConfiguration configuration)
        {
            // read connection string from appsettings.json
            _ConnectionString = configuration.GetConnectionString("Default");
        }


        public DataSet Execute(string commandText, CommandType commandType, SqlParameter[] parameters = null)
        {
            using (_Connection = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(commandText, _Connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    return Execute(command);
                }
            }
        }
        public DataSet Execute(SqlCommand command)
        {
            DataSet ds = new DataSet();
            try
            {
                using (_Connection = new SqlConnection(_ConnectionString))
                {
                    _Connection.Open();
                    command.Connection = _Connection;
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(ds);
                }
                return ds;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                _Connection.Close();
            }
        }

        protected string Value(object obj)
        {
            if (obj == null)
                return null;
            return obj.ToString();
        }
    }
}
