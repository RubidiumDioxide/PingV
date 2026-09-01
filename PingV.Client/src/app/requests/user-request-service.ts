import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserDto } from '../interfaces/user-dto.interface'; 
import { Observable } from 'rxjs'; 


@Injectable({ 
  providedIn: 'root' 
})
export class UserRequestService {
  private httpClient = inject(HttpClient);
  private readonly apiBase = 'https://localhost:7170/api/user';

  get(): Observable<UserDto[]> {
    return this.httpClient.get<UserDto[]>(
      this.apiBase
    )
  }

  getById(id: string): Observable<UserDto> {
    return this.httpClient.get<UserDto>(
      `${this.apiBase}/${id}` 
    );
  }
}
