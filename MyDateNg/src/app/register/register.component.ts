import { Component, OnInit, /*Input,*/ EventEmitter, Output } from '@angular/core';
import { __importDefault } from 'tslib';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

import {
  FormGroup,
  FormControl,
  Validators,
  FormBuilder
} from '@angular/forms';
// import { BsDatepickerConfig } from 'ngx-bootstrap';
import { User } from '../_models/user';
import { Router } from '@angular/router';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  // model: any = {};
  user: User;
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;

  constructor(
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService,
    private fb: FormBuilder
    ) { }

  ngOnInit() {
    /*this.registerForm = new FormGroup({
      username: new FormControl('', Validators.required),
      password: new FormControl('',
        [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
      confirmPassword: new FormControl('', Validators.required)
    }, this.passwordMatchValidator);*/

    this.bsConfig = {
      containerClass: 'theme-red'
    };
    this.createRegisterForm();
  }

  createRegisterForm() {
    this.registerForm = this.fb.group(
      {
        gender: ['male'],
        username: ['', Validators.required],
        knownAs: ['', Validators.required],
        dateOfBirth: [null, Validators.required],
        city: ['', Validators.required],
        country: ['', Validators.required],
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(7),
            Validators.maxLength(11)
          ]
        ],
        confirmPassword: ['', Validators.required]
      },
      { validator: this.passwordMatchValidator }
    );
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value
      ? null : { mismatch: true };
  }

  register() {
    if (this.registerForm.valid) {
      this.user = Object.assign({}, this.registerForm.value);
      console.log(this.user);
      this.authService.register(this.user).subscribe(
        () => {
          this.alertify.success('Registration succesful');
        },
        error => {
          this.alertify.error(error);
        },
        () => {
          this.authService.login(this.user).subscribe(() => {
            this.router.navigate(['/members']);
          });
        }
      );
    }
  }

  /*register() {
    /*this.authService.register(this.model).subscribe(() => {
      this.alertify.success('registration successful');
    }, error => {
      this.alertify.error(error);
    });
    console.log(this.registerForm.value);
  }*/

  cancel() {
    this.cancelRegister.emit(false);
    console.log('cancelled');
  }

}
