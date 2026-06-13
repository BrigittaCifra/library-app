import { Component, inject, signal } from '@angular/core';

import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

import { BookModel } from '../../models/book.model';
import { BookStore } from '../../stores/book.store';

@Component({
  selector: 'app-book-form',
  imports: [ReactiveFormsModule],
  templateUrl: './book-form.component.html',
  styles: [],
})
export class BookForm {

  private readonly bookStore = inject(BookStore);
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);
  private route = inject(ActivatedRoute);

  //Skapar ett formulär med FormBuilder
  //Varje fält får ett default värde som används när formuläret laddas
  bookForm = this.fb.group({
    id: [0],
    title: [''],
    author: [''],
    publishedDate: ['1937-09-21'],
  })

  // Access route parameters from snapshot
  id = parseInt(this.route.snapshot.params['id']);
  pageTitle = signal("Lägg till bok");

  //ngOnInit körs exakt en gång efter att en komponent har skapats. Lite som useEffect med en tom dependency array
  ngOnInit() {

    //update och create använder samma formulär. Här kontrolleras via routen om det är en update eller en create
    if (this.router.url === `/book/edit/${this.id}`) {

      this.pageTitle.set("Redigera bok");

      // Hämtar boken vars id matchar id:t i url:en
      const book = this.bookStore.book().find(b => b.id === this.id);

      //fyller i formuläret med bokens värden om boken hittades
      if (book) this.bookForm.patchValue(book);

    }

  }

  //Resetar formuläret
  onReset() {
    this.bookForm.reset();
  }

  //När man skickar in formuläret
  onSubmit() {
    // Hämtar formulärets värden och omvandlar dem till en BookModel
    const book = this.bookForm.value as BookModel;

    if (this.router.url === `/book/edit/${this.id}`) {
      //Om url:en är /edit då uppdaterar den boken med rätt id
      this.bookStore.updateBook(book);
    } else {
      //Om url:en är /new då skapar den en ny bok
      this.bookStore.addBook(book);
    }

    //cleanup
    this.onReset();
    this.router.navigate(['/']);
  }

}
