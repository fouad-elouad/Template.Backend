
namespace Template.Backend.Data.Utilities
{
    /// <summary>
    /// PagedListExtensions class
    /// </summary>
    public static class PagedListExtensions
    {
        /// <summary>
        /// return list of source object.
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="page">The page index.</param>
        /// <param name="pageSize">the page size.</param>
        /// <returns></returns>
        public static List<T> ToPagedList<T>(this IQueryable<T> source, int page, int pageSize)
        {
            int Page = page < 1 ? 0 : page - 1;
            if (page<1)
                return null;
            return source.Skip(Page * pageSize).Take(pageSize).ToList();
        }
    }
}
