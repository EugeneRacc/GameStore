import {Component, Input, OnInit} from '@angular/core';
import {IGame} from "../../../../models/game.model";
import {GameService} from "../../../../services/game.service";
import {ActivatedRoute, Router} from "@angular/router";
import {IGenre} from "../../../../models/genre.model";
import {GenreService} from "../../../../services/genre.service";
import {ImageService} from "../../../../services/image.service";

@Component({
  selector: 'app-edit-game-page',
  templateUrl: './edit-game-page.component.html',
  styleUrls: ['./edit-game-page.component.css']
})
export class EditGamePageComponent implements OnInit {
  @Input() gameToChange:IGame = {
    id: "00000000-0000-0000-0000-000000000000",
    title: "",
    description: "",
    price: 0,
    genreIds: [],
    imageIds: []
  };
  gameId: string;
  genresToAdd: IGenre[] = [];
  allGenres: IGenre[] = [];
  selectedImage: File;
  constructor(private gameService: GameService, private activatedRoute: ActivatedRoute,
              private genreService: GenreService, private router: Router, private imageService: ImageService) { }

  ngOnInit(): void {
    this.gameId = this.activatedRoute.snapshot.params['id'];
    this.gameService.getGameById(this.gameId)
      .subscribe(game => {
        this.gameToChange = game;
      });
    this.genreService.getGenres()
      .subscribe(genres => this.allGenres = genres);
  }

  addGenreToGameGenres(genre: IGenre) {
    this.genresToAdd.push(genre);
    this.gameToChange.genreIds.push(genre.id);
    this.allGenres.splice(this.allGenres.indexOf(genre), 1);
  }

  removeGenreFromGame(genre: IGenre) {
    this.allGenres.push(genre);
    this.gameToChange.genreIds.splice(this.gameToChange.genreIds.indexOf(genre.id), 1);
    this.genresToAdd.splice(this.genresToAdd.indexOf(genre), 1);
  }

  onSubmit() {
    this.gameService.updateGame(this.gameToChange)
      .subscribe(() => {
        if (this.selectedImage && this.selectedImage['type'].split('/')[0] === 'image') {
          this.imageService.uploadImage(this.selectedImage, this.gameId)
            .subscribe(() => this.router.navigate(['main-page']));
        }
        else {
          this.router.navigate(['main-page'])
        }
      })
  }

  onFileSelected(event: any) {
    this.selectedImage = event.target.files[0];
  }
}
