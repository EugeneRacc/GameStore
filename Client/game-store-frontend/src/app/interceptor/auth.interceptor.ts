import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {BehaviorSubject, catchError, filter, Observable, switchMap, take, throwError} from "rxjs";
import {AuthenticationService} from "../services/authentication.service";
import {IRefreshTokenModel} from "../models/refresh.model";
import {Router} from "@angular/router";
import {TokenService} from "../services/token.service";
import {ITokenModel} from "../models/token.model";
@Injectable()
export class AuthInterceptor implements HttpInterceptor{
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);
  constructor(private authService: AuthenticationService, private router: Router,
              private tokenService: TokenService) {}
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let authReq = req;
    const token = this.tokenService.getToken();
    if (token != null) {
      authReq = this.addTokenHeader(req, token);
    }

    return next.handle(authReq).pipe(catchError(error => {
      if (error instanceof HttpErrorResponse && !authReq.url.includes('authentication/login') && error.status === 401) {
        return this.handle401Error(authReq, next);
      }

      return throwError(error);
    }));
  }

  addTokenHeader(request: HttpRequest<any>, token: string) {
      return request.clone({
        headers: request.headers.set('Authorization', 'Bearer ' + token)
      });
  }

  private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      const token = {
        token: localStorage.getItem('token') ?? "",
        refreshToken: localStorage.getItem('refresh') ?? ""
      } as IRefreshTokenModel;

      if (token.refreshToken)
        return this.authService.authorizeByRefresh(token).pipe(
          switchMap((token: ITokenModel) => {
            this.isRefreshing = false;

            this.tokenService.saveToken(token.token);
            this.refreshTokenSubject.next(token.token);

            return next.handle(this.addTokenHeader(request, token.token));
          }),
          catchError((err) => {
            this.isRefreshing = false;

            this.tokenService.signOut();
            return throwError(err);
          })
        );
    }
    return this.refreshTokenSubject.pipe(
      filter(token => token !== null),
      take(1),
      switchMap((token) => next.handle(this.addTokenHeader(request, token)))
    );
  }
}
