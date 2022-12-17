import { Injectable } from '@angular/core';
import {ICommentModel} from "../models/comment.model";
import {BehaviorSubject, Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {IEditCommentModel} from "../models/edit-comment.model";

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  baseURI = "https://localhost:7043/api/";
  testComments = new BehaviorSubject<ICommentModel[]>([]);
  constructor(private http: HttpClient) { }
  getAllCommentsByGameId(id: string): Observable<ICommentModel[]>{
    return this.http.get<ICommentModel[]>(this.baseURI + "comment/" + id);
  }

  createComment(gameId: string, comment: IEditCommentModel): Observable<ICommentModel> {
    return this.http.post<ICommentModel>(this.baseURI + "comment/" + gameId, comment);
  }

  getCommentReplies(currentComment: ICommentModel): ICommentModel[] {
    let comments: ICommentModel[] = [];
    if (currentComment.childComments == null || currentComment.childComments.length <= 0) {
      return comments;
    }
    for(const childId of currentComment.childComments){
      for (const testComment of this.testComments.getValue()) {
        if (testComment.id == childId){
          comments.push(testComment);
        }
      }
    }
    return comments;
  }
}
