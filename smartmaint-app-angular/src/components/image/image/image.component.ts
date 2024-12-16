import { CommonModule } from '@angular/common';
import { ImageService } from './../../../services/image.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-image',
  templateUrl: './image.component.html',
  standalone: true,
  imports: [CommonModule],
  styleUrls: ['./image.component.css']
})
export class ImageComponent implements OnInit {
  images: any[] = [];
  searchResults: any[] = [];
  selectedFile: File | null = null; // Add this line
  selectedImage: any = null;

  constructor(private ImageService: ImageService) { }

  ngOnInit(): void {
    this.loadImages();
  }

  loadImages(): void {
    this.ImageService.getImages().subscribe(images => {
      this.images = images;
    });
  }

  searchFiles(type: string, fileName: string, startDate: string, endDate: string): void {
    this.ImageService.searchFiles(type, fileName, startDate, endDate).subscribe(results => {
      this.searchResults = results;
    });
  }

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  onUpload(): void {
    if (this.selectedFile) {
      this.ImageService.uploadImage(this.selectedFile).subscribe(() => {
        this.loadImages();
      });
    }
  }

  openImage(image: any): void {
    this.selectedImage = image;
    // Logic to open image in a modal or separate view
  }

  downloadImage(imageId: string): void {
    this.ImageService.downloadImage(imageId).subscribe(blob => {
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `${imageId}.jpg`;
      a.click();
      window.URL.revokeObjectURL(url);
    });
}

}
