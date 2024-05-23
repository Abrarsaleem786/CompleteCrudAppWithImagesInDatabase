using CompleteCrudAppWithImagesInDatabase.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompleteCrudAppWithImagesInDatabase.Controllers
{
    public class HomeController : Controller
    {
        CrudAppWithImagesEntities db = new CrudAppWithImagesEntities();
        // GET: Home
        public ActionResult Index()
        {
            var data = db.employes.ToList();
            return View(data);
        }
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(employe e)
        {
            if (ModelState.IsValid == true) {
                string fileName = Path.GetFileNameWithoutExtension(e.imageFile.FileName);
                string extension = Path.GetExtension(e.imageFile.FileName);
                HttpPostedFileBase postedFile = e.imageFile;
                double length = postedFile.ContentLength;


                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
                {

                    if (length <= 1000000)
                    {
                        fileName = fileName + extension;
                        e.image_path = "~/images/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                        e.imageFile.SaveAs(fileName);
                        db.employes.Add(e);
                        int a = db.SaveChanges();
                        if (a > 0) {

                            TempData["insertMessage"] = "Data Inserted!!";

                            return RedirectToAction("Index", "Home");

                        }

                        else
                        {
                            TempData["insertMessage"] = "data not inserted";
                        }


                    }
                    else
                    {

                        TempData["sizeMessage"] = "image size must be equal or less than 1mb";
                    }

                }
                else
                {
                    TempData["extensionMessage"] = "file not supported!!";
                }

            }

            return View();
    }

    public ActionResult Edit(int id)
        {
            var row = db.employes.Where(model => model.id == id).FirstOrDefault();
            Session["image"] = row.image_path;
            return View(row);
        }

        [HttpPost]
        public ActionResult Edit(employe e)
        {
            if (ModelState.IsValid == true)
            {

                if (e.imageFile != null) {

                    string fileName = Path.GetFileNameWithoutExtension(e.imageFile.FileName);
                    string extension = Path.GetExtension(e.imageFile.FileName);
                    HttpPostedFileBase postedFile = e.imageFile;
                    int length = postedFile.ContentLength;


                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
                    {

                        if (length <= 1000000)
                        {
                            fileName = fileName + extension;
                            e.image_path = "~/images/" + fileName;
                            fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                            e.imageFile.SaveAs(fileName);
                            db.Entry(e).State = System.Data.Entity.EntityState.Modified;
                            int a = db.SaveChanges();
                            if (a > 0)
                            {
                                string image = Request.MapPath(Session["image"].ToString());
                                if (System.IO.File.Exists(image))
                                {
                                    System.IO.File.Delete(image);

                                }

                                TempData["UpdateMessage"] = "Data Updated!!";
                                return RedirectToAction("Index", "Home");

                            }

                            else
                            {
                                TempData["UpdateMessage"] = "Data Updation failed !!";
                            }

                           

                        }
                        else
                        {
                            TempData["sizeMessage"] = "image size must be equal or less than 1mb!!";
                        }


                    }
                   
                    else
                    {
                        TempData["extensionMessage"] = "file not supported!!";
                    }
                }
                else
                {
                    e.image_path = Session["image"].ToString();
                    db.Entry(e).State = System.Data.Entity.EntityState.Modified;
                    int a = db.SaveChanges();
                    if (a > 0)
                    {

                        TempData["UpdateMessage"] = "Data Updated!!";
                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        TempData["UpdateMessage"] = "Data Updation failed !!";
                    }

                }
            }
           
            return View();
        }


        public ActionResult Delete(int id) {
            if (id > 0)
            {
                var data = db.employes.Where(model => model.id == id).FirstOrDefault();
               
                if (data != null) {

                    db.Entry(data).State = System.Data.Entity.EntityState.Deleted;
                    string image = Request.MapPath(data.image_path);
                    if(System.IO.File.Exists(image)){
                        System.IO.File.Delete(image);

                    }
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                
                }



            }
            return RedirectToAction("Index", "Home");
        
        }
        public ActionResult Details(int id)
        {
            var data = db.employes.Where(model =>model.id == id).FirstOrDefault();
            Session["image2"] = data.image_path;
            return View(data);
        }
         
    
    }
}