using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.Util;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using System.Reflection;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        private string AtualCpf;
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            Util util = new Util();
            
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                if (bo.VerificarExistencia(model.CPF))
                {
                    List<string> erros = new List<string>{"Este CPF já existe na base de dados"};

                    Response.StatusCode = 420;//422 Unprocessable Content
                    return Json(string.Join(Environment.NewLine, erros));
                }

                if (!Util.ValidarCPF(model.CPF))
                {
                    List<string> erros = new List<string> { "Este CPF não é válido" };

                    Response.StatusCode = 422;//422 Unprocessable Content
                    return Json(string.Join(Environment.NewLine, erros));
                }

                model.Id = bo.Incluir(new Cliente()
                {                    
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });

           
                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public ActionResult Alterar(ClienteModel model)
        {            

            BoCliente bo = new BoCliente();
       
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 200;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                var x = model.CPF_Aux;
                if (model.CPF_Aux != model.CPF && bo.VerificarExistencia(model.CPF_Aux))
                {
                    return Json(new { Result = "ERROR", Message = "CPF existente na base de dados" });
                }

                if (!Util.ValidarCPF(model.CPF))
                {
                    List<string> erros = new List<string> { "Este CPF não é válido" };

                    Response.StatusCode = 422;
                    return Json(string.Join(Environment.NewLine, erros));
                }

                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });
                               
                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                List<Beneficiario> beneficiarios = new BoBeneficiario().Consultar(id);
                List<BeneficiarioModel> resultBeneficiarios = new List<BeneficiarioModel>();

                foreach (var item in beneficiarios)
                {
                    resultBeneficiarios.Add(new BeneficiarioModel
                    {
                        Id = item.Id,
                        Nome = item.Nome,
                        CPF = item.CPF,
                        IdCliente = item.IdCliente,
                    });
                }

                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF= cliente.CPF,
                    CPF_Aux = cliente.CPF,
                    Beneficiarios = resultBeneficiarios
                };

                ViewBag.CPF = model.CPF;
            
            }

            return View(model);
        }       

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult IncluirBeneficiario(BeneficiarioModel model)
        {

            BoBeneficiario bo = new BoBeneficiario();
            
            if (!Util.ValidarCPF(model.CPF))
            {
                List<string> erros = new List<string> { "Este CPF não é válido" };

                Response.StatusCode = 422;
                return Json(string.Join(Environment.NewLine, erros));
            }

            model.Id = bo.Incluir(new Beneficiario()
            {                    
                Nome = model.Nome,
                CPF = model.CPF.Replace("-","").Replace(".",""),
                IdCliente = model.IdCliente
            });


            return Json("Cadastro efetuado com sucesso");
            

        }

        [HttpPut]
        public ActionResult AlterarBeneficiario(BeneficiarioModel model)
        {

            BoBeneficiario bo = new BoBeneficiario();
            
            if (!Util.ValidarCPF(model.CPF))
            {
                List<string> erros = new List<string> { "Este CPF não é válido" };

                Response.StatusCode = 422;
                return Json(string.Join(Environment.NewLine, erros));
            }

            bo.Alterar(new Beneficiario()
            {
                Id = model.Id,
                Nome = model.Nome,
                CPF = model.CPF.Replace("-", "").Replace(".", ""),
                IdCliente = model.IdCliente
            });

            return Json("Beneficiário alterado com sucesso");           

        }

        [HttpDelete]
        public ActionResult DeletarBeneficiario(long id, long idcliente)
        {

            BoBeneficiario bo = new BoBeneficiario();

            bo.Excluir(id);

            return Json("Beneficiario deletado com sucesso");            

        }

        public PartialViewResult ItemsTable(long idCliente)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(idCliente);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                List<Beneficiario> beneficiarios = new BoBeneficiario().Consultar(idCliente);
                List<BeneficiarioModel> resultBeneficiarios = new List<BeneficiarioModel>();

                foreach (var item in beneficiarios)
                {
                    resultBeneficiarios.Add(new BeneficiarioModel
                    {
                        Id = item.Id,
                        Nome = item.Nome,
                        CPF = item.CPF,
                        IdCliente = item.IdCliente,
                    });
                }

                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF,
                    CPF_Aux = cliente.CPF,
                    Beneficiarios = resultBeneficiarios
                };
            }
            return PartialView("_ItemsTable", model);
        }

    }
}