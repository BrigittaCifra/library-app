import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

//Komponenter
import { NavbarComponent } from '../nav/nav.component';

@Component({
  selector: 'app-layout',
  imports: [RouterOutlet, NavbarComponent],
  templateUrl: `./layout.component.html`,
  styles: ``,
})
export class LayoutComponent { }