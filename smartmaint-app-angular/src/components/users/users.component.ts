import { HelperService } from './../../helper/helper.service';
import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { User } from '../../models/User.model';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, TableModule],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css',
})
export class UsersComponent implements OnInit {
  users: User[] = [];
  errorMessage: string = '';
  displayedHeaders: string[] = [];
  excludedProperties = ['Id', 'EntityInfo_Deleted', 'EntityInfo_DeletedBy'];
  cols: any[] = [];

  constructor(
    private userService: UserService,
    private helperService: HelperService
  ) {}

  ngOnInit() {
    this.getUsers();
    this.getTableHeaders();
  }

  private getUsers(): void {
    this.userService.getUsers().subscribe({
      next: (data: User[]) => {
        this.users = data;
        //this.displayedHeaders = this.getTableHeaders();
      },
      error: (error) => {
        this.errorMessage = error;
        console.error('Error retrieving users:', error);
      },
      complete: () => {
        // TO DO: Remove if not implemented really
        console.log('User data retrieval complete'); // Optional for debugging
      },
    });
  }

  private getTableHeaders = (): void => {
    // return Object.keys(this.users[0])
    //   .filter((prop) => !this.excludedProperties.includes(prop))
    //   .map((prop) => this.helperService.getPropertyDisplayName(prop));

    this.cols = [
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
      {
        field: 'userName',
        header: 'User Name',
      },
    ];
  };
}
