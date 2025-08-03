namespace MentorSync.SharedKernel.CommonEntities;

public sealed class PaginatedList<T>
{
	public List<T> Items { get; set; } = [];
	public int PageNumber { get; set; }
	public int PageSize { get; set; }
	public int TotalCount { get; set; }
	public bool HasNextPage => PageNumber * PageSize < TotalCount;
	public bool HasPreviousPage => PageNumber > 1;
	public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
