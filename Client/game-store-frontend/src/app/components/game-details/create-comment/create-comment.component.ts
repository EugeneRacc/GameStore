import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {IEditCommentModel} from "../../../models/edit-comment.model";
import {ActivatedRoute} from "@angular/router";
import {AuthenticationService} from "../../../services/authentication.service";
import {CommentService} from "../../../services/comment.service";

@Component({
  selector: 'app-create-comment',
  templateUrl: './create-comment.component.html',
  styleUrls: ['./create-comment.component.css']
})
export class CreateCommentComponent implements OnInit {
  @Output() onShowCreatePage = new EventEmitter<boolean>(false);
  @Input() parentId = "";
  @Input() message = "Write your comment";
  @Input() update = false;
  gameId = "";
  commentToAdd: IEditCommentModel = {
    body: "",
    userId: "",
  }
  constructor(private active: ActivatedRoute, private authService: AuthenticationService,
              private commentService: CommentService) { }

  ngOnInit(): void {
    this.gameId = this.active.snapshot.params["id"];
    this.authService.currentUser
      .subscribe(user => {
        if (user != null) {
          this.commentToAdd.userId = user.id
        }
      })
  }

  onSubmit(comment: string) {
    this.commentToAdd.body = comment;
    if (this.parentId) {
      this.commentToAdd.replyId = this.parentId;
    }
    if (this.commentToAdd.body && this.commentToAdd.userId) {
      this.commentService.createComment(this.gameId, this.commentToAdd)
        .subscribe(() => {
          this.commentService.getAllCommentsByGameId(this.gameId)
            .subscribe(allComments => this.commentService.testComments.next(allComments));
          this.showCreatingPage(true);
        })
    }
  }

  showCreatingPage(value: boolean){
    this.onShowCreatePage.emit(value);
  }
}
