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

  //Resetar formuläret
  onReset() {
    this.quoteForm.reset({
      id: 0,
      content: '',
      userId: 1
    });
  }

  onSubmit() {
    const quote = this.quoteForm.value as QuoteModel;
    this.quoteStore.addQuote(quote);

    //cleanup
    this.onReset();
  }

}
