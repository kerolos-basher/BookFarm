
import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment.development';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'; // Import FormsModule

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule ,FormsModule],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss'
})



export class AdminComponent implements OnInit {
  rooms: any[] = [];
  isAuthenticated = false;
  password = '';
  errorMessage = '';
  private correctPassword = '1233456'; // Set your secure password here

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.fetchRooms();
  }


  authenticate(): void {
    if (this.password.trim() === '123456') {
      this.isAuthenticated = true;
      this.errorMessage = '';
    } else {
      this.errorMessage = 'Incorrect password. Please try again.';
    }
  }
  fetchRooms(): void {
    debugger
    this.http.get<any[]>(environment.apiUrl+'/BookFarm').subscribe({
      next: (data) => {
        // this.rooms = data;
        this.rooms = data.map((room) => ({
          id: room.Id, // Extracting specific properties
          Name: room.Name,
          Email: room.Email,
          PhoneNumber: room.PhoneNumber,
          DateFrom: room.DateFrom,
          DateTo: room.DateTo,
          price: room.place?.PriceForNight || 0, // Default value if price is missing
          description: room.place?.Description || 'No description available',
          picturePath: 'https://api.liwa.my-alphatech.com'+ room.PicturePath , // Fallback image
        }));
      },
      error: (err) => {
        console.error('Error fetching rooms:', err);
        //[alert]('Failed to fetch room data. Please try again later.');
      },
    });
  }

  deleteRoom(roomId: number): void {
    debugger
    this.http.delete(environment.apiUrl+`/DeleteBook/${roomId}`).subscribe({
      next: () => {
        this.rooms = this.rooms.filter((room) => room.Id !== roomId);
      //  alert('Room deleted successfully.');
      },
      error: (err) => {
        console.error(`Error deleting room with ID ${roomId}:`, err);
      //  alert('Failed to delete room. Please try again later.');
      },
    });
  }
}
