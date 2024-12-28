import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-confirm-book',
  standalone: true,
   imports:  [
        ReactiveFormsModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatFormFieldModule,
        MatInputModule,
        CommonModule,ReactiveFormsModule,
        RouterModule,
      ],
  templateUrl: './confirmBook.component.html',
  styleUrl: './confirmook.component.scss'
})
export class ConfirmBookComponent {

  checkDataForm: FormGroup;
  showUploadForm: boolean = false;
  showNotFoundMessage: boolean = false;
  showErrorMessage: boolean = false;
  selectedFile: File | null = null;


  confirmCodeForm: FormGroup; // Declare the form group

  carouselItems: any[] = [];
  apiUrl = 'https://localhost:7125/api/Carousel/carousel'; // Replace with your API endpoint

  constructor(private fb: FormBuilder ,private http: HttpClient ) {
    // Initialize the form group
    this.checkDataForm = this.fb.group({
      code: ['', Validators.required],
    });
    this.confirmCodeForm = this.fb.group({
      confirmCode: ['', Validators.required] // Add Validators as needed
    });
  }
  ngOnInit(): void {
    this.fetchCarouselItems();

  
  }

  onSubmit(): void {
    const code = this.checkDataForm.value.code;

    if (!code) return;

    this.http
      .post('https://localhost:7125/api/CheckBooksID', { Id: code })
      .subscribe({
        next: (response: any) => {
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
}
