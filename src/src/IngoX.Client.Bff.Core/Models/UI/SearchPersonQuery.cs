using IngoX.Client.Bff.Core.Models.UI;

namespace IngoX.Client.Bff.Core.Models.UIModels;

// https://tfs.corp.ingos.ru/tfs/Ingos/IngoGate/_git/IngoX.Client.Frontend?path=%2Fsearch-mf%2Fsrc%2Fapp%2Fservices%2Fclients.service.ts
public class SearchPersonQuery
{
    public string Phone { get; set; }

    public string Name { get; set; }

    public DateTime? BirthDate { get; set; }

    public SortOrdering SortCode { get; set; } = SortOrdering.Ascending;
}
