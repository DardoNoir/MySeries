export interface UsuarioDto {
  id: number;
  userName: string;
  email?: string;
  notificationsByEmail: boolean;
  notificationsByApp: boolean;
  rol: number;
}

export interface CreateUsuarioDto {
  userName: string;
  password: string;
  email?: string;
  notificationsByEmail: boolean;
  notificationsByApp: boolean;
}
