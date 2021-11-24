﻿using DataSource;
using DataSource.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAccess
{
    public class DataAccess : IDataAccess
    {

        public List<ProductDTO> Products = new List<ProductDTO>();
        public List<CustomerDTO> Customers = new List<CustomerDTO>();

        public IDataSource _dataSource { get; private set; }

        public DataAccess(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public List<ProductDTO> GetListProd()
        {
            List<ProductDTO> prodlist = _dataSource.GetAllProducts().ToList(); ;
            return prodlist;
        }

        public void AccessProducts() //onödig??
        {
            Products = (List<ProductDTO>)_dataSource.GetAllProducts();

        }

        public List<CustomerDTO> GetListCust()
        {
            List<CustomerDTO> custlist = _dataSource.GetAllCustomers().ToList(); ;
            return custlist;
        }


        public void AccessCustomers() //onödig?
        {
            Customers = (List<CustomerDTO>)_dataSource.GetAllCustomers();
        }

        public CustomerDTO CustGetById(int idwhere)
        {
            //int index = 0;
            //CustomerDTO result =  t in Customers where t.CustomerID = idwhere;//
            //var result = (CustomerDTO)Customers.Where(s => s.CustomerID == idwhere);
            //CustomerDTO result = Customers.FirstOrDefault(o => o.CustomerID == idwhere);
            //return result;
            //return ((CustomerDTO)(from i in Customers where i.CustomerID == idwhere select i));
            //for (int i = 0; i < Customers.Count; i++)
            //{
            //    if (Customers[i].CustomerID == idwhere)
            //    {
            //        index = i;
            //    }
            //}
            //return Customers[index];
            //return result;
            List<CustomerDTO> CustomerList = GetListCust();
            foreach (var item in CustomerList)
            {
                if (item.CustomerID == idwhere)
                {
                    return item;
                }
            }
            return null;
        }
        public ProductDTO ProdGetById(int idwhere)
        {
            //ProductDTO result = (ProductDTO)Products.Where(s => s.ProductID == idwhere);
            List<ProductDTO> ProductList = GetListProd();
            foreach (ProductDTO item in ProductList)
            {
                if (item.ProductID == idwhere)
                {
                    return item;
                }
            }
            return null;

        }

        public IEnumerable<ProductDTO> GetAllProducts()
        {
            var jsonResponse = _dataSource.ProductsDataProvider();
            IEnumerable<ProductDTO> products = JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(jsonResponse);

            return products;
        }

        public int CalculateShipping(List<ProductDTO> prodlist)
        {
            int result = 0;
            foreach (var item in prodlist)
            {
                if (item.ProductType == "Physical")
                {
                    result += 49;
                }
            }
            return result;
        }


        public IEnumerable<ProductDTO> SortListProName()
        {

            return GetAllProducts().OrderBy(p => p.ProductName);


        }


        public IEnumerable<ProductDTO> SortListProPrice()
        {

            return GetAllProducts().OrderBy(p => p.ProductPrice);

        }



        public IEnumerable<ProductDTO> SearchBarName(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return GetAllProducts();
            }
            else
            {
                return GetAllProducts().Where(p => p.ProductName.Contains(search));
            }
        }



        //JSON SERIALIZE CUSTOMER

        public void CustomerListSerialize(List<CustomerDTO> custlist)
        {

            var path = @"C:\Users\danne\source\repos\ComicWebstoreExa\DataSource\JSONData\CustomersJson.json";
            var serializedUsers = JsonConvert.SerializeObject(custlist, Formatting.Indented);
            File.WriteAllText(path, serializedUsers);

        }


        public int CreateCartID(CustomerDTO cust)
        {
            int cartid = cust.CustomerID + 1;

            if ( cust.Reciepts.Count != 0)
            {
                foreach (var item in cust.Reciepts)
                {
                    cartid = +1;
                }
            }


            return cartid;
        }



        public void ShowCarts(CustomerDTO cust)
        {

        }

        public CreditCard CreateCreditCard(CustomerDTO cust)
        {
            CreditCard cCard = new CreditCard { CardNumber = cust.CustomerID + cust.CustomerID + cust.CustomerID, CardName = cust.FullName() };
            return cCard;
        }

        //public Reciept CreateReciept(CustomerDTO cust, Cart cart, CreditCard card)
        //{
        //    Reciept reciept = new Reciept() { RecieptCartID = cart.CartID, RecieptProducts = cart.ProductsInCart, RecieptSum = cart.CartSum(), isPaid = true, ccard = card };
        //    return reciept;
        //}



        public void UpdateCustomerList(CustomerDTO cust)
        {
            Customers = GetListCust().ToList();

            foreach (var item in Customers.ToList())
            {
                if (cust.CustomerID == item.CustomerID)
                {
                    var IndexOfUser = Customers.IndexOf(item);
                    Customers[IndexOfUser] = cust;

                }
            }
            CustomerListSerialize(Customers);

            //var index = Customers.FindIndex(c => c.CustomerID == cust.CustomerID);
            //Customers[index] = cust;
        }





        //public int IndexOfCart(CustomerDTO cust, int cartid)
        //{
        //    var index = cust.CartList.FindIndex(c => c.CartID == cartid);
        //    return index;
        //}

        public Reciept ReturnReciept(CustomerDTO cust, int id, Cart cart, int total, CreditCard card)
        {

            Reciept reciept = new Reciept() { RecieptCartID = cart.CartID, RecieptProducts = cart.ProductsInCart, RecieptSum = cart.CartSum(), isPaid = true, ccard = card };
            return reciept;

        }

    }
}