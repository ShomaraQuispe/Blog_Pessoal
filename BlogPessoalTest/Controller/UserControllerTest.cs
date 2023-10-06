using blogpessoal.Model;
using BlogPessoalTest.Factory;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit.Extensions.Ordering;

namespace BlogPessoalTest.Controller
{
    public class UserControllerTest : IClassFixture<WebAppFactory>
    {
        protected readonly WebAppFactory _factory;
        protected HttpClient _client;

        private readonly dynamic token;
        private string Id { get; set; } = string.Empty;

        public UserControllerTest(WebAppFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();

            token = GetToken();
        }
        private static dynamic GetToken()
        {
            dynamic data = new ExpandoObject();
            data.sub = "root@root.com";
            return data;
        }

        [Fact, Order(1)]
        public async Task DeveCriarUmUsuario()
        {
            //cria um usuario
            var novoUsuario = new Dictionary<string, string>()
            {
                { "nome", "Shomara"},
                {"usuario", "shomara@gmail.com" },
                { "senha", "123445678"},
                {"foto","" }
            };
            //transforma os dados no formato Json
            var usuarioJson = JsonConvert.SerializeObject(novoUsuario);
            //guarda o Objeto usuarioJson e criará o Corpo da Requisição
            var corpoRequisicao = new StringContent(usuarioJson, Encoding.UTF8, "application/json");

            var resposta = await _client.PostAsync("/usuarios/cadastrar", corpoRequisicao);

            resposta.EnsureSuccessStatusCode();

            resposta.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact, Order(2)]
        public async Task DeveDarErroEmail()
        {
            var novoUsuario = new Dictionary<string, string>()
        {
            { "nome", "Shomara"},
                {"usuario", "shomaragmail.com" },
                { "senha", "123445678"},
                {"foto","" }
        };

            var usuarioJson = JsonConvert.SerializeObject(novoUsuario);
            var corpoRequisicao = new StringContent(usuarioJson, Encoding.UTF8, "application/json");

            var resposta = await _client.PostAsync("/usuarios/cadastrar", corpoRequisicao);

            resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact, Order(3)]
        public async Task NaoDeveCriarUsuarioDuplicado()
        {
            var novoUsuario = new Dictionary<string, string>()
            {
                { "nome", "Quispe"},
                {"usuario", "quispe@gmail.com" },
                { "senha", "123445678"},
                {"foto","" }
            };

            var usuarioJson = JsonConvert.SerializeObject(novoUsuario);

            var corpoRequisicao = new StringContent(usuarioJson, Encoding.UTF8, "application/json");

            await _client.PostAsync("/usuarios/cadastrar", corpoRequisicao);

            //Enviar a segunda vez

            var resposta = await _client.PostAsync("/usuarios/cadastrar", corpoRequisicao);

            resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact, Order(4)]
        public async Task DeveListarTodosOsUsuarios()
        {
            _client.SetFakeBearerToken((object)token);

            var resposta = await _client.GetAsync("/usuarios/all");

            resposta.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact, Order(5)]
        public async Task DeveAtualizarUmUsuario()
        {
            var novoUsuario = new Dictionary<string, string>()
        {
            { "nome", "Flores"},
                {"usuario", "flores@gmail.com" },
                { "senha", "123445678"},
                {"foto","" }
        };

            var usuarioJson = JsonConvert.SerializeObject(novoUsuario);
            var corpoRequisicao = new StringContent(usuarioJson, Encoding.UTF8, "application/json");

            var resposta = await _client.PostAsync("/usuarios/cadastrar", corpoRequisicao);

            var corpoRespostaPost = await resposta.Content.ReadFromJsonAsync<User>();

            if (corpoRespostaPost != null)
                Id = corpoRespostaPost.Id.ToString();

            var usuarioAtualizado = new Dictionary<string, string>()
        {
            { "id", Id },
             { "nome", "Flores2"},
                {"usuario", "flores@gmail.com" },
                { "senha", "123445678"},
                {"foto","" }
        };

            var usuarioJsonAtualizado = JsonConvert.SerializeObject(usuarioAtualizado);
            var corpoRequisicaoAtualizado = new StringContent(usuarioJsonAtualizado, Encoding.UTF8, "application/json");

            //para fazer a atualizar é necessário ter o token
            _client.SetFakeBearerToken((object)token);

            var respostaPut = await _client.PutAsync("/usuarios/atualizar", corpoRequisicaoAtualizado);

            respostaPut.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        //DeveListarUmUsuario()
        //DeveAutenticarUmUsuario()
        [Fact, Order(6)]
        public async Task DeveListarUmUsuario()
        {
            //resposta.EnsureSuccessStatusCode();
            _client.SetFakeBearerToken((object)token);

            Id = "1";

            var resposta = await _client.GetAsync($"/usuarios/{Id}");

            resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        //DeveAutenticarUmUsuario()

        [Fact, Order(7)]
        public async Task DeveAutenticarUsuario()
        {
            var novoUsuario = new Dictionary<string, string>()
            {
                {"usuario", "shomara@gmail.com" },
                { "senha", "123445678"}
            };

            var usuarioJson = JsonConvert.SerializeObject(novoUsuario);
            var corpoRequisicao = new StringContent(usuarioJson, Encoding.UTF8, "application/json");

            var resposta = await _client.PostAsync("/usuarios/logar", corpoRequisicao);

            resposta.StatusCode.Should().Be(HttpStatusCode.OK);
        }

    }
}