namespace WCNApi.Infrastructure.Errors
{
    /// <summary>
    /// Add controller wise error number to identify in logs easily
    /// </summary>
    public enum ErrorCode
    {
        NoError = 0,
        //ValidationModelError = 2000,
        //Weather = 2001,
        ValidationModelError = 1,
        DatabaseError = 2,
        UnauthorizedError = 3,
        NotFoundError = 4,
        InternalServerError = 5
    }
}
