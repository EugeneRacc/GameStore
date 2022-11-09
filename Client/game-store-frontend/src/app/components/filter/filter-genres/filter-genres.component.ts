import { Component, OnInit } from '@angular/core';
import {IGenre} from "../../../models/genre.model";

@Component({
  selector: 'app-filter-genres',
  templateUrl: './filter-genres.component.html',
  styleUrls: ['./filter-genres.component.css']
})
export class FilterGenresComponent implements OnInit {
  genres: IGenre[] = [
    {id: "1", name: "RPG"},
    {id: "2", name: "Strategy"},
    {id: "3", name: "Action"},
    {id: "4", name: "RPG1"},
    {id: "5", name: "RPG1"},
    {id: "6", name: "RPG1"},
    {id: "7", name: "RPG1"},

  ]

  constructor() { }

  ngOnInit(): void {
  }

}
