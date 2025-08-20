using IngoX.Client.Bff.Core.Abstract;
using IngoX.Client.Bff.Core.Models.UI;

namespace IngoX.Client.Bff.Core.Services.Mock;

public class LastUserQueriesServiceMock : ILastUserQueriesService<Person>
{
    public List<Person> GetLastUserQueries(string userId)
    {
        // TODO: добавить каких-то данных, которые может возвращать сервис
        return new List<Person>();
    }
}
