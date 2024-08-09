import { CommonModule } from '@angular/common';
import { ChatResponse } from './../../../../SmartMaintSvelte/smartmaintwebapp/src/definitions/interfaces';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-chat-component',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './chat-component.component.html',
  styleUrl: './chat-component.component.css'
})
export class ChatComponent {
  message = '';
  responses: ChatResponse[] = this.generateTestData(); // Generate test data on init
  userInput: string = '';

  constructor(private http: HttpClient) { }

  sendMessage() {
    // Simulate server response (replace with actual API call)
    const response = { message: `You said: ${this.message}`, sender: 'system', timestamp: new Date() };
    this.responses.push(response);
    this.message = '';
  }

  generateTestData(): ChatResponse[] {
    return [
      { message: 'Welcome to the chat!', sender: 'system', timestamp: new Date(Date.now() - 60000) },
      { message: 'How can I help you today?', sender: 'system', timestamp: new Date() },
    ];
  }
}
