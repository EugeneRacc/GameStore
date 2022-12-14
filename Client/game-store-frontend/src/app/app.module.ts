import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { FilterComponent } from './components/filter/filter.component';
import { FilterGenresComponent } from './components/filter/filter-genres/filter-genres.component';
import { GenreComponent } from './components/filter/filter-genres/genre/genre.component';
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import { GameListComponent } from './components/game/game-list/game-list.component';
import {GameComponent} from "./components/game/game-list/game/game.component";
import { FooterComponent } from './components/footer/footer.component';
import { MainPageComponent } from './components/main-page/main-page.component';
import { GameDetailsComponent } from './components/game-details/game-details.component';
import { EditGameComponent } from './components/main-page/edit-game/edit-game.component';
import { EditGamePageComponent } from './components/main-page/edit-game/edit-game-page/edit-game-page.component';
import { CreateGameComponent } from './components/main-page/create-game/create-game.component';
import {FormsModule} from "@angular/forms";
import { RegistrationComponent } from './components/authentication/registration/registration.component';
import { LoginComponent } from './components/authentication/login/login.component';
import { StopPropagationDirective } from './directives/stop-propagation.directive';
import { CommentListComponent } from './components/game-details/comment-list/comment-list.component';
import { CommentComponent } from './components/game-details/comment-list/comment/comment.component';
import { CreateCommentComponent } from './components/game-details/create-comment/create-comment.component';
import { UpdateCommentComponent } from './components/game-details/update-comment/update-comment.component';
import {AuthInterceptor} from "./interceptor/auth.interceptor";
import { CartComponent } from './components/cart/cart.component';
import { ButtonComponent } from './components/helpers/button/button.component';
import { CartItemComponent } from './components/cart/cart-item/cart-item.component';
import { ConfirmationOrderComponent } from './components/confirmation-order/confirmation-order.component';
import { AdminPanelComponent } from './components/admin-panel/admin-panel.component';
import { FilterNameComponent } from './components/filter/filter-name/filter-name.component';
import { UserListComponent } from './components/admin-panel/user-list/user-list.component';
import { UserComponent } from './components/admin-panel/user-list/user/user.component';
import { EditRoleComponent } from './components/admin-panel/user-list/user/edit-role/edit-role.component';

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
    EditGamePageComponent,
    CreateGameComponent,
    RegistrationComponent,
    LoginComponent,
    StopPropagationDirective,
    CommentListComponent,
    CommentComponent,
    CreateCommentComponent,
    UpdateCommentComponent,
    CartComponent,
    ButtonComponent,
    CartItemComponent,
    ConfirmationOrderComponent,
    AdminPanelComponent,
    FilterNameComponent,
    UserListComponent,
    UserComponent,
    EditRoleComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
