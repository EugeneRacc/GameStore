import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {IGenre} from "../models/genre.model";

@Injectable({
  providedIn: 'root'
})
export class GenreService {

  constructor(private http: HttpClient) { }

  getGenres(): Observable<IGenre[]>{
    return this.http.get<IGenre[]>("https://localhost:7043/api/genre");
  }
}
