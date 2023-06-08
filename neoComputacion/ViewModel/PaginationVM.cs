public class PaginationVM<T>
{
    public List<T> Posts { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }

    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
    public int PreviousPage => HasPreviousPage ? PageNumber - 1 : 1;
    public int NextPage => HasNextPage ? PageNumber + 1 : TotalPages;
}
