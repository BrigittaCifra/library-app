import { Component, Input, Output, EventEmitter, inject } from '@angular/core';

import { BookModel } from '../../models/book.model';

import { BookService } from '../../services/book.service';

@Component({
  selector: 'app-book-card',
  imports: [],
  templateUrl: `./book-card.component.html`,
  styles: ``,
})
export class BookCardComponent {

  //injekterar bok servicen
  private bookService = inject(BookService);

  //@Input() är en decorator som funkar som props i react. Det låter föräldern skicka data till barn komponent
  //"Titel" är namnet på propertyn som sedan används som html attribut i föräldern
  //här sätts typen till bookmodel för att få extra stöd från vs code
  //sätter default värden på egenskaperna som saknas
  @Input() book: BookModel = { id: 0, title: '', author: '', publishedDate: '', createdAt: '', userId: 0 };

  // Skickar upp bokens id till föräldern när radera trycks
  @Output() bookDeleted = new EventEmitter<number>();

  deleteBook(id: number): void {
    this.bookService.deleteBook(id).subscribe(() => {
      this.bookDeleted.emit(id);
    });
  }

}
