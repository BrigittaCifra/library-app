import { Component, inject, signal } from '@angular/core';
import { QuoteStore } from '../../stores/quote.store';

import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { QuoteModel } from '../../models/quote.model';

//Komponenter
import { QuoteCard } from '../../components/quote-card/quote-card';

@Component({
  selector: 'app-quote-page',
  imports: [QuoteCard, ReactiveFormsModule],
  templateUrl: `./quote-page.html`,
  styles: ``,
})
export class QuotePage {

  private readonly quoteStore = inject(QuoteStore);
  private readonly fb = inject(FormBuilder);

  quote = this.quoteStore.quote;
  loading = this.quoteStore.loading;
  error = this.quoteStore.error;

  quoteForm = this.fb.group({
    id: [0],
    content: [''],
    userId: [1]
  })

  showForm = signal(false);
  newQuote = signal(true);

  //Resetar formuläret
  onReset() {
    this.quoteForm.reset({
      id: 0,
      content: '',
      userId: 1
    });
  }

  cleanUp() {
    this.newQuote.set(true);
    this.onReset();
    this.showForm.set(false);
  }

  onEdit(quote: QuoteModel) {
    this.quoteForm.patchValue(quote);
    this.showForm.set(true);
    this.newQuote.set(false);
  }

  //när man skickar in formuläret
  onSubmit() {
    const quote = this.quoteForm.value as QuoteModel;

    if (this.newQuote() === true) {
      this.quoteStore.addQuote(quote);
    } else {
      this.quoteStore.updateQuote(quote);
    }

    //cleanup
    this.cleanUp()
  }

}
