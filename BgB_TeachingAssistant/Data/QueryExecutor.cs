using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BgB_TeachingAssistant.Data
{
    public static class QueryExecutor
    {
        public static List<T> ExecuteQuery<T>(DataAccess dataAccess, string queryName, object parameters = null)
        {
            using (IDbConnection connection = dataAccess.GetConnection())
            {
                string query = dataAccess.GetQuery(queryName);
                return connection.Query<T>(query, parameters).ToList();
            }
        }

        public static T ExecuteQuerySingle<T>(DataAccess dataAccess, string queryName, object parameters = null)
        {
            using (IDbConnection connection = dataAccess.GetConnection())
            {
                string query = dataAccess.GetQuery(queryName);
                return connection.QuerySingleOrDefault<T>(query, parameters);
            }
        }

        public static int ExecuteCommand(DataAccess dataAccess, string queryName, object parameters = null)
        {
            using (IDbConnection connection = dataAccess.GetConnection())
            {
                string query = dataAccess.GetQuery(queryName);
                return connection.Execute(query, parameters);
            }
        }
    }
}

