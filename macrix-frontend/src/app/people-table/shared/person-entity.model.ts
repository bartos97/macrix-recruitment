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

export class Person implements PersonEntity {
    firstName!: string;
    lastName!: string;
    streetName!: string;
    houseNumber!: string;
    apartmentNumber: string | null | undefined;
    postalCode!: string;
    town!: string;
    phoneNumber!: string;
    private _dateOfBirth!: Date;
    private _age!: number;

    public get age(): number {
        if (!this._age)
            this._age = Person.getAgeFromDate(this._dateOfBirth);
        return this._age;
    }

    public get dateOfBirth(): Date {
        return this._dateOfBirth;
    }

    public set dateOfBirth(date: Date) {
        this._dateOfBirth = date;
        this._age = Person.getAgeFromDate(this._dateOfBirth);
    }

    constructor(init: PersonEntity) {
        Object.assign(this, init);
    }

    private static getAgeFromDate(birthDate: Date): number {
        const today = new Date();
        const m = today.getMonth() - birthDate.getMonth();
        let age = today.getFullYear() - birthDate.getFullYear();
        if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
            age--;
        }
        return age;
    }
}