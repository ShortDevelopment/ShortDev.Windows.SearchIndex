using Microsoft.Search.Interop;
using System.Collections.Generic;
using System.Linq;

namespace ShortDev.Windows.SearchIndex
{
    public sealed class SearchQueryBuilder
    {
        public string UserQuery { get; private set; }
        public SearchQueryBuilder(string userQuery)
        {
            this.UserQuery = userQuery;
        }

        public List<SearchIndexColumn> Columns { get; } = new();
        public string WhereRestrictions { get; set; } = "";

        public SearchQueryBuilder AddColumn(SearchIndexColumn column)
        {
            Columns.Add(column);
            return this;
        }

        public SearchQueryBuilder UseFreeText(string keyword)
        {
            WhereRestrictions = $"or FREETEXT('{keyword}')";
            return this;
        }

        public string Build(ISearchQueryHelper queryHelper)
        {
            queryHelper.QuerySelectColumns = string.Join(",", Columns.Select((x) => x.Name));
            queryHelper.QueryWhereRestrictions = WhereRestrictions;
            return queryHelper.GenerateSQLFromUserQuery(UserQuery);
        }
    }
}
