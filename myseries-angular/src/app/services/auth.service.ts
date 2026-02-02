import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { UsuarioDto, CreateUsuarioDto } from '../models/UsuarioDto';
import { Observable, tap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private baseUrl = '/api/app/usuarios';

  constructor(private http: HttpClient) {}

  // LOGIN 
  login(userName: string, password: string): Observable<UsuarioDto> {
    const params = new HttpParams()
      .set('userName', userName)
      .set('password', password);

    return this.http.get<UsuarioDto>(
      `${this.baseUrl}/usuario`,
      { params }
    ).pipe(
      tap(user => localStorage.setItem('user', JSON.stringify(user)))
    );
  }

  // CREAR USUARIO 
 createUser(input: CreateUsuarioDto): Observable<UsuarioDto> {
  const params = new HttpParams()
    .set('userName', input.userName)
    .set('password', input.password)
    .set('email', input.email ?? '')
    .set('notificationsByEmail', input.notificationsByEmail)
    .set('notificationsByApp', input.notificationsByApp);

  return this.http.get<UsuarioDto>(
    `${this.baseUrl}/crear-usuario`,
    { params }
  );
}


  logout(): void {
    localStorage.removeItem('user');
  }

  isLogged(): boolean {
    return !!localStorage.getItem('user');
  }

  isAdmin(): boolean {
    const user = localStorage.getItem('user');
    if (!user) return false;

    const parsed = JSON.parse(user);
    return parsed.rol === 1;
  }

  getUser() {
  const user = localStorage.getItem('user');
  if (!user) throw new Error('Usuario no logueado');
  return JSON.parse(user);
}



  setUser(user: UsuarioDto) {
    localStorage.setItem('user', JSON.stringify(user));
  }

}
