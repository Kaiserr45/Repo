namespace IngoX.Client.Bff.Core.Models.UI;

////  id: string;
////+ name: string;
//// + company?: string;
////+ phone?: string;
////+ birthDate?: string;
////+ city?: string;
////+ cars: number;
////+ houses: number;
////+ people: number;
////+ inn?: string;
////+ kpp?: string;
////type: EClientType; notEmptyBasket
////code?: string; ?????
////indicators: string[]; ????
////notEmptyBasket?: boolean; ?????? что это за поле? не могу отдать, судя из контекста
////unfinishedContracts?: boolean; ??????
////noAccess?: boolean; ?????
////new?: boolean; ?????
////https://tfs.corp.ingos.ru/tfs/Ingos/IngoGate/_git/IngoX.Client.Frontend?path=%2Fsearch-mf%2Fsrc%2Fapp%2Fmodels%2Fclient.model.ts
public class Person
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Phone { get; set; }

    public string BirthDate { get; set; }

    public string City { get; set; }

    public string INN { get; set; }

    public string Company { get; set; }

    public int VehicleCount { get; set; }

    public int InsuredCount { get; set; }

    public int EstateCount { get; set; }

    public int KPP { get; set; }

    public bool NotEmptyBasket { get; set; }
}
