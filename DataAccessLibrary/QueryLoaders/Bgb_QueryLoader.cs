using Bgb_DataAccessLibrary.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bgb_DataAccessLibrary.QueryLoaders
{
    public class Bgb_QueryLoader : IQueryLoader
    {
        private readonly Dictionary<string, string> _queries;

        public Bgb_QueryLoader(string basePath)
        {
            _queries = LoadQueries(basePath);
        }

        private Dictionary<string, string> LoadQueries(string basePath)
        {
            var queries = new Dictionary<string, string>();
            var sqlFiles = Directory.GetFiles(basePath, "*.sql", SearchOption.AllDirectories);

            foreach (var file in sqlFiles)
            {
                var key = Path.GetFileNameWithoutExtension(file);
                var query = File.ReadAllText(file);
                queries[key] = query;
            }

            return queries;
        }

        public string GetQuery(string queryName)
        {
            return _queries.ContainsKey(queryName) ? _queries[queryName] : null;
        }
    }
}
