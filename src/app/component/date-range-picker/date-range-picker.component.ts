import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-date-range-picker',
  standalone: true,
  imports: [ CommonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatInputModule,
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,],
    schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './date-range-picker.component.html',
  styleUrl: './date-range-picker.component.scss'
})
export class DateRangePickerComponent {
  @Input() unavailableDates: Date[] = [];
  @Input() minDate: Date | null = null;
  @Input() maxDate: Date | null = null;
  @Output() dateRangeSelected = new EventEmitter<{ startDate: Date | null; endDate: Date | null }>();

  startDate: Date | null = null;
  endDate: Date | null = null;
  isSelectingStartDate: boolean = true; // Toggle between start and end date
  validationMessage: string | null = null;
constructor(private snackBar: MatSnackBar){}
  
  // onDateChange(event: any): void {
  //   const selectedDate = event.value;

  //   if (this.isSelectingStartDate) {
  //     this.startDate = selectedDate;
  //     this.endDate = null; // Reset end date when start date changes
  //     this.validationMessage = null; // Reset validation message
  //   } else {
  //     if (this.startDate && selectedDate >= this.startDate) {
  //       this.endDate = selectedDate;
  //       this.validateDateRange(this.startDate, this.endDate!);
  //     } else {
  //       this.validationMessage = 'End date must be after or equal to the start date.';
  //     }
  //   }

  //   this.isSelectingStartDate = !this.isSelectingStartDate;
  // }

  onDateChange(event: any): void {
    const selectedDate = event.value;

    if (this.isSelectingStartDate) {
      this.startDate = selectedDate;
      this.endDate = null; // Reset end date when start date changes
      this.validationMessage = null; // Reset validation message
    } else {
      if (this.startDate && selectedDate >= this.startDate) {
        this.endDate = selectedDate;
        this.validateDateRange(this.startDate, this.endDate!);

        // Emit the selected date range
        this.dateRangeSelected.emit({ startDate: this.startDate, endDate: this.endDate });
      } else {
        this.validationMessage = 'End date must be after or equal to the start date.';
      }
    }

    this.isSelectingStartDate = !this.isSelectingStartDate;
  }
  isDateAvailable = (date: Date | null): boolean => {
    if (!date) return false;
    return !this.unavailableDates.some(
      (unavailableDate) =>
        new Date(unavailableDate).toDateString() === date.toDateString()
    );
  };
  validateDateRange(startDate: Date, endDate: Date): void {
    const currentDate = new Date(startDate);

    while (currentDate <= endDate) {
      const isUnavailable = this.unavailableDates.some(
        (unavailableDate) =>
          new Date(unavailableDate).toDateString() === currentDate.toDateString()
      );

      if (isUnavailable) {
        this.openSnackbar(
          'The selected date range includes unavailable dates. Please choose a valid range.'
        );
        this.startDate = null;
        this.endDate = null;
        this.isSelectingStartDate = true;
        return;
      }

      // Move to the next day
      currentDate.setDate(currentDate.getDate() + 1);
    }
  }
  openSnackbar(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 5000, // Duration in milliseconds
      verticalPosition: 'bottom', // Position: 'top' | 'bottom'
      horizontalPosition: 'center', // Position: 'start' | 'center' | 'end' | 'left' | 'right'
    });
  }
}