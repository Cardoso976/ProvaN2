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
        private const int _quantMaxLinhasPorPagina = 5;


        public ActionResult Index()
        {
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = CadastroModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
            var quant = CadastroModel.RecuperarQuantidade();

            var difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuantPaginas;

            return View(lista);
        }        

        [HttpPost]        
        public JsonResult CadastroPagina(int pagina, int tamPag)
        {
            var lista = CadastroModel.RecuperarLista(pagina, tamPag);

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
        public JsonResult BuscarPessoa(string Busca)
        {
            return Json(CadastroModel.BuscarPessoa(Busca));
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