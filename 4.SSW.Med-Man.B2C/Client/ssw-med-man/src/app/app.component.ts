import { Component } from '@angular/core';
import {Router} from '@angular/router';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'SSW.Medication-Manager';
  router: Router;
  loggedIn: boolean;
  userService: UserService;

  constructor(private myRouter: Router, private userSvc: UserService) {
    this.router = myRouter;
    this.userService = userSvc;

    this.loggedIn = this.userService.isLoggedIn();
    this.router.navigate(['/home']);
  }
}
