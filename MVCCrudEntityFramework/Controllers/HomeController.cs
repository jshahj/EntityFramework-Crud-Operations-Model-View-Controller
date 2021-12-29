using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCCrudEntityFramework.EntityModel;
using MVCCrudEntityFramework.Models;

namespace MVCCrudEntityFramework.Controllers
{
    public class HomeController : Controller
    {
        dbDataEntities _db = new dbDataEntities();

        public ActionResult Index(int id = 0)
        {
            if (id > 0)
            {
                 
                var ProductDetails = _db.products.Where(m => m.ProdId == id).FirstOrDefault();


                ProductModel proModel = new ProductModel();
                proModel.ProdId = ProductDetails.ProdId;
                proModel.ProdName = ProductDetails.ProdName;
                proModel.ProdImage = ProductDetails.ProdImage;

                
                return View(proModel);

            }
            else
            {
              
                return View();
            }
        }

        [HttpPost]
        public ActionResult Index(ProductModel prodModel, int? id)
        {
            product prodEntity = new product();


            if (id > 0)
            {

                if (prodModel.ProdImageFile != null)
                {


                    prodEntity.ProdId = prodModel.ProdId;
                    prodEntity.ProdName = prodModel.ProdName;

                    //get extension of file
                    var extensions = Path.GetExtension(prodModel.ProdImageFile.FileName);
                    //create new guid
                    Guid guidValue = Guid.NewGuid();

                    //guid  + extension became new filename
                    var filename = guidValue + extensions;
                    filename = filename.Replace("-", "");
                    prodEntity.ProdImage = "~/ProductImage/" + filename;

                    filename = Path.Combine(Server.MapPath("~/ProductImage/"), filename);

                    //assign file name to product entity
                    //upload file to folder
                    prodModel.ProdImageFile.SaveAs(filename);



                    _db.Entry(prodEntity).State = System.Data.Entity.EntityState.Modified;
                    _db.SaveChanges();
                  
                    return RedirectToAction("ProductList");

                }
                else
                {
                    prodEntity.ProdId = prodModel.ProdId;
                    prodEntity.ProdImage = prodModel.ProdImage;
                    prodEntity.ProdName = prodModel.ProdName;
                    _db.Entry(prodEntity).State = System.Data.Entity.EntityState.Modified;
                    _db.SaveChanges();
                 
                    return RedirectToAction("ProductList");
                }
            }
            else
            {
                prodEntity.ProdName = prodModel.ProdName;

                //get extension of file
                var extensions = Path.GetExtension(prodModel.ProdImageFile.FileName);
                //create new guid
                Guid guidValue = Guid.NewGuid();

                //guid  + extension became new filename
                var filename = guidValue + extensions;
                filename = filename.Replace("-", "");
                prodEntity.ProdImage = "~/ProductImage/" + filename;

                filename = Path.Combine(Server.MapPath("~/ProductImage/"), filename);

                //assign file name to product entity
                //upload file to folder
                prodModel.ProdImageFile.SaveAs(filename);

                _db.products.Add(prodEntity);
                _db.SaveChanges();
                ModelState.Clear();
                ViewBag.AddMessage = "Product Added Succefully";

                return View();
            }
        }


        public ActionResult ProductList()
        {
            List<ProductModel> proModelList = new List<ProductModel>();
            var data = _db.products.ToList();
            foreach (var proData in data)
            {
                proModelList.Add(new ProductModel
                {
                    ProdId = proData.ProdId,
                    ProdName=proData.ProdName,
                    ProdImage=proData.ProdImage,

                });
            }
            return View(proModelList);

        }

        public ActionResult ProductDelete(int id)
        {
            if (id > 0)
            {
                var prodData = _db.products.Where(m => m.ProdId == id).FirstOrDefault();
                _db.Entry(prodData).State = System.Data.Entity.EntityState.Deleted;
                _db.SaveChanges();
                return RedirectToAction("ProductList");

            }
            return View();
        }


    }
}