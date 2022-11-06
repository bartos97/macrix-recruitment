namespace macrix_api.Models;

public record PersonEntity(
    long id,
    string FirstName,
    string LastName,
    string StreetName,
    string HouseNumber,
    string? ApartmentNumber,
    string PostalCode,
    string Town,
    string PhoneNumber,
    DateTime DateOfBirth
);
