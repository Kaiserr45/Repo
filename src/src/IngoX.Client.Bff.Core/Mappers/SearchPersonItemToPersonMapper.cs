using IngoX.Client.Bff.Core.Abstract;
using IngoX.Client.Bff.Core.Models.External;
using IngoX.Client.Bff.Core.Models.UI;

namespace IngoX.Client.Bff.Core.Mappers;

public class SearchPersonItemToPersonMapper : IDirectMapper<SearchPersonItem, Person>
{
    public Person Map(SearchPersonItem personItem)
    {
        return new Person()
        {
            Id = personItem.MdmId,
            Name = personItem.FullName,
            VehicleCount = personItem.VehicleCount ?? 0,
            EstateCount = personItem.EstateCount ?? 0,
            InsuredCount = personItem.InsuredCount ?? 0,
            City = personItem.AddressCity,
            Phone = personItem.FullName,
            BirthDate = personItem.BirthDate?.ToString("yyyy-MM-dd")
        };
    }
}
