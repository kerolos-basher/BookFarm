import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import {ConfirmService  } from '../../services/confirm.service';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-confirm-book',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatFormFieldModule,
    MatInputModule,
    CommonModule,
    RouterModule,
  ],
  templateUrl: './Confirmbook.component.html',
  styleUrls: ['./ConfirmBook.component.scss'],
})
export class ConfirmBookComponent implements OnInit {
  confirmCodeForm: FormGroup;
  ConfirmForm: FormGroup;
  selectedFile: File | null = null;
  carouselItems: any[] = [];
  apiUrl = 'https://localhost:7125/api/Carousel/carousel';

  showUploadForm = false;
  showNotFoundMessage = false;
  showErrorMessage = false;

  constructor(private fb: FormBuilder, private http: HttpClient ,public confirmServes: ConfirmService) {
    this.confirmCodeForm = this.fb.group({
      confirmCode: ['', Validators.required],
    })

    this.ConfirmForm = this.fb.group({
      picture: [null, Validators.required],
    });
  }

  ngOnInit(): void {
    this.fetchCarouselItems();
  }

  fetchCarouselItems(): void {
    this.http.get<any[]>(this.apiUrl).subscribe({
      next: (data) => {
        this.carouselItems = data;
      },
      error: (error) => {
        console.error('Error fetching carousel items:', error);
      },
    });
  }
 

  onSubmit(): void {
    if (this.confirmCodeForm.valid) {
      const code = this.confirmCodeForm.value.confirmCode;

      this.http.post('https://localhost:7125/api/CheckBooksID', { Id: code }).subscribe({
    
        next: () => {
          this.showUploadForm = true;
          this.showNotFoundMessage = false;
          this.showErrorMessage = false;
        },
        error: (err) => {
          if (err.status === 404) {
            this.showUploadForm = false;
            this.showNotFoundMessage = true;
          } else {
            this.showUploadForm = false;
            this.showErrorMessage = true;
          }
        },
      });
    }
  }

  onConfirmSubmit(): void {
    debugger;
    if (this.ConfirmForm.invalid || !this.selectedFile) {
      alert('Please fill out all fields and upload a valid image.');
      return;
    }
  
    const ConfirmCode = this.confirmCodeForm.value.confirmCode;
    const base64String = this.ConfirmForm.value.picture; // Base64 string from form
  
    if (!base64String) {
      alert('Error: Base64 string not generated.');
      return;
    }
  
    const postData = {
      ConfirmCode,
      Picture :base64String,
    };
  
    this.confirmServes.postBooking(postData).subscribe(
      (response) => {
        alert('تم الحجز بنجاح!');
      },
      (error) => {
        console.error('Error:', error);
        alert('Error submitting the booking.');
      }
    );
  }
  // onConfirmSubmit(): void {
  //   debugger
  //   if (this.ConfirmForm.invalid || !this.selectedFile) {
  //     alert('Please fill out all fields and upload a valid image.');
  //     return;
  //   }
 
  //   const reader = new FileReader();
  //   reader.onload = () => {
  //     const base64String = reader.result?.toString().split(',')[1];
  //     const ConfirmCode =this.confirmCodeForm.value.confirmCode;
  //     const postData = {
  //       ConfirmCode,
  //       base64String
       
  //     };
      
    

  //     this.confirmServes.postBooking(postData).subscribe(
  //       (response) => {
  //         alert('تم الحجز بنجاح!');
  //       },
  //       (error) => {
  //         console.error('Error:', error);
  //         alert('Error submitting the booking.');
  //       }
  //     );
  //   };

  //   // reader.readAsDataURL(this.selectedFile);
  // }

  onFileChange(event: any): void {
    debugger;
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
  
      // Use FileReader to convert file to Base64
      const reader = new FileReader();
      reader.onload = () => {
        // Check if FileReader has a result
        if (reader.result) {
          const base64String = reader.result.toString().split(',')[1];
          this.ConfirmForm.patchValue({ picture: base64String }); // Patch Base64 string to the form
          console.log('Base64 String:', base64String); // Debugging log
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


  // onFileChange(event: any): void {
  //   debugger
  //   const file = event.target.files[0];
  //   if (file) {
  //     this.selectedFile = file;
  //     this.ConfirmForm.patchValue({ picture: file.name });
  //   }
  // }
  // onFileChange(event: Event): void {
  //   const input = event.target as HTMLInputElement;
  //   if (input?.files?.length) {
  //     this.selectedFile = input.files[0];
  //     this.ConfirmForm.patchValue({ picture: this.selectedFile });
  //   }
  // }
}
