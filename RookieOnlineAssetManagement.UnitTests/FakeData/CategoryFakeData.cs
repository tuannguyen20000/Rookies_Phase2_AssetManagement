using RookieOnlineAssetManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.UnitTests.FakeData
{
    public class CategoryFakeData
    {
        public static List<Category> ListCategories()
        {
            return new List<Category>() {
                new Category()
                {
                    Id = 1,
                    CategoryName = "Cate1",
                    CategoryPrefix = "CA",
                    CreatedDate = DateTime.Now.Date
                },
                new Category()
                {
                    Id = 2,
                    CategoryName = "Cate2",
                    CategoryPrefix = "CB",
                    CreatedDate = DateTime.Now.Date
                },
                new Category()
                {
                    Id = 3,
                    CategoryName = "Cate3",
                    CategoryPrefix = "CC",
                    CreatedDate = DateTime.Now.Date
                },
                new Category()
                {
                    Id = 4,
                    CategoryName = "Cate4",
                    CategoryPrefix = "CD",
                    CreatedDate = DateTime.Now.Date
                },
            };
        }
    }
}
