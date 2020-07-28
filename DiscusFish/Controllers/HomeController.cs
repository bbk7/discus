using DiscusFish.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DiscusFish.Controllers
{
    public class HomeController : Controller
    {


        private DiscusEntities db = new DiscusEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Gallery()
        {
            return View();
        }


        public ActionResult About()
        {
            return View();
        }

        public ActionResult Strains(string search, string discus)
        {
            {
                List<string> nameList = new List<string>();
                var nameQuery = from g in db.Discus
                                orderby g.name
                                select g.name;
                nameList.AddRange(nameQuery.Distinct());
                ViewBag.discus = new SelectList(nameList);
                //Linq query to get all records/rows from movies table. 
                var allname = from m in db.Discus
                              select m;
                if (!string.IsNullOrEmpty(discus))
                {
                    allname = allname.Where(x => x.name == discus);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    allname = allname.Where(x => x.name.Contains(search));
                }
                //return all movies to the index view. 
                return View(allname);
            }
        }

        public ActionResult Database(string searchString, string discusName)
        {
            {
                List<string> nameList = new List<string>();
                var nameQuery = from g in db.Discus
                                orderby g.name
                                select g.name;
                nameList.AddRange(nameQuery.Distinct());
                ViewBag.discusName = new SelectList(nameList);
                //Linq query to get all records/rows from movies table. 
                var allname = from m in db.Discus
                              select m;
                if (!string.IsNullOrEmpty(discusName))
                {
                    allname = allname.Where(x => x.name == discusName);
                }
                if (!string.IsNullOrEmpty(searchString))
                {
                    allname = allname.Where(x => x.name.Contains(searchString));
                }
                //return all movies to the index view. 
                return View(allname);
            }
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "name, image, video, ImageFile  ")] Discus strains)
        {

            if (strains.image == null)
            {
                strains.image = "default1.jpg";
            }

            if (ModelState.IsValid)
            {
                string imagename = Path.GetFileNameWithoutExtension(strains.ImageFile.FileName);
                string extension = Path.GetExtension(strains.ImageFile.FileName);
                imagename = imagename + extension;
                strains.image = imagename;

                imagename = Path.Combine(Server.MapPath("~/Images/"), imagename);
                strains.ImageFile.SaveAs(imagename);

                db.Discus.Add(strains);
                db.SaveChanges();
                return RedirectToAction("Database");
            }


            return View(strains);
        }



        [HttpGet]
        public ActionResult Edit(int id)
        {
            Discus strains = db.Discus.Find(id);
            return View(strains);
        }

        [HttpPost]
        public ActionResult Edit(Discus strains)
        {

            if (strains.image == null)
            {
                strains.image = "default1.jpg";
            }

            if (ModelState.IsValid)
            {
                string imagename = Path.GetFileNameWithoutExtension(strains.ImageFile.FileName);
                string extension = Path.GetExtension(strains.ImageFile.FileName);
                imagename = imagename + extension;
                strains.image = imagename;


                imagename = Path.Combine(Server.MapPath("~/Images/"), imagename);
                strains.ImageFile.SaveAs(imagename);



                db.Entry(strains).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Database");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discus strains = db.Discus.Find(id);
            if (strains == null)
            {
                return HttpNotFound();
            }
            return View(strains);
        }


        [HttpGet]
        public ActionResult Video(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discus strains = db.Discus.Find(id);
            if (strains == null)
            {
                return HttpNotFound();
            }
            return View(strains);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Discus strains = db.Discus.Find(id);
            return View(strains);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            Discus strains = db.Discus.Find(id);
            db.Discus.Remove(strains);
            db.SaveChanges();
            return RedirectToAction("Database");
        }



    }
}