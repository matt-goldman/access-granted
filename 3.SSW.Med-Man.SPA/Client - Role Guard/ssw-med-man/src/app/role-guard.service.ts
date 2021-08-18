import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { UserService } from './services/user.service';
import { JwtHelperService, JwtModule} from '@auth0/angular-jwt'

@Injectable({
  providedIn: 'root'
})
export class RoleGuardService implements CanActivate {

  constructor(private userService: UserService, private router: Router, private helper: JwtHelperService) { }

  canActivate(route: ActivatedRouteSnapshot) : boolean {
    const expectedRole = route.data.expectedRole;
    const token = this.userService.getToken();
    const tokenPayload = this.helper.decodeToken(token);
    if(!this.userService.isLoggedIn || tokenPayload.role != expectedRole) {
      this.router.navigate(['/unauth']); //add you shall not pass page here
      console.log("Navigation faile");
      console.log("Expected role: " + expectedRole);
      console.log("Actual role: " + tokenPayload.role);
      return false;
    }
    return true;
   }
}
