using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Guid userId, string email, string name, string role);
    }
}
