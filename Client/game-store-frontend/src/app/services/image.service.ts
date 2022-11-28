import { Injectable } from '@angular/core';
import {IImage} from "../models/image.model";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {DomSanitizer, SafeUrl} from "@angular/platform-browser";

@Injectable({
  providedIn: 'root'
})
export class ImageService {

  constructor(private http: HttpClient, private sanitizer: DomSanitizer) { }

  getImagesByGameId(guid: string): Observable<IImage[]> {
    return this.http.get<IImage[]>(
      `https://localhost:7043/api/image?id=${guid}`);
  }

  sanitizeImage(images: IImage[]):SafeUrl {
    let objectURL = 'data:image/jpeg;base64,' + images[0].image;
    return this.sanitizer.bypassSecurityTrustUrl(objectURL);
  }
}
