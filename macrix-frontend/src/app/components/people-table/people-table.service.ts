import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import sampleSize from 'lodash-es/sampleSize';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { MOCK_PEROPLE_REPO } from './shared/mock-people';
import { PersonEntity } from './shared/person-entity.model';

const PREFIX = 'https://localhost:7227/api/People';
const IS_MOCK = false;

@Injectable({
  providedIn: 'root'
})
export class PeopleTableService {
  constructor(private http: HttpClient) { }

  public getAll(): Observable<PersonEntity[]> {
    if (IS_MOCK)
      return of(sampleSize(MOCK_PEROPLE_REPO, 10));
    
    return this.http.get<PersonEntity[]>(`${PREFIX}`).pipe(map(response => {
      for (const item of response) {
        item.dateOfBirth = new Date(item.dateOfBirth);
      }
      return response;
    }));
  }

  public removeOne(id: number) {
    if (!Number.isInteger(id) || IS_MOCK)
      return of();
    
    return this.http.delete(`${PREFIX}/${id}`);
  }

  public saveChanges(enities: PersonEntity[]):  Observable<PersonEntity[] | null> {
    return this.http.post<PersonEntity[]>(`${PREFIX}/batchInsertUpdate`, enities);
  }
}
