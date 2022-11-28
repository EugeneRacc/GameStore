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
import { FooterComponent } from './components/footer/footer.component';
import { MainPageComponent } from './components/main-page/main-page.component';
import { GameDetailsComponent } from './components/game-details/game-details.component';
import { EditGameComponent } from './components/main-page/edit-game/edit-game.component';
import { EditGamePageComponent } from './components/main-page/edit-game/edit-game-page/edit-game-page.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    FilterComponent,
    FilterGenresComponent,
    GenreComponent,
    GameListComponent,
    GameComponent,
    FooterComponent,
    MainPageComponent,
    GameDetailsComponent,
    EditGameComponent,
    EditGamePageComponent
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
