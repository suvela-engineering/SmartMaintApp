import { HelperService } from './../../helper/helper.service';
import { Component, OnInit, WritableSignal, signal } from '@angular/core';
import { UserService } from '../../services/user.service';
import { User } from '../../models/User.model';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { UserModalComponent } from './user-modal/user-modal.component';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { sign } from 'crypto';
import { CustomtableComponent } from '../customtable/customtable.component';
import { LoadingComponent } from '../../helper/helperComponents/loading/loading.component';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [
    CommonModule,
    TableModule,
    UserModalComponent,
    ButtonModule,
    DialogModule,
    CustomtableComponent,
    LoadingComponent,
  ],
  providers: [UserModalComponent],
  templateUrl: './users.component.html',
})
export class UsersComponent implements OnInit {
  users: User[] = [];
  errorMessage: string = '';
  cols: any[] = [];
  isLoading: boolean = true;

  protected showUserModal: boolean = false;


  constructor(
    private userService: UserService,
    private userModal: UserModalComponent,
    private dialog: DialogModule
  ) { }

  ngOnInit() {
    this.getUsers();
    this.getTableHeaders();
  }

  protected getUsers(): void {
    this.isLoading = true;

    this.userService.getUsers().subscribe({
      next: (data: User[]) => {
        this.users = data;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = error;
        console.error('Error retrieving users:', error);
        this.isLoading = false;
      },
      complete: () => {
        // TO DO: Remove if not implemented really
        console.log('User data retrieval complete'); // Optional for debugging
        this.isLoading = false;
      },
    });
  }

  private getTableHeaders = (): void => {
    // return Object.keys(this.users[0])
    //   .filter((prop) => !this.excludedProperties.includes(prop))
    //   .map((prop) => this.helperService.getPropertyDisplayName(prop));

    this.cols = [
      {
        field: 'userName',
        header: 'User Name',
      },
      {
        field: 'firstName',
        header: 'First Name',
      },
      {
        field: 'lastName',
        header: 'Last Name',
      },
      {
        field: 'title',
        header: 'Title',
      },
      {
        field: 'email',
        header: 'Email',
      },
      {
        field: 'phoneNumber',
        header: 'Phone Number',
      },
    ];
  };

  // toggleUserModal = () => {
  //   this.userModal.visible = !this.userModal.visible;
  // };

  openUserModal() {
    this.showUserModal = true;
    console.log('modal openUserModal():' + " , showUserModal: " + this.showUserModal); //, this.user);
  }
}
