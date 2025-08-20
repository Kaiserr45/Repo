using IngoX.Client.Bff.Core.Abstract;

namespace IngoX.Client.Bff.Core.Services.Mock;

public class NameSuggestionServiceMock : INameSuggestionService
{
    private readonly Dictionary<string, List<string>> names = new Dictionary<string, List<string>> {
        { "иван", new List<string>() { "Иванов Антон Игоревич", "Иванкин", "Иванапина" } },
        { "петр", new List<string>() { "Петров", "Петров Павел Петрович", "Петухов Валера Сидоджин" } },
        { "сидо", new List<string>() { "Сидоров Сидр Пивзович", "Сидоюхи", "Сидоенко" } },
    };

    // TODO: накидать тестовых данных для сервиса подбора имен
    public async Task<List<string>> Suggest(string query)
    {
        query = query.Substring(0, 4).ToLowerInvariant();
        var result = names.FirstOrDefault(x => x.Key == query).Value;
        if (result == null)
        {
            return new List<string>();
        }
        return result;
    }
}
