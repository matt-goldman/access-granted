import { Component } from '@angular/core';
import {Router} from '@angular/router';
import { AuthService } from './auth-zero.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'SSW.Medication-Manager';
  router: Router;
  loggedIn: boolean;

  constructor(private myRouter: Router, private authService: AuthService) {
    this.router = myRouter;

    this.loggedIn = this.authService.isLoggedIn();
    this.router.navigate(['/home']);
  }
}
