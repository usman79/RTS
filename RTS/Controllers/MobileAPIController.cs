using IndigoAdmin.DAL.Data.EF;
using IndigoAdmin.Filters;
using IndigoAdmin.Helpers;
using IndigoAdmin.Models;
using IndigoAdmin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndigoAdmin.Controllers
{
    [UserAuthenticationFilter]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MobileApiController : ControllerBase
    {
        IProductService _PrdocutService;
        IOrderService _OrderService;
        IBrandService _BrandService;
        IOrderDetailService _OrderDetailService;
        IUserAccountService _UserAccountService;
        IWishListService _WishListService;
        ICoinsService _CoinsService;
        ITransactionService _TransactionService;


        public MobileApiController(IProductService PrdocutService, IOrderService OrderService, IOrderDetailService OrderDetailService, IBrandService BrandService, IUserAccountService userAccountService, IWishListService wishListService, ICoinsService coinsService, ITransactionService TransactionService)
        {
            _PrdocutService = PrdocutService;
            _OrderService = OrderService;
            _OrderDetailService = OrderDetailService;
            _BrandService = BrandService;
            _UserAccountService = userAccountService;
            _WishListService = wishListService;
            _CoinsService = coinsService;
            _TransactionService = TransactionService;
        }

        [HttpGet]
        public List<Product> GetAllPrdocuts(string text, int page)
        {
            if (text == null)
            {
                text = "";
            }
            var listCompanies = _PrdocutService.GetAllFiltered(text, page);

            if (listCompanies != null)
            {
                return listCompanies.ToList();
            }
            return new List<Product>();
        }
        [HttpGet]
        public List<Product> GetAllPrdocutsByBrand(int brandId)
        {
            var listCompanies = _PrdocutService.GetAllByBrand(brandId);

            if (listCompanies != null)
            {
                return listCompanies.ToList();
            }
            return new List<Product>();
        }

        [HttpGet]
        public List<Brand> GetAllBrands()
        {
            var listCompanies = _BrandService.GetAll();

            if (listCompanies != null)
            {
                return listCompanies.ToList();
            }
            return new List<Brand>();
        }

        [HttpGet]
        public List<Order> GetAllOrders()
        {
            var listCompanies = _OrderService.GetAll();

            if (listCompanies != null)
            {
                return listCompanies.ToList();
            }///
            return new List<Order>();
        }

        [HttpGet]
        public List<Order> GetUserOrders()
        {
            var listCompanies = _OrderService.GetAllByUser((int)UserPrncipleManager.Principle.UserId);

            if (listCompanies != null)
            {
                return listCompanies.ToList();
            }///
            return new List<Order>();
        }

        [HttpGet]
        public List<Order> GetOtOrders()
        {
            var listCompanies = _OrderService.GetAllByOt((int)UserPrncipleManager.Principle.UserId);

            if (listCompanies != null)
            {
                return listCompanies.ToList();
            }///
            return new List<Order>();
        }

        [HttpGet]
        public List<OrderDetail> GetDetailsByOrder(int OrderId)
        {
            var listCompanies = _OrderDetailService.GetByOrder(OrderId);

            if (listCompanies != null)
            {
                return listCompanies.ToList();
            }
            return new List<OrderDetail>();
        }
        [HttpGet]
        public List<UserAccount> GetAllCustomers(int OrderTakerId)
        {
            var listCustomers = _UserAccountService.GetAllByOt(3, OrderTakerId);

            if (listCustomers != null)
            {
                return listCustomers.ToList();
            }
            return new List<UserAccount>();
        }


        [HttpPost]
        public bool OrderTakerOrder([FromBody] PostOrder orderModal)
        {
            try
            {
                Order objorder = new Order();
                objorder.BillingAddress = orderModal.Address;
                objorder.OrderTakerId = (int)UserPrncipleManager.Principle.UserId;
                objorder.Store = orderModal.Store;
                objorder.BillingPhone = orderModal.Phone;
                objorder.CreatedAt = DateTime.Now;
                objorder.CreatedBy = (int)UserPrncipleManager.Principle.UserId;
                objorder.PaymentType = 1;
                objorder.OrderType = 2;
                objorder.UserName = orderModal.Customer;
                objorder.DeliveryBoyId = _UserAccountService.GetById(UserPrncipleManager.Principle.UserId).AssignedDeliveryBoyId;
                objorder.Status = 1;

                var OrderId = _OrderService.Create(objorder);
                float amount = 0;
                foreach (var product in orderModal.Cart)
                {
                    OrderDetail objDetails = new OrderDetail();
                    objDetails.OrderId = (int)OrderId;
                    objDetails.ProductId = product.Product.ProductId;
                    objDetails.Quantity = product.Quantity;
                    _OrderDetailService.Create(objDetails);
                    var ProductDTO = _PrdocutService.GetById(product.Product.ProductId);
                    if (ProductDTO != null)
                    {
                        ProductDTO.Quantity -= objDetails.Quantity;
                        float mrp;
                        float disc;
                        mrp = ProductDTO.Price - ((ProductDTO.Price / 100) * 15) ?? 0;
                        disc = ((mrp / 100) * ProductDTO.Discount) ?? 0;
                        var tempAmount = (mrp - disc) * product.Quantity;
                        amount = (amount + (float)tempAmount);
                        _PrdocutService.Update(ProductDTO);
                    }


                }

                //Transaction TransactionDTO = new Transaction()
                //{
                //    CreatedAt = DateTime.Now,
                //    OrderId = (int)OrderId,
                //    TotalAmount = (float)Math.Floor(amount),
                //    UserId = objorder.UserId,
                //    TransactionTypeId = 1,//1 for credit
                //    Status = 1,
                //    Balance= (float)Math.Floor(amount)
                //};
                //_TransactionService.Create(TransactionDTO);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        [HttpPost]
        public bool CustomerOrder([FromBody] PostOrder orderModal)
        {
            try
            {
                Order objorder = new Order();
                objorder.BillingAddress = orderModal.Address;
                objorder.UserId = (int)UserPrncipleManager.Principle.UserId;
                objorder.Store = orderModal.Store;
                objorder.BillingPhone = orderModal.Phone;
                objorder.CreatedAt = DateTime.Now;
                objorder.CreatedBy = (int)UserPrncipleManager.Principle.UserId;
                objorder.PaymentType = orderModal.PaymentMethod;
                objorder.OrderType = 1;
                objorder.UserName = orderModal.Customer;
                objorder.Status = 1;
                objorder.DeliveryBoyId = _UserAccountService.GetById(UserPrncipleManager.Principle.UserId).AssignedDeliveryBoyId;
                var OrderId = _OrderService.Create(objorder);
                float amount = 0;
                foreach (var product in orderModal.Cart)
                {
                    OrderDetail objDetails = new OrderDetail();
                    objDetails.OrderId = (int)OrderId;
                    objDetails.ProductId = product.Product.ProductId;
                    objDetails.Quantity = product.Quantity;
                    _OrderDetailService.Create(objDetails);


                    var ProductDTO = _PrdocutService.GetById(product.Product.ProductId);
                    if (ProductDTO != null)
                    {
                        ProductDTO.Quantity -= objDetails.Quantity;
                        float mrp;
                        float disc;
                        mrp = ProductDTO.Price - ((ProductDTO.Price / 100) * 15) ?? 0;
                        disc = ((mrp / 100) * ProductDTO.Discount) ?? 0;
                        var tempAmount = (mrp - disc) * product.Quantity;
                        amount = (amount + (float)tempAmount);
                        _PrdocutService.Update(ProductDTO);
                    }


                }
                var CoinDTO = _CoinsService.GetByUserId((int)objorder.UserId);
                if (CoinDTO != null)
                {
                    CoinDTO.coins -= orderModal.Coins;
                    _CoinsService.Update(CoinDTO);
                }

                //Transaction TransactionDTO = new Transaction()
                //{
                //    CreatedAt = DateTime.Now,
                //    OrderId = (int)OrderId,
                //    TotalAmount = (float)Math.Floor(amount),
                //    UserId = objorder.UserId,
                //    TransactionTypeId = 1,//1 for credit
                //    Status = 1
                //};
                //_TransactionService.Create(TransactionDTO);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        [HttpGet]
        public List<Product> GetWishListById()
        {
            List<Product> listProducts = new List<Product>();
            var listCustomers = _WishListService.GetById((int)UserPrncipleManager.Principle.UserId);

            if (listCustomers != null)
            {

                foreach (var item in listCustomers)
                {
                    listProducts.Add(_PrdocutService.GetById((long)item.ProductId));
                }

                return listProducts.ToList();
            }
            return new List<Product>(); ;
        }
        [HttpGet]
        public bool AddToWishList(int id)
        {
            bool isAlreadyEntered = false;

            try
            {
                WishList obj = new WishList();

                obj.UserId = (int)UserPrncipleManager.Principle.UserId;


                obj.CreatedAt = DateTime.Now;
                obj.CreatedBy = (int)UserPrncipleManager.Principle.UserId;


                obj.ProductId = id;

                var list = _WishListService.GetById((int)UserPrncipleManager.Principle.UserId);
                if (list != null)
                    foreach (var item in list)
                    {
                        if (item.ProductId == id)
                        {
                            isAlreadyEntered = true;
                        }
                    }
                if (!isAlreadyEntered)
                    _WishListService.Create(obj);

            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        [HttpGet]
        public bool DeleteWishList(int id)
        {

            try
            {
                var list = _WishListService.GetById((int)UserPrncipleManager.Principle.UserId);
                foreach (var item in list)
                {
                    if (item.ProductId == id)
                    {
                        _WishListService.Delete(item.Id);
                        return true;
                    }
                }



            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }
        [HttpGet]
        public List<Coins> GetCoinsById()
        {

            var listCoins = _CoinsService.GetById((int)UserPrncipleManager.Principle.UserId);

            if (listCoins != null)
            {



                return listCoins.ToList();
            }
            return new List<Coins>(); ;
        }
    }
}
