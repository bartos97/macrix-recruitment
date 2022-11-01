export interface PersonEntity {
    firstName: string;
    lastName: string;
    streetName: string;
    houseNumber: string;
    apartmentNumber: string | null | undefined;
    postalCode: string;
    town: string;
    phoneNumber: string;
    dateOfBirth: Date;
}
