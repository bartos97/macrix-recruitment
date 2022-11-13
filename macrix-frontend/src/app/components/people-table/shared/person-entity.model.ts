export interface PersonEntity {
    id: number;
    firstName: string;
    lastName: string;
    streetName: string;
    houseNumber: string;
    apartmentNumber: string | null | undefined;
    postalCode: string;
    town: string;
    phoneNumber: string;
    dateOfBirth: Date | string;
    createdTimestamp: Date | string;
    lastUpdateTimestamp: Date | string;
}
