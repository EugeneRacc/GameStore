import {Component, Input, OnInit} from '@angular/core';
import {Router} from "@angular/router";

@Component({
  selector: 'app-edit-game',
  templateUrl: './edit-game.component.html',
  styleUrls: ['./edit-game.component.css']
})
export class EditGameComponent implements OnInit {
  @Input() gameId: string;

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  onEditGame() {
    this.router.navigate(['edit', this.gameId]);
  }
}
