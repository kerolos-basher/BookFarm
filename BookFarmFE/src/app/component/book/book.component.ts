import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, effect, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { BookService } from '../../services/book.service';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Place } from './book.interface';
import { DateRangePickerComponent } from '../date-range-picker/date-range-picker.component';
import { environment } from '../../../environments/environment.development';
import { DateService } from '../../services/Date.service';
import { MatDialog } from '@angular/material/dialog';
import { PopUpComponent } from '../pop-up/pop-up.component';
import { Router } from '@angular/router';

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
    DateRangePickerComponent,
    PopUpComponent,
    

  ],
  templateUrl: './book.Component.html',
  styleUrl: './book.component.scss'
})
export class BookComponent implements OnInit  {
  carouselData: any[] = [];
  apiUrl: string = environment.apiUrl; // Replace with your actual API URL

  unavailableDates: Date[] = [];
  minDate: Date = new Date(); // January 1, 2024
  maxDate: Date = new Date(2050, 11, 31); // December 31, 2024
  base64String = ""
  selectedStartDate:string|null=null;
  selectedEndDate: string | null = null;
  isPlaceSelected: boolean = false;


  bookingForm: FormGroup;
  selectedFile: File | null = null;
  disabledDays: number[] = []; 
  disabledMonths: number[] = [];
  disabledYears: number[] = []; // Example: Disable the 15th of every month
  disabledWeekdays: number[] = [];
  constructor(private fb: FormBuilder,private http: HttpClient,public dateService:DateService,private dialog:MatDialog ,public bookService: BookService) {
  
   
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
    this.fetchCarouselData();
   
   
  }
  showPopup(sign: string, message: string,color:string): void {
    this.dialog.open(PopUpComponent, {
      width: '400px',
      data: { sign, message ,color },
    });
  }
 
  fetchCarouselData(): void {
    this.http.get<any[]>(`${this.apiUrl}/Carousel/carousel`).subscribe({
      next: (data) => {
        this.carouselData = data.map(item => ({
          ...item,
          ImageUrl: `${environment.ImgUrl}${item.ImageUrl}`
        }));
      },
      error: (err) => {
        console.error('Error fetching carousel data:', err);
      }
    });}

    

  onPlaceSelected(): void {
    debugger
    this.isPlaceSelected = true;
    this.bookService.clearTotalPrice();
    this.dateService.globalEnddate ="";
    this.dateService.globalStartdate ="";
    this.dateService.triggerReset();
    
    const selectedPlace = this.bookingForm.get('dropdown')?.value;
    this.bookService.getDates(selectedPlace);
    this.bookService.currentSelectedPlace.set(selectedPlace);   
    if (selectedPlace) {
      this.bookService.dates().forEach(element => {
        this.unavailableDates.push (new Date (element))
      });
      // [new Date(2024, 11, 25), new Date(2024, 11, 31)]
      // const days = this.bookService.days();
      // const months = this.bookService.months();
      // const years = this.bookService.years();

      // Make arrays distinct
    
      // this.bookingForm.get('startDate')?.enable();
      // this.bookingForm.get('endDate')?.enable();
    } 
  }
  // onFileChange(event: any): void {
  //   const file = event.target.files[0];
  //   if (file) {
  //     this.selectedFile = file;
  //     this.bookingForm.patchValue({ picture: file.name });
  //   }
  // }
 
  onFileChange(event: any): void {
    ;

    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      
      // Use FileReader to convert file to Base64
      const reader = new FileReader();
      reader.onload = () => {
        // Check if FileReader has a result
        if (reader.result) {
          this.base64String = reader.result.toString().split(',')[1];
          // this.bookingForm.patchValue({ picture: base64String }); // Patch Base64 string to the form
          // console.log('Base64 String:', base64String); // Debugging log
        }
      };
  
      reader.onerror = (error) => {
        console.error('Error reading file:', error);
      };
  
      reader.readAsDataURL(file); // Trigger Base64 conversion
    } else {
      console.error('No file selected');
    }
  }
  onDateRangeSelected(): void {
    
    //this.selectedStartDate = this.dateService.globalStartdate;
   // this.selectedEndDate =  this.dateService.globalStartdate;
   
  }
 
  openDialog(){
    this.dialog.open(PopUpComponent);
  };
  onSubmit(): void {
    
   if(this.dateService.globalStartdate==''||this.dateService.globalEnddate=='')
   {
    //this.dialog.open();
    this.showPopup('⚠','خطاء في البيانات يرجى التأكيد ','orange');
 
    
   }
    // if (this.bookingForm.invalid || !this.selectedFile) {
    //   alert('Please fill out all fields and upload a valid image.');
    //   return;
    // }
 
   // const reader = new FileReader();
    //reader.onload = () => {
      //const base64String = reader.result?.toString().split(',')[1];

      const postData = {

        Name: this.bookingForm.get('name')?.value,
        Email: this.bookingForm.get('email')?.value,
        PhoneNumber: this.bookingForm.get('phone')?.value,
        Picture: this.base64String, // Send Base64 image to backend
        DateFrom: (this.dateService.globalStartdate),
        DateTo: (this.dateService.globalEnddate),
        ConfirmCode: Array.from({ length: 12 }, () => Math.floor(Math.random() * 10)).join(''),
        PlaceID: this.bookingForm.get('dropdown')?.value
      };

      this.bookService.postBooking(postData).subscribe(
        
        (response) => {
          this.showPopup('✔', ' تم الحجز بنجاح! لقد تم إرسال كود تأكيد علي بريدك الإلكترونى','green');
          //alert('تم الحجز بنجاح!');
          this.router.navigate(['/ConfirmBook']);
        },
        (error) => {
          console.error('Error:', error);
          //this.showPopup('✔', ' تم الحجز بنجاح! لقد تم إرسال كود تأكيد علي بريدك الإلكترونى','green');

          this.showPopup('⚠','خطاء في الحجز يرجي اعادة المحاولة ','red');

        }
      );
   // };

    //reader.readAsDataURL(this.selectedFile);

    // this.bookingForm.reset({
    //   name: '',
    //   email: '',
    //   phone: '',
    //   picture: null,
    //   dropdown: null,
      
    // });
    

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