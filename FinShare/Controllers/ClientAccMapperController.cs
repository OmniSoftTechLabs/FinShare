using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinShare.Controllers
{
    public class ClientAccMapperController : Controller
    {
        // GET: ClientAccMapper
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Index(Guid clientId)
        {
            ClientDAL objdal = new ClientDAL();
            List<ClientAccMapper> clientAccMapper = new List<ClientAccMapper>();
            try
            {
                clientAccMapper = objdal.GetAcountNumberByClinetId(clientId).ToList();

            }
            catch (Exception ex)
            {
                View(clientAccMapper);
            }
            return View(clientAccMapper);
        }

        // GET: ClientAccMapper/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClientAccMapper/Create
        public ActionResult Create(Guid clientId)
        {
            ClientDAL objdal = new ClientDAL();

            ClientAccMapper objMapper = new ClientAccMapper();
            Client objClient = objdal.GetSingle(clientId);
            objMapper.ClientName = objClient.ClientName;
            objMapper.ClientID = objClient.ClientID;

            return View(objMapper);
        }

        // POST: ClientAccMapper/Create
        [HttpPost]
        public ActionResult Create(ClientAccMapper objAccMapper)
        {
            try
            {
                // TODO: Add insert logic here
                ClientDAL objdal = new ClientDAL();
                string str = objdal.InsertAccountNumber(objAccMapper);
                if (str == "success")
                    return RedirectToAction("Index", new { clientId = objAccMapper.ClientID });
                else
                    return View(objAccMapper);

            }
            catch
            {
                return View(objAccMapper);
            }
        }

        // GET: ClientAccMapper/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClientAccMapper/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ClientAccMapper/Delete/5
        public ActionResult Delete(int id, Guid clientId)
        {

            ClientDAL objdal = new ClientDAL();
            int num = objdal.DeleteAccountNumber(id);
            return RedirectToAction("Index", new { clientId = clientId });

        }

        // POST: ClientAccMapper/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
