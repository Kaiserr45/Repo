using IngoX.Client.Bff.Core.Abstract;
using IngoX.Client.Bff.Core.Models;
using IngoX.Client.Bff.Core.Models.UI;
using IngoX.Client.Bff.Core.Models.UIModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IngoX.Client.Bff.Controllers;

/// <summary>
/// Working with individual client's.
/// </summary>
[ApiController]

// [Authorize("Bearer")]
[Route("IndividualClient")]
public class IndividualClientController : ControllerBase
{
    private readonly IPersonSearchService _personSearchService;
    private readonly ILastUserQueriesService<Person> _lastUserQueriesService;
    private readonly ILogger<IndividualClientController> _logger;
    private readonly INameSuggestionService _suggestionService;

    /// <summary>
    /// Initializes a new instance of the <see cref="IndividualClientController"/> class.
    /// </summary>
    /// <param name="searchService">.</param>
    /// <param name="logger">logger.</param>
    /// <param name="lastUserQueriesService">lastUserQueriesService.</param>
    /// <param name="nameSuggestionService">nameSuggestionService.</param>
    public IndividualClientController(
        IPersonSearchService searchService,
        ILogger<IndividualClientController> logger,
        ILastUserQueriesService<Person> lastUserQueriesService,
        INameSuggestionService nameSuggestionService)
    {
        _personSearchService = searchService;
        _lastUserQueriesService = lastUserQueriesService;
        _logger = logger;
        _suggestionService = nameSuggestionService;
    }

    /// <summary>
    /// Поиск клиента.
    /// </summary>
    /// <returns>Список клинетов, удовлетворяющий условиям поиска.</returns>
    /// <param name="query">Поисковой запрос.</param>
    // [HttpGet]
    [HttpPost]
    [Route("clients/advanced")]
    [ProducesResponseType<ResultModel<List<Person>>>(200)]
    [ProducesResponseType<ResultModel<List<Person>>>(400)]
    [ProducesResponseType<ResultModel<List<Person>>>(404)]
    public async Task<IActionResult> AdvancedSearch([FromBody] SearchPersonQueryAdvanced query)
    {
        // TODO: валидация параметров
        if (string.IsNullOrEmpty(query.Name))
        {
            return BadRequest();
        }

        var persons = await _personSearchService.SearchPersonAnvancedAsync(query);

        return persons.IsSuccess ? Ok(persons) : NotFound(persons);
    }

    /// <summary>
    /// Поиск клиента.
    /// </summary>
    /// <returns>Список клинетов, удовлетворяющий условиям поиска.</returns>
    /// <param name="query">Поисковой запрос.</param>
    // [HttpGet]
    [HttpPost]
    [Route("clients")]
    [ProducesResponseType<ResultModel<List<Person>>>(200)]
    [ProducesResponseType<ResultModel<List<Person>>>(400)]
    [ProducesResponseType<ResultModel<List<Person>>>(404)]
    public async Task<IActionResult> SearchPerson([FromBody] SearchPersonQuery query)
    {
        if (string.IsNullOrEmpty(query.Name))
        {
            return BadRequest();
        }

        var persons = await _personSearchService.SearchPersonAsync(query);

        return persons.IsSuccess ? Ok(persons) : NotFound(persons);
    }
}