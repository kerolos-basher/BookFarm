import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Place } from '../component/book/book.interface';
import { datesByPlace } from '../component/book/book.interface';



@Injectable({
  providedIn: 'root'
})
export class BookService {
 
  baseFileApi: string = environment.apiUrl;
  httpClient = inject(HttpClient);

  baseApi: string = environment.apiUrl;
  constructor() { }

  places = signal<Place[]>([]);
  getPlaces(): void {
    this.httpClient
      .get<Place[]>(`${this.baseApi}/Places`)
      .subscribe((data) => {
        this.places.set(data);
      });
  }
  dates = signal<string[]>([]); // Assuming the API returns an array of strings

getDates(id: number): void {
  this.httpClient
    .get<string[]>(`${this.baseApi}/PlaceBookDate/${id}`)
    .subscribe((data) => {
      this.dates.set(data); // Update the signal with the fetched dates

      // Extract days, months, and years from the dates
      // Extracting the year
    });
}
// Total = signal<string>(''); // Assuming the API returns an array of strings
// onCheckboxClick(id:number,from:string,to:string): void {
//   this.httpClient
//   .get<string>(`${this.baseApi}`+`/api/GetTotalPrice?PlaceID=${id}&FromDate=${from}&ToDate=${to}`)
//   .subscribe((data)=>this.Total.set(data));
  
// }
 
  postBooking(data: any) {
    debugger
    const apiUrl = `${this.baseApi}/BookPost/AddUser`; // Replace with your actual API endpoint
    return this.httpClient.post(apiUrl, data);
  }
}
