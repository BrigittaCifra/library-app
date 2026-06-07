import { Component, signal } from '@angular/core';

//layout komponent. Innehåller <router-outlet />
import { LayoutComponent } from './components/layout/layout.component';

@Component({
  selector: 'app-root',
  imports: [LayoutComponent],
  template: `
    <app-layout/>
  `,
  styles: [],
})
export class App {
  protected readonly title = signal('frontend');
}