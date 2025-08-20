using IngoX.Client.Bff.Core.Abstract;
using IngoX.Client.Bff.Core.Models;
using IngoX.Client.Bff.Core.Models.External;
using IngoX.Client.Bff.Core.Models.UI;
using IngoX.Client.Bff.Core.Models.UIModels;
using Microsoft.Extensions.Logging;

namespace IngoX.Client.Bff.Core.Services;

/// <summary>
/// Класс-адаптер для работы с Api Ingox.IndividualClient.App.
/// </summary>
public class PersonSearchService : IPersonSearchService
{
    private readonly IIndividualClientApp _mdmApi;
    private readonly ILogger<PersonSearchService> _logger;
    private readonly IDirectMapper<SearchPersonItem, Person> _personMapper;
    private readonly IDirectMapper<AdvancedSearchPersonItem, Person> _advancedPersonMapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonSearchService"/> class.
    /// Сервис для работы с MDM. По сути своей - прокси между моделями <see cref="PersonSearchService"/> class.
    /// </summary>
    /// <param name="mdmService">MDM api.</param>
    /// <param name="logger">.</param>
    /// <param name="personMapper">Mapper.</param>
    /// <param name="advancedPersonMapper">AMapper.</param>
    public PersonSearchService(
        IIndividualClientApp mdmService,
        ILogger<PersonSearchService> logger,
        IDirectMapper<SearchPersonItem, Person> personMapper,
        IDirectMapper<AdvancedSearchPersonItem, Person> advancedPersonMapper)
    {
        _mdmApi = mdmService;
        _logger = logger;
        _personMapper = personMapper;
        _advancedPersonMapper = advancedPersonMapper;
    }

    /// <summary>
    /// Поиск физ.лица расширенный.
    /// </summary>
    /// <param name="query">.</param>
    /// <returns></returns>
    public async Task<ResultModel<List<Person>>> SearchPersonAnvancedAsync(SearchPersonQueryAdvanced query)
    {
        try
        {
            var result = await _mdmApi.Advanced(
                                isn: string.Empty,
                                lastName: string.Empty,
                                firstName: string.Empty,
                                fullName: query.Name,
                                phoneNumber: query.Phone,
                                birthDate: query.BirthDate?.ToString("yyyy-MM-dd") ?? string.Empty,
                                documentClassIsn: string.Empty,
                                documentSeries: string.Empty,
                                documentNumber: string.Empty,
                                withoutObjectsAmount: false);
            return ResultModel<List<Person>>.Success(result.Select(_advancedPersonMapper.Map).ToList());
        }
        catch (Refit.ApiException refitException)
        {
            _logger.LogError("Ошибка при вызове метода поиска клиента.", new object[] { query, refitException });

            return ResultModel<List<Person>>.Failure(new ErrorModel("Ошибка поиска клиента", ErrorKind.Technical, $"Ошибка поиска клиента {refitException.StatusCode}"));
        }
    }

    /// <summary>
    /// Поиск физ.лица.
    /// </summary>
    /// <param name="query">.</param>
    /// <returns></returns>
    public async Task<ResultModel<List<Person>>> SearchPersonAsync(SearchPersonQuery query)
    {
        try
        {
            var result = await _mdmApi.ClientsAll(
                                name: query.Name,
                                phoneNumber: query.Phone,
                                birthDate: query.BirthDate?.ToString("yyyy-MM-dd") ?? string.Empty,
                                passportNumber: string.Empty,
                                passportSeries: string.Empty,
                                withoutObjectsAmount: false);

            result = query.SortCode == SortOrdering.Ascending ? result.OrderBy(x => x.FullName).ToList() : result.OrderByDescending(x => x.FullName).ToList();

            return ResultModel<List<Person>>.Success(result.Select(_personMapper.Map).ToList());
        }
        catch (Refit.ApiException refitException)
        {
            _logger.LogError("Ошибка при вызове метода поиска клиента.", new object[] { query, refitException });
            return ResultModel<List<Person>>.Failure(new ErrorModel("Ошибка поиска клиента", ErrorKind.Technical, "Ошибка поиска клиента"));
        }
    }
}
