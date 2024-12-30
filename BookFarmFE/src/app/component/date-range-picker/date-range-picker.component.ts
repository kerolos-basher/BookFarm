import { CommonModule } from '@angular/common';
import { Component, effect, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import{DateService}from '../../services/Date.service'
import { BookService } from '../../services/book.service';
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
export class DateRangePickerComponent implements OnInit{
  @Input() set unavailableDates(dates: Date[]) {
  this._unavailableDates = dates.map(
    (date) => new Date(date.getFullYear(), date.getMonth(), date.getDate()) // Strip time
  );
}
private _unavailableDates: Date[] = [  
  new Date('2024-12-31T10:30:00'),
  new Date('2024-12-29T15:45:00'),

];
get unavailableDates(): Date[
  
] {
  return this._unavailableDates;
}
  @Input() minDate: Date | null = null;
  @Input() maxDate: Date | null = null;
  @Output() dateRangeSelected = new EventEmitter<{ startDate: Date | null; endDate: Date | null }>();

  selectedStartDate: Date | null = null;
  selectedEndDate: Date | null = null;
  isSelectingStartDate: boolean = true; // Toggle between start and end date
  validationMessage: string | null = null;
constructor(private snackBar: MatSnackBar ,public dateService:DateService,public bookService:BookService ){

  effect(()=>{
    debugger
    this.bookService.dates().forEach(element => {
      this.unavailableDates.push (new Date (element))
    });
  })
}
  ngOnInit(): void {
    debugger
   console.log(this.unavailableDates);
  }
  
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

  // onDateChange(event: any): void {
  //   const selectedDate = event.value;

  //   if (this.isSelectingStartDate) {
  //     this.startDate = selectedDate;
  //     // Reset end date when start date changes
  //     this.validationMessage = null; // Reset validation message
  //   } 
  //      else {
  //       this.validationMessage = 'End date must be after or equal to the start date.';
  //     }
  //   }

  onDateChange(type: 'startDate' | 'endDate', event: any): void {
    debugger
    if (type === 'startDate') {
      this.selectedStartDate = event.value;
      this.dateService.globalStartdate=event.value;
      if (!this.isDateAvailable(this.selectedStartDate))
      {
        this.dateService.globalStartdate =''
      }
      console.log('Selected Start Date:', this.selectedStartDate);
    } else if (type === 'endDate') {
      this.selectedEndDate = event.value;
      this.dateService.globalEnddate=event.value;
      this.validateDateRange(new Date(this.dateService.globalStartdate),event.value)

      if (!this.isDateAvailable(this.selectedEndDate))
        {
          this.dateService.globalEnddate =''
        }
        effect(()=>{
          debugger
          this.bookService.dates().forEach(element => {
            this.unavailableDates.push (new Date (element))
          });
        })
      console.log('Selected End Date:', this.selectedEndDate);
    }

    // Validate date range
    if (this.selectedStartDate && this.selectedEndDate && this.selectedStartDate > this.selectedEndDate) {
      console.error('End date must be after the start date.');
    }
  }

  
  // onDateChange2(event: any): void {
  //   const selectedDate = event.value;

  //   if (this.isSelectingStartDate) {
  //     if (this.startDate && selectedDate >= this.startDate) {
  //       this.endDate = selectedDate;
  //       this.validateDateRange(this.startDate, this.endDate!);

  //       // Emit the selected date range
  //       this.dateRangeSelected.emit({ startDate: this.startDate, endDate: this.endDate });
  //     } else {
  //       this.validationMessage = 'End date must be after or equal to the start date.';
  //     }
  //   }

    
  // }
  isDateAvailable = (date: Date | null): boolean => {
    debugger
    if (!date) return false;
  
    const inputDate = new Date(date.getFullYear(), date.getMonth(), date.getDate()); // Normalize input date
    return !this.unavailableDates.some((unavailableDate) => {
      const normalizedUnavailableDate = new Date(
        unavailableDate.getFullYear(),
        unavailableDate.getMonth(),
        unavailableDate.getDate()
      );
      return inputDate.getTime() === normalizedUnavailableDate.getTime();
    });
  };
  validateDateRange(startDate: Date, endDate: Date): void {
    const currentDate = new Date(startDate);

    while (currentDate <= endDate) {
      const isUnavailable = this.unavailableDates.some(
        (unavailableDate) =>
          new Date(unavailableDate).toDateString() === currentDate.toDateString()
      );

      if (isUnavailable) {
        debugger
        this.openSnackbar(
          'The selected date range includes unavailable dates. Please choose a valid range.'
        );
        this.selectedStartDate = null;
        this.selectedEndDate = null;
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