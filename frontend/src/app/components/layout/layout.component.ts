import { Component } from '@angular/core';

//Komponenter
import { NavbarComponent } from '../nav/nav.component';

@Component({
  selector: 'app-layout',
  imports: [NavbarComponent],
  templateUrl: `./layout.component.html`,
  styles: ``,
})
export class LayoutComponent { }
