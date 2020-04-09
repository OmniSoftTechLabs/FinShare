using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using System.Threading;
using System.Globalization;
using System.Text.RegularExpressions;
using PagedList;
using PagedList.Mvc;
using System.Data.SqlClient;
namespace FinShare.Controllers
{
    public class ClientController : Controller
    {

        //
        // GET: /Client/
        public ActionResult Index(string search, int? page)
        {

            ClientDAL objdal = new ClientDAL();
            List<Client> client = new List<Client>();
            try
            {
                if (!string.IsNullOrEmpty(search))
                    client = objdal.GetAll().Where(x => x.ClientName.StartsWith(search, StringComparison.OrdinalIgnoreCase)).ToList();
                else
                    client = objdal.GetAll().ToList();
            }
            catch( Exception ex)
            {
                View(client);
            }
            return View(client.ToPagedList(page ?? 1, 10));

        }

        [HttpGet]
        [ActionName("Create")]
        public ActionResult Create_Get()
        {
            LeadGeneratorDAL objdal = new LeadGeneratorDAL();
            var model = new Client()
            {
                LGlist = objdal.GetAll().ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ActionName("Create")]
        public ActionResult Create_Post(Client objen)
        {
            if (ModelState.IsValid)
            {
                ClientDAL objdal = new ClientDAL();

                //if (Guid.Empty != objen.LG1 && Guid.Empty != objen.LG2)
                //{
                //    if (objen.LG1 == objen.LG2)
                //    {
                //        ModelState.AddModelError("LG2", "Both the Lead Generator can not be same!!");

                //        LeadGeneratorDAL objdallg = new LeadGeneratorDAL();
                //        objen.LGlist = objdallg.GetAll().ToList();
                //        return View("Create", objen);
                //    }
                //}

                bool errflg = false;
                if (Guid.Empty != objen.LG1 && Guid.Empty != objen.LG2)
                {
                    if (objen.LG1 == objen.LG2)
                    {
                        ModelState.AddModelError("LG2", "Both the Lead Generator can not be same!!");
                        errflg = true;
                    }
                    if (objen.LG1Share.HasValue && objen.LG2Share.HasValue)
                    {
                        if ((objen.LG1Share + objen.LG2Share) > 100)
                        {
                            ModelState.AddModelError("LG2Share", "Share of both the Lead Generator can not be more than 100!!");
                            errflg = true;
                        }
                    }
                    if (errflg == true)
                    {
                        LeadGeneratorDAL objdallg = new LeadGeneratorDAL();
                        objen.LGlist = objdallg.GetAll().ToList();
                        return View("Edit", objen);
                    }
                }

                try
                {
                    objdal.InsertUpdate(objen, 0);
                }
                catch (SqlException se)
                {
                    if (se.Number == 2627)
                    {
                        LeadGeneratorDAL objdallg = new LeadGeneratorDAL();
                        objen.LGlist = objdallg.GetAll().ToList();
                        ModelState.AddModelError("ClientName", objen.ClientName + " already exists!");
                        return View("Create", objen);
                    }
                }
                catch (Exception ex)
                {

                }

                return RedirectToAction("Index");
                
            }
            else
            {
                return View(objen);
            }
        }

        [HttpGet]
        [ActionName("Edit")]
        public ActionResult Edit_Get(Guid clientid)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
            ClientDAL objdal = new ClientDAL();
            Client entobj = objdal.GetSingle(clientid);
            LeadGeneratorDAL objdallg = new LeadGeneratorDAL();
            ClientAccMapper listAccontNumber = new ClientAccMapper();
            var model = new Client();
            model = entobj;
            model.LGlist = objdallg.GetAll().ToList();
            model.AccountNumberList = objdal.GetAcountNumberByClinetId(clientid);
            return View(model);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult Edit_Post(Client objen)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

            if (ModelState.IsValid)
            {
                ClientDAL objdal = new ClientDAL();
                bool errflg = false;
                if (Guid.Empty != objen.LG1 && Guid.Empty != objen.LG2)
                {
                    if (objen.LG1 == objen.LG2)
                    {
                        ModelState.AddModelError("LG2", "Both the Lead Generator can not be same!!");
                        errflg = true;
                    }
                    if (objen.LG1Share.HasValue && objen.LG2Share.HasValue)
                    {
                        if ((objen.LG1Share + objen.LG2Share) > 100)
                        {
                            ModelState.AddModelError("LG2Share", "Share of both the Lead Generator can not be more than 100!!");
                            errflg = true;
                        }
                    }
                    if (errflg == true)
                    {
                        LeadGeneratorDAL objdallg = new LeadGeneratorDAL();
                        objen.LGlist = objdallg.GetAll().ToList();
                        return View("Edit", objen);
                    }
                }

                try
                {
                    objdal.InsertUpdate(objen, 1);
                }
                catch (SqlException se)
                {
                    if (se.Number == 2627)
                    {
                        LeadGeneratorDAL objdallg = new LeadGeneratorDAL();
                        objen.LGlist = objdallg.GetAll().ToList();
                        ModelState.AddModelError("ClientName", objen.ClientName + " already exists!");
                        return View("Edit", objen);
                    }
                }
                catch (Exception ex)
                {

                }

                return RedirectToAction("Index");
            }
            else
            {
                return View(objen);
            }
        }

        [HttpGet]
        public ActionResult Delete(Guid clientid)
        {

            ClientDAL objdal = new ClientDAL();
            int i = objdal.Delete(clientid);
            return RedirectToAction("Index");
        }

    }
}