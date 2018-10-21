using FindingYouSupplier.BL;
using FindingYouSupplier.Helping_Classes;
using FindingYouSupplier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FindingYouSupplier.Controllers
{
    public class AuthController : Controller
    {
        SessionDTO sessiondto = new SessionDTO();

        [HttpPost]
        public int validateEmail(string Email)
        {
            int emailCount = new UserBL().getUserList().Where(x => x.Email == Email).Count();
            if (emailCount > 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public ActionResult Signup(string package = "")
        {
            if (package == "")
            {
                return RedirectToAction("GetStarted", "Home");
            }
            else
            {
                package = StringCipher.Base64Decode(package);

                BrainTreePayment payment = new BrainTreePayment();
                ViewBag.package = package;
                return View();
            }

        }

        [HttpPost]
        public ActionResult PostRegister(FormCollection collection, string package, User u, string promoCode)
        {
            BrainTreePayment payments = new BrainTreePayment();
            string nonceFromTheClient = collection["payment_method_nonce"];
            int trial = 0;
            if (promoCode == "FYS3MONTH")
            {
                trial = 3;
            }
            else if (promoCode == "FYS2MONTH")
            {
                trial = 2;
            }
            else if (promoCode == "FYS1MONTH")
            {
                trial = 1;
            }
            if (package == "Silver_Buyer" || package == "Golden_Buyer")
            {
                string Subscription_Id = payments.proceedPayment(nonceFromTheClient, package, u, trial);
                if (Subscription_Id == "-1")
                {

                    return RedirectToAction("SignupResult", "Auth", new { Message = "Requirements of customer for Braintree is not complete." });
                }
                else if (Subscription_Id == "-2")
                {
                    return RedirectToAction("SignupResult", "Auth", new { Message = "Problem in your card." });
                }
                else if (Subscription_Id == "-3")
                {
                    return RedirectToAction("SignupResult", "Auth", new { Message = "Problem in your funds." });
                }
                else
                {
                    u.Role = 0; // For Buyer
                    u.Subscription_Id = Subscription_Id;
                    new UserBL().AddUser(u);

                    string Subject = "New Buyer";
                    string Body = "<strong>Name: </strong>" + u.Name + "<br><strong>Email: </strong> " + u.Email + "<br><strong>Phone: </strong> " + u.Phone + "<br><strong>Website Address:</strong>"
                               + u.Website_Address + "<br><strong>Country: </strong> " + u.Country + "<br><strong>Role: </strong>Buyer";

                    MailToAdmin(u, Subject, Body);

                    return RedirectToAction("BuyerSignUpVideo", "Auth", new { mail = u.Email, Message = "An email is sent to the Admin to make you an authorized user of this website. You will get an email from Admin within 24 hours." });


                }
            }
            else if (package == "Silver_UK_Supplier" || package == "Golden_UK_Supplier")
            {
                string Subscription_Id = payments.proceedPayment(nonceFromTheClient, package, u, trial);

                if (Subscription_Id == "-1")
                {

                    return RedirectToAction("SignupResult", "Auth", new { Message = "Requirements of customer for Braintree is not complete." });
                }
                else if (Subscription_Id == "-2")
                {
                    return RedirectToAction("SignupResult", "Auth", new { Message = "Problem in your card." });
                }
                else if (Subscription_Id == "-3")
                {
                    return RedirectToAction("SignupResult", "Auth", new { Message = "Problem in your funds." });
                }
                else
                {
                    u.Role = 1; // For UK Suppliers
                    u.Subscription_Id = Subscription_Id;
                    new UserBL().AddUser(u);

                    string Subject = "New UK Supplier";
                    string Body = "<strong>Name: </strong>" + u.Name + "<br><strong>Email: </strong> " + u.Email + "<br><strong>Phone: </strong> " + u.Phone + "<br><strong>Website Address: </strong>"
                               + u.Website_Address + "<br><strong>Country: </strong> " + u.Country + "<br><strong>Role: </strong>Buyer<br><strong>Company Name: </strong>" + u.Company_Name
                               + "<br><strong>Company Number: </strong>" + u.Company_Number + "<br><strong>Company Address: </strong> " + u.Company_Address + "<br><strong>VAT Registration Number: </strong> " + u.VAT_Registration_Number;

                    MailToAdmin(u, Subject, Body);

                    return RedirectToAction("SupplierSignUpVideo", "Auth", new { Message = "An email is sent to the Admin to make you an authorized user of this website. You will get an email from Admin within 24 hours." });

                }

            }
            else if (package == "Silver_International_Supplier" || package == "Golden_International_Supplier")
            {
                string Subscription_Id = payments.proceedPayment(nonceFromTheClient, package, u, trial);

                if (Subscription_Id == "-1")
                {

                    return RedirectToAction("SignupResult", "Auth", new { Message = "Requirements of customer for Braintree is not complete." });
                }
                else if (Subscription_Id == "-2")
                {
                    return RedirectToAction("SignupResult", "Auth", new { Message = "Problem in your card." });
                }
                else if (Subscription_Id == "-3")
                {
                    return RedirectToAction("SignupResult", "Auth", new { Message = "Problem in your funds." });
                }
                else
                {
                    u.Role = 2; // For International Suppliers
                    u.Subscription_Id = Subscription_Id;
                    new UserBL().AddUser(u);

                    string Subject = "New International Supplier";
                    string Body = "<strong>Name: </strong>" + u.Name + "<br><strong>Email: </strong> " + u.Email + "<br><strong>Phone: </strong> " + u.Phone + "<br><strong>Website Address: </strong>"
                               + u.Website_Address + "<br><strong>Country: </strong> " + u.Country + "<br><strong>Role: </strong>Buyer<br><strong>Company Name: </strong>" + u.Company_Name
                               + "<br><strong>Company Number: </strong>" + u.Company_Number + "<br><strong>Company Address: </strong> " + u.Company_Address + "<br><strong>VAT Registration Number: </strong> " + u.VAT_Registration_Number;

                    MailToAdmin(u, Subject, Body);

                    return RedirectToAction("SupplierSignUpVideo", "Auth", new { Message = "An email is sent to the Admin to make you an authorized user of this website. You will get an email from Admin within 24 hours." });

                }

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public bool MailToAdmin(User u, string Subject, string Body)
        {
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress("no-reply@findingyousuppliers.com");
            msg.To.Add("support@findingyousuppliers.com");
            msg.Subject = Subject;
            msg.IsBodyHtml = true;
            msg.Body = Body;

            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = false;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("no-reply@findingyousuppliers.com", "harpareet@53");
                client.Host = "webmail.findingyousuppliers.com";
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Send(msg);
                return true;
            }
        }

        public ActionResult BuyerSignUpVideo(string mail, string Message)
        {
            if (sessiondto.getName() == null)
            {
                ViewBag.Message = Message;
                ViewBag.mail = mail;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public ActionResult PostBuyerSignUp(string mail, string option)
        {
            if (sessiondto.getName() == null)
            {
                MailMessage msg = new MailMessage();

                msg.From = new MailAddress("no-reply@findingyousuppliers.com");
                msg.To.Add("support@findingyousuppliers.com");
                msg.Subject = "Buyer's Selected Options";
                msg.IsBodyHtml = true;

                string o = "<p>Buyer's Email: </p><strong>" + mail + "</strong><br /> Buyer have selected following option:";


                if (option == "1")
                {
                    msg.Body = o + "<br /><p>* We would advise you on where and how to register your domain and hosting <br />•	Set up your fully functional eccommerce store with upto 100 products and integrated payment options so you are ready to start selling.<br />•	This package would cost £499</p> ";
                }
                else if (option == "2")
                {
                    msg.Body = o + "<br /><p>We would advise you on where and how to register your domain and hosting<br />◦	Set up your fully functional eccommerce store with upto 100 products and integrated payment options so you are ready to start selling.<br />◦	Set up your email marketing for you <br />◦	Set up your email marketing done for you templates<br />◦	Set up your social media <br />◦	This package would cost £799 <br /></p> ";
                }
                else
                {
                    msg.Body = o + "<br /><p>No Thank you, I need no more further assistance</p>";
                }

                using (SmtpClient client = new SmtpClient())
                {
                    client.EnableSsl = false;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("no-reply@findingyousuppliers.com", "harpareet@53");
                    client.Host = "webmail.webmail.findingyousuppliers.com";
                    client.Port = 25;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Send(msg);

                }

                return RedirectToAction("BuyerSignUp", "Auth", new { Message = "An email is sent to the Admin to make you an authorized user of this website. You will get an email from Admin within 24 hours." });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public ActionResult BuyerSignUp(string Message)
        {
            if (sessiondto.getName() == null)
            {
                ViewBag.Message = Message;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public ActionResult SupplierSignUpVideo(string Message)
        {
            if (sessiondto.getName() == null)
            {
                ViewBag.Message = Message;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public ActionResult Login(string Id, string err = "")
        {
            if (sessiondto.getName() == null)
            {
                ViewBag.message = err;
                ViewBag.Id = StringCipher.Base64Decode(Id);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult PostLogin(string Id, string Email, string Password)
        {
            List<User> Users = new UserBL().getUserList().Where(x => x.Is_Authorize == 1).ToList();

            foreach (User User in Users)
            {
                if (User.Email == Email && User.Password == Password)
                {
                    SessionDTO session = new SessionDTO();
                    session.Name = User.Name;
                    session.Id = User.Id;
                    session.Role = User.Role;
                    Session["Session"] = session;

                    SessionDTO sdto = (SessionDTO)Session["Session"];

                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("Login", "Auth", new { Id = StringCipher.Base64Encode(Id), err = "Incorrect Email or Password" });
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index", "Home");
        }



        public ActionResult EmailVerification(string err = "")
        {
            if (sessiondto.getName() == null)
            {
                ViewBag.message = err;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult PostEmailVerification(string Email)
        {
            User User = new UserBL().getUserList().Where(x => x.Is_Authorize == 1).FirstOrDefault(x => x.Email == Email);

            if (User == null)
            {
                return RedirectToAction("EmailVerification", "Auth", new { err = "The Email your Entered is not belonged to any Record." });
            }
            else
            {
                sendMail(Email);
                return RedirectToAction("EmailVerification", "Auth", new { err = "An Email has been sent to your Email to recover the password." });
            }
        }

        public bool sendMail(string email)
        {
            MailMessage msg = new MailMessage();

            string text = "<link href='https://fonts.googleapis.com/css?family=Bree+Serif' rel='stylesheet'><style>  * {";
            text += "  font-family: 'Bree Serif', serif; }";
            text += " .list-group-item {       border: none;  }    .hor {      border-bottom: 5px solid black;   }";
            text += " .line {       margin-bottom: 20px; }";

            msg.From = new MailAddress("no-reply@findingyousuppliers.com");
            msg.To.Add(email);
            msg.Subject = "Password Reset";
            msg.IsBodyHtml = true;
            string temp = "<html><head></head><body><nav class='navbar navbar-default'><div class='container-fluid'></div> </nav><center><div><img src='https://findingyousuppliers.com/Content/images/logo.png' alt='Logo' width='150' height='100' /><br /><h1 class='text-center'>Password Reset!</h1><p class='text-center'> Simply Click the button showing below to reset your password: </p><br><button style = 'background-color: rgb(0,174,239);'><a href='LINKFORFORGOTPASSWORD' style='text-decoration:none;font-size:15px;color:white;'>Reset Password</a></button></div></center><br />";

            temp += "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";
            string link = "https://findingyousuppliers.com/Auth/Reset?email=" + email;
            link = link.Replace("+", "%20");
            temp = temp.Replace("LINKFORFORGOTPASSWORD", link);
            msg.Body = temp;

            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = false;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("no-reply@findingyousuppliers.com", "harpareet@53");
                client.Host = "webmail.findingyousuppliers.com";
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Send(msg);
            }
            return true;
        }

        public ActionResult Reset(string email, string err = "")
        {
            ViewBag.Email = email;
            ViewBag.message = err;
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string Email, string Password, string ConfirmPassword)
        {
            User User = new UserBL().getUserList().Where(x => x.Is_Authorize == 1 && x.Email == Email).FirstOrDefault();

            if (Password == ConfirmPassword)
            {
                User.Password = Password;
                new UserBL().UpdateUser(User);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Reset", "Auth", new { email = Email, err = "Password and Confirm Password doesn't match" });

            }
        }

        public ActionResult SignupResult(string Message)
        {
            ViewBag.Message = Message;
            return View();
        }
    }
}
