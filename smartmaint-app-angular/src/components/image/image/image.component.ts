import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { ImageService } from './../../../services/image.service';
import { Component, OnInit } from '@angular/core';
import { DialogModule } from 'primeng/dialog';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CustomtableComponent } from '../../customtable/customtable.component';
import { LoadingComponent } from '../../../helper/helperComponents/loading/loading.component';

@Component({
  selector: 'app-image',
  templateUrl: './image.component.html',
  standalone: true,
  imports: [CommonModule, DialogModule, CustomtableComponent, LoadingComponent, HttpClientModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  styleUrls: ['./image.component.css']
})
export class ImageComponent implements OnInit {
  images: any[] = [];
  selectedFile: File | null = null;
  selectedFileName: string | null = null; // Lisää muuttuja
  isLoading: boolean = true;
  cols: any[] = [];
  displayImageDialog: boolean = false;
  selectedImageUrl: string = '';
  zoomLevel: string = 'scale(1)';

  constructor(private imageService: ImageService) { }

  ngOnInit(): void {
    this.loadImages();
    this.defineCols();
  }

  defineCols(): void {
    this.cols = [
      { field: 'thumbnailUrl', header: 'Thumbnail' },
      { field: 'name', header: 'Name' },
      { field: 'description', header: 'Description' },
      { field: 'formattedDate', header: 'Date' },
      { field: '', header: 'Actions', action: true }
    ];
  }

  loadImages(): void {
    this.isLoading = true;
    this.imageService.getImages().subscribe({
      next: images => {
        this.images = images;
        this.isLoading = false;
      },
      error: error => {
        console.error('Error retrieving images:', error);
        this.isLoading = false;
      }
    });
  }

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
    if (this.selectedFile) {
      this.selectedFileName = this.selectedFile.name;
    }
  }

  onUpload(): void {
    if (this.selectedFile) {
      this.imageService.uploadImage(this.selectedFile).subscribe(() => {
        this.loadImages();
      });
    }
  }

  handleAction(event: any): void {
    const action = event.action;
    const imageId = event.data.id;
    if (action === 'download') {
      this.download(imageId);
    } else if (action === 'view') {
      this.openView(imageId);
    }
  }

  download(imageId: string): void {
    this.imageService.downloadImage(imageId).subscribe(blob => {
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `${imageId}.jpg`;
      a.click();
      window.URL.revokeObjectURL(url);
    });
  }

  openView(imageId: string): void {
    this.imageService.downloadImage(imageId).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        this.selectedImageUrl = url;
        this.displayImageDialog = true;
      },
      error: (error) => {
        console.error('Error fetching image URL:', error);
      }
    });
  }

  closeImageDialog(): void {
    this.displayImageDialog = false;
    this.selectedImageUrl = '';
  }
  // Zoomaus-toiminnallisuus
  zoomIn(): void {
    this.adjustZoom(0.1);
  }

  zoomOut(): void {
    this.adjustZoom(-0.1);
  }

  adjustZoom(factor: number): void {
    const currentScale = parseFloat(this.zoomLevel.replace('scale(', '').replace(')', ''));
    const newScale = Math.max(0.1, currentScale + factor);
    this.zoomLevel = `scale(${newScale})`;
  }
}
