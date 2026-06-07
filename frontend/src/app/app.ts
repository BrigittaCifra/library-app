import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { LayoutComponent } from './components/layout/layout.component';
import { BookCardComponent } from './components/bookCard/book-card.component'

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, LayoutComponent, BookCardComponent],
  template: `
    <app-layout/>
    <app-book-card/>
    <router-outlet />
  `,
  styles: [],
})
export class App {
  protected readonly title = signal('frontend');
}
