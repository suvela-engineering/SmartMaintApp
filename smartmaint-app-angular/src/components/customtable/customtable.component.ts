import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { TableColumn, TableData } from '../../models/components/customtable/customtable.model';

@Component({
  selector: 'app-customtable',
  standalone: true,
  imports: [CommonModule],
  styleUrl: './customtable.component.css',
  template: `
  <table>
    <thead>
      <tr class="table-header">
        <th *ngFor="let col of columns">{{ col.header }}</th>
      </tr>
    </thead>
    <tbody>
      <tr *ngFor="let row of data">
        <td *ngFor="let col of columns">{{ row[col.field] }}</td>
      </tr>
    </tbody>
  </table>
`,
})
export class CustomtableComponent implements OnInit {
  @Input() columns: TableColumn[] = [];
  @Input() data: TableData[] = [];

  constructor() { }

  ngOnInit() {
    if (this.data.length > 0) {
      this.validateInputs();
    }
  }

  validateInputs() {
    if (!this.columns || this.columns.length === 0) {
      throw new Error('CustomTableComponent: Columns input is required and cannot be empty.');
    }
  }
}
