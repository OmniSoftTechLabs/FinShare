using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using PagedList;
using PagedList.Mvc;
using System.Data;
using System.Data.SqlClient;
namespace FinShare.Controllers
{
    public class LeadGeneratorController : Controller
    {
        //
        // GET: /LeadGenerator/
        public ActionResult Index(string search, int? page)
        {
            LeadGeneratorDAL objdal = new LeadGeneratorDAL();
            List<LeadGenerator> leadgenerator = new List<LeadGenerator>();
            try
            {
                if (!string.IsNullOrEmpty(search))
                    leadgenerator = objdal.GetAll().Where(x => x.LeadGeneratorName.StartsWith(search, StringComparison.OrdinalIgnoreCase)).ToList();
                else
                    leadgenerator = objdal.GetAll().ToList();
            }
            catch (Exception ex)
            {
                return View(leadgenerator);
            }
            return View(leadgenerator.ToPagedList(page ?? 1, 10));
        }
        /// <summary>
        /// Add new Lead Generator
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Create")]
        public ActionResult Create_Get()
        {
            return View();
        }

        /// <summary>
        /// Save Lead Generator
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Create")]
        public ActionResult Create_Post(LeadGenerator objen)
        {
            //Employee employee = new Employee();
            //TryUpdateModel(employee);// this method handels exception
            if (ModelState.IsValid)
            {
                LeadGeneratorDAL objdal = new LeadGeneratorDAL();
                //Bussiness layer logic to save data go here
                try
                {
                    objdal.InsertUpdate(objen, 0);
                }
                catch (SqlException se)
                {
                    if (se.Number == 2627)
                    {
                        ModelState.AddModelError("LeadGeneratorName", objen.LeadGeneratorName + " already exists!");
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
                return View();
            }
        }

        /// <summary>
        /// Get Lead Generator Data for Edit
        /// </summary>
        /// <param name="lgid"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Edit")]
        public ActionResult Edit_Get(Guid lgid)
        {

            LeadGeneratorDAL objdal = new LeadGeneratorDAL();
            LeadGenerator entobj = objdal.GetSingle(lgid);
            return View(entobj);
        }

        /// <summary>
        /// Update Lead Generator data
        /// </summary>
        /// <param name="lg"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Edit")]
        public ActionResult Edit_Post(LeadGenerator objen)
        {

            if (ModelState.IsValid)
            {
                LeadGeneratorDAL objdal = new LeadGeneratorDAL();
                //Bussiness layer logic to save data go here

                try
                {
                    objdal.InsertUpdate(objen, 1);
                }
                catch (SqlException se)
                {
                    if (se.Number == 2627)
                    {
                        ModelState.AddModelError("LeadGeneratorName", objen.LeadGeneratorName + " already exists!");
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
        public ActionResult Delete(Guid lgid)
        {

            LeadGeneratorDAL objdal = new LeadGeneratorDAL();
            try
            {
                int i = objdal.Delete(lgid);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }
            return RedirectToAction("Index");
        }

    }
}