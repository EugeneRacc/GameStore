import { Component, OnInit } from '@angular/core';
import {IGenre} from "../../../models/genre.model";
import {GenreService} from "../../../services/genre.service";

@Component({
  selector: 'app-filter-genres',
  templateUrl: './filter-genres.component.html',
  styleUrls: ['./filter-genres.component.css']
})
export class FilterGenresComponent implements OnInit {
  genres: IGenre[] = [];

  constructor(private genreService: GenreService) { }

  ngOnInit(): void {
    this.genreService.getGenres().subscribe(
      genres => {
        this.genres = genres
      })
  }

}
