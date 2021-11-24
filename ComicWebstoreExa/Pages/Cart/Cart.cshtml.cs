using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using DataSource.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ComicWebstoreExa.Pages.Cart
{
    public class CartModel : PageModel
    {

        //public int cartID { get; set; }
        public CartModel(IDataAccess _dataaccess, ILoggedIn _loggedin)
        {
            DataAccess = _dataaccess;
            LoggedIn = _loggedin;
        }



        [BindProperty]
        public int itemID { get; set; }
        public CustomerDTO CurrentCustomer { get; set; }

        public IDataAccess DataAccess { get; }

        public ILoggedIn LoggedIn { get; }

        public void OnGet(/*List<ProductDTO> thisCartList*/)
        {
            CurrentCustomer = LoggedIn.giveCust();
            if (LoggedIn.IsLoggedIn() == true)
            {
            //    cartID = DataAccess.CreateCart(CurrentCustomer, CurrentCustomer.ProductsInCart);
            //    LoggedIn.SetCartID(cartID);
                if (CurrentCustomer.cCard == null)
                {
                    DataAccess.CreateCreditCard(CurrentCustomer);
                }
                //DataAccess.UpdateCustomerList(LoggedIn.giveCust());
            }

            //DataAccess.CustomerListSerialize(DataAccess.GetListCust());
            //if (CurrentCustomer.ProductsInCart.Count > 0)
            //{
            //    foreach (var item in CurrentCustomer.ProductsInCart)
            //    {
            //        CurrentCustomer.ProductsInCart.Add(item);
            //        CurrentCart.CustCartID = CurrentCustomer.CustomerID;
            //        CurrentCustomer.CartList.Add(CurrentCart.CartID);
            //    }
            //}
        }
        public int ProductsTotal()
        {
            CurrentCustomer = LoggedIn.giveCust();
            int total = 0;
            foreach (var item in CurrentCustomer.customerCart.ProductsInCart)
            {
                total += item.ProductPrice;
            }

            return total;
        }


        public int ShippingTotal()
        {
            CurrentCustomer = LoggedIn.giveCust();
            int shiptot = DataAccess.CalculateShipping(CurrentCustomer.customerCart.ProductsInCart);
            return shiptot;
        }
        public void OnPostRemoveItem() 
        {
            LoggedIn.RemoveItemAt(itemID);
            CurrentCustomer = LoggedIn.giveCust();
           
        }
    }
}
