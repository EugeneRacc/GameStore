import {Component, Input, OnInit} from '@angular/core';
import {IGame} from "../../../../models/game.model";
import {ImageService} from "../../../../services/image.service";
import {IImage} from "../../../../models/image.model";
import {GenreService} from "../../../../services/genre.service";
import {IGenre} from "../../../../models/genre.model";
import {DomSanitizer, SafeUrl} from "@angular/platform-browser";

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent implements OnInit {
  @Input() index: number;
  @Input() game: IGame;
  @Input() largeImage: boolean = false;
  gameImages: IImage[];
  gameGenres: IGenre[];
  objectURL: string;
  urlS: SafeUrl;

  constructor(private gameService: ImageService, private genreService: GenreService, private sanitizer: DomSanitizer) {}

  ngOnInit(): void {
    this.gameService.getImagesByGameId(this.game.id)
      .subscribe((images) => {
        this.gameImages = images;
        this.objectURL = 'data:image/jpeg;base64,' + images[0].image;
        this.urlS = this.sanitizer.bypassSecurityTrustUrl(this.objectURL);
        console.log(this.urlS);
      });
    this.genreService.getGenresByGameId(this.game.id)
        .subscribe((genres) => this.gameGenres = genres);
  }
}

