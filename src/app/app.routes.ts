import { Routes } from '@angular/router';

export const routes: Routes = [
    { path: '', loadComponent: () => import('./component/home/home.component').then((x) => x.HomeComponent) },

    { path: "book", loadComponent: () => import('./component/book/book.component').then((x) => x.BookComponent) },
    // { path: "confirmBook", loadComponent: () => import('./component/confirmBook/confirmBook.component').then((x) => x.ConfirmBookComponent) },

   

];
