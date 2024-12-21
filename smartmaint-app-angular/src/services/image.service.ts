import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ImageService {
  private baseUrl = 'http://localhost:5264/api/Image';

  constructor(private http: HttpClient) { }

  getImages(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}`);
  }

  uploadImage(file: File): Observable<any> {
    const formData: FormData = new FormData();
    formData.append('file', file, file.name);

    return this.http.post<any>(`${this.baseUrl}/upload`, formData);
  }

  downloadImage(id: string): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/download/${id}`, { responseType: 'blob' });
  }

  downloadImageScaled(imageId: string, width: number, height: number): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/download/${imageId}?width=${width}&height=${height}`, { responseType: 'blob' });
  }

  downloadImageResized(imageId: string, width: number, height: number): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/download/resized/${imageId}?width=${width}&height=${height}`, { responseType: 'blob' });
  }

  searchFiles(type?: string, fileName?: string, startDate?: string, endDate?: string): Observable<any[]> {
    let params = new HttpParams();

    if (type) {
      params = params.set('type', type);
    }

    if (fileName) {
      params = params.set('fileName', fileName);
    }

    if (startDate) {
      params = params.set('startDate', startDate);
    }

    if (endDate) {
      params = params.set('endDate', endDate);
    }

    return this.http.get<any[]>(`${this.baseUrl}/search`, { params });
  }

  // getFullImage(id: string): Observable<any> {
  //   return this.http.get<any>(`${this.baseUrl}/full/${id}`);
  // }

  getImageUrl(imageId: string): Observable<string> {
    return this.http.get(`${this.baseUrl}/get-image-url/${imageId}`, { responseType: 'text' });
  }
}
