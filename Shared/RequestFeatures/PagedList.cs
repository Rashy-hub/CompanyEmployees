namespace Shared.RequestFeatures
{
    public class PagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {

            MetaData = new MetaData()
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = count,
                TotalPage = (int)Math.Ceiling(count / (double)pageSize),
            };
            AddRange(items);
        }

    }
}
