namespace MentorSync.SharedKernel.CommonEntities;

/// <summary>
/// Represents a paginated list with pagination metadata
/// </summary>
/// <typeparam name="T">The type of items in the list</typeparam>
public sealed class PaginatedList<T>
{
	/// <summary>
	/// Gets or sets the items in the current page
	/// </summary>
	public IReadOnlyList<T> Items { set; get; }

	/// <summary>
	/// Gets or sets the current page number (1-based)
	/// </summary>
	public int PageNumber { get; set; }

	/// <summary>
	/// Gets or sets the number of items per page
	/// </summary>
	public int PageSize { get; set; }

	/// <summary>
	/// Gets or sets the total number of items across all pages
	/// </summary>
	public int TotalCount { get; set; }

	/// <summary>
	/// Gets a value indicating whether there is a next page available
	/// </summary>
	public bool HasNextPage => PageNumber * PageSize < TotalCount;

	/// <summary>
	/// Gets a value indicating whether there is a previous page available
	/// </summary>
	public bool HasPreviousPage => PageNumber > 1;

	/// <summary>
	/// Gets the total number of pages
	/// </summary>
	public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
