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
  currentSelectedPlace = signal<number>(0);
  baseApi: string = environment.apiUrl;
  constructor() { }

  places = signal<Place[]>([]);
  getPlaces(): void {
    this.httpClient
      .get<Place[]>(`${this.baseApi}/Places`)
      .subscribe((data) => {
        data.map(item=>({
          ...item,
          imgUrl :`${environment.ImgUrl}${item.imgUrl}`

        }))
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
 TotalPrice = signal<string>(''); // Assuming the API returns an array of strings

getTotalPrice(fromDate: string, toDate: string): void {
  
  const url = environment.apiUrl+`/GetTotalPrice/TotalPrice?PlaceID=${this.currentSelectedPlace()}&FromDate=${fromDate}&ToDate=${toDate}`;
 
  this.httpClient.get<{price: string}>(url).subscribe({
    next: (data) => {
     this.TotalPrice.set(data.price);
     
      //this.TotalPrice .set(data);// Update the signal with the fetched dates

      // Further processing, if needed
    },
   
    error: (err) => {
      return '';
      console.error('Error fetching data:', err);
    }

    
  });
}
clearTotalPrice(): void {
  this.TotalPrice.set(''); // Clear the total price
}


  postBooking(data: any) {
    
    const apiUrl = `${this.baseApi}/BookPost/AddUser`; // Replace with your actual API endpoint
    return this.httpClient.post(apiUrl, data);
  }
}
