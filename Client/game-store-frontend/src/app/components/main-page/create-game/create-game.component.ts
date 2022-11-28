import { Component, OnInit } from '@angular/core';
import {IGame} from "../../../models/game.model";
import {IGenre} from "../../../models/genre.model";
import {GenreService} from "../../../services/genre.service";
import {GameService} from "../../../services/game.service";
import {NgForm} from "@angular/forms";
import {Router} from "@angular/router";

@Component({
  selector: 'app-create-game',
  templateUrl: './create-game.component.html',
  styleUrls: ['./create-game.component.css']
})
export class CreateGameComponent implements OnInit {
  allGenres: IGenre[] = [];
  genresToAdd: IGenre[] = [];
  gameToAdd: IGame = {
    id: "00000000-0000-0000-0000-000000000000",
    title: "",
    description: "",
    price: 0,
    genreIds: [],
    imageIds: []
  }

  constructor(private genreService: GenreService, private gameService: GameService, private router: Router) { }

  ngOnInit(): void {
    this.genreService.getGenres()
      .subscribe(genres => this.allGenres = genres)
  }

  onSubmit(form: NgForm) {
    if (form.valid){
      this.gameService.createGame(this.gameToAdd)
        .subscribe(() => {
          this.router.navigate(["../"])
        });
    }
    else {
      console.log("Invalid input");
    }
  }

  addGenreToGameGenres(genre: IGenre) {
    this.genresToAdd.push(genre);
    this.gameToAdd.genreIds.push(genre.id);
    this.allGenres.splice(this.allGenres.indexOf(genre), 1);
  }

  removeGenreFromGame(genre: IGenre) {
    this.allGenres.push(genre);
    this.gameToAdd.genreIds.splice(this.gameToAdd.genreIds.indexOf(genre.id), 1);
    this.genresToAdd.splice(this.genresToAdd.indexOf(genre), 1);
  }
}
