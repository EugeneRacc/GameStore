import { Injectable } from '@angular/core';
import {ICommentModel} from "../models/comment.model";
import {BehaviorSubject, Observable} from "rxjs";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {IEditCommentModel} from "../models/edit-comment.model";

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  baseURI = "https://localhost:7043/api/";
  testComments = new BehaviorSubject<ICommentModel[]>([]);
  commentsToDelete: ICommentModel[] = [];
  constructor(private http: HttpClient) { }
  getAllCommentsByGameId(id: string): Observable<ICommentModel[]>{
    return this.http.get<ICommentModel[]>(this.baseURI + "comment/" + id);
  }

  createComment(gameId: string, comment: IEditCommentModel): Observable<ICommentModel> {
    return this.http.post<ICommentModel>(this.baseURI + "comment/" + gameId, comment);
  }

  updateComment(gameId: string, comment: IEditCommentModel): Observable<ICommentModel>{
    return this.http.put<ICommentModel>(this.baseURI + "comment/" + gameId, comment);
  }

  deleteComment(gameId: string, comment: ICommentModel): Observable<{response: string}> {
    let tokenHeader = new HttpHeaders({'Authorization':'Bearer ' + localStorage.getItem("token")});
    return this.http.delete<{response: string}>(this.baseURI + "comment/" + gameId,
      {body: comment, headers: tokenHeader});
  }

  deleteRangeOfComments(comments: ICommentModel[], token: string) : Observable<{response: string}> {
    let tokenHeader = new HttpHeaders({'Authorization':'Bearer ' + token})
    return this.http.delete<{response: string}>(this.baseURI + "comment",
      {headers: tokenHeader , body: {comments: comments}});
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
