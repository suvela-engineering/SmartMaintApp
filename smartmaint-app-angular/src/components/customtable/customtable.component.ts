import { CommonModule } from '@angular/common';
import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { TableColumn, TableData } from '../../models/components/customtable/customtable.model';

@Component({
  selector: 'app-customtable',
  standalone: true,
  imports: [CommonModule],
  styleUrls: ['./customtable.component.css'],
  template: `
  <table>
    <thead>
      <tr class="table-header">
        <th *ngFor="let col of columns">{{ col.header }}</th>
      </tr>
    </thead>
    <tbody>
      <tr *ngFor="let row of data" (click)="openView(row)">
        <td *ngFor="let col of columns">
          <ng-container *ngIf="col.field !== ''; else actions">
            <ng-container *ngIf="col.field === 'thumbnailUrl'; else defaultField">
              <img [src]="row[col.field]" alt="Thumbnail" [style.width.px]="thumbnailWidth" [style.height.px]="thumbnailHeight">
            </ng-container>
            <ng-template #defaultField>{{ row[col.field] }}</ng-template>
          </ng-container>
          <ng-template #actions>
            <button class="download-button button-active" (click)="download(row['id']); $event.stopPropagation();">
              Download
            </button>
            <button class="delete-button button-active" (click)="delete(row['id']); $event.stopPropagation();">
              Delete
            </button>
          </ng-template>
        </td>
      </tr>
    </tbody>
  </table>
  `,
})
export class CustomtableComponent implements OnInit {
  @Input() columns: TableColumn[] = [];
  @Input() data: TableData[] = [];
  @Input() thumbnailWidth: number = 100; // Oletusarvoinen leveys
  @Input() thumbnailHeight: number = 100; // Oletusarvoinen korkeus
  @Output() action = new EventEmitter<any>();

  constructor() { }

  ngOnInit() {
    if (this.data.length > 0) {
      this.validateInputs();
    }
  }

  validateInputs() {
    if (!this.columns || this.columns.length === 0) {
      throw new Error('CustomtableComponent: Columns input is required and cannot be empty.');
    }
  }

  openView(item: any) {
    this.action.emit({ action: 'view', data: item });
  }

  download(id: string) {
    this.action.emit({ action: 'download', data: { id } });
  }

  delete(id: string) {
    this.action.emit({ action: 'delete', data: { id } });
  }
}
