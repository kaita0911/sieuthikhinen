using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCMS.MVC4.Data;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;
using System.Data.Entity;
using System.IO;

namespace VCMS.MVC4.Initializer
{
    public class VCMSDataInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            //init language list
            List<Language> lst = new List<Language>() 
            { new Language() { Code = "vi", Locale= "vi-VN", DisplayName = "Tiếng Việt" }, 
              new Language() { Code = "en", Locale= "en-US", DisplayName = "English" } };
            
            //init default website
            context.Websites.Add(new Website()
            {
                Code = "VienNam",
                DateCreated = DateTime.Now,
                Status = WebsiteStatus.ACTIVE,
                DefaultDomain = "vcms.viennam.info",
                DefaultLanguage = 1,
                Flag = 0,
                Languages = lst,
                AmountShippingToFree = 0.0M,
                WatermarkPosition = 0,
                WebsiteDetail = new VList<WebsiteDetail> { new WebsiteDetail { Name = "Viễn Nam",  Title="Web Viễn Nam", LanguageId = 1 }, 
                    new WebsiteDetail { Name = "Viễn Nam",Title="Web Viễn Nam", LanguageId = 2 } }
            });
            
            var at = context.ArticleTypes.Add(new ArticleType()
            {
                Code = "NEWS",
                ArticleTypeDetail = new VList<ArticleTypeDetail>() { 
                    new ArticleTypeDetail(){LanguageId = 1, Name="Tin tức", UrlFriendly="Tin-tuc"},
                    new ArticleTypeDetail(){LanguageId = 2, Name="News", UrlFriendly="News"}
                },
                Flag = ArticleTypeFlags.ALL,
                PropertyFlag = PropertyFlags.IMAGE | PropertyFlags.DESCRIPTION | PropertyFlags.META,
                DisplayFlag = 0,
                AttributeFlag = 0,
                ShoppingCartFlag = 0,
                HomePageType = 0,
                CategoryPageType = 0
            });
            
            context.ArticleTypes.Add(new ArticleType()
            {
                Code = "PRODUCT",
                ArticleTypeDetail = new VList<ArticleTypeDetail>() { 
                    new ArticleTypeDetail(){LanguageId = 1, Name="Sản phẩm",UrlFriendly="San-pham"},
                    new ArticleTypeDetail(){LanguageId = 2, Name="Products",UrlFriendly="Product"}
                },
                Flag = ArticleTypeFlags.ALL,
                PropertyFlag = PropertyFlags.ALL,
                DisplayFlag = 0,
                AttributeFlag = 0,
                ShoppingCartFlag = 0,
                HomePageType = 0,
                CategoryPageType = 0
            });

            context.ArticleTypes.Add(new ArticleType()
            {
                Code = "ABOUTUS",
                ArticleTypeDetail = new VList<ArticleTypeDetail>() { 
                    new ArticleTypeDetail(){LanguageId = 1, Name="Giới thiệu",UrlFriendly="Gioi-thieu"},
                    new ArticleTypeDetail(){LanguageId = 2, Name="About Us",UrlFriendly="About-us"}
                },
                PropertyFlag = PropertyFlags.IMAGE | PropertyFlags.DESCRIPTION | PropertyFlags.META,
                Flag = ArticleTypeFlags.ALL,
                DisplayFlag = 0,
                AttributeFlag = 0,
                ShoppingCartFlag = 0,
                HomePageType = 0,
                CategoryPageType = 0
            });

            context.CategoryTypes.Add(new CategoryType()
            {
                Code = "BRAND",
                NoneType = false,
                CategoryTypeDetail = new VList<CategoryTypeDetail>() { 
                    new CategoryTypeDetail(){LanguageId = 1, Name="Thương hiệu"},
                    new CategoryTypeDetail(){LanguageId = 2, Name="Brand"}
                }
            });

            context.Currencies.Add(new Currency()
                {
                    Code = "VND",
                    Name = "Việt Nam đồng",
                    Rate = 21000,
                    Formatting = "0:#,##0",
                    IsDefault = true,
                    CheckFormat= false,
                    CurrencyPositivePattern = 0
                });
            context.SaveChanges();

            //init users and roles
            WebSecurity.InitializeDatabaseConnection("VCMS.DataConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            WebSecurity.CreateUserAndAccount("sa", "Cviennam", new { DisplayName = "Super Admin", DateCreated = DateTime.Now, Email = "support@viennam.com", City = 0, State = 0, Newsletter = 0, Flags = 0, Accumulated = 0 });
            WebSecurity.CreateUserAndAccount("kinhdoanh", "C123456789", new { DisplayName = "kinh doanh", DateCreated = DateTime.Now, Email = "kinhdoanh@viennam.com", City = 0, State = 0, Newsletter = 0, Flags = 0, Accumulated = 0 });
            WebSecurity.CreateUserAndAccount("admin", "C123456789", new { DisplayName = "Admin", DateCreated = DateTime.Now, Email = "admin@viennam.com", City = 0, State = 0, Newsletter = 0, Flags = 0, Accumulated = 0 });
            Roles.CreateRole("Super Administrators");
            Roles.CreateRole("Administrators");
            Roles.CreateRole("Moderators");
            Roles.CreateRole("Users");
            Roles.CreateRole("Guests");
            Roles.AddUserToRole("sa", "Super Administrators");
            Roles.AddUserToRole("kinhdoanh", "Administrators");
            Roles.AddUserToRole("admin", "Administrators");
        }  
    }

    public class CreateTableInitialize<T> : IDatabaseInitializer<T> where   T: DataContext
    {
        public void InitializeDatabase(T context)
        {   
        }
    }
}
