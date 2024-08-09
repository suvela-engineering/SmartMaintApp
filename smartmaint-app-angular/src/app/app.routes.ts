import { Routes } from '@angular/router';
import { HomeComponent } from '../components/home/home.component';
import { AboutComponent } from '../components/about/about.component';
import { UsersComponent } from '../components/users/users.component';
import { ChatComponent } from '../components/chat-component/chat-component.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'about', component: AboutComponent },
  { path: 'user', component: UsersComponent },
  { path: 'chat', component: ChatComponent }
];
