import {EventEmitter, Injectable, Output} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {IGenre} from "../models/genre.model";

@Injectable({
  providedIn: 'root'
})
export class GenreService {
  @Output() private selectedGenres: IGenre[] = [];
  selectedGenresChanged = new EventEmitter<IGenre[]>();

  constructor(private http: HttpClient) { }

  getGenres(): Observable<IGenre[]>{
    return this.http.get<IGenre[]>("https://localhost:7043/api/genre");
  }

  addNewGenreToFilter(genre: IGenre){
    let indexOf: number = this.selectedGenres.indexOf(genre);
    if(indexOf > -1){
      this.selectedGenres.splice(indexOf, 1);
    }
    else {
      this.selectedGenres.push(genre);
    }
    this.selectedGenresChanged.emit(this.selectedGenres.slice());
  }

  getSelectedGenres(): IGenre[]{
    return this.selectedGenres.slice();
  }

  getGenresByGameId(guid: string): Observable<IGenre[]> {
    return this.http.get<IGenre[]>(`https://localhost:7043/api/genre?gameId=${guid}`);
  }
}
