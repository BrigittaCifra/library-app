import { Component, Input, inject } from '@angular/core';
import { Router } from '@angular/router';

import { BookModel } from '../../models/book.model';
import { BookStore } from '../../stores/book.store';

@Component({
  selector: 'app-book-card',
  imports: [],
  templateUrl: `./book-card.component.html`,
  styles: ``,
})
export class BookCardComponent {

  private readonly router = inject(Router);

  //injekterar bok storen
  bookStore = inject(BookStore);

  //@Input() är en decorator som funkar som props i react. Det låter föräldern skicka data till barn komponent
  //"Titel" är namnet på propertyn som sedan används som html attribut i föräldern
  //här sätts typen till bookmodel för att få extra stöd från vs code
  //sätter default värden på egenskaperna som saknas
  @Input() book: BookModel = { id: 0, title: '', author: '', publishedDate: '', createdAt: '', userId: 0 };

  onEdit() {
    //Skickar med bokens id till formuläret
    this.router.navigate([`book/edit/${this.book.id}`]);
  }

}
