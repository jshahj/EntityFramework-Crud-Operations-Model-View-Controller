using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCCrudEntityFramework.Models
{
    public class ProductModel
    {
        [Key]
        public int ProdId { get; set; }

        [Required(ErrorMessage ="Product Name is required.")]
        [DisplayName("Product Name")]
        public string ProdName { get; set; }

        [DisplayName("Product Image")]
        public string ProdImage { get; set; }

        public HttpPostedFileBase ProdImageFile { get; set; }
    }
}