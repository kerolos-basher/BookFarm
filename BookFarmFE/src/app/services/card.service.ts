import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
export interface CardObject {
    Id: number;
    ImageUrl: string;
    CaptionTitle: string;
    CaptionText: string;
  }

@Injectable({
  providedIn: 'root',
})

export class CardService {
  private apiUrl = environment.apiUrl+'/Carousel/carousel'; // Replace with your actual API URL

  constructor(private http: HttpClient) {}

  getCards(): Observable<CardObject[]> {
    return this.http.get<CardObject[]>(this.apiUrl);
  }
}
