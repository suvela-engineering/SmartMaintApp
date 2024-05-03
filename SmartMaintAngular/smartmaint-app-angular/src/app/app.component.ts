import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { HomeComponent } from '../components/home/home.component';
import { AppNavigationComponent } from '../layout/app-navigation/app-navigation.component';
import { AppFooterComponent } from '../layout/app-footer/app-footer.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive,
    HomeComponent,AppNavigationComponent,AppFooterComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'smartmaint-app-angular';
}
