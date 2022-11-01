import { Component, OnInit } from '@angular/core';
import { MOCK_PEROPLE_REPO } from './shared/mock-people';
import { PersonEntity } from './shared/person-entity.model';
import { FormGroup, FormControl, FormBuilder, Validators, FormArray, AbstractControl } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';

const DEFAULT_DISPLAYED_COLUMNS = [
  "firstName",
  "lastName",
  "streetName",
  "houseNumber",
  "apartmentNumber",
  "postalCode",
  "town",
  "phoneNumber",
  "dateOfBirth",
  "age",
  "actions",
];

@Component({
  selector: 'app-people-table',
  templateUrl: './people-table.component.html',
  styleUrls: ['./people-table.component.scss']
})
export class PeopleTableComponent implements OnInit {
  public displayedColumns = DEFAULT_DISPLAYED_COLUMNS;
  public tableForm: FormGroup;
  public dataSource = new MatTableDataSource<AbstractControl>;
  private readonly originalData: PersonEntity[];

  public get isDataUnchaged(): boolean {
    return this.peopleFormArray.controls.every(x => x.pristine)
      && this.originalData.length == this.peopleFormArray.controls.length;
  }

  get peopleFormArray(): FormArray {
    return this.tableForm.get('peopleFormArray') as FormArray;
  }

  constructor(private formBuilder: FormBuilder) {
    this.originalData = this.fetchData();
    this.tableForm = formBuilder.group({
      peopleFormArray: formBuilder.array(
        this.originalData.map(dataItem => this.createFormGroupRow(dataItem))
      )
    });
    this.dataSource.data = this.peopleFormArray.controls;
  }

  ngOnInit(): void { }

  private fetchData(): PersonEntity[] {
    //TODO: backend integration
    return MOCK_PEROPLE_REPO.slice(0, 20);
  }

  private createFormGroupRow(dataItem?: PersonEntity): FormGroup {
    return this.formBuilder.group({
      firstName: [dataItem?.firstName ?? "", Validators.required],
      lastName: [dataItem?.lastName ?? "", Validators.required],
      streetName: [dataItem?.streetName ?? "", Validators.required],
      houseNumber: [dataItem?.houseNumber ?? "", Validators.required],
      apartmentNumber: [dataItem?.apartmentNumber ?? ""],
      postalCode: [dataItem?.postalCode ?? "", Validators.required],
      town: [dataItem?.town ?? "", Validators.required],
      phoneNumber: [dataItem?.phoneNumber ?? "", Validators.required],
      dateOfBirth: [dataItem?.dateOfBirth ?? "", Validators.required],
    });
  }

  private refreshTable(): void {
    this.dataSource.data = this.peopleFormArray.controls;
  }

  public addRow(): void {
    this.peopleFormArray.push(this.createFormGroupRow());
    this.refreshTable();
  }

  public deleteRow(row: FormGroup): void {
    const index = this.peopleFormArray.controls.indexOf(row);
    if (index < 0) return;

    this.peopleFormArray.removeAt(index);
    this.refreshTable();
  }

  public cancelChanges(): void {
    this.peopleFormArray.clear();
    this.originalData.forEach(dataItem => {
      this.peopleFormArray.push(this.createFormGroupRow(dataItem));
    });
    this.refreshTable();
  }

  public onFormSubmit(): void {
    const dirtyRows = this.peopleFormArray.controls.filter(formGroup => formGroup.dirty);
    const formValue = dirtyRows.map(row => row.value);
    console.log(formValue);
    //TODO: backend integration
  }
}
