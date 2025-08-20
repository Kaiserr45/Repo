namespace IngoX.Client.Bff.Core.Abstract;

public interface ILastUserQueriesService<T>
{
    /// <summary>
    /// get last user Queries.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="T">Type of Objects to store.</param>
    /// <returns></returns>
    List<T> GetLastUserQueries(string userId);
}
