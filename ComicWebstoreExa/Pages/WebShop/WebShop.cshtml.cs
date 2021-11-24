using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using DataSource.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ComicWebstoreExa.Pages.WebShop
{
    public class WebShopModel : PageModel
    {

        [BindProperty]
        public int CustomerID { get; set; }

        [BindProperty]
        public string SearchTerm { get; set; }
        public CustomerDTO thisCustomer { get; set; }
        public int cartid { get; set; }
        [BindProperty]
        public int ProductID { get; set; }
        public ILoggedIn _login { get; }
        public IDataAccess _dataAccess { get; }
        public string FirstName { get; set; }

        public WebShopModel(IDataAccess dataAccess, ILoggedIn login)
        {
            _login = login;
            _dataAccess = dataAccess;
            SetCustomer();
            LoadProducts();
        }


        public List<ProductDTO> ProductList { get; set; }
        



        public IActionResult OnPostSearchName()
        {

            ProductList = _dataAccess.SearchBarName(SearchTerm).ToList();
            return Page();

        }

        public List<ProductDTO> LoadProducts()
        {

            return _dataAccess.GetAllProducts().ToList();
        }
        
        public IActionResult OnPostSortThisPrice()
        {
            ProductList = _dataAccess.SortListProPrice().ToList();
            return Page();
        }
        public IActionResult OnPostSortThisName()
        {
            ProductList = _dataAccess.SortListProName().ToList();
            return Page();
        }


        public CustomerDTO SetCustomer()
        {
            return thisCustomer = _login.giveCust();
        }


        public void OnGet()
        {
          
            ProductList = _dataAccess.GetListProd();

            if (_login.IsLoggedIn() == true)
            {
                
                CustomerID = thisCustomer.CustomerID;
                FirstName = thisCustomer.FirstName;
                if (thisCustomer.cCard == null)
                {
                    _login.setCCard(thisCustomer.FullName(), thisCustomer.CustomerID * 300);

                }
                if (thisCustomer.customerCart == null)
                {
                    _login.setCart(thisCustomer.CustomerID);
                }
               
                thisCustomer = _login.giveCust();
                
            }
            else
            {
                FirstName = "Nobody";
            }
            
            
            
            
        }

        public void OnPostPutInCart()
        {
            ProductList = _dataAccess.GetListProd();
            thisCustomer = SetCustomer();
            ProductDTO result = _dataAccess.ProdGetById(ProductID);

            thisCustomer.customerCart.ProductsInCart.Add(result);


        }




    }
}
