import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AvailabilitySlotDto } from '../interfaces/availability-slot-dto.interface'; 
import { CreateAvailabilitySlotRequest } from '../interfaces/create-availability-slot-request.interface'; 
import { Observable } from 'rxjs'; 


@Injectable({ 
  providedIn: 'root' 
})
export class AvailabilitySlotRequestService { 
  private httpClient = inject(HttpClient);
  private readonly apiBase = '/api/availabilityslot';

  getByMonth(userId: string, year: number, month: number): Observable<AvailabilitySlotDto[]> {
    return this.httpClient.get<AvailabilitySlotDto[]>(
      `${this.apiBase}/${userId}/${year}/${month}`
    )
  }

  create(request: CreateAvailabilitySlotRequest): Observable<void> {
    return this.httpClient.post<void>(
      `${this.apiBase}`,
      request 
    );
  }

  delete(id: string): Observable<void> {
    return this.httpClient.delete<void>(
      `${this.apiBase}/${id}`,
    );
  }
}
