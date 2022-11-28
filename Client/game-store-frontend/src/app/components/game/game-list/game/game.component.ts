import {Component, HostListener, Input, OnInit} from '@angular/core';
import {IGame} from "../../../../models/game.model";
import {ImageService} from "../../../../services/image.service";
import {IImage} from "../../../../models/image.model";
import {GenreService} from "../../../../services/genre.service";
import {IGenre} from "../../../../models/genre.model";
import {DomSanitizer} from "@angular/platform-browser";
import {Router} from "@angular/router";

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
  onShowEditPage = false;

  @HostListener('mouseenter', ['$event']) onEnter() {
    this.onShowEditPage = true;
  }
  @HostListener('mouseleave', ['$event']) onLeave() {
    this.onShowEditPage = false;
  }
  @HostListener('click', ['$event']) onClick() {

  }

  constructor(private imageService: ImageService, private genreService: GenreService, private sanitizer: DomSanitizer,
              private router: Router) {}

  ngOnInit(): void {
    this.imageService.getImagesByGameId(this.game.id)
      .subscribe((images) => {
        this.gameImages = images;
        this.objectURL = 'data:image/jpeg;base64,' + images[0].image;
      });
    this.genreService.getGenresByGameId(this.game.id)
        .subscribe((genres) => this.gameGenres = genres);
  }

  onOpenDetails() {
    this.router.navigate([`${this.game.id}`]);
  }
}

