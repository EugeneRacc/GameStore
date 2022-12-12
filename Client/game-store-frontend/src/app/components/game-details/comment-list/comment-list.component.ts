import { Component, OnInit } from '@angular/core';
import {ICommentModel} from "../../../models/comment.model";

@Component({
  selector: 'app-comment-list',
  templateUrl: './comment-list.component.html',
  styleUrls: ['./comment-list.component.css']
})
export class CommentListComponent implements OnInit {

  testComments: ICommentModel[] = [
    {"id": "1",
      "body": "This is my first try comment",
      "createdDate": "2022-11-10T18:09:12.3439913",
      "gameId": "fce6a248-3b23-4b2c-87f1-11cd1fc12849",
      "userId": "143f2716-85df-46bc-a69a-f247e8b97e5b",
      "replyId": null},
    {
      "id": "2",
      "body": "This is my second try comment",
      "createdDate": "2022-11-10T18:09:12.3439913",
      "gameId": "fce6a248-3b23-4b2c-87f1-11cd1fc12849",
      "userId": "143f2716-85df-46bc-a69a-f247e8b97e5b",
      "replyId": null
    },
    {
      "id": "3",
      "body": "This is my third try comment",
      "createdDate": "2022-11-10T18:09:12.3439913",
      "gameId": "fce6a248-3b23-4b2c-87f1-11cd1fc12849",
      "userId": "143f2716-85df-46bc-a69a-f247e8b97e5b",
      "replyId": null
    },
    {
      "id": "4",
      "body": "This is my fourth try comment",
      "createdDate": "2022-11-10T18:09:12.3439913",
      "gameId": "fce6a248-3b23-4b2c-87f1-11cd1fc12849",
      "userId": "143f2716-85df-46bc-a69a-f247e8b97e5b",
      "replyId": "1"
    }
  ];
  constructor() { }

  ngOnInit(): void {
  }

}
