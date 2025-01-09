import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { Subject } from 'rxjs';

export interface CardObject {
    Id: number;
    ImageUrl: string;
    CaptionTitle: string;
    CaptionText: string;
  }

@Injectable({
  providedIn: 'root',
})
export class DateService {
    public globalStartdate: Date |null= null;
    public globalEnddate:  Date |null= null;
   
    private resetSubject = new Subject<void>();

    // Observable to listen for reset triggers
    reset$ = this.resetSubject.asObservable();
  
    // Method to trigger the reset
    triggerReset(): void {
      this.resetSubject.next();
    }

    

    
  }
