import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterModule, RouterOutlet } from '@angular/router';
import { HomeComponent } from '../components/home/home.component';
import { AppNavigationComponent } from '../layout/app-navigation/app-navigation.component';
import { AppFooterComponent } from '../layout/app-footer/app-footer.component';
import { HttpClientModule } from '@angular/common/http';
import { TableModule } from 'primeng/table';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterModule,
    HttpClientModule,
    HomeComponent,
    AppNavigationComponent,
    AppFooterComponent,
    TableModule,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'smartmaint-app-angular';
}
