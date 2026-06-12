import { Routes } from '@angular/router';

//sidor
import { BookPage } from './pages/book-page/book-page';
import { QuotePage } from './pages/quote-page/quote-page';
import { LoginPage } from './pages/login-page/login-page';
import { BookForm } from './pages/book-form/book-form.component';
import { PageNotFound } from './pages/page-not-found/page-not-found';

export const routes: Routes = [
    { path: '', component: BookPage }, //home page
    { path: 'book/new', component: BookForm },
    { path: 'book/edit/:id', component: BookForm },
    { path: 'quotes', component: QuotePage },
    { path: 'user/login', component: LoginPage },
    { path: '**', component: PageNotFound } //wildcard route
];