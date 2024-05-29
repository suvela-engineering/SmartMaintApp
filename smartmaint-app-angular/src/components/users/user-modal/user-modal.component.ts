import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  WritableSignal,
  input,
  model,
} from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { AvatarModule } from 'primeng/avatar';
import { User } from '../../../models/User.model';
import { DialogModule } from 'primeng/dialog';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../../../services/user.service';
import { catchError, throwError } from 'rxjs';

@Component({
  selector: 'user-modal',
  standalone: true,
  imports: [ButtonModule, InputTextModule, DialogModule, AvatarModule, CommonModule, ReactiveFormsModule],
  templateUrl: './user-modal.component.html',
})
export class UserModalComponent implements OnInit {
  @Input() show: boolean = false;
  @Input() title: string = '';
  @Input() data: any = {};
  @Output() showChange = new EventEmitter<boolean>(); // optional event for closing

  editForm: FormGroup;

  constructor(private userService: UserService, private formBuilder: FormBuilder) {
    this.editForm = this.formBuilder.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
    });
  };

  ngOnInit() {
    console.log('UserModalComponent ngOnInit, show: ' + this.show);
  }

  // Function to handle form submission or actions within the dialog (optional)
  saveUser() {
    const formData = this.editForm.value as User; // Get form data as User object
    this.userService.saveUser(formData)
      .pipe(
        catchError(error => {
          console.error('Error saving user:', error);
          return throwError(() => new Error('Error saving user')); // Re-throw a user-friendly error
        })
      )
      .subscribe({
        next: savedUser => {
          this.showChange.emit(false); // Close modal
        //  this.userSaved.emit(savedUser); // Emit saved user data (optional)
        },
        error: error => {
          console.error('Error saving user:', error);
          // You can display a user-friendly error message here
        }
      });
  }

  onCancel() {
    this.showChange.emit(false);
    console.log('modal onCancel():' + ' , show: ' + this.show);
  }
}
