import { RouterLink, RouterLinkActive } from '@angular/router';
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Link } from '../../models/layout/Link.model';

@Component({
  selector: 'app-navigation',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './app-navigation.component.html',
  styleUrl: './app-navigation.component.css'
})
export class AppNavigationComponent {
  links: Link[] = [
    { text: 'Home', path: '/' },
    { text: 'About', path: '/about' },
    { text: 'User', path: '/user' },
    { text: 'Chat', path: '/chat' },
  ];
}
