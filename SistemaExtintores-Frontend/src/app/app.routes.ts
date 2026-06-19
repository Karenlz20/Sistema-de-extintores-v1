import { Routes } from '@angular/router';
import { Login } from './login/login';
import { Dashboard } from './dashboard/dashboard';
import { Usuarios } from './usuarios/usuarios';
import { Clientes } from './clientes/clientes';
import { Inventario } from './inventario/inventario';
import { Extintores } from './extintores/extintores';
import { Ventas } from './ventas/ventas';
import { PerfilCliente } from './perfil-cliente/perfil-cliente';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'dashboard', component: Dashboard },
  { path: 'usuarios', component: Usuarios },
  { path: 'clientes', component: Clientes },
  { path: 'inventario', component: Inventario },
  { path: 'extintores', component: Extintores },
  { path: 'ventas', component: Ventas },
  { path: 'clientes/:id', component: PerfilCliente },
];
