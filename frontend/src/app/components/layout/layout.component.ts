import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { AuthStore } from '../../stores/auth.store';

//Komponenter
import { NavbarComponent } from '../nav/nav.component';

@Component({
  selector: 'app-layout',
  imports: [RouterOutlet, NavbarComponent],
  templateUrl: `./layout.component.html`,
  styles: ``,
})
export class LayoutComponent {
  authStore = inject(AuthStore);
}