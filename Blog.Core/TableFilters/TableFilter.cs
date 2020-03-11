namespace Blog.Core.TableFilters
{
    using Blog.Core.Enums;
    using System.Collections.Generic;
    using System.Linq;
    public class TableFilter
    {

        public int Start { get; set; }
        public int Length { get; set; }
        public TableSearchModel Search { get; set; }
        public IEnumerable<TableSortingModel> Order { get; set; }
        public List<TableColumn> Columns { get; set; }

        public int PageCount => Start / Length + 1;
        public int PageSize => Length;

        public OrderType OrderType => Order.FirstOrDefault()?.Dir == "asc"
                                                                ? OrderType.Ascending
                                                                : OrderType.Descending;
        public string ColumnName => Order.Any() ? Columns[Order.First().Column].Data : string.Empty;
    }

    public class TableColumn
    {
        public string Data { get; set; }
    }
}
