import { Component } from '@angular/core';

//Komponenter
import { BookCardComponent } from '../../components/bookCard/book-card.component';

@Component({
  selector: 'app-book-page',
  imports: [BookCardComponent],
  templateUrl: `./book-page.html`,
  styles: ``,
})
export class BookPage { }
