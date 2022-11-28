import {Component, Input, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {IGame} from "../../../models/game.model";
import {GameService} from "../../../services/game.service";

@Component({
  selector: 'app-edit-game',
  templateUrl: './edit-game.component.html',
  styleUrls: ['./edit-game.component.css']
})
export class EditGameComponent implements OnInit {
  @Input() game: IGame;

  constructor(private router: Router, private gameService: GameService) { }

  ngOnInit(): void {
  }

  onEditGame() {
    this.router.navigate(['edit', this.game.id]);
  }

  onDeleteGame() {
    this.gameService.deleteGame(this.game)
      .subscribe(
        (message) => {
          console.log(message)

        },
        error => {
          console.log(error.message())
        }
      );
  }

  onOpenGameDetails() {
    this.router.navigate([this.game.id])
  }
}
