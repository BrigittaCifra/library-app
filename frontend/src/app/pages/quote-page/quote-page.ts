import { Component, inject } from '@angular/core';
import { QuoteStore } from '../../stores/quote.store';

//Komponenter
import { QuoteCard } from '../../components/quote-card/quote-card';

@Component({
  selector: 'app-quote-page',
  imports: [QuoteCard],
  templateUrl: `./quote-page.html`,
  styles: ``,
})
export class QuotePage {

  private readonly quoteStore = inject(QuoteStore);

  quote = this.quoteStore.quote;
  loading = this.quoteStore.loading;
  error = this.quoteStore.error;

}
