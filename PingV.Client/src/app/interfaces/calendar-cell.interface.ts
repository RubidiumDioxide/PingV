import { AvailabilitySlotDto } from './availability-slot-dto.interface';

export interface CalendarCell {
  date: Date;
  isCurrentMonth: boolean;
  slots: AvailabilitySlotDto[];
}
