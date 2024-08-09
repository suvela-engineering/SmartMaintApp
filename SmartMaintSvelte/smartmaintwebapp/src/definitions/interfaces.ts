export interface Link {
    text: string;
    path: string;
  }

  export interface ChatResponse {
    message: string;
    sender: string; // 'user' or 'system'
    timestamp: Date; // Optional
    attachments?: Attachment[]; // Optional array of attachments
  }
  
  interface Attachment {
    name: string;
    // Add other properties for attachment details (e.g., url, type)
  }