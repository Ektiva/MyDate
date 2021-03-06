import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthService } from './_services/auth.service';
import { setTheme } from 'ngx-bootstrap/utils';
import { User } from './_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'MyDateNg';
  jwtHelper = new JwtHelperService();

  constructor(private authService: AuthService){
    setTheme('bs4');
  }

    ngOnInit() {
      const token = localStorage.getItem('token');
      const user: User = JSON.parse(localStorage.getItem('user'));
      if (token) {
        this.authService.decodedToken = this.jwtHelper.decodeToken(token);
      }
      if (user) {
        this.authService.currentUser = user;
        this.authService.changeMemberPhoto(user.photoUrl);
      }
    }
}
