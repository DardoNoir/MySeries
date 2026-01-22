using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace MySeries.Usuarios
{
    public interface IUsuariosAppService: IApplicationService
    {
        Task<UsuarioDto> crearUsuarioAsync(CreateUsuarioDto input);        
        Task<UsuarioDto> GetUsuarioAsync(string UserName, string Password);
    }
}