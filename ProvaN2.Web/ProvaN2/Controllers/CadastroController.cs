using ProvaN2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProvaN2.Controllers
{
    public class CadastroController : Controller
    {
        public ActionResult Index()
        {
            return View(CadastroModel.RecuperarLista());
        }        

        [HttpPost]        
        public JsonResult CadastroPagina(int pagina, int tamPag)
        {
            var lista = CadastroModel.RecuperarLista();

            return Json(lista);
        }

        [HttpPost]
        public JsonResult RecuperarPessoa(int id)
        {
            return Json(CadastroModel.RecuperarPeloId(id));
        }

        [HttpPost] 
        public JsonResult ExcluirPessoa(int id)
        {
            return Json(CadastroModel.ExcluirPeloId(id));
        }

        [HttpPost]
        public JsonResult BuscarPessoa(string filtro)
        {
            return Json(CadastroModel.BuscarPessoa(filtro));
        }

        [HttpPost]        
        public JsonResult Salvar(CadastroModel model)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    var id = model.Salvar();
                    if (id > 0)
                    {
                        idSalvo = id.ToString();
                    }
                    else
                    {
                        resultado = "ERRO";
                    }
                }
                catch (Exception ex)
                {
                    resultado = "ERRO";
                }
            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }
    }
}