export interface ICommentModel {
  id: string,
  body: string,
  createdDate: string,
  gameId: string,
  userId: string,
  replyId: string | null,
  childComments: string[] | null
}
