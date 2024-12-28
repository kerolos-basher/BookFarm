import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { BookService } from '../../services/book.service';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Place } from './book.interface';
import { DateRangePickerComponent } from '../date-range-picker/date-range-picker.component';
@Component({
  selector: 'app-book',
  standalone: true,
  imports: [
    MatDatepickerModule,
    MatNativeDateModule,
    MatInputModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    CommonModule,
    DateRangePickerComponent
  ],
  templateUrl: './book.component.html',
  styleUrl: './book.component.scss'
})
export class BookComponent implements OnInit  {

  unavailableDates: Date[] = [new Date(2024, 11, 25), new Date(2024, 11, 31)];
  minDate: Date = new Date(2024, 0, 1); // January 1, 2024
  maxDate: Date = new Date(2024, 11, 31); // December 31, 2024


  bookingForm: FormGroup;
  selectedFile: File | null = null;
  disabledDays: number[] = []; 
  disabledMonths: number[] = [];
  disabledYears: number[] = []; // Example: Disable the 15th of every month
  disabledWeekdays: number[] = [];
  constructor(private fb: FormBuilder, public bookService: BookService) {
    this.bookingForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern('[0-9]+')]],
      picture: [null, Validators.required],
      dropdown: [null, Validators.required],
      dateRange: this.fb.group({
        start: ['', Validators.required],
        end: ['', Validators.required],
      }),
    });
  }

  ngOnInit(): void {
    this.bookService.getPlaces();
   
   
  }

  onPlaceSelected(): void {
    debugger
    const selectedPlace = this.bookingForm.get('dropdown')?.value;
    this.bookService.getDates(selectedPlace);
   
    if (selectedPlace) {
      const days = this.bookService.days();
      const months = this.bookService.months();
      const years = this.bookService.years();

      // Make arrays distinct
      this.disabledDays = [...new Set(days)];
      this.disabledMonths = [...new Set(months)];
      this.disabledYears = [...new Set(years)];
      // this.bookingForm.get('startDate')?.enable();
      // this.bookingForm.get('endDate')?.enable();
    } 
  }
  onFileChange(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      this.bookingForm.patchValue({ picture: file.name });
    }
  }

  onSubmit(): void {
    if (this.bookingForm.invalid || !this.selectedFile) {
      alert('Please fill out all fields and upload a valid image.');
      return;
    }
 
    const reader = new FileReader();
    reader.onload = () => {
      const base64String = reader.result?.toString().split(',')[1];

      const postData = {
        Name: this.bookingForm.get('name')?.value,
        Email: this.bookingForm.get('email')?.value,
        PhoneNumber: this.bookingForm.get('phone')?.value,
        Picture: base64String, // Send Base64 image to backend
        DateFrom: this.bookingForm.get('startDate')?.value,
        DateTo: this.bookingForm.get('endDate')?.value,
        ConfirmCode: Array.from({ length: 12 }, () => Math.floor(Math.random() * 10)).join(''),
        PlaceID: this.bookingForm.get('dropdown')?.value
      };

      this.bookService.postBooking(postData).subscribe(
        (response) => {
          alert('تم الحجز بنجاح!');
        },
        (error) => {
          console.error('Error:', error);
          alert('Error submitting the booking.');
        }
      );
    };

    reader.readAsDataURL(this.selectedFile);
  }

  dateFilter = (date: Date | null): boolean => {
    if (!date) return false;

    const day = date.getDate(); // Day of the month
    const month = date.getMonth() + 1; // Month (0 = January, so +1 for 1-based indexing)
    const year = date.getFullYear(); // Year
    const weekday = date.getDay(); // Weekday (0 = Sunday, 6 = Saturday)

    return (
      !this.disabledDays.includes(day) && // Allow only enabled days
      !this.disabledMonths.includes(month) && // Allow only enabled months
      !this.disabledYears.includes(year) && // Allow only enabled years
      !this.disabledWeekdays.includes(weekday) // Allow only enabled weekdays
    );
  }
}
  // dateFilter = (date: Date | null): boolean => {
  //   if (!date) return false;
  
  //   const day = date.getDate(); // Day of the month
  //   const month = date.getMonth() + 1; // Month (0 = January, so +1 for 1-based indexing)
  //   const year = date.getFullYear(); // Year
  
  //   return (
  //     !this.disabledDays.includes(day) && 
  //     !this.disabledMonths.includes(month) && 
  //     !this.disabledYears.includes(year)
  //   );
  // };
// }