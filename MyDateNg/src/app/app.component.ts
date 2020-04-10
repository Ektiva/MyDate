import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthService } from './_services/auth.service';
import { setTheme } from 'ngx-bootstrap/utils';

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

    ngOnInit(){
      const token = localStorage.getItem('token');
      if(token){
        this.authService.decodedToken = this.jwtHelper.decodeToken(token);
      }
    }
}
