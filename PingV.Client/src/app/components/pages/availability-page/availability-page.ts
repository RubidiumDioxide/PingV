import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Component, OnInit, computed, inject, signal, PLATFORM_ID } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner'; 
import { AddSlotFormComponent } from '../../forms/add-slot-form/add-slot-form';
import { AvailabilitySlotDto } from '../../../interfaces/availability-slot-dto.interface'; 
import { CreateAvailabilitySlotRequest } from '../../../interfaces/create-availability-slot-request.interface';
import { CalendarCell } from '../../../interfaces/calendar-cell.interface';
import { AvailabilitySlotRequestService } from '../../../requests/availability-slot-request-service';
import { FormsModule } from '@angular/forms';
import { AddSlotFormData } from '../../../interfaces/add-slot-form-data.interface';
import { CascadingService } from '../../../service/cascading.service';
import { SnackbarService } from '../../../service/snackbar.service';


@Component({
  selector: 'app-availability-slot-page',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule, 
    MatIconModule, 
    MatCardModule, 
    MatButtonModule, 
    MatSelectModule, 
    MatToolbarModule, 
    MatProgressSpinnerModule, 
    MatFormFieldModule, 
    MatInputModule, 
  ], 
  templateUrl: './availability-page.html',
  styleUrl: './availability-page.scss'
})
export class AvailabilityPage implements OnInit { 
  private readonly platformId = inject(PLATFORM_ID);
  readonly cascadingService = inject(CascadingService); 
  readonly snackbarService = inject(SnackbarService); 
  private readonly dialog = inject(MatDialog);

  private readonly availabilitySlotRequestService = inject(AvailabilitySlotRequestService);

  protected readonly year = signal(new Date().getFullYear());
  protected readonly month = signal(new Date().getMonth() + 1);
  protected readonly availabilitySlots = signal<AvailabilitySlotDto[]>([]);
  protected readonly isLoading = signal(false);
  protected readonly errorMessage = signal('');
  protected readonly weekdays = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
  protected readonly monthNames = [
    'January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December'
  ];

  // loading & operations 
  ngOnInit(): void {
    if(isPlatformBrowser(this.platformId)){
      this.getMonthlySchedule(2026, 8);
    }
  }

  getMonthlySchedule(year: number = this.year(), month: number = this.month()): void {
    this.isLoading.set(true);
    
    var userId = this.cascadingService.selectedUser()?.id; 

    if(userId){
      this.availabilitySlotRequestService.getByMonth(userId, year, month).subscribe({
        next: (data) => {
          this.availabilitySlots.set(data);
          this.isLoading.set(false);
        },
        error: (err) => {
          this.snackbarService.showError('Failed to load slots.');
          this.isLoading.set(false);
        }
      });    
    }
  }

  createSlot(start: Date, end: Date, note: string): void {
    var userId = this.cascadingService.selectedUser()?.id; 

    if(userId){
      var request: CreateAvailabilitySlotRequest = {
        creatorId: userId, 
        start: new Date(start).toISOString(), 
        end: new Date(end).toISOString(), 
        note: note 
      }

      this.availabilitySlotRequestService.create(request).subscribe({
        next: (createdSlot) => {
          this.getMonthlySchedule(); 
        },
        error: (err) => this.snackbarService.showError('Could not create slot.')
      });
    }
  }

  deleteSlot(slotId: string): void {
    this.availabilitySlotRequestService.delete(slotId).subscribe({
      next: () => {
        this.getMonthlySchedule(); 
      },
      error: (err) => this.snackbarService.showError('Could not delete slot.')
    });
  }


  protected readonly calendarDays = computed<CalendarCell[]>(() => {
    const year = this.year();
    const month = this.month();
    const firstOfMonth = new Date(year, month - 1, 1);
    const firstWeekday = firstOfMonth.getDay();
    const startDate = new Date(year, month - 1, 1 - firstWeekday);
    const cells: CalendarCell[] = [];

    for (let i = 0; i < 42; i += 1) {
      const cellDate = new Date(startDate);
      cellDate.setDate(startDate.getDate() + i);
      const dayKey = cellDate.toISOString().slice(0, 10);
      const slots = this.availabilitySlots().filter(slot => slot.start.startsWith(dayKey));
      cells.push({
        date: cellDate,
        isCurrentMonth: cellDate.getMonth() === month - 1,
        slots
      });
    }

    return cells;
  });

  protected readonly displayMonthLabel = computed(() => {
    return `${this.monthNames[this.month() - 1]} ${this.year()}`;
  });

  protected setYear(value: Event | number) {
    const numberValue = typeof value === 'number' ? value : (value.target as HTMLInputElement).value;
    this.year.set(Number(numberValue) || new Date().getFullYear());
    void this.getMonthlySchedule();
  }

  protected setMonth(value: number) {
    this.month.set(Number(value) || 1);
    void this.getMonthlySchedule();
  }

  protected openCreateModal(date: Date) {
    const startDate = new Date(date.getFullYear(), date.getMonth(), date.getDate(), 9, 0, 0, 0);
    const endDate = new Date(date.getFullYear(), date.getMonth(), date.getDate(), 10, 0, 0, 0);
    const dialogData: AddSlotFormData = {
      selectedDate: date,
      startTime: this.formatDateTimeLocal(startDate),
      endTime: this.formatDateTimeLocal(endDate),
      note: ''
    };

    const dialogRef = this.dialog.open(AddSlotFormComponent, { data: dialogData, width: '500px', maxWidth: '100%' });
    dialogRef.afterClosed().subscribe(result => {
      if (result) void this.createSlot(result.startTime, result.endTime, result.note);
    });
  }

  protected formatDateTimeLocal(d: Date) {
    const tzOffset = d.getTimezoneOffset();
    const local = new Date(d.getTime() - tzOffset * 60000);
    return local.toISOString().slice(0, 16);
  }

  protected changeMonth(offset: number) {
    const date = new Date(this.year(), this.month() - 1 + offset, 1);
    this.year.set(date.getFullYear());
    this.month.set(date.getMonth() + 1);
    void this.getMonthlySchedule();
  }
}
