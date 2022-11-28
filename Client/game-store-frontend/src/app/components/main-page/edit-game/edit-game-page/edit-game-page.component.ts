import { Component, OnInit } from '@angular/core';
import {IGame} from "../../../../models/game.model";

@Component({
  selector: 'app-edit-game-page',
  templateUrl: './edit-game-page.component.html',
  styleUrls: ['./edit-game-page.component.css']
})
export class EditGamePageComponent implements OnInit {
  game:IGame;


  constructor() { }

  ngOnInit(): void {
  }

}
