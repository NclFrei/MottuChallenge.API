namespace MottuChallenge.API.Domain.Dtos.Response;

public class PagedResponse<T>
{
    public IEnumerable<T> Data { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }
    public int Total { get; set; }
    public string? Next { get; set; }
    public string? Prev { get; set; }

    public PagedResponse(IEnumerable<T> data, int page, int limit, int total, string? next, string? prev)
    {
        Data = data;
        Page = page;
        Limit = limit;
        Total = total;
        Next = next;
        Prev = prev;
    }
}