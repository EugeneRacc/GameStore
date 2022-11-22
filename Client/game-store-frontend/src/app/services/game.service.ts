import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {IGame} from "../models/game.model";
import {GenreService} from "./genre.service";
import {IGenre} from "../models/genre.model";

@Injectable({
  providedIn: 'root'
})
export class GameService {

  constructor(private http: HttpClient, private genreService: GenreService) {
  }

  getGames(): Observable<IGame[]> {
    let checkedGenres: IGenre[] = this.genreService.getSelectedGenres();
    if (checkedGenres.length > 0) {
      return this.http.get<IGame[]>(
        `https://localhost:7043/api/game?genre=${checkedGenres[0].title}`);
    } else {
      return this.http.get<IGame[]>(
        `https://localhost:7043/api/game`);
    }
  }
}
