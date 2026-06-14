import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';

//sidor
import { BookPage } from './pages/book-page/book-page';
import { QuotePage } from './pages/quote-page/quote-page';
import { AuthForm } from './pages/auth-form/auth-form';
import { BookForm } from './pages/book-form/book-form.component';
import { PageNotFound } from './pages/page-not-found/page-not-found';

export const routes: Routes = [
    { path: '', component: BookPage, canActivate: [authGuard] }, //home page
    { path: 'book/new', component: BookForm, canActivate: [authGuard] },
    { path: 'book/edit/:id', component: BookForm, canActivate: [authGuard] },
    { path: 'quotes', component: QuotePage, canActivate: [authGuard] },
    { path: 'user/login', component: AuthForm },
    { path: 'user/register', component: AuthForm },
    { path: '**', component: PageNotFound, canActivate: [authGuard] } //wildcard route
];