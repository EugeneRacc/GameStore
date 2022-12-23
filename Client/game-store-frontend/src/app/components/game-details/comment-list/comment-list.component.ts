import { Component, OnInit } from '@angular/core';
import {ICommentModel} from "../../../models/comment.model";
import {CommentService} from "../../../services/comment.service";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-comment-list',
  templateUrl: './comment-list.component.html',
  styleUrls: ['./comment-list.component.css']
})
export class CommentListComponent implements OnInit {
  testComments: ICommentModel[] = [];

  constructor(private commentService: CommentService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.commentService.getAllCommentsByGameId(this.route.snapshot.params["id"])
      .subscribe(gameComments => {
        this.commentService.testComments.next(gameComments);
        this.commentService.testComments
          .subscribe(comments => this.testComments = comments);
      },
        error => {
          console.error(error.message)
        });
  }

  getCommentReplies(currentComment: ICommentModel): ICommentModel[] {
    return this.commentService.getCommentReplies(currentComment);
  }
}
