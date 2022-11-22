import {Component, Input, OnInit} from '@angular/core';
import {IGenre} from "../../../../models/genre.model";
import {GenreService} from "../../../../services/genre.service";

@Component({
  selector: 'app-genre',
  templateUrl: './genre.component.html',
  styleUrls: ['./genre.component.css']
})
export class GenreComponent implements OnInit {
  @Input() genre: IGenre;

  constructor(private genreService: GenreService) { }

  ngOnInit(): void {
  }

  onAddGenreToFilter(){
    this.genreService.addNewGenreToFilter(this.genre);
  }
}
