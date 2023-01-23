using System.IdentityModel.Tokens.Jwt;
using ChapterAPI.Controllers;
using ChapterAPI.Interfaces;
using ChapterAPI.Models;
using ChapterAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TesteIntegracao
{

public class LoginControllerTeste
{
    [Fact]
    public void LoginController_Retornar_Usuario_Invalido()
    {
        var repositoryEspelhado = new Mock<IUsuarioRepository>();

        repositoryEspelhado.Setup( x=> x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns((Usuario)null);

        var controller = new LoginController(repositoryEspelhado.Object);

        LoginViewModel dadosUsuario = new LoginViewModel();
        dadosUsuario.email = "batata@email.com";
        dadosUsuario.senha = "batata";

        var resultado = controller.Login(dadosUsuario);

        Assert.IsType<UnauthorizedObjectResult>(resultado);

    }

    [Fact]

    public void Logincontroller_Retornar_Token(){
    
    Usuario usuarioRetornado = new Usuario();
    usuarioRetornado.Email = "email@email.com";
    usuarioRetornado.Senha = "12345";
    usuarioRetornado.Tipo = "0";  
    usuarioRetornado.id = 1;  

    var repositoryEspelhado = new Mock<IUsuarioRepository>();
    repositoryEspelhado.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(usuarioRetornado);   

    LoginViewModel dadosUsuario = new LoginViewModel();
    dadosUsuario.email = "com";
    dadosUsuario.senha = "batata";

    var controller = new LoginController(repositoryEspelhado.Object);
    string issuerValido = "chapter.webapi";


    OkObjectResult resultado = (OkObjectResult)controller.Login(dadosUsuario);

    string tokenString = resultado.Value.ToString().Split(' ')[3];

    var jwtHandler = new JwtSecurityTokenHandler();

    var tokenJwt = jwtHandler.ReadToken(tokenString);


    Assert.Equal(issuerValido, tokenJwt.Issuer);

    }
}
}