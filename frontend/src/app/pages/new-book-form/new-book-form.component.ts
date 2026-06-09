import { Component, inject } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { BookModel } from '../../models/book.model';
import { BookStore } from '../../stores/book.store';

@Component({
  selector: 'app-book-form',
  imports: [ReactiveFormsModule],
  templateUrl: './new-book-form.component.html',
  styles: [],
})
export class BookForm {

  private readonly bookStore = inject(BookStore);
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);

  bookForm = this.fb.group({
    id: [0],
    title: [''],
    author: [''],
    publishedDate: ['1937-09-21T00:00:00Z'],
    userId: [1]
  })

  onReset() {
    this.bookForm.reset();
  }

  onSubmit() {
    const book = this.bookForm.value as BookModel;
    this.bookStore.addBook(book);

    this.onReset();

    this.router.navigate(['/']);
  }

}
