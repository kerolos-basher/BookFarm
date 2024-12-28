import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';



@Injectable({
  providedIn: 'root'
})
export class ConfirmService {
 
  baseFileApi: string = environment.apiUrl;
  httpClient = inject(HttpClient);

  baseApi: string = environment.apiUrl;
  constructor() { }

 




  postBooking(data: any) {
    debugger
    const apiUrl = `${this.baseApi}/ConfirmBook/comfirmBook`; // Replace with your actual API endpoint
    return this.httpClient.post(apiUrl, data);
  }
}
