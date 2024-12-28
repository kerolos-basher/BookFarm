import { Routes } from '@angular/router';

export const routes: Routes = [
    { path: '', loadComponent: () => import('./component/home/home.component').then((x) => x.HomeComponent) },

    { path: "book", loadComponent: () => import('./component/book/book.component').then((x) => x.BookComponent) },
    { path: "ConfirmBook", loadComponent: () => import('./component/ComfirmBook/ConfirmBook.component').then((x) => x.ConfirmBookComponent) },

   

];
