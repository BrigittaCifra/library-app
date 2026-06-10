import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { QuoteModel } from '../models/quote.model';

//Injectable decorator. Tillåter en att injectera servicen i komponenter
//providedIn: 'root' blir global
@Injectable({
  providedIn: 'root',
})
export class QuoteService {
  //inject är det moderna sättet att injectera på? Injecterar Angulars inbyggda HTTP klient. Utan den kan man inte skicka HTTP req
  //private innebär att egenskapen kan bara användas i BookService klassen. Komponenter som använder servicen kan inte komma åt http direkt, bara metoderna som getBooks()
  private http = inject(HttpClient);

  //http://localhost:5222/quote
  private url = environment.apiUrl + '/quote';

  //Hämtar alla citat
  getQuote() {
    //<Quote[]> talar om för TypeScript vilken typ svaret ska vara. [] returnerar en lista
    return this.http.get<QuoteModel[]>(this.url);
  }

  //lägger till en bok
  addQuote(quote: QuoteModel) {
    return this.http.post<QuoteModel>(this.url, quote);
  }

  //tar bort ett citat
  deleteQuote(id: number) {
    return this.http.delete<void>(`${this.url}/${id}`);
  }

}