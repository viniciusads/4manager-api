namespace _4Tech._4Manager.Application.Common.Pagination
{
    public interface IPagedQuery
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
    }
}

