import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {MainPageComponent} from "./components/main-page/main-page.component";
import {GameDetailsComponent} from "./components/game-details/game-details.component";
import {EditGamePageComponent} from "./components/main-page/edit-game/edit-game-page/edit-game-page.component";
import {CreateGameComponent} from "./components/main-page/create-game/create-game.component";

const routes: Routes = [
  {
    path: "",
    component: MainPageComponent
  },
  {
    path: "create",
    component: CreateGameComponent
  },
  {
    path: ":id",
    component: GameDetailsComponent
  },
  {
    path: "edit/:id",
    component: EditGamePageComponent
  },
  {
    path: "**",
    component: MainPageComponent
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
