using BILWeb.View_Product;
using System.Web.Mvc;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.ProductOrders
{
    [RoleActionFilter(Message = "ProductOrders/ProductOrders")]
    public class ProductOrdersController : BaseController<View_Product_Model>
    {
        private IView_ProductService productService;

        public ProductOrdersController()
        {
            productService = (IView_ProductService)ServiceFactory.CreateObject("View_Product.View_Product_Func");
            baseservice = productService;
        }

        public ActionResult Index()
        {
            return View("GetModelList");
        }

    }
}