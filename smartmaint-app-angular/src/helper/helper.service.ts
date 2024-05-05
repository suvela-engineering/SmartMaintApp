import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root', // Or a specific module if needed
})

export class HelperService {

  // Function to convert class property names to displayable headers
  getPropertyDisplayName = (propertyName: string): string => {
    return (
      propertyName.charAt(0).toUpperCase() +
      propertyName.slice(1).replace(/([A-Z])/g, ' $1') // Handle camelCase and uppercase words
    );
  };


}
