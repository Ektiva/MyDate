import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  model: any = {}; 
  photoUrl: string;

  constructor(
    public authService: AuthService, 
    private alertify: AlertifyService,
    private router: Router) { }

  ngOnInit() {
    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('logged in successfully');
    }, error => {
      this.alertify.error(error);
      console.log(error);
    }, () => {
        this.router.navigate(['/members']);
    });
  }

  loggedIn() {
    /*const token = localStorage.getItem('token');
    //Return true if the token is not empty
    return !!token;*/
    // use jwt 3rd party instead
    return this.authService.loggedIn();
  }

  logOut() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.decodedToken = null;
    this.authService.currentUser = null;
    this.alertify.message('logged out');
    this.router.navigate(['/home']);
  }

}
