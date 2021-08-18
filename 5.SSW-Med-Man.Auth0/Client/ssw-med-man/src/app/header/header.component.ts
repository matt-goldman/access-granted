import { Component, OnInit,Input } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from '../auth-zero.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  auth: AuthService;

  constructor(private authService: AuthService, private _snackBar: MatSnackBar, private router: Router) {
    this.auth = authService;
   }

  @Input()
  loggedIn: boolean;
  subscription: Subscription;

  ngOnInit() {
    this.subscription = this.authService.authNavStatus$.subscribe(loggedin => this.loggedIn = loggedin);
  }

  logout(){
    this.authService.logout();
    this.loggedIn = false;
    this._snackBar.open("You have been logged out", "OK" ,{ duration: 3000} );
    this.router.navigate(['/home']);
  }

}
