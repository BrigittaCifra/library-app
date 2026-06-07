import { Routes } from '@angular/router';

//sidor
import { BookPage } from './pages/book-page/book-page';
import { PageNotFound } from './pages/page-not-found/page-not-found';

export const routes: Routes = [
    { path: '', component: BookPage }, //home page
    { path: '**', component: PageNotFound } //wildcard route
];