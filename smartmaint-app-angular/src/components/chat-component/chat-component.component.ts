import { ChatResponse } from './../../../../SmartMaintSvelte/smartmaintwebapp/src/definitions/interfaces';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-chat-component',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './chat-component.component.html',
  styleUrl: './chat-component.component.css'
})
export class ChatComponent {
  message = '';
  responses: ChatResponse[] = [];
  userInput: string = '';

  constructor(private http: HttpClient) { }

  sendMessage() {
    this.http.post('/api/chat', { message: this.message }) // TO DO: pitää laittaa palauttamaan ChatResponse olioo
      .subscribe(response => {
        if (typeof response === 'object' && 'message' in response && typeof response.message === 'string') {
          this.responses.push(response as ChatResponse); // Tyypin vahvistus vain, jos kaikki ehdot täyttyvät
        } else {
          console.error('Virheellinen vastaus palvelimelta:', response);
        }
        this.message = '';
      });
  }
}
