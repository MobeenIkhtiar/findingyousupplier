using FindingYouSupplier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindingYouSupplier.Helping_Classes
{
    public class CategoryDTO
    {
            public int CategoryId { get; set; }
            public string Category_Name { get; set; }
            public List<Category> Sub_Categories  { get; set; }
            public int Count { get; set; }
    }
}
