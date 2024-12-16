import { User } from './../models/User.model';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, of, retry, throwError } from 'rxjs';

const apiUrl = 'http://localhost:5264/API/User'; // TO DO: This to env etc file

@Injectable({
  providedIn: 'root',
})

export class UserService {
  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(apiUrl)
      .pipe(
        retry(2), // Retry up to 2 times on network failures
        catchError(this.handleError)
      );
  }

  saveUser(user: User): Observable<User> {
    return this.http.post<User>(apiUrl, user)
      .pipe(
        catchError(this.handleError) // Built-in behavior of catchError(). When you use catchError(handleError), the catchError operator implicitly wraps your handleError function with another function.
      );
  }

  private handleError(error: any): Observable<never> {
    let errorMessage = '';
    let errorStatus = 'Unknown Error';

    if (typeof error.error === 'object') {
      // Client-side or network error occurred. Handle it accordingly.
      errorMessage = 'An error occurred: ' + error.error.message;
    } else {
      // The backend returned an unsuccessful response code.
      errorMessage = `Backend returned code ${error.status} - ${error.message}`;
      errorStatus = error.status.toString(); // Get status code as string
    }

    // Log or display the error message to the user (optional)
    console.error(errorMessage);

    // Optionally, return a user-friendly error message or an empty Observable
    // based on your error handling strategy.
    return throwError(() => new Error(errorMessage));
  }
}
