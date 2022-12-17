import { Component, OnInit } from '@angular/core';
import {GameService} from "../../services/game.service";
import {IGame} from "../../models/game.model";
import {ActivatedRoute} from "@angular/router";
import {ImageService} from "../../services/image.service";
import {IImage} from "../../models/image.model";
import {DomSanitizer, SafeUrl} from "@angular/platform-browser";
import {IGenre} from "../../models/genre.model";
import {GenreService} from "../../services/genre.service";
import {AuthenticationService} from "../../services/authentication.service";

@Component({
  selector: 'app-game-details',
  templateUrl: './game-details.component.html',
  styleUrls: ['./game-details.component.css']
})
export class GameDetailsComponent implements OnInit {
  game: IGame = {
    id: "",
    title: "",
    description: "",
    price: 0,
    genreIds: [],
    imageIds: []
  };
  gameGenres: IGenre[];
  gameImages: IImage[];
  urlS: SafeUrl;
  createComment = false;
  isAuth = false;
  constructor(private gameService: GameService, private imageService: ImageService, private router: ActivatedRoute,
              private sanitizer: DomSanitizer, private genreService: GenreService,
              private authService: AuthenticationService) {
  }

  ngOnInit(): void {
    this.gameService.getGameById(this.router.snapshot.params['id'])
      .subscribe(
        game => {
          this.game = game
          this.imageService.getImagesByGameId(this.game.id)
            .subscribe((images) => {
              this.gameImages = images;
              this.urlS = this.imageService.sanitizeImage(images)
            });
          this.genreService.getGenresByGameId(this.game.id)
            .subscribe(
              genres => {
                this.gameGenres = genres
              }
            )
        }
      );

    this.authService.isAuth
      .subscribe(userAuth => this.isAuth = userAuth);
  }

  onShowCreate(show: boolean) {
    if (show) {
      this.createComment = false;
    }
  }
}
