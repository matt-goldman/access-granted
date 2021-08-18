import { Observable, from, BehaviorSubject } from 'rxjs';
import {Injectable} from '@angular/core';
import { LoginUserDTO, AuthClient } from '../../helpers/api-client';
import { map } from 'rxjs/operators';

@Injectable()

export class UserService{
    baseUrl: string = '';

    //observable navItem source
    private _authNavStatusSource = new BehaviorSubject<boolean>(false);
    //observable navItem stream
    authNavStatus$ = this._authNavStatusSource.asObservable();

    private loggedIn = false;

    constructor(private authClient: AuthClient) {
        this.loggedIn = !!localStorage.getItem('auth_token');
        this._authNavStatusSource.next(this.loggedIn);
        this.baseUrl = '';
    }

    getToken(){
      return localStorage.getItem('auth_token');
    }

    login(loginUser): Observable<Boolean> {
        return this.authClient.login(loginUser).pipe(
          map(
              (res) => { 
                localStorage.setItem('auth_token', res.token);
                this.loggedIn = true;
                this._authNavStatusSource.next(true);
                console.log('Login succesful! Token received:');
                console.log(res.token);
                return true;
              },
              (error) => {
                console.log('Error:');
                console.log(error);
                return false;
              }));
    }

    logout(){
        localStorage.removeItem('auth_token');
        this.loggedIn = false;
        this._authNavStatusSource.next(false);
    }

    isLoggedIn() {
        return this.loggedIn;
    }
}