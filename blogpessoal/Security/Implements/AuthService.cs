using blogpessoal.Model;
using blogpessoal.Service;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace blogpessoal.Security.Implements
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        public AuthService(IUserService userSrvice)
        {
            _userService = userSrvice;
        }
        public async Task<UserLogin?> Autenticar(UserLogin userlogin)
        {
            string FotoDefault = "https://imgur.com/a/soRGehO";

            if (userlogin is null || string.IsNullOrEmpty(userlogin.Usuario) || string.IsNullOrEmpty(userlogin.Senha))
                return null;

            var BuscaUsuario = await _userService.GetByUsuario(userlogin.Usuario);
            if (BuscaUsuario is null)
                return null;

            //verifica se as senhas não são iguais
            if(!BCrypt.Net.BCrypt.Verify(userlogin.Senha, BuscaUsuario.Senha))
                return null;

            var tokeHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(Setting.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userlogin.Usuario)
                }),
                //tempo de duração do token
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokeHandler.CreateToken(tokenDescriptor); //gera o token
            //passa o token para o usuario

            userlogin.Id = BuscaUsuario.Id;
            userlogin.Nome = BuscaUsuario.Nome;
            userlogin.Foto = BuscaUsuario.Foto is null ? FotoDefault:BuscaUsuario.Foto;
            userlogin.Token = "Bearer "+ tokeHandler.WriteToken(token).ToString();
            userlogin.Senha = "";

            return userlogin;
        }
    }
}
