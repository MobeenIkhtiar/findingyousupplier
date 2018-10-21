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
    public class AccountController : Controller
    {
        SessionDTO sessiondto = new SessionDTO();

        public ActionResult Index()
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.name = sessiondto.getName();
                return View();
            }
        }

        public ActionResult EditProfile(string message = "")
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                User u = new UserBL().getUserById(sessiondto.getId());

                ViewBag.message = message;
                ViewBag.user = u;

                return View();
            }
        }

        [HttpPost]
        public ActionResult UpdateProfile(string Name, string Email, string Phone, string Website_Address, string Country)
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                User User = new UserBL().getUserById(sessiondto.getId());
                User.Name = Name;
                User.Email = Email;
                User.Phone = Phone;
                User.Website_Address = Website_Address;
                User.Country = Country;

                new UserBL().UpdateUser(User);
                return RedirectToAction("EditProfile", "Account", new { message = "Your profile has been updated" });
            }
        }

        public ActionResult EditCompanyProfile(string message = "")
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                User u = new UserBL().getUserById(sessiondto.getId());

                ViewBag.message = message;
                ViewBag.user = u;

                return View();
            }
        }

        [HttpPost]
        public ActionResult UpdateCompanyProfile(string Company_Name, string VAT_Registration_Number, string Company_Number, string Company_Address)
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                User User = new UserBL().getUserById(sessiondto.getId());
                User.Company_Name = Company_Name;
                User.VAT_Registration_Number = VAT_Registration_Number;
                User.Company_Number = Company_Number;
                User.Company_Address = Company_Address;

                new UserBL().UpdateUser(User);

                return RedirectToAction("EditCompanyProfile", "Account", new { message = "Your company profile has been updated" });
            }
        }

        public ActionResult ChangePassword(string message = "")
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.message = message;

                return View();
            }
        }

        [HttpPost]
        public ActionResult UpdatePassword(string OldPassword, string NewPassword, string ConfirmPassword)
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                User User = new UserBL().getUserById(sessiondto.getId());

                if (OldPassword == User.Password)
                {
                    if (NewPassword == ConfirmPassword)
                    {
                        User.Password = NewPassword;
                        new UserBL().UpdateUser(User);
                        return RedirectToAction("ChangePassword", "Account", new { message = "Your password has been changed" });
                    }
                    else
                    {
                        return RedirectToAction("ChangePassword", "Account", new { message = "New password and confirm password doesn't match" });

                    }
                }
                else
                {
                    return RedirectToAction("ChangePassword", "Account", new { message = "You entered wrong old password" });
                }
            }
        }

        public ActionResult ViewProducts()
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<Product> products = new UserBL().getProductList().Where(x => x.User_Id == sessiondto.getId()).OrderByDescending(x => x.Id).ToList();

                return View(products);
            }
        }

        public ActionResult AddProduct(string message = "")
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if(sessiondto.getRole() == 0)
            {
                return RedirectToAction("Index", "Account");
            }
            else
            {
                List<Category> categories = new UserBL().getCategoryList();

                ViewBag.categories = categories;
                ViewBag.message = message;

                return View();
            }
        }

        [HttpPost]
        public ActionResult SaveProduct(string Name, float Minimum_Order, float Delievery_Charges, string Description, int Category_Id)
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (sessiondto.getRole() == 0)
            {
                return RedirectToAction("Index", "Account");
            }
            else
            {
                Product p = new Product()
                {
                    Name = Name,
                    Minimum_Order = Minimum_Order,
                    Delievery_Charges = Delievery_Charges,
                    Description = Description,
                    Category_Id = Category_Id,
                    User_Id = sessiondto.getId(),
                    Is_Authorized = 0
                };

                new UserBL().AddProduct(p);

                Category c = new UserBL().getCategoryById(Category_Id);
                User u = new UserBL().getUserById(sessiondto.getId());

                //Image of Product
                
                Directory.CreateDirectory(Server.MapPath("~") + "Content\\Images\\Products\\" + sessiondto.getId());
                string ext = null;
                var fileName = "";
                string path = "Content\\Images\\Products\\" + sessiondto.getId();

                var file = Request.Files[0];
                fileName = Path.GetFileName(file.FileName);
                ext = Path.GetExtension(fileName);

                if (file != null && file.ContentLength > 0)
                {
                    if (ext.Equals(".png") || ext.Equals(".jpg") || ext.Equals(".jpeg"))
                    {
                        file.SaveAs(Server.MapPath("~") + path + fileName);
                    }
                }

                Image image = new Image()
                {
                    Path = path + fileName,
                    Product_Id = p.Id
                };
                new UserBL().AddImage(image);


                //Mail to Admin
                MailMessage msg = new MailMessage();

                msg.From = new MailAddress("support@casearea.com");
                msg.To.Add("support@findingyousuppliers.com");
                msg.Subject = "New Product";
                msg.IsBodyHtml = true;
                msg.Body = "<strong>Name: </strong>" + Name + "<br><strong>Category: </strong> " + c.Name + "<br><strong>Minimum Order: </strong> " + Minimum_Order +
                            "<br><strong>Delievery Charges:</strong> " + Delievery_Charges + "<br><strong>Description: </strong> " + Description + "<br><strong>Supplier Name: </strong>" + u.Name
                            + "<br><strong>Supplier Email: </strong>" + u.Email;

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

                return RedirectToAction("AddProduct", "Account", new { message = "Product has been submitted to admin to get approved." });
            }
        }

        public ActionResult EditProduct(int Id, string message = "")
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (sessiondto.getRole() == 0)
            {
                return RedirectToAction("Index", "Account");
            }
            else
            {
                List<Category> categories = new UserBL().getCategoryList();
                Product p = new UserBL().getProductById(Id);

                ViewBag.categories = categories;
                ViewBag.product = p;
                ViewBag.message = message;

                return View();
            }
        }

        [HttpPost]
        public ActionResult UpdateProduct(int Id, string Name, float Minimum_Order, float Delievery_Charges, string Description, int Category_Id)
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (sessiondto.getRole() == 0)
            {
                return RedirectToAction("Index", "Account");
            }
            else
            {
                Product p = new Product()
                {
                    Id = Id,
                    Name = Name,
                    Minimum_Order = Minimum_Order,
                    Delievery_Charges = Delievery_Charges,
                    Description = Description,
                    Category_Id = Category_Id,
                    User_Id = sessiondto.getId(),
                    Is_Authorized = 0
                };

                new UserBL().UpdateProduct(p);

                //Image of Product

                Directory.CreateDirectory(Server.MapPath("~") + "Content\\Images\\Products\\" + sessiondto.getId());
                string ext = null;
                var fileName = "";
                string path = "Content\\Images\\Products\\" + sessiondto.getId();

                var file = Request.Files[0];
                fileName = Path.GetFileName(file.FileName);
                ext = Path.GetExtension(fileName);

                if (file != null && file.ContentLength > 0)
                {
                    if (ext.Equals(".png") || ext.Equals(".jpg") || ext.Equals(".jpeg"))
                    {
                        file.SaveAs(Server.MapPath("~") + path + fileName);
                    }
                }

                Image i = new UserBL().getImageList().Where(x => x.Product_Id == Id).FirstOrDefault();

                Image image = new Image()
                {
                    Id = i.Id,
                    Path = path + fileName,
                    Product_Id = p.Id
                };
                
                new UserBL().UpdateImage(image);


                return RedirectToAction("EditProduct", "Account", new { Id = Id , message = "Product has been submitted to admin to get approved." });
            }
        }


        public ActionResult Inquiry(int Id)
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Login", "Auth", new { Id = StringCipher.Base64Encode("0"), err = "" });
            }
            else if (sessiondto.getRole() == 1 || sessiondto.getRole() == 2)
            {
                return RedirectToAction("Index", "Account");
            }
            else
            {

                ViewBag.user = new UserBL().getUserById(Id);
                ViewBag.phone = new UserBL().getUserById(sessiondto.getId()).Phone;
                ViewBag.email = new UserBL().getUserById(sessiondto.getId()).Email;

                return View();
            }
        }

        [HttpPost]
        public ActionResult PostInquiry(int To_User_Id, string Msg)
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (sessiondto.getRole() == 1 || sessiondto.getRole() == 2)
            {
                return RedirectToAction("Index", "Account");
            }
            else
            {
                Message m = new Message()
                {
                    From_User_Id = sessiondto.getId(),
                    To_User_Id = To_User_Id,
                    Msg = Msg,
                    Date = DateTime.Now
                };

                new UserBL().AddMessage(m);

                return RedirectToAction("Inquiry", "Account", new { Id = To_User_Id });
            }
        }

        public ActionResult Messages()
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if(sessiondto.getRole() == 0)
                {
                    List<Message> messages = new UserBL().getMessageList().Where(x => x.From_User_Id == sessiondto.getId()).OrderByDescending(x => x.Id).ToList();

                    ViewBag.messages = messages;
                }
                else
                {
                    List<Message> messages = new UserBL().getMessageList().Where(x => x.To_User_Id == sessiondto.getId()).OrderByDescending(x => x.Id).ToList();

                    ViewBag.messages = messages;
                }
                

                return View();
            }
        }
    }
}