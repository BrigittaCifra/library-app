import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

//Komponenter
import { BookCardComponent } from '../../components/bookCard/book-card.component';
import { BookStore } from '../../stores/book.store';

@Component({
  selector: 'app-book-page',
  imports: [BookCardComponent, RouterLink],
  providers: [BookStore],
  templateUrl: `./book-page.html`,
  styles: ``,
})

export class BookPage {
  //injekterar bok servicen
  //private bookService = inject(BookService);
  private readonly bookStore = inject(BookStore);

  book = this.bookStore.book;
  loading = this.bookStore.loading;
  error = this.bookStore.error;

}
