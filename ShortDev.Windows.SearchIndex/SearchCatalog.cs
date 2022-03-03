using Microsoft.Search.Interop;
using System.Data;
using System.Data.OleDb;

namespace ShortDev.Windows.SearchIndex
{
    public sealed class SearchCatalog
    {
        public string CatalogId { get; private set; }

        public CSearchManager SearchManager { get; private set; }
        public CSearchCatalogManager CatalogManager { get; private set; }
        public CSearchQueryHelper SearchQueryHelper { get; private set; }

        /// <summary>
        /// See <see cref="SystemIndex"/> for default instance
        /// </summary>
        public SearchCatalog(string catalogId)
        {
            this.CatalogId = catalogId;
            SearchManager = new();
            CatalogManager = SearchManager.GetCatalog(catalogId);
            SearchQueryHelper = CatalogManager.GetQueryHelper();
        }

        public static SearchCatalog SystemIndex { get => new("SystemIndex"); }

        public DataTable Query(SearchQueryBuilder queryBuilder)
            => Query(queryBuilder.Build(this.SearchQueryHelper));

        public DataTable Query(string sqlQuery)
        {
            using (OleDbConnection Connection = new(SearchQueryHelper.ConnectionString))
            {
                Connection.Open();
                OleDbCommand cmd = new(sqlQuery, Connection);
                cmd.CommandType = CommandType.Text;
                using (var reader = cmd.ExecuteReader())
                {
                    DataTable data = new();
                    data.Load(reader);
                    return data;
                }
            }
        }
    }
}
