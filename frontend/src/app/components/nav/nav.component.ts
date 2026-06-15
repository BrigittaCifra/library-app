import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';

import { AuthStore } from '../../stores/auth.store';

@Component({
  selector: 'app-nav',
  imports: [RouterLink],
  templateUrl: './nav.component.html',
  styles: [],
})
export class NavbarComponent {
  authStore = inject(AuthStore);
}
