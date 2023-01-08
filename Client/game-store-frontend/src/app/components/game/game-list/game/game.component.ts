import {Component, HostListener, Input, OnInit} from '@angular/core';
import {IGame} from "../../../../models/game.model";
import {ImageService} from "../../../../services/image.service";
import {IImage} from "../../../../models/image.model";
import {GenreService} from "../../../../services/genre.service";
import {IGenre} from "../../../../models/genre.model";
import {DomSanitizer} from "@angular/platform-browser";
import {ActivatedRoute, Router} from "@angular/router";
import {AuthenticationService} from "../../../../services/authentication.service";
import {CartService} from "../../../../services/cart.service";

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
  currentUserRole: string[] = [];

  @HostListener('mouseenter', ['$event']) onEnter() {
    if (this.currentUserRole.includes("Admin") || this.currentUserRole.includes("Manager")){
      this.onShowEditPage = true;
    }
  }
  @HostListener('mouseleave', ['$event']) onLeave() {
    this.onShowEditPage = false;
  }

  constructor(private imageService: ImageService, private genreService: GenreService, private sanitizer: DomSanitizer,
              private router: Router, private active: ActivatedRoute, private authService: AuthenticationService,
              private cartService: CartService) {}

  ngOnInit(): void {
    this.imageService.getImagesByGameId(this.game.id)
      .subscribe((images) => {
        this.gameImages = images;
        this.objectURL = 'data:image/jpeg;base64,' + images[0].image;
      });
    this.genreService.getGenresByGameId(this.game.id)
        .subscribe((genres) => this.gameGenres = genres);
    this.authService.currentUser.subscribe(
      user => {
        if (user != null){
          this.currentUserRole = user.roleNames;
        }
        else {
          this.currentUserRole = [];
        }
      }
    )
  }

  onOpenDetails() {
    this.router.navigate(['game-details', `${this.game.id}`]);
  }

  onAddToCart(event: Event) {
    event.preventDefault();
    event.stopPropagation();
    this.cartService.addGameToOrder(this.game);
  }
}

