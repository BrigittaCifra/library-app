import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

//Service
import { BookService } from '../../services/book.service';

//Model
import { BookModel } from '../../models/book.model';

//Komponenter
import { BookCardComponent } from '../../components/bookCard/book-card.component';

@Component({
  selector: 'app-book-page',
  imports: [BookCardComponent, RouterLink],
  templateUrl: `./book-page.html`,
  styles: ``,
})
export class BookPage {
  //injekterar bok servicen
  private bookService = inject(BookService);

  //<BookModel[]> typen är en lista av böcker
  //([]) tom lista som startvärde
  books = signal<BookModel[]>([]);

  //ngOnInit körs när komponenten laddas
  ngOnInit() {
    //HttpClient returnerar en Observable istället för en promise därför används .subscribe()
    //.subscribe() returnerar en callback funktion
    this.bookService.getBook().subscribe(data => {
      this.books.set(data);
    });

  }

  onBookDeleted(id: number): void {
    // Filtrerar bort den raderade boken från listan
    this.books.update(books => books.filter(b => b.id !== id));
  }

}
