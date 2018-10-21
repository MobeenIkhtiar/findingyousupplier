using Braintree;
using FindingYouSupplier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace FindingYouSupplier.Helping_Classes
{
    public class BrainTreePayment
    {
        public string token;
        BraintreeGateway gateway = new BraintreeGateway
        {
            Environment = Braintree.Environment.PRODUCTION,
            MerchantId = "tw8mmpsdjv5mpdmv",
            PublicKey = "h2y74q4yfyhfytjy",
            PrivateKey = "2d153108038cf5a50df47715a5a9bbc9"
        };

        public BrainTreePayment()
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            var clientToken = gateway.ClientToken.generate();
            token = clientToken;
            if (HttpContext.Current.Session != null)
                HttpContext.Current.Session["token"] = clientToken;
        }
        

        public string proceedPayment(string nonce, string package, User u,int trial)
        {
            string nonceFromTheClient = nonce;

            if (u.Company_Name == null)
            {
                u.Company_Name = "Not Given";
            }
            var request = new CustomerRequest
            {
                FirstName = u.Name,
                Company = u.Company_Name,
                Email = u.Email,
                Phone = u.Phone,
                Website = u.Website_Address,
            };
            Result<Customer> result = gateway.Customer.Create(request);

            if (result.IsSuccess())
            {

                string customerId = result.Target.Id;
                var request1 = new PaymentMethodRequest
                {
                    CustomerId = customerId,
                    PaymentMethodNonce = nonce,
                    Options = new PaymentMethodOptionsRequest
                    {
                        VerifyCard = true
                    }
                };

                Result<PaymentMethod> result1 = gateway.PaymentMethod.Create(request1);
                Result<Subscription> result2;
                if (result1.IsSuccess())
                {
                     
                    if(trial!=0)
                    {
                        var request2 = new SubscriptionRequest
                        {
                            PaymentMethodToken = result1.Target.Token,
                            PlanId = package,
                            Options = new SubscriptionOptionsRequest
                            {
                                StartImmediately = false
                            },
                            FirstBillingDate = DateTime.Now.AddMonths(trial)
                        };
                        result2 = gateway.Subscription.Create(request2);
                    }
                    else
                    {
                        var request2 = new SubscriptionRequest
                        {
                            PaymentMethodToken = result1.Target.Token,
                            PlanId = package,
                            Options = new SubscriptionOptionsRequest
                            {
                                StartImmediately = true
                            },
                        };
                        result2 = gateway.Subscription.Create(request2);
                    }
                    if (result2.IsSuccess())
                    {
                        string Subscription_Id = result2.Target.Id;
                        return Subscription_Id;
                    }
                    else
                    {
                        //Please check your funds.
                        return "-3";
                    }
                }
                else
                {
                    //Your card is valid or not.
                    return "-2";
                }
            }
            else
            {
                // Requirements of customer for Braintree is not complete
                return "-1";
            }

        }

    }
}