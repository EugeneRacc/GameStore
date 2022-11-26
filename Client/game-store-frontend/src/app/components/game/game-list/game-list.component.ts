import { Component, OnInit } from '@angular/core';
import {GameService} from "../../../services/game.service";
import {IGame} from "../../../models/game.model";
import {GenreService} from "../../../services/genre.service";
import {IGenre} from "../../../models/genre.model";

@Component({
  selector: 'app-game-list',
  templateUrl: './game-list.component.html',
  styleUrls: ['./game-list.component.css']
})
export class GameListComponent implements OnInit {
  games: IGame[] = [];
  genres: IGenre[] = [];
  nameFiltering: string;
  constructor(private gameService: GameService, private genreService: GenreService) { }

  ngOnInit(): void {
    this.genreService.selectedGenresChanged.subscribe(
      (genres: IGenre[]) => {
        this.genres = genres
        this.gameService.getGames().subscribe( (games: IGame[]) =>
          this.games = games
        )
      }
    );
    this.gameService.selectedNamesChanged.subscribe(
      name => {
        this.nameFiltering = name
        this.gameService.getGames(this.nameFiltering).subscribe(
          (games) => {
            this.games = games
          });
        console.log("Name filtering is " + this.nameFiltering)
      }
    );
    this.gameService.getGames().subscribe(
      (games) => {
        this.games = games
        this.games.push(...games)
        this.games.push(...games)
      });
  }

  onCheckImageWithBiggerWidth(index: number):boolean
  {
    let amountOfCircles = 0;
    if (index <= 8 && index % 4 === 0){
      return true;
    }
    if (index <= 8 && index % 4 !== 0) {
      return false;
    }
    while (index > amountOfCircles && amountOfCircles + 9 <= index) {
      amountOfCircles = amountOfCircles + 9;
    }
    return (index - amountOfCircles) % 4 === 0;
  }

}
