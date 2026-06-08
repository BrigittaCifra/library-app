import { Component, Input, signal } from '@angular/core';

import { BookModel } from '../../models/book.model'

@Component({
  selector: 'app-book-card',
  imports: [],
  templateUrl: `./book-card.component.html`,
  styles: ``,
})
export class BookCardComponent {
  //@Input() är en decorator som funkar som props i react. Det låter föräldern skicka data till barn komponent
  //"Titel" är namnet på propertyn som sedan används som html attribut i föräldern
  //här sätts typen till bookmodel för att få extra stöd från vs code
  @Input() book: BookModel = { id: 0, title: '', author: '', publishedDate: '', createdAt: '', userId: 0 };
}
