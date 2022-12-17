import {Component, Input, OnInit} from '@angular/core';
import {ICommentModel} from "../../../../models/comment.model";
import {UserService} from "../../../../services/user.service";
import {CommentService} from "../../../../services/comment.service";

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.css']
})
export class CommentComponent implements OnInit {
  @Input() currentComment: ICommentModel;
  @Input("replies") commentReplies: ICommentModel[];
  currentUserName: string = "";
  constructor(private userService: UserService, private commentService: CommentService) { }

  ngOnInit(): void {
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

}
