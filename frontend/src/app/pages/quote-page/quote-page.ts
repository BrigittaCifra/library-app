import { Component } from '@angular/core';

//Komponenter
import { QuoteCard } from '../../components/quote-card/quote-card';

@Component({
  selector: 'app-quote-page',
  imports: [QuoteCard],
  templateUrl: `./quote-page.html`,
  styles: ``,
})
export class QuotePage { }
