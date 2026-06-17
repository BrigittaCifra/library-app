import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { BookModel } from '../models/book.model';

import { AuthStore } from '../stores/auth.store';

//Injectable decorator. Tillåter en att injectera servicen i komponenter
//providedIn: 'root' blir global
@Injectable({
  providedIn: 'root',
})
export class BookService {
  //inject är det moderna sättet att injectera på? Injecterar Angulars inbyggda HTTP klient. Utan den kan man inte skicka HTTP req
  //private innebär att egenskapen kan bara användas i BookService klassen. Komponenter som använder servicen kan inte komma åt http direkt, bara metoderna som getBooks()
  private http = inject(HttpClient);
  private authStore = inject(AuthStore);

  //http://localhost:5222/book
  private url = environment.apiUrl + '/book';

  //Hämtar alla böcker
  getBook() {
    //<BookModel[]> talar om för TypeScript vilken typ svaret ska vara. [] returnerar en lista
    return this.http.get<BookModel[]>(this.url, { headers: this.authStore.getHeaders() });
  }

  //Hämtar en book baserat på id
  getBookByID(id: number) {
    return this.http.get<BookModel>(`${this.url}/${id}`, { headers: this.authStore.getHeaders() });
  }

  //lägger till en bok
  addBook(book: BookModel) {
    return this.http.post<BookModel>(this.url, book, { headers: this.authStore.getHeaders() });
  }

  //uppdaterar en bok
  updateBook(book: BookModel) {
    return this.http.put<void>(`${this.url}/${book.id}`, book, { headers: this.authStore.getHeaders() });
  }

  //tar bort en bok
  deleteBook(id: number) {
    return this.http.delete<void>(`${this.url}/${id}`, { headers: this.authStore.getHeaders() });
  }
}