using IngoX.Client.Bff.Core.Models;
using IngoX.Client.Bff.Core.Models.UI;
using IngoX.Client.Bff.Core.Models.UIModels;

namespace IngoX.Client.Bff.Core.Abstract;

public interface IPersonSearchService
{
    public Task<ResultModel<List<Person>>> SearchPersonAnvancedAsync(SearchPersonQueryAdvanced personSearchQuery);

    public Task<ResultModel<List<Person>>> SearchPersonAsync(SearchPersonQuery personSearchQuery);
}
