import { UserService } from './services/user.service';
import { Router, CanActivate } from '@angular/router';
import { Injectable } from '@angular/core';

@Injectable()
export class AuthGuard implements CanActivate{
    constructor(private user:UserService, private router:Router){}

    canActivate() {
        if(!this.user.isLoggedIn()) {
            this.router.navigate(['/login']);
            return false;
        }

        return true;
    }
}