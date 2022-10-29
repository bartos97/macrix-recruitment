import { Component, OnInit } from '@angular/core';
import { PEOPLE_REGISTRY } from './shared/mock-people';
import { Person, PersonEntity } from './shared/person-entity.model';

@Component({
  selector: 'app-people-table',
  templateUrl: './people-table.component.html',
  styleUrls: ['./people-table.component.scss']
})
export class PeopleTableComponent implements OnInit {
  displayedColumns = [
    "firstName",
    "lastName",
    // "streetName",
    // "houseNumber",
    "apartmentNumber",
    // "postalCode",
    // "town",
    // "phoneNumber",
    "dateOfBirth",
    "age",
  ];
  tableDataSource: Person[] = PEOPLE_REGISTRY.slice(0, 10).map(x => new Person(x));

  constructor() { }

  ngOnInit(): void {
  }

}
