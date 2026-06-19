import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiClient } from '../core/http/api-client';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.html',
})
export class Login {
  nombreUsuario = '';
  password = '';
  error = '';

  constructor(private api: ApiClient, private router: Router) {}

  iniciarSesion() {
    this.error = '';
    this.api.post<any>('http://localhost:5057/Auth/login', {
      nombreUsuario: this.nombreUsuario,
      password: this.password
    }).subscribe({
      next: (usuario) => {
        localStorage.setItem('usuario', JSON.stringify(usuario));
        this.router.navigate(['/dashboard']);
      },
      error: () => {
        this.error = 'Usuario o contraseña incorrectos.';
      }
    });
  }
}
