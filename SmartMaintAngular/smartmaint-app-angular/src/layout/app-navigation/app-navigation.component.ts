import { RouterLink, RouterLinkActive } from '@angular/router';
import { Link } from './../../../../../SmartMaintSvelte/smartmaintwebapp/src/definitions/interfaces';
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-navigation',
  standalone: true,
  imports: [RouterLink, RouterLinkActive,CommonModule],
  templateUrl: './app-navigation.component.html',
  styleUrl: './app-navigation.component.css'
})
export class AppNavigationComponent {
  links: Link[] = [
    { text: 'Home', path: '/' },
    { text: 'About', path: '/about' }
  ];
}
