import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {IEditCommentModel} from "../../../models/edit-comment.model";
import {ActivatedRoute} from "@angular/router";
import {AuthenticationService} from "../../../services/authentication.service";
import {CommentService} from "../../../services/comment.service";
import {ICommentModel} from "../../../models/comment.model";

@Component({
  selector: 'app-update-comment',
  templateUrl: './update-comment.component.html',
  styleUrls: ['./update-comment.component.css']
})
export class UpdateCommentComponent implements OnInit {
  @Output() onShowCreatePage = new EventEmitter<boolean>(false);
  @Input() currentComment: ICommentModel;
  gameId = "";
  commentToUpdate: IEditCommentModel = {
    id: "",
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
          this.commentToUpdate.userId = user.id
        }
      })
  }

  onSubmit(comment: string) {
    this.commentToUpdate.body = comment;
    this.commentToUpdate.id = this.currentComment.id;
      if (this.commentToUpdate.body && this.commentToUpdate.userId) {
      this.commentService.updateComment(this.gameId, this.commentToUpdate)
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
