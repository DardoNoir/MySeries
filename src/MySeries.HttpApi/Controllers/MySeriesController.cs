using Microsoft.AspNetCore.Mvc;
using MySeries.Application.Contracts;
using MySeries.Application.Usuarios;
using MySeries.Series;
using MySeries.SerieService;
using MySeries.Usuarios;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace MySeries.Controllers
{
    [Route("api/app/serie")]
    public class SerieController : AbpController
    {
        private readonly SerieAppService _serieAppService;

        public SerieController(SerieAppService serieAppService)
        {
            _serieAppService = serieAppService;
        }

        [HttpGet("search-by-title")]
        public async Task<ICollection<SerieDto>> SearchByTitleAsync([FromQuery] string title, [FromQuery] string? genre)
        {
            return await _serieAppService.SearchByTitleAsync(title, genre);
        }
    }

    [Route("api/app/usuarios")]
    public class UsuariosController : AbpController
    {
        private readonly UsuariosAppService _usuariosAppService;

        public UsuariosController(UsuariosAppService usuariosAppService)
        {
            _usuariosAppService = usuariosAppService;
        }

        [HttpGet("crear-usuario")]
        public async Task<UsuarioDto> CrearUsuarioAsync(
            [FromQuery] string userName,
            [FromQuery] string password,
            [FromQuery] string? email,
            [FromQuery] bool notificationsByEmail = false,
            [FromQuery] bool notificationsByApp = false
        )
        {
            var input = new CreateUsuarioDto
            {
                UserName = userName,
                Password = password,
                Email = email,
                NotificationsByEmail = notificationsByEmail,
                NotificationsByApp = notificationsByApp
            };

            return await _usuariosAppService.CrearUsuarioAsync(input);
        }
    }

}
