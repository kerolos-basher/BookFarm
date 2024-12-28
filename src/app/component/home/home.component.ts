import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {  ReactiveFormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { RouterModule } from '@angular/router';
import { CardService } from '../../services/card.service';
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

  constructor(private cardService: CardService) {}

  ngOnInit(): void {
    this.fetchCards();
  }

  fetchCards(): void {
    this.cardService.getCards().subscribe(
      (data) => {
        this.cards = data;
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
