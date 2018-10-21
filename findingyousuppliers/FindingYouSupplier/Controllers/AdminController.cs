using FindingYouSupplier.BL;
using FindingYouSupplier.Helping_Classes;
using FindingYouSupplier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindingYouSupplier.Controllers
{
    public class AdminController : Controller
    {
        SessionDTO sessiondto = new SessionDTO();

        [HttpPost]
        public int validateEmail(string Email)
        {
            int adminCount = new UserBL().getUserList().Where(x => x.Email == Email).Count();
            if (adminCount > 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        [HttpPost]
        public string validatePromoCode(string PromoCode)
        {            
            if (PromoCode == "FYS3MONTH")
            {
                string result = "1";
                return result;
            }
            else if(PromoCode == "FYS2MONTH")
            {
                return "2";
            }
            else if (PromoCode == "FYS1MONTH")
            {
                return "3";
            }
            else if(PromoCode == "")
            {
                return "-2";
            }
            else
            {
                return "-1";
            }
        }

        public ActionResult Index()
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (sessiondto.getRole() != 3)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                int totalbuyersCount = new UserBL().getUserList().Where(x => x.Role == 0).Count();
                int totaluksuppliersCount = new UserBL().getUserList().Where(x => x.Role == 1).Count();
                int totalinternationalsuppliersCount = new UserBL().getUserList().Where(x => x.Role == 2).Count();

                int totalactivebuyersCount = new UserBL().getUserList().Where(x => x.Role == 0 && x.Is_Authorize == 1).Count();
                int totalactiveuksuppliersCount = new UserBL().getUserList().Where(x => x.Role == 1 && x.Is_Authorize == 1).Count();
                int totalactiveinternationalsuppliersCount = new UserBL().getUserList().Where(x => x.Role == 2 && x.Is_Authorize == 1).Count();


                ViewBag.totalbuyersCount = totalbuyersCount;
                ViewBag.totaluksuppliersCount = totaluksuppliersCount;
                ViewBag.totalinternationalsuppliersCount = totalinternationalsuppliersCount;
                ViewBag.totalactivebuyersCount = totalactivebuyersCount;
                ViewBag.totalactiveuksuppliersCount = totalactiveuksuppliersCount;
                ViewBag.totalactiveinternationalsuppliersCount = totalactiveinternationalsuppliersCount;

                return View();
            }
        }

        #region User
        public ActionResult ListUsers()
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (sessiondto.getRole() != 3)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<User> buyers = new UserBL().getUserList().Where(x => x.Role == 0).OrderByDescending(x => x.Id).ToList();
                List<User> uksuppliers = new UserBL().getUserList().Where(x => x.Role == 1).OrderByDescending(x => x.Id).ToList();
                List<User> internationalsuppliers = new UserBL().getUserList().Where(x => x.Role == 2).OrderByDescending(x => x.Id).ToList();

                ViewBag.buyers = buyers;
                ViewBag.uksuppliers = uksuppliers;
                ViewBag.internationalsuppliers = internationalsuppliers;

                return View();
            }
        }

        public ActionResult AddUser()
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (sessiondto.getRole() != 3)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult SaveUser(User User)
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (sessiondto.getRole() != 3)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                User.Is_Authorize = 1;
                new UserBL().AddUser(User);

                return RedirectToAction("ListUsers", "Admin");

            }
        }

        public ActionResult EditUser(int Id)
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (sessiondto.getRole() != 3)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                User User = new UserBL().getUserById(Id);

                return View(User);
            }
        }

        [HttpPost]
        public ActionResult UpdateUser(int Id, User User)
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (sessiondto.getRole() != 3)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                User user = new UserBL().getUserById(Id);
                User u = new User()
                {
                    Id = Id,
                    Name = User.Name,
                    Phone = User.Phone,
                    Website_Address = User.Website_Address,
                    Email = User.Email,
                    Password = user.Password,
                    Is_Authorize = user.Is_Authorize,
                    Company_Name = User.Company_Name,
                    Company_Address = User.Company_Address,
                    Company_Number = User.Company_Number,
                    VAT_Registration_Number = User.VAT_Registration_Number,
                    Role = User.Role,
                    Country = User.Country
                };

                new UserBL().UpdateUser(u);

                return RedirectToAction("ListUsers", "Admin");
            }
        }

        public ActionResult ToggleUser(int Id)
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (sessiondto.getRole() != 3)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                User user = new UserBL().getUserById(Id);

                if (user.Is_Authorize == 0)
                {
                    user.Is_Authorize = 1;
                }
                else
                {
                    user.Is_Authorize = 0;
                }
                new UserBL().UpdateUser(user);

                return RedirectToAction("ListUsers", "Admin");
            }
        }
        #endregion

        public ActionResult ListProducts()
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (sessiondto.getRole() != 3)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<Product> products = new UserBL().getProductList().OrderByDescending(x => x.Id).ToList();

                ViewBag.products = products;

                return View();
            }
        }

        
        public ActionResult ToggleProduct(int Id)
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (sessiondto.getRole() != 3)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Product product = new UserBL().getProductById(Id);

                Product p = new Product()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Category_Id = product.Category_Id,
                    Minimum_Order = product.Minimum_Order,
                    Delievery_Charges = product.Delievery_Charges,
                    Description = product.Description,
                    User_Id = product.User_Id
                };

                if (product.Is_Authorized == 0)
                {
                    p.Is_Authorized = 1;
                }
                else
                {
                    p.Is_Authorized = 0;
                }

                new UserBL().UpdateProduct(p);

                return RedirectToAction("ListProducts", "Admin");
            }
        }
    }
}