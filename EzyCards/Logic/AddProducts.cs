using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EzyCards.Models;

namespace EzyCards.Logic
{
  public class AddProducts
  {
    public bool AddProduct(string ProductName, string ProductDesc, string ProductPrice, string ProductCategory,string ProductFaceBookPage, string ProductImagePath)
    {
      var myProduct = new Product();
      myProduct.ProductName = ProductName;
      myProduct.Description = ProductDesc;
      myProduct.UnitPrice = Convert.ToDouble(ProductPrice);
      myProduct.ImagePath = ProductImagePath;
      myProduct.CategoryID = Convert.ToInt32(ProductCategory);
	  using (var _db = new ProductContext())
	  {
		  var myItem = (from c in _db.FaceBookPages where c.FaceBookPageId == ProductFaceBookPage select c).FirstOrDefault() ;
		  if (myItem != null)
			  myProduct.faceBookPage = myItem;
	  }

      using (ProductContext _db = new ProductContext())
      {
        // Add product to DB.
        _db.Products.Add(myProduct);
        _db.SaveChanges();
      }
      // Success.
      return true;
    }
  }
}