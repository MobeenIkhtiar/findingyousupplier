using FindingYouSupplier.BL;
using FindingYouSupplier.Helping_Classes;
using FindingYouSupplier.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace FindingYouSupplier.Controllers
{
    [Compress]
    public class HomeController : Controller
    {
        SessionDTO sessiondto = new SessionDTO();


        public ActionResult Index(string message = "")
        {        

            List<CategoryDTO> categoryDTO = new List<CategoryDTO>();

            List<Category> categories = new UserBL().getCategoryList();

            foreach (Category c in categories)
            {
                if (c.Category_Id == null)
                {
                    CategoryDTO cdto = new CategoryDTO()
                    {
                        CategoryId = c.Id,
                        Category_Name = c.Name,
                        Sub_Categories = new UserBL().getCategoryList().Where(x => x.Category_Id == c.Id).ToList(),
                        Count = new UserBL().getCategoryList().Where(x => x.Category_Id == c.Id).ToList().Count()
                    };
                    categoryDTO.Add(cdto);
                }
            }

            ViewBag.categoryDTO = categoryDTO;


            int productscount = new UserBL().getProductList().Where(x => x.Is_Authorized == 1).Count();


            if (productscount != 0)
            {
                List<Product> products = new UserBL().getProductList().Where(x => x.Is_Authorized == 1).OrderByDescending(p => p.Id).Take(10).ToList();
                ViewBag.products = products;

            }

            ViewBag.productscount = productscount;
            ViewBag.message = message;
            return View();
        }

        [HttpPost]
        public ActionResult PostContact(string Name, string Email, string Subject, string Message)
        {
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress("support@casearea.com");
            msg.To.Add("hussnaindar17@gmail.com");
            msg.Subject = "Contact Us";
            msg.IsBodyHtml = true;
            msg.Body = "<strong>Name: </strong>" + Name + "<br><strong>Email: </strong> " + Email + "<br><strong>Subject: </strong> " + Subject +
                        "<br><strong>Message:</strong> " + Message;

            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = false;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("support@casearea.com", "delta@O27");
                client.Host = "webmail.casearea.com";
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Send(msg);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CategoryProducts(int Id)
        {
            Category category = new UserBL().getCategoryById(Id);
            List<Product> categoryproducts = new List<Product>();
            //Father Category
            if (category.Category_Id == null)
            {
                List<Category> subcategories = new UserBL().getCategoryList().Where(x => x.Category_Id == Id).ToList();
                foreach (Category c in subcategories)
                {
                    Product p = new UserBL().getProductList().Where(x => x.Category_Id == c.Id).FirstOrDefault();

                    if (p != null)
                    {
                        categoryproducts.Add(p);

                    }
                }
                ViewBag.categoryproducts = categoryproducts.OrderByDescending(x => x.Id);
                ViewBag.subcategories = subcategories;
            }
            //Son Category
            else
            {
                categoryproducts = new UserBL().getProductList().Where(x => x.Category_Id == Id).OrderByDescending(x => x.Id).ToList();
                Category parentcategory = new UserBL().getCategoryById(Convert.ToInt32(category.Category_Id));

                if (categoryproducts != null)
                {
                    ViewBag.categoryproducts = categoryproducts;
                    ViewBag.parentcategory = parentcategory;
                }
            }

            ViewBag.category = category;
            return View();
        }

        public ActionResult UKCategoryProducts(int Id)
        {
            Category category = new UserBL().getCategoryById(Id);
            List<Product> categoryproducts = new List<Product>();
            //Father Category
            if (category.Category_Id == null)
            {
                List<Category> subcategories = new UserBL().getCategoryList().Where(x => x.Category_Id == Id).ToList();
                foreach (Category c in subcategories)
                {
                    Product p = new UserBL().getProductList().Where(x => x.Category_Id == c.Id).FirstOrDefault();

                    if (p != null && p.User.Role == 1)
                    {
                        categoryproducts.Add(p);

                    }
                }
                ViewBag.categoryproducts = categoryproducts.OrderByDescending(x => x.Id);
                ViewBag.subcategories = subcategories;
            }
            //Son Category
            else
            {
                categoryproducts = new UserBL().getProductList().Where(x => x.Category_Id == Id && x.User.Role == 1).OrderByDescending(x => x.Id).ToList();
                Category parentcategory = new UserBL().getCategoryById(Convert.ToInt32(category.Category_Id));

                if (categoryproducts != null)
                {
                    ViewBag.categoryproducts = categoryproducts;
                    ViewBag.parentcategory = parentcategory;
                }
            }


            ViewBag.category = category;
            return View("CategoryProducts");
        }

        public ActionResult InternationalCategoryProducts(int Id)
        {
            Category category = new UserBL().getCategoryById(Id);
            List<Product> categoryproducts = new List<Product>();
            //Father Category
            if (category.Category_Id == null)
            {
                List<Category> subcategories = new UserBL().getCategoryList().Where(x => x.Category_Id == Id).ToList();
                foreach (Category c in subcategories)
                {
                    Product p = new UserBL().getProductList().Where(x => x.Category_Id == c.Id).FirstOrDefault();

                    if (p != null && p.User.Role == 2)
                    {
                        categoryproducts.Add(p);

                    }
                }
                ViewBag.categoryproducts = categoryproducts.OrderByDescending(x => x.Id);
                ViewBag.subcategories = subcategories;
            }
            //Son Category
            else
            {
                categoryproducts = new UserBL().getProductList().Where(x => x.Category_Id == Id && x.User.Role == 2).OrderByDescending(x => x.Id).ToList();
                Category parentcategory = new UserBL().getCategoryById(Convert.ToInt32(category.Category_Id));

                if (categoryproducts != null)
                {
                    ViewBag.categoryproducts = categoryproducts;
                    ViewBag.parentcategory = parentcategory;
                }
            }


            ViewBag.category = category;
            return View("CategoryProducts");
        }

        [HttpPost]
        public ActionResult CategorySearch(string category)
        {
            List<CategoryDTO> categoryDTO = new List<CategoryDTO>();
            List<Category> fathercategories = new UserBL().getCategoryList().Where(x => x.Category_Id == null).ToList();

            foreach (Category cat in fathercategories)
            {
                if (cat.Name.ToUpper().Contains(category.ToUpper()))
                {
                    CategoryDTO cdto = new CategoryDTO()
                    {
                        CategoryId = cat.Id,
                        Category_Name = cat.Name,
                        Sub_Categories = new UserBL().getCategoryList().Where(x => x.Category_Id == cat.Id).ToList(),
                        Count = new UserBL().getCategoryList().Where(x => x.Category_Id == cat.Id).ToList().Count()
                    };
                    categoryDTO.Add(cdto);
                }
                else
                {
                    List<Category> subcategories = new UserBL().getCategoryList().Where(x => x.Category_Id == cat.Id).ToList();
                    List<Category> matchingsubcategories = new List<Category>();

                   

                    foreach (Category ca in subcategories)
                    {
                        if (ca.Name.ToUpper().Contains(category.ToUpper()))
                        {
                            matchingsubcategories.Add(ca);
                        }
                    }

                    if (matchingsubcategories.Count() > 0)
                    {
                        CategoryDTO cdto = new CategoryDTO()
                        {
                            CategoryId = cat.Id,
                            Category_Name = cat.Name,
                            Sub_Categories = matchingsubcategories,
                            Count = new UserBL().getCategoryList().Where(x => x.Category_Id == cat.Id).ToList().Count()
                        };
                        categoryDTO.Add(cdto);
                    }                      
                }
            }

            ViewBag.categoryDTO = categoryDTO;

            int productscount = new UserBL().getProductList().Where(x => x.Is_Authorized == 1).Count();


            if (productscount != 0)
            {
                List<Product> products = new UserBL().getProductList().Where(x => x.Is_Authorized == 1).OrderByDescending(p => p.Id).Take(10).ToList();
                ViewBag.products = products;

            }

            ViewBag.productscount = productscount;

            return View("Index");
        }

        [HttpPost]
        public ActionResult UKCategorySearch(string category)
        {
            List<CategoryDTO> categoryDTO = new List<CategoryDTO>();
            List<Category> fathercategories = new UserBL().getCategoryList().Where(x => x.Category_Id == null).ToList();

            foreach (Category cat in fathercategories)
            {
                if (cat.Name.ToUpper().Contains(category.ToUpper()))
                {
                    CategoryDTO cdto = new CategoryDTO()
                    {
                        CategoryId = cat.Id,
                        Category_Name = cat.Name,
                        Sub_Categories = new UserBL().getCategoryList().Where(x => x.Category_Id == cat.Id).ToList(),
                        Count = new UserBL().getCategoryList().Where(x => x.Category_Id == cat.Id).ToList().Count()
                    };
                    categoryDTO.Add(cdto);
                }
                else
                {
                    List<Category> subcategories = new UserBL().getCategoryList().Where(x => x.Category_Id == cat.Id).ToList();
                    List<Category> matchingsubcategories = new List<Category>();



                    foreach (Category ca in subcategories)
                    {
                        if (ca.Name.ToUpper().Contains(category.ToUpper()))
                        {
                            matchingsubcategories.Add(ca);
                        }
                    }

                    if (matchingsubcategories.Count() > 0)
                    {
                        CategoryDTO cdto = new CategoryDTO()
                        {
                            CategoryId = cat.Id,
                            Category_Name = cat.Name,
                            Sub_Categories = matchingsubcategories,
                            Count = new UserBL().getCategoryList().Where(x => x.Category_Id == cat.Id).ToList().Count()
                        };
                        categoryDTO.Add(cdto);
                    }
                }
            }

            ViewBag.categoryDTO = categoryDTO;

            int productscount = new UserBL().getProductList().Where(x => x.Is_Authorized == 1).Count();


            if (productscount != 0)
            {
                List<Product> products = new UserBL().getProductList().Where(x => x.Is_Authorized == 1).OrderByDescending(p => p.Id).Take(10).ToList();
                ViewBag.products = products;

            }

            ViewBag.productscount = productscount;

            return View("UKSuppliers");
        }


        [HttpPost]
        public ActionResult InternationalCategorySearch(string category)
        {
            List<CategoryDTO> categoryDTO = new List<CategoryDTO>();
            List<Category> fathercategories = new UserBL().getCategoryList().Where(x => x.Category_Id == null).ToList();

            foreach (Category cat in fathercategories)
            {
                if (cat.Name.ToUpper().Contains(category.ToUpper()))
                {
                    CategoryDTO cdto = new CategoryDTO()
                    {
                        CategoryId = cat.Id,
                        Category_Name = cat.Name,
                        Sub_Categories = new UserBL().getCategoryList().Where(x => x.Category_Id == cat.Id).ToList(),
                        Count = new UserBL().getCategoryList().Where(x => x.Category_Id == cat.Id).ToList().Count()
                    };
                    categoryDTO.Add(cdto);
                }
                else
                {
                    List<Category> subcategories = new UserBL().getCategoryList().Where(x => x.Category_Id == cat.Id).ToList();
                    List<Category> matchingsubcategories = new List<Category>();



                    foreach (Category ca in subcategories)
                    {
                        if (ca.Name.ToUpper().Contains(category.ToUpper()))
                        {
                            matchingsubcategories.Add(ca);
                        }
                    }

                    if (matchingsubcategories.Count() > 0)
                    {
                        CategoryDTO cdto = new CategoryDTO()
                        {
                            CategoryId = cat.Id,
                            Category_Name = cat.Name,
                            Sub_Categories = matchingsubcategories,
                            Count = new UserBL().getCategoryList().Where(x => x.Category_Id == cat.Id).ToList().Count()
                        };
                        categoryDTO.Add(cdto);
                    }
                }
            }

            ViewBag.categoryDTO = categoryDTO;

            int productscount = new UserBL().getProductList().Where(x => x.Is_Authorized == 1).Count();


            if (productscount != 0)
            {
                List<Product> products = new UserBL().getProductList().Where(x => x.Is_Authorized == 1).OrderByDescending(p => p.Id).Take(10).ToList();
                ViewBag.products = products;

            }

            ViewBag.productscount = productscount;

            return View("InternationalSuppliers");
        }

        public ActionResult GetStarted()
        {
            if(sessiondto.getName() == null)
            {                
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Product(string Id)
        {
            Id = StringCipher.Base64Decode(Id);

            Product p = new UserBL().getProductById(Convert.ToInt32(Id));

            if (p != null)
            {
                string path = p.Images.FirstOrDefault().Path;
                ViewBag.path = path;
            }

            ViewBag.product = p;

            return View();
        }

        public ActionResult Help()
        {
            return View();
        }

        public ActionResult UKSuppliers()
        {
            List<CategoryDTO> categoryDTO = new List<CategoryDTO>();

            List<Category> categories = new UserBL().getCategoryList();

            foreach (Category c in categories)
            {
                if (c.Category_Id == null)
                {
                    CategoryDTO cdto = new CategoryDTO()
                    {
                        CategoryId = c.Id,
                        Category_Name = c.Name,
                        Sub_Categories = new UserBL().getCategoryList().Where(x => x.Category_Id == c.Id).ToList(),
                        Count = new UserBL().getCategoryList().Where(x => x.Category_Id == c.Id).ToList().Count()
                    };
                    categoryDTO.Add(cdto);
                }
            }

            ViewBag.categoryDTO = categoryDTO;


            List<Product> products = new UserBL().getProductList().Where(x => x.User.Role == 1).OrderByDescending(p => p.Id).Take(10).ToList();

            ViewBag.products = products;
            ViewBag.productscount = products.Count();

            return View();
        }

        public ActionResult InternationalSuppliers()
        {
            List<CategoryDTO> categoryDTO = new List<CategoryDTO>();

            List<Category> categories = new UserBL().getCategoryList();

            foreach (Category c in categories)
            {
                if (c.Category_Id == null)
                {
                    CategoryDTO cdto = new CategoryDTO()
                    {
                        CategoryId = c.Id,
                        Category_Name = c.Name,
                        Sub_Categories = new UserBL().getCategoryList().Where(x => x.Category_Id == c.Id).ToList(),
                        Count = new UserBL().getCategoryList().Where(x => x.Category_Id == c.Id).ToList().Count()
                    };
                    categoryDTO.Add(cdto);
                }
            }

            ViewBag.categoryDTO = categoryDTO;


            List<Product> products = new UserBL().getProductList().Where(x => x.User.Role == 2).OrderByDescending(p => p.Id).Take(10).ToList();

            ViewBag.products = products;
            ViewBag.productscount = products.Count();
            return View();
        }

        public ActionResult Packages(string Id)
        {
            Id = StringCipher.Base64Decode(Id);

            ViewBag.Id = Id;

            return View();
        }

        public ActionResult SellOnline()
        {
            return View();
        }

        public ActionResult TrafficToEcommerceStore()
        {
            return View();
        }

        public ActionResult ImproveEcommerceStore()
        {
            return View();
        }

        public ActionResult SocialMediaMarketing()
        {
            return View();
        }

        public ActionResult MarketingOnlineStore()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult Terms()
        {
            return View();
        }

        [OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
        public ActionResult cssCache()
        {
            return PartialView("cssCacheView", "Shared");
        }
        [OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
        public ActionResult jsCache()
        {
            return PartialView("jsCacheView", "Shared");
        }
    }
}