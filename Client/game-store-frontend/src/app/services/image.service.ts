import { Injectable } from '@angular/core';
import {IImage} from "../models/image.model";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ImageService {

  constructor(private http: HttpClient) { }

  getImagesByGameId(guid: string): Observable<IImage[]> {
    return this.http.get<IImage[]>(
      `https://localhost:7043/api/image?id=${guid}`);
  }
}
