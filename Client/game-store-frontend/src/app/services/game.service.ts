import {EventEmitter, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {IGame} from "../models/game.model";
import {GenreService} from "./genre.service";
import {IGenre} from "../models/genre.model";

@Injectable({
  providedIn: 'root'
})
export class GameService {
  selectedNamesChanged = new EventEmitter<string>();
  successfulUpdates: boolean[] = []
  updatedGames = new EventEmitter<boolean[]>();

  constructor(private http: HttpClient, private genreService: GenreService) {
  }

  getGames(gameName?: string): Observable<IGame[]> {
    let checkedGenres: IGenre[] = this.genreService.getSelectedGenres();
    if (checkedGenres.length > 0) {
      return this.http.get<IGame[]>(
        `https://localhost:7043/api/game?genre=${checkedGenres[0].title}`);
    }
    if (gameName) {
      return this.http.get<IGame[]>(
        `https://localhost:7043/api/game?name=${gameName}`);
    }
    if (gameName && checkedGenres.length > 0) {
      return this.http.get<IGame[]>(
        `https://localhost:7043/api/game?genre=${checkedGenres[0].title}&name=${gameName}`);
    }
    else {
      return this.http.get<IGame[]>(
        `https://localhost:7043/api/game`);
    }
  }

  nameFilteringChanged(gameName: string) {
    this.selectedNamesChanged.emit(gameName);
  }

  getGameById(id: string): Observable<IGame> {
    return this.http.get<IGame>(`https://localhost:7043/api/game/${id}`)
  }

  deleteGame(gameModel: IGame): Observable<string> {
    let httpResponse = this.http.delete<string>(`https://localhost:7043/api/game`, {
      body: gameModel
    });
    if(!httpResponse.subscribe({error: err => err.message()})) {
      this.successfulUpdates.push(true);
    }
    this.updatedGames.emit(this.successfulUpdates);
    return httpResponse;
  }
}
