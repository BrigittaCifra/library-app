import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { BookModel } from '../models/book.model';

//Injectable decorator. Tillåter en att injectera servicen i komponenter
//providedIn: 'root' blir global
@Injectable({
  providedIn: 'root',
})
export class BookService {
  //inject är det moderna sättet att injectera på? Injecterar Angulars inbyggda HTTP klient. Utan den kan man inte skicka HTTP req
  //private innebär att egenskapen kan bara användas i BookService klassen. Komponenter som använder servicen kan inte komma åt http direkt, bara metoderna som getBooks()
  private http = inject(HttpClient);

  //http://localhost:5222/book
  private url = environment.apiUrl + '/book';

  //Hämtar alla böcker
  getBook() {
    //<BookModel[]> talar om för TypeScript vilken typ svaret ska vara
    return this.http.get<BookModel[]>(this.url);
  }

  getBookByID(id: number) {
    return this.http.get<BookModel>(`${this.url}/{id}`);
  }
}