import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { FilterComponent } from './components/filter/filter.component';
import { FilterGenresComponent } from './components/filter/filter-genres/filter-genres.component';
import { GenreComponent } from './components/filter/filter-genres/genre/genre.component';
import {HttpClientModule} from "@angular/common/http";
import { GameListComponent } from './components/game/game-list/game-list.component';
import {GameComponent} from "./components/game/game-list/game/game.component";

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    FilterComponent,
    FilterGenresComponent,
    GenreComponent,
    GameListComponent,
    GameComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
