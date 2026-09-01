import { CommonModule } from '@angular/common';
import { Component, Inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { AddSlotFormData } from '../../../interfaces/add-slot-form-data.interface';


@Component({
  selector: 'app-add-slot-form',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
  ],
  templateUrl: './add-slot-form.html',
})
export class AddSlotFormComponent {
  protected readonly start: any;
  protected readonly end: any;
  protected readonly note: any;

  constructor(
    public dialogRef: MatDialogRef<AddSlotFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: AddSlotFormData
  ) {
    this.start = signal(data.startTime);
    this.end = signal(data.endTime);
    this.note = signal(data.note);
  }

  protected setStart(e: Event) {
    this.start.set((e.target as HTMLInputElement).value);
  }

  protected setEnd(e: Event) {
    this.end.set((e.target as HTMLInputElement).value);
  }

  protected setNote(e: Event) {
    this.note.set((e.target as HTMLInputElement).value);
  }

  protected onCancel() {
    this.dialogRef.close();
  }

  protected onSubmit() { 
    this.dialogRef.close({
      startTime: this.start(),
      endTime: this.end(),
      note: this.note()
    });
  }
}
