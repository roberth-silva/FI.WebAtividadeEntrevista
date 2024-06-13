using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Controllers
{
    public class ItemsController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Items
        public ActionResult Index(long idCliente)
        {
            BoBeneficiario bo = new BoBeneficiario();
            var list = bo.Consultar(idCliente);
            return View(list);
        }

        [HttpGet]
        // Action to return a partial view of the items table
        public PartialViewResult ItemsTable(long idCliente)
        {
            BoBeneficiario bo = new BoBeneficiario();
            var list = bo.Consultar(idCliente);

            return PartialView("_ItemsTable",list);
        }

        // Action to get an item by id
        //public JsonResult GetItem(int id)
        //{
        //    var item = db.Items.Find(id);
        //    if (item == null)
        //    {
        //        return Json(new { success = false, message = "Item not found" }, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new { success = true, data = item }, JsonRequestBehavior.AllowGet);
        //}

        // Action to save the item
        //[HttpPost]
        //public JsonResult SaveItem(Item item)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (item.Id == 0)
        //        {
        //            db.Items.Add(item);
        //        }
        //        else
        //        {
        //            db.Entry(item).State = EntityState.Modified;
        //        }
        //        db.SaveChanges();
        //        return Json(new { success = true, message = "Item saved successfully" });
        //    }
        //    return Json(new { success = false, message = "Error while saving item" });
        //}
    }

}