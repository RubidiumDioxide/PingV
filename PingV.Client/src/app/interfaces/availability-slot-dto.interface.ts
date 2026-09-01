export interface AvailabilitySlotDto {
  id: string;
  creatorId: string; 
  start: string;
  end: string;
  note?: string | null;
}
