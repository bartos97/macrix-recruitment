namespace macrix_api.Models;

public class PersonEntity
{
    public long id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string StreetName { get; set; }
    public string HouseNumber { get; set; }
    public string? ApartmentNumber { get; set; }
    public string PostalCode { get; set; }
    public string Town { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime? CreatedTimestamp { get; set; }
    public DateTime? LastUpdateTimestamp { get; set; }
}
