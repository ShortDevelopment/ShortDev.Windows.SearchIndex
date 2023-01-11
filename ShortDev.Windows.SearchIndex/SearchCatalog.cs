using Microsoft.Search.Interop;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;

namespace ShortDev.Windows.SearchIndex;

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

    #region Query
    public IEnumerable<string> QueryPaths(SearchQueryBuilder queryBuilder)
    {
        queryBuilder
            .ClearColumns()
            .AddColumn(SearchIndexColumn.ItemPathDisplay);
        DataTable data = Query(queryBuilder);
        return data.Rows
                    .Cast<DataRow>()
                    .Select((row) => row[0].ToString());
    }

    public DataTable Query(SearchQueryBuilder queryBuilder)
        => Query(queryBuilder.Build(this.SearchQueryHelper));

    public DataTable Query(string sqlQuery)
    {
        using (OleDbConnection Connection = new(SearchQueryHelper.ConnectionString))
        {
            Connection.Open();
            using (OleDbCommand cmd = new(sqlQuery, Connection)
            {
                CommandType = CommandType.Text
            })
            using (var reader = cmd.ExecuteReader())
            {
                DataTable data = new();
                data.Load(reader);
                return data;
            }
        }
    }
    #endregion
}