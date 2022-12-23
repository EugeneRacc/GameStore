import {Component, HostListener, OnInit} from '@angular/core';
import {AuthenticationService} from "./services/authentication.service";
import {CommentService} from "./services/comment.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  @HostListener('window:beforeunload', ['$event']) async beforeUnloadHandler(event: any) {
    let refresh = localStorage.getItem('refresh');
    let token = localStorage.getItem('token');
    if (!refresh) {
      localStorage.removeItem('token');
    }
    if (this.commentService.commentsToDelete.length > 0) {
      event.preventDefault();
      this.commentService.deleteRangeOfComments(this.commentService.commentsToDelete,
        token ?? "")
        .subscribe();
    }
  }

  constructor(private authService: AuthenticationService, private commentService: CommentService) {}
  ngOnInit(): void {
    this.checkIfUserAuthorized();
  }

  checkIfUserAuthorized() {
    let refresh = localStorage.getItem('refresh');
    let token = localStorage.getItem('token');
    if (refresh && token){
      this.authService.authorizeByRefresh({ token: token, refreshToken: refresh })
        .subscribe(token => {
          localStorage.setItem('token', token.token);
          this.authService.isAuth.next(true);
          this.authService.getUserDetails(token.token)
            .subscribe(user => this.authService.setNewCurrentUser(user))
        })
    }
  }
}
