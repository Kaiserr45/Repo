namespace IngoX.Client.Bff.Core.Abstract;

public interface INameSuggestionService
{
    public Task<List<string>> Suggest(string query);
}
