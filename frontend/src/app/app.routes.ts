import { Routes } from '@angular/router';

//sidor
import { BookPage } from './pages/book-page/book-page';
import { QuotePage } from './pages/quote-page/quote-page';
import { AuthForm } from './pages/auth-form/auth-form';
import { BookForm } from './pages/book-form/book-form.component';
import { PageNotFound } from './pages/page-not-found/page-not-found';

export const routes: Routes = [
    { path: '', component: BookPage }, //home page
    { path: 'book/new', component: BookForm },
    { path: 'book/edit/:id', component: BookForm },
    { path: 'quotes', component: QuotePage },
    { path: 'user/login', component: AuthForm },
    { path: 'user/register', component: AuthForm },
    { path: '**', component: PageNotFound } //wildcard route
];