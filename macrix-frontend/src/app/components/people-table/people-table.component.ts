import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { PeopleTableService } from './people-table.service';
import { PersonEntity } from './shared/person-entity.model';

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
  public readonly displayedColumns = DEFAULT_DISPLAYED_COLUMNS;
  public readonly dataSource = new MatTableDataSource<AbstractControl>;
  public readonly tableForm: FormGroup;
  private unchangedData: PersonEntity[] = [];

  public get isDataUnchaged(): boolean {
    return this.peopleFormArray.controls.every(x => x.pristine)
      && this.unchangedData.length == this.peopleFormArray.controls.length;
  }

  get peopleFormArray(): FormArray {
    return this.tableForm.get('peopleFormArray') as FormArray;
  }

  constructor(
    private formBuilder: FormBuilder,
    private service: PeopleTableService,
  ) {
    this.tableForm = this.formBuilder.group({
      peopleFormArray: this.formBuilder.array([])
    });

    this.service.getAll().subscribe(data => {
      this.unchangedData = data;
      this.addDataToTable(data);
      this.refreshTable();
    });
  }

  ngOnInit(): void { }

  public addRow(): void {
    this.peopleFormArray.push(this.createFormGroupRow());
    this.refreshTable();
  }

  public deleteRow(row: FormGroup, id: number): void {
    const isUserConfirmed = window.confirm("Are you sure, that you want to delete that?");
    const index = this.peopleFormArray.controls.indexOf(row);
    const shouldMakeServerRequest = id > 0;

    if (!isUserConfirmed || index < 0)
      return;

    if (!shouldMakeServerRequest) {
      this.peopleFormArray.removeAt(index);
      this.refreshTable();
      return;
    }

    this.service.removeOne(id)?.subscribe({
      next: () => {
        this.peopleFormArray.removeAt(index);
        this.refreshTable();
      }
    });
  }

  public cancelChanges(): void {
    this.peopleFormArray.clear();
    this.addDataToTable(this.unchangedData);
    this.refreshTable();
  }

  public onFormSubmit(): void {
    const formValue = this.getChangedRows().map(row => row.value) as PersonEntity[];

    this.service.saveChanges(formValue).subscribe(result => {
      window.alert("Changes saved successufully!");

      if (result) {
        const indices = [];
        for (const row of this.peopleFormArray.controls) {
          const entity = row.value as PersonEntity;
          if (!entity.id)
            indices.push(this.peopleFormArray.controls.indexOf(row));
        }
        for (const index of indices) {
          this.peopleFormArray.removeAt(index);
        }
        this.addDataToTable(result);
        this.refreshTable();
      }

      this.peopleFormArray.markAsPristine();
      this.unchangedData = this.peopleFormArray.controls.map(row => row.value) as PersonEntity[];
    });
  }

  private addDataToTable(data: PersonEntity[]): void {
    data.forEach(dataItem => {
      this.peopleFormArray.push(this.createFormGroupRow(dataItem));
    });
  }

  private createFormGroupRow(dataItem?: PersonEntity): FormGroup {
    return this.formBuilder.group({
      id: [dataItem?.id ?? 0],
      firstName: [dataItem?.firstName ?? "", Validators.required],
      lastName: [dataItem?.lastName ?? "", Validators.required],
      streetName: [dataItem?.streetName ?? "", Validators.required],
      houseNumber: [dataItem?.houseNumber ?? "", Validators.required],
      apartmentNumber: [dataItem?.apartmentNumber ?? ""],
      postalCode: [dataItem?.postalCode ?? "", Validators.required],
      town: [dataItem?.town ?? "", Validators.required],
      phoneNumber: [dataItem?.phoneNumber ?? "", Validators.required],
      dateOfBirth: [dataItem?.dateOfBirth ?? "", Validators.required],
      createdTimestamp: [dataItem?.createdTimestamp]
    });
  }

  private refreshTable(): void {
    this.dataSource.data = this.peopleFormArray.controls;
  }

  private getChangedRows(): AbstractControl[] {
    return this.peopleFormArray.controls.filter(formGroup => formGroup.dirty);
  }
}
