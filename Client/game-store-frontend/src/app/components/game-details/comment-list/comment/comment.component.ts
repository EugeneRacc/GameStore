import {Component, Input, OnInit} from '@angular/core';
import {ICommentModel} from "../../../../models/comment.model";
import {UserService} from "../../../../services/user.service";
import {CommentService} from "../../../../services/comment.service";
import {AuthenticationService} from "../../../../services/authentication.service";

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.css']
})
export class CommentComponent implements OnInit {
  @Input() currentComment: ICommentModel;
  @Input("replies") commentReplies: ICommentModel[];
  currentUserName: string = "";
  currentUserId: string = "";
  replyComment = false;
  updateComment = false;
  constructor(private userService: UserService, private commentService: CommentService,
              private authService: AuthenticationService) { }

  ngOnInit(): void {
    this.authService.currentUser
      .subscribe(user => {
        if (user) {
          this.currentUserId = user.id;
        }
      });
    this.userService.getUserById(this.currentComment.userId)
      .subscribe(user => {
        if (user != null) {
          if (!(user.userName != null || user.userName != undefined)) {
            this.currentUserName = user.firstName + " " + user.lastName;
          } else {
            this.currentUserName = user.userName;
          }
        }
      })
  }

  getCommentReplies(currentComment: ICommentModel): ICommentModel[] {
    return this.commentService.getCommentReplies(currentComment);
  }

  showCreateReply(show: boolean) {
    if (show) this.replyComment = false;
  }

  showUpdate(show: boolean) {
    if (show) this.updateComment = false;
  }
}
