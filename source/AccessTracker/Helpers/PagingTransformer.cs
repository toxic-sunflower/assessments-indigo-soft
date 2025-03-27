namespace AccessTracker.Helpers;

public class PagingTransformer
{
    public static readonly PagingTransformer Shared = new ();
    
    public (int skip, int take) GetSkipTake(
        int? page,
        int? pageSize,
        int defaultPageSize)
    {
        page ??= 1;
        pageSize ??= defaultPageSize;
        
        var skip = pageSize * (page - 1);
        
        return (skip.Value, pageSize.Value);
    }
}