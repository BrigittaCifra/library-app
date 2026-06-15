import { Component, inject, Input, Output, EventEmitter } from '@angular/core';

import { QuoteModel } from '../../models/quote.model';
import { QuoteStore } from '../../stores/quote.store';

@Component({
  selector: 'app-quote-card',
  imports: [],
  templateUrl: `./quote-card.html`,
  styles: ``,
})
export class QuoteCard {

  //injekterar bok storen
  quoteStore = inject(QuoteStore);

  @Input() quote: QuoteModel = { id: 0, content: '', author: '', createdAt: '', userId: 0 };

  @Output() onEdit = new EventEmitter<QuoteModel>();

}
