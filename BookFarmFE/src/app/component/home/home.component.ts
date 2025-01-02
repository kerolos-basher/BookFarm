import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {  ReactiveFormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { RouterModule } from '@angular/router';
import { CardService } from '../../services/card.service';
import { HttpClient } from '@angular/common/http';
import{environment} from'../../../environments/environment.development'
export interface CardObject {
  Id: number;
  ImageUrl: string;
  CaptionTitle: string;
  CaptionText: string;
}
@Component({
  selector: 'app-home',
  
  standalone: true,
  imports:  [
      ReactiveFormsModule,
      MatDatepickerModule,
      MatNativeDateModule,
      MatFormFieldModule,
      MatInputModule,
      CommonModule,ReactiveFormsModule,
      RouterModule  
      
    ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})

export class HomeComponent implements OnInit{
  cards: CardObject[] = [];
  apiUrl: string = environment.apiUrl; // Replace with your actual API URL
  carouselData: any[] = [];

  constructor(private cardService: CardService,private http: HttpClient) {}

  ngOnInit(): void {
   this.fetchCarouselData();
    this.fetchCards();
  }


  fetchCarouselData(): void {
    this.http.get<any[]>(`${this.apiUrl}/Carousel/carousel`).subscribe({
      next: (data) => {
        this.carouselData = data.map(item => ({
          ...item,
          ImageUrl: `${environment.ImgUrl}${item.ImageUrl}`
        }));
     //   this.carouselData.ImageUrl = environment.apiUrl+this.carouselData.ImageUrl;
      },
      error: (err) => {
        console.error('Error fetching carousel data:', err);
      }
    });
  }
  fetchCards(): void {
    this.cardService.getCards().subscribe(
      (data) => {
        this.cards = data.slice(1).map(Item=>({
          ...Item,
          ImageUrl:`${environment.ImgUrl}${Item.ImageUrl}`
        }))
      },
      (error) => {
        console.error('Error fetching cards', error);
      }
    );
  }
  // cards = [
  //   { image: 'assets/img/1.jpg', alt: 'Pic 4', caption: 'Caption 4' },
  //   { image: 'assets/img/2.jpeg', alt: 'Pic 5', caption: 'Caption 5' },
  //   { image: 'assets/img/3.jpeg', alt: 'Pic 6', caption: 'Caption 6' },
  //   { image: 'assets/img/4.jpeg', alt: 'Pic 6', caption: 'Caption 7' },
  // ];
}
