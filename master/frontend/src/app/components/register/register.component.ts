import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './register.component.html'
})
export class RegisterComponent {
    form

    constructor(public auth: AuthService, private fb: FormBuilder) {
        this.form = fb.group({
            email: ['', Validators.required],
            name: ['', Validators.required],
            password: ['', Validators.required]
        });
    }
}
