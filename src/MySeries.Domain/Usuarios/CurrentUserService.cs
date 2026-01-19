using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Users;

namespace MySeries.Usuarios
{
    public class CurrentUserService
    {
        private readonly ICurrentUser _currentUser;

        public CurrentUserService(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public Guid? GetCurrentUserId()
        {
            return _currentUser.Id;
        }
    }
}
