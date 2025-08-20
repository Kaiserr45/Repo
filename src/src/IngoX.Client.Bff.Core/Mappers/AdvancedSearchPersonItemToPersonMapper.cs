using IngoX.Client.Bff.Core.Abstract;
using IngoX.Client.Bff.Core.Models.External;
using IngoX.Client.Bff.Core.Models.UI;

namespace IngoX.Client.Bff.Core.Mappers;

public class AdvancedSearchPersonItemToPersonMapper : IDirectMapper<AdvancedSearchPersonItem, Person>
{
    public Person Map(AdvancedSearchPersonItem personItem)
    {
        return new Person()
        {
            Name = personItem.FullName,
            VehicleCount = personItem.VehicleCount ?? 0,
            EstateCount = personItem.EstateCount ?? 0,
            InsuredCount = personItem.InsuredCount ?? 0,
            City = personItem.AddressCity,
            Phone = personItem.FullName,
            BirthDate = personItem.BirthDate.ToString()
        };
    }
}
