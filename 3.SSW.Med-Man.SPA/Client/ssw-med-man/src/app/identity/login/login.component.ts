import { Component, OnInit } from '@angular/core';
import { LoginUserDTO } from '../../../helpers/api-client';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  lottieConfig: Object;

  private anim: any;
  private animationSpeed: number;
  loginUserDTO: LoginUserDTO = { email: '', password: ''};
  errors: string;
  isRequesting: boolean;
  submitted: boolean = false;

  ngOnInit() {
      
    this.animationSpeed = 15;
  }

  constructor(private userService: UserService, private router: Router, private _snakBar: MatSnackBar) {
    this.lottieConfig = {
        path: '../assets/snakestaff.json', 
        autoplay: true,
        loop: false
    };
  }

  login({value} : {value: LoginUserDTO}) {
    console.log("Attempting login with:");
    console.log(value)
    this.submitted = true;
    this.isRequesting = true;
    this.errors='';
    this.userService.login(value)
        .subscribe(
          result => {
            if(result) {
              this.router.navigate(['/home']);
            } else {
              this._snakBar.open("Login Failed", "OK" , {duration: 3000});
            }
          },
          error => {
            this.errors = error;
            this._snakBar.open("Login Failed", "OK" , {duration: 3000});
            console.log(this.errors);
          });
  }

  handleAnimation(anim: any) {
      this.anim = anim;
  }

  stop() {
      this.anim.stop();
  }

  play() {
      this.anim.play();
  }

  pause() {
      this.anim.pause();
  }

  setSpeed(speed: number) {
      this.animationSpeed = speed;
      this.anim.setSpeed(speed);
  }
}
