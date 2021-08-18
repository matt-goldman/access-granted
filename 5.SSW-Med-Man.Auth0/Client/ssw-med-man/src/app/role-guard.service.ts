import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt'
import { AuthService } from './auth-zero.service';
import { Observable, Subscription } from 'rxjs';
import { map } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class RoleGuardService implements CanActivate {

  constructor(private authService: AuthService, private router: Router, private helper: JwtHelperService) { }
  subscription: Subscription;
  token: string;
  roles: string[] = [];

  ngOnInit(){
    this.subscription = this.authService.tokenSource$.subscribe(token => {
      this.token = token;
      console.log("Got new token:");
      console.log(this.token);
    });
  }

   canActivate(route: ActivatedRouteSnapshot): Observable<boolean> {
    return this.authService.getTokenSilently$().pipe(
      map(token => {
        console.log(token);
        let tokenPayload = this.helper.decodeToken(token);
        console.log(tokenPayload);
        let expectedRole = route.data.expectedRole;
        console.log("Expected role:");
        console.log(expectedRole);
        let actualRole = tokenPayload.role;
        for(let key in tokenPayload){
          var sections = key.split('/');
          var actualKey = sections[sections.length -1];
          if(actualKey.toLowerCase() == 'roles'){
            var roles = tokenPayload[key];
            for(let role of roles){
              this.roles.push(role);
              console.log("Role found in array:" + role);
            }
          }
          if(actualKey.toLowerCase() == 'role'){
            this.roles.push(tokenPayload[key]);
            console.log("Role found in token: " + tokenPayload[key]);
          }
        }
        if(this.authService.loggedIn && expectedRole == actualRole || this.roles.includes(expectedRole)){
          return true;
        }
        else{
          this.router.navigate(['/unauth']);
          return false;
        }
      }));
  }
}
