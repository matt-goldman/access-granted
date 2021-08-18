import { Injectable } from '@angular/core';
import { AuthService } from './auth-zero.service';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanActivate } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable()
export class AuthGuard implements CanActivate{
    constructor(private auth:AuthService){}

    canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
      ): Observable<boolean> | Promise<boolean|UrlTree> | boolean {
        return this.auth.isAuthenticated$.pipe(
          tap(loggedIn => {
            if (!loggedIn) {
              this.auth.login(state.url);
            }
          })
        );
      }
}