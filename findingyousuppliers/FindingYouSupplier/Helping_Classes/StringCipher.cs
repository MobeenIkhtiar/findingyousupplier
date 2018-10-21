﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindingYouSupplier.Helping_Classes
{
    public class StringCipher
    {
        public static string Base64Encode(string plainText)
        {
            plainText = plainText.Replace(" ", "+");
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}