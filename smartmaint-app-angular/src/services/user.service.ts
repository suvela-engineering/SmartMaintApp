import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../models/User.model';
import { Observable, catchError, of, retry, throwError } from 'rxjs';

const apiUrl = 'http://localhost:5277/API/User'; // TO DO: This to env etc file

@Injectable({
  providedIn: 'root',
})

export class UserService {
  constructor(private http: HttpClient) {}

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(apiUrl)
    // .pipe(
    //   retry(2), // Retry up to 2 times on network failures (optional)
    //   catchError(this.handleError)
    // );
  }

  private handleError(error: any) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // Client-side or network error occurred. Handle it accordingly.
      errorMessage = 'An error occurred: ' + error.error.message;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong.
      errorMessage =
        `Backend returned code ${error.status}, ` + `body was: ${error.error}`;
    }
    // Return an observable with a user-facing error message
    return of<User[]>([]); // Return an empty array in case of error
  }
}
