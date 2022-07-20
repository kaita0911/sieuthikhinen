namespace VCMS.MVC4.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DetailIdAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 5, unicode: false),
                        DisplayName = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Websites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Flag = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DefaultDomain = c.String(nullable: false, maxLength: 100, unicode: false),
                        Email = c.String(maxLength: 100, unicode: false),
                        Hotline = c.String(maxLength: 100, unicode: false),
                        Watermark = c.String(maxLength: 50),
                        WatermarkPosition = c.Byte(nullable: false),
                        Status = c.Int(nullable: false),
                        WebsiteAddressId = c.Int(),
                        DefaultLanguage = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WebsiteAddress", t => t.WebsiteAddressId)
                .Index(t => t.WebsiteAddressId);
            
            CreateTable(
                "dbo.WebsiteAddress",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Phone = c.String(maxLength: 100, unicode: false),
                        Fax = c.String(maxLength: 100, unicode: false),
                        Email = c.String(maxLength: 100, unicode: false),
                        Facebook = c.String(maxLength: 100, unicode: false),
                        Twitter = c.String(maxLength: 100, unicode: false),
                        GooglePlus = c.String(maxLength: 100, unicode: false),
                        Blog = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WebsiteAddressDetail",
                c => new
                    {
                        WebsiteId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(maxLength: 200),
                        Address = c.String(maxLength: 200),
                        ContactPerson = c.String(maxLength: 200),
                        SEOKeywords = c.String(maxLength: 100),
                        SEODescription = c.String(maxLength: 200),
                        WebsiteAddress_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.WebsiteId, t.LanguageId })
                .ForeignKey("dbo.WebsiteAddress", t => t.WebsiteAddress_Id)
                .Index(t => t.WebsiteAddress_Id);
            
            CreateTable(
                "dbo.WebsiteDetail",
                c => new
                    {
                        WebsiteId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Title = c.String(nullable: false, maxLength: 200),
                        SEODescription = c.String(maxLength: 200),
                        SEOKeywords = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => new { t.WebsiteId, t.LanguageId })
                .ForeignKey("dbo.Websites", t => t.WebsiteId, cascadeDelete: true)
                .Index(t => t.WebsiteId);
            
            CreateTable(
                "dbo.WebsiteConfigValues",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        WebsiteId = c.Int(nullable: false),
                        WebsiteConfigId = c.Int(nullable: false),
                        LanguageId = c.Int(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Websites", t => t.WebsiteId, cascadeDelete: true)
                .ForeignKey("dbo.WebsiteConfigs", t => t.WebsiteConfigId, cascadeDelete: true)
                .Index(t => t.WebsiteId)
                .Index(t => t.WebsiteConfigId);
            
            CreateTable(
                "dbo.WebsiteConfigs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20, unicode: false),
                        MultiLanguage = c.Boolean(nullable: false),
                        DefaultValue = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArticleDetail",
                c => new
                    {
                        LanguageId = c.Int(nullable: false),
                        Id = c.Int(nullable: false, identity: true),
                        ArticleId = c.Int(nullable: false),
                        ArticleName = c.String(nullable: false, maxLength: 200),
                        UrlFriendly = c.String(maxLength: 200, unicode: false),
                        ShortDesc = c.String(),
                        Description = c.String(),
                        SEOKeywords = c.String(maxLength: 200),
                        SEODescription = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.ArticleId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        Status = c.Int(),
                        SortOrder = c.Int(),
                        Rating = c.Int(),
                        Number = c.Int(),
                        Flags = c.Int(nullable: false),
                        UserCreated = c.Int(nullable: false),
                        UserLastUpdated = c.Int(),
                        ImageUrl = c.String(maxLength: 200),
                        WebsiteId = c.Int(nullable: false),
                        WebsiteCode = c.String(maxLength: 50),
                        ArticleTypeId = c.Int(nullable: false),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.UserCreated, cascadeDelete: true)
                .ForeignKey("dbo.ArticleTypes", t => t.ArticleTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Articles", t => t.ParentId)
                .Index(t => t.UserCreated)
                .Index(t => t.ArticleTypeId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 50, unicode: false),
                        DisplayName = c.String(maxLength: 100),
                        Email = c.String(maxLength: 100, unicode: false),
                        Phone = c.String(maxLength: 50, unicode: false),
                        DateCreated = c.DateTime(),
                        Address = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.ArticleTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50, unicode: false),
                        Flag = c.Int(nullable: false),
                        PropertyFlag = c.Int(nullable: false),
                        DisplayFlag = c.Int(nullable: false),
                        AttributeFlag = c.Int(nullable: false),
                        ShoppingCartFlag = c.Int(nullable: false),
                        WebsiteId = c.Int(nullable: false),
                        SortOrder = c.Int(),
                        HomePageType = c.Int(nullable: false),
                        CategoryPageType = c.Int(nullable: false),
                        StrIsNew = c.String(),
                        StrIsHot = c.String(),
                        StrIsMostView = c.String(),
                        StrIsNewCate = c.String(),
                        StrIsHotCate = c.String(),
                        StrIsMostViewCate = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArticleTypeDetail",
                c => new
                    {
                        ArticleTypeId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        UrlFriendly = c.String(maxLength: 100, unicode: false),
                        Description = c.String(),
                        SEOKeywords = c.String(),
                        SEODescription = c.String(),
                    })
                .PrimaryKey(t => new { t.ArticleTypeId, t.LanguageId })
                .ForeignKey("dbo.ArticleTypes", t => t.ArticleTypeId, cascadeDelete: true)
                .Index(t => t.ArticleTypeId);
            
            CreateTable(
                "dbo.Properties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20, unicode: false),
                        MultiLanguage = c.Boolean(nullable: false),
                        MultiValue = c.Boolean(nullable: false),
                        PropertyType = c.Byte(nullable: false),
                        Choices = c.String(),
                        EntityType = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PropertyDetail",
                c => new
                    {
                        PropertyId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => new { t.PropertyId, t.LanguageId })
                .ForeignKey("dbo.Properties", t => t.PropertyId, cascadeDelete: true)
                .Index(t => t.PropertyId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        UserCreatedId = c.Int(),
                        UserUpdatedId = c.Int(),
                        Status = c.Int(nullable: false),
                        SortOrder = c.Int(nullable: false),
                        ImageUrl = c.String(maxLength: 200, unicode: false),
                        ParentId = c.Int(),
                        WebsiteId = c.Int(nullable: false),
                        CategoryTypeId = c.Int(),
                        ArticleTypeId = c.Int(),
                        Flags = c.Int(nullable: false),
                        Font = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.ParentId)
                .ForeignKey("dbo.CategoryTypes", t => t.CategoryTypeId)
                .ForeignKey("dbo.ArticleTypes", t => t.ArticleTypeId)
                .Index(t => t.ParentId)
                .Index(t => t.CategoryTypeId)
                .Index(t => t.ArticleTypeId);
            
            CreateTable(
                "dbo.CategoryTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CategoryTypeDetail",
                c => new
                    {
                        CategoryTypeId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(maxLength: 100),
                        Description = c.String(),
                    })
                .PrimaryKey(t => new { t.CategoryTypeId, t.LanguageId })
                .ForeignKey("dbo.CategoryTypes", t => t.CategoryTypeId, cascadeDelete: true)
                .Index(t => t.CategoryTypeId);
            
            CreateTable(
                "dbo.CategoryDetails",
                c => new
                    {
                        CategoryId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        CategoryName = c.String(nullable: false, maxLength: 200),
                        UrlFriendly = c.String(maxLength: 200, unicode: false),
                        Description = c.String(),
                        SEOKeywords = c.String(maxLength: 100),
                        SEODescription = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => new { t.CategoryId, t.LanguageId })
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Discounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        DateStart = c.DateTime(nullable: false),
                        DateEnd = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        DiscountPercent = c.Decimal(precision: 18, scale: 2),
                        DiscountAmount = c.Decimal(precision: 18, scale: 2),
                        UsePercent = c.Boolean(nullable: false),
                        AllItems = c.Boolean(nullable: false),
                        CurrencyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currency", t => t.CurrencyId, cascadeDelete: true)
                .Index(t => t.CurrencyId);
            
            CreateTable(
                "dbo.Currency",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 10),
                        Name = c.String(nullable: false, maxLength: 200),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsDefault = c.Boolean(nullable: false),
                        Formatting = c.String(),
                        CheckFormat = c.Boolean(nullable: false),
                        CurrencyPositivePattern = c.Int(nullable: false),
                        CurrencySymbol = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArticleId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        Email = c.String(),
                        Name = c.String(),
                        Message = c.String(),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .Index(t => t.ArticleId);
            
            CreateTable(
                "dbo.PropertyMultiValue",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PropertyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Properties", t => t.PropertyId, cascadeDelete: true)
                .Index(t => t.PropertyId);
            
            CreateTable(
                "dbo.PropertyMultiValueDetail",
                c => new
                    {
                        PropertyMultiValueId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => new { t.PropertyMultiValueId, t.LanguageId })
                .ForeignKey("dbo.PropertyMultiValue", t => t.PropertyMultiValueId, cascadeDelete: true)
                .Index(t => t.PropertyMultiValueId);
            
            CreateTable(
                "dbo.ArticlePropertyValue",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        MultiId = c.Guid(nullable: false),
                        ArticleId = c.Int(nullable: false),
                        PropertyId = c.Int(nullable: false),
                        LanguageId = c.Int(),
                        Value = c.String(),
                        Name = c.String(maxLength: 50),
                        SortOrder = c.Int(),
                        IsDefault = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.MultiId })
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.Properties", t => t.PropertyId, cascadeDelete: true)
                .Index(t => t.ArticleId)
                .Index(t => t.PropertyId);
            
            CreateTable(
                "dbo.Keywords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tag = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArticleFile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArticleId = c.Int(nullable: false),
                        FileType = c.Int(nullable: false),
                        OriginalFileName = c.String(maxLength: 300),
                        FileName = c.String(maxLength: 300),
                        FullPath = c.String(maxLength: 300),
                        FileSize = c.Int(nullable: false),
                        IsDefault = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .Index(t => t.ArticleId);
            
            CreateTable(
                "dbo.ArticleFileDetail",
                c => new
                    {
                        ArticleFileId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Title = c.String(maxLength: 200),
                        Description = c.String(),
                    })
                .PrimaryKey(t => new { t.ArticleFileId, t.LanguageId })
                .ForeignKey("dbo.ArticleFile", t => t.ArticleFileId, cascadeDelete: true)
                .Index(t => t.ArticleFileId);
            
            CreateTable(
                "dbo.Price",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArticleId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 300),
                        IsDefault = c.Boolean(nullable: false),
                        Inactive = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        SortOrder = c.Int(),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceShortOrder = c.Int(),
                        CurrencyId = c.Int(nullable: false),
                        PropertyMultiValueId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.Currency", t => t.CurrencyId, cascadeDelete: true)
                .ForeignKey("dbo.PropertyMultiValue", t => t.PropertyMultiValueId)
                .Index(t => t.ArticleId)
                .Index(t => t.CurrencyId)
                .Index(t => t.PropertyMultiValueId);
            
            CreateTable(
                "dbo.WebsitePlugins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20, unicode: false),
                        Name = c.String(maxLength: 50),
                        Flag = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MenuItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WebsiteId = c.Int(nullable: false),
                        SortOrder = c.Int(nullable: false),
                        ItemType = c.Int(nullable: false),
                        Flag = c.Int(nullable: false),
                        Url = c.String(),
                        ArticleTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Websites", t => t.WebsiteId, cascadeDelete: true)
                .ForeignKey("dbo.ArticleTypes", t => t.ArticleTypeId)
                .Index(t => t.WebsiteId)
                .Index(t => t.ArticleTypeId);
            
            CreateTable(
                "dbo.MenuItemDetails",
                c => new
                    {
                        MenuItemId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Text = c.String(),
                    })
                .PrimaryKey(t => new { t.MenuItemId, t.LanguageId })
                .ForeignKey("dbo.MenuItems", t => t.MenuItemId, cascadeDelete: true)
                .Index(t => t.MenuItemId);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        UserId = c.Int(),
                        Anonymous = c.Boolean(nullable: false),
                        UniqueId = c.Guid(nullable: false),
                        WebsiteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CartItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArticleId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CartId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.Carts", t => t.CartId, cascadeDelete: true)
                .Index(t => t.ArticleId)
                .Index(t => t.CartId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderNumber = c.String(maxLength: 20, unicode: false),
                        DateCreated = c.DateTime(nullable: false),
                        CustomerId = c.Int(),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalQty = c.Int(nullable: false),
                        Username = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        PostalCode = c.String(),
                        Country = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        WebsiteId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.CustomerId)
                .ForeignKey("dbo.Websites", t => t.WebsiteId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.WebsiteId);
            
            CreateTable(
                "dbo.OrderLines",
                c => new
                    {
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ProductName = c.String(),
                        Qty = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.OrderId, t.ProductId })
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.OrderHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        UserId = c.Int(),
                        Status = c.Int(nullable: false),
                        DateChanged = c.DateTime(nullable: false),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .Index(t => t.OrderId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Widget",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20, unicode: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Class = c.String(),
                        WidgetType = c.Int(nullable: false),
                        Flag = c.Int(nullable: false),
                        ArticleTypeId = c.Int(nullable: false),
                        WidgetAttribute = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        WidgetSortOrder = c.Int(nullable: false),
                        WidgetSortDirection = c.Int(nullable: false),
                        WidgetView = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WidgetDetail",
                c => new
                    {
                        WidgetId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Title = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => new { t.WidgetId, t.LanguageId })
                .ForeignKey("dbo.Widget", t => t.WidgetId, cascadeDelete: true)
                .Index(t => t.WidgetId);
            
            CreateTable(
                "dbo.Locales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LocaleKey = c.String(maxLength: 100, unicode: false),
                        DefaultValue = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LocaleDetails",
                c => new
                    {
                        LocaleId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => new { t.LocaleId, t.LanguageId })
                .ForeignKey("dbo.Locales", t => t.LocaleId, cascadeDelete: true)
                .Index(t => t.LocaleId);
            
            CreateTable(
                "dbo.WebsiteLanguages",
                c => new
                    {
                        Website_Id = c.Int(nullable: false),
                        Language_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Website_Id, t.Language_Id })
                .ForeignKey("dbo.Websites", t => t.Website_Id, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.Language_Id, cascadeDelete: true)
                .Index(t => t.Website_Id)
                .Index(t => t.Language_Id);
            
            CreateTable(
                "dbo.PropertyArticleTypes",
                c => new
                    {
                        Property_Id = c.Int(nullable: false),
                        ArticleType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Property_Id, t.ArticleType_Id })
                .ForeignKey("dbo.Properties", t => t.Property_Id, cascadeDelete: true)
                .ForeignKey("dbo.ArticleTypes", t => t.ArticleType_Id, cascadeDelete: true)
                .Index(t => t.Property_Id)
                .Index(t => t.ArticleType_Id);
            
            CreateTable(
                "dbo.CategoryTypeArticleTypes",
                c => new
                    {
                        CategoryType_Id = c.Int(nullable: false),
                        ArticleType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CategoryType_Id, t.ArticleType_Id })
                .ForeignKey("dbo.CategoryTypes", t => t.CategoryType_Id, cascadeDelete: true)
                .ForeignKey("dbo.ArticleTypes", t => t.ArticleType_Id, cascadeDelete: true)
                .Index(t => t.CategoryType_Id)
                .Index(t => t.ArticleType_Id);
            
            CreateTable(
                "dbo.DiscountArticles",
                c => new
                    {
                        Discount_Id = c.Int(nullable: false),
                        Article_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Discount_Id, t.Article_Id })
                .ForeignKey("dbo.Discounts", t => t.Discount_Id, cascadeDelete: true)
                .ForeignKey("dbo.Articles", t => t.Article_Id, cascadeDelete: true)
                .Index(t => t.Discount_Id)
                .Index(t => t.Article_Id);
            
            CreateTable(
                "dbo.DiscountCategories",
                c => new
                    {
                        Discount_Id = c.Int(nullable: false),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Discount_Id, t.Category_Id })
                .ForeignKey("dbo.Discounts", t => t.Discount_Id, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: true)
                .Index(t => t.Discount_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.ArticleCategories",
                c => new
                    {
                        Article_Id = c.Int(nullable: false),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Article_Id, t.Category_Id })
                .ForeignKey("dbo.Articles", t => t.Article_Id, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: true)
                .Index(t => t.Article_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.PropertyMultiValueArticles",
                c => new
                    {
                        PropertyMultiValue_Id = c.Int(nullable: false),
                        Article_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PropertyMultiValue_Id, t.Article_Id })
                .ForeignKey("dbo.PropertyMultiValue", t => t.PropertyMultiValue_Id, cascadeDelete: true)
                .ForeignKey("dbo.Articles", t => t.Article_Id, cascadeDelete: true)
                .Index(t => t.PropertyMultiValue_Id)
                .Index(t => t.Article_Id);
            
            CreateTable(
                "dbo.KeywordArticles",
                c => new
                    {
                        Keyword_Id = c.Int(nullable: false),
                        Article_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Keyword_Id, t.Article_Id })
                .ForeignKey("dbo.Keywords", t => t.Keyword_Id, cascadeDelete: true)
                .ForeignKey("dbo.Articles", t => t.Article_Id, cascadeDelete: true)
                .Index(t => t.Keyword_Id)
                .Index(t => t.Article_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.KeywordArticles", new[] { "Article_Id" });
            DropIndex("dbo.KeywordArticles", new[] { "Keyword_Id" });
            DropIndex("dbo.PropertyMultiValueArticles", new[] { "Article_Id" });
            DropIndex("dbo.PropertyMultiValueArticles", new[] { "PropertyMultiValue_Id" });
            DropIndex("dbo.ArticleCategories", new[] { "Category_Id" });
            DropIndex("dbo.ArticleCategories", new[] { "Article_Id" });
            DropIndex("dbo.DiscountCategories", new[] { "Category_Id" });
            DropIndex("dbo.DiscountCategories", new[] { "Discount_Id" });
            DropIndex("dbo.DiscountArticles", new[] { "Article_Id" });
            DropIndex("dbo.DiscountArticles", new[] { "Discount_Id" });
            DropIndex("dbo.CategoryTypeArticleTypes", new[] { "ArticleType_Id" });
            DropIndex("dbo.CategoryTypeArticleTypes", new[] { "CategoryType_Id" });
            DropIndex("dbo.PropertyArticleTypes", new[] { "ArticleType_Id" });
            DropIndex("dbo.PropertyArticleTypes", new[] { "Property_Id" });
            DropIndex("dbo.WebsiteLanguages", new[] { "Language_Id" });
            DropIndex("dbo.WebsiteLanguages", new[] { "Website_Id" });
            DropIndex("dbo.LocaleDetails", new[] { "LocaleId" });
            DropIndex("dbo.WidgetDetail", new[] { "WidgetId" });
            DropIndex("dbo.OrderHistories", new[] { "UserId" });
            DropIndex("dbo.OrderHistories", new[] { "OrderId" });
            DropIndex("dbo.OrderLines", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "WebsiteId" });
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropIndex("dbo.CartItems", new[] { "CartId" });
            DropIndex("dbo.CartItems", new[] { "ArticleId" });
            DropIndex("dbo.Carts", new[] { "UserId" });
            DropIndex("dbo.MenuItemDetails", new[] { "MenuItemId" });
            DropIndex("dbo.MenuItems", new[] { "ArticleTypeId" });
            DropIndex("dbo.MenuItems", new[] { "WebsiteId" });
            DropIndex("dbo.Price", new[] { "PropertyMultiValueId" });
            DropIndex("dbo.Price", new[] { "CurrencyId" });
            DropIndex("dbo.Price", new[] { "ArticleId" });
            DropIndex("dbo.ArticleFileDetail", new[] { "ArticleFileId" });
            DropIndex("dbo.ArticleFile", new[] { "ArticleId" });
            DropIndex("dbo.ArticlePropertyValue", new[] { "PropertyId" });
            DropIndex("dbo.ArticlePropertyValue", new[] { "ArticleId" });
            DropIndex("dbo.PropertyMultiValueDetail", new[] { "PropertyMultiValueId" });
            DropIndex("dbo.PropertyMultiValue", new[] { "PropertyId" });
            DropIndex("dbo.Comments", new[] { "ArticleId" });
            DropIndex("dbo.Discounts", new[] { "CurrencyId" });
            DropIndex("dbo.CategoryDetails", new[] { "CategoryId" });
            DropIndex("dbo.CategoryTypeDetail", new[] { "CategoryTypeId" });
            DropIndex("dbo.Categories", new[] { "ArticleTypeId" });
            DropIndex("dbo.Categories", new[] { "CategoryTypeId" });
            DropIndex("dbo.Categories", new[] { "ParentId" });
            DropIndex("dbo.PropertyDetail", new[] { "PropertyId" });
            DropIndex("dbo.ArticleTypeDetail", new[] { "ArticleTypeId" });
            DropIndex("dbo.Articles", new[] { "ParentId" });
            DropIndex("dbo.Articles", new[] { "ArticleTypeId" });
            DropIndex("dbo.Articles", new[] { "UserCreated" });
            DropIndex("dbo.ArticleDetail", new[] { "LanguageId" });
            DropIndex("dbo.ArticleDetail", new[] { "ArticleId" });
            DropIndex("dbo.WebsiteConfigValues", new[] { "WebsiteConfigId" });
            DropIndex("dbo.WebsiteConfigValues", new[] { "WebsiteId" });
            DropIndex("dbo.WebsiteDetail", new[] { "WebsiteId" });
            DropIndex("dbo.WebsiteAddressDetail", new[] { "WebsiteAddress_Id" });
            DropIndex("dbo.Websites", new[] { "WebsiteAddressId" });
            DropForeignKey("dbo.KeywordArticles", "Article_Id", "dbo.Articles");
            DropForeignKey("dbo.KeywordArticles", "Keyword_Id", "dbo.Keywords");
            DropForeignKey("dbo.PropertyMultiValueArticles", "Article_Id", "dbo.Articles");
            DropForeignKey("dbo.PropertyMultiValueArticles", "PropertyMultiValue_Id", "dbo.PropertyMultiValue");
            DropForeignKey("dbo.ArticleCategories", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.ArticleCategories", "Article_Id", "dbo.Articles");
            DropForeignKey("dbo.DiscountCategories", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.DiscountCategories", "Discount_Id", "dbo.Discounts");
            DropForeignKey("dbo.DiscountArticles", "Article_Id", "dbo.Articles");
            DropForeignKey("dbo.DiscountArticles", "Discount_Id", "dbo.Discounts");
            DropForeignKey("dbo.CategoryTypeArticleTypes", "ArticleType_Id", "dbo.ArticleTypes");
            DropForeignKey("dbo.CategoryTypeArticleTypes", "CategoryType_Id", "dbo.CategoryTypes");
            DropForeignKey("dbo.PropertyArticleTypes", "ArticleType_Id", "dbo.ArticleTypes");
            DropForeignKey("dbo.PropertyArticleTypes", "Property_Id", "dbo.Properties");
            DropForeignKey("dbo.WebsiteLanguages", "Language_Id", "dbo.Languages");
            DropForeignKey("dbo.WebsiteLanguages", "Website_Id", "dbo.Websites");
            DropForeignKey("dbo.LocaleDetails", "LocaleId", "dbo.Locales");
            DropForeignKey("dbo.WidgetDetail", "WidgetId", "dbo.Widget");
            DropForeignKey("dbo.OrderHistories", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.OrderHistories", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderLines", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "WebsiteId", "dbo.Websites");
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.UserProfile");
            DropForeignKey("dbo.CartItems", "CartId", "dbo.Carts");
            DropForeignKey("dbo.CartItems", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.Carts", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.MenuItemDetails", "MenuItemId", "dbo.MenuItems");
            DropForeignKey("dbo.MenuItems", "ArticleTypeId", "dbo.ArticleTypes");
            DropForeignKey("dbo.MenuItems", "WebsiteId", "dbo.Websites");
            DropForeignKey("dbo.Price", "PropertyMultiValueId", "dbo.PropertyMultiValue");
            DropForeignKey("dbo.Price", "CurrencyId", "dbo.Currency");
            DropForeignKey("dbo.Price", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.ArticleFileDetail", "ArticleFileId", "dbo.ArticleFile");
            DropForeignKey("dbo.ArticleFile", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.ArticlePropertyValue", "PropertyId", "dbo.Properties");
            DropForeignKey("dbo.ArticlePropertyValue", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.PropertyMultiValueDetail", "PropertyMultiValueId", "dbo.PropertyMultiValue");
            DropForeignKey("dbo.PropertyMultiValue", "PropertyId", "dbo.Properties");
            DropForeignKey("dbo.Comments", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.Discounts", "CurrencyId", "dbo.Currency");
            DropForeignKey("dbo.CategoryDetails", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.CategoryTypeDetail", "CategoryTypeId", "dbo.CategoryTypes");
            DropForeignKey("dbo.Categories", "ArticleTypeId", "dbo.ArticleTypes");
            DropForeignKey("dbo.Categories", "CategoryTypeId", "dbo.CategoryTypes");
            DropForeignKey("dbo.Categories", "ParentId", "dbo.Categories");
            DropForeignKey("dbo.PropertyDetail", "PropertyId", "dbo.Properties");
            DropForeignKey("dbo.ArticleTypeDetail", "ArticleTypeId", "dbo.ArticleTypes");
            DropForeignKey("dbo.Articles", "ParentId", "dbo.Articles");
            DropForeignKey("dbo.Articles", "ArticleTypeId", "dbo.ArticleTypes");
            DropForeignKey("dbo.Articles", "UserCreated", "dbo.UserProfile");
            DropForeignKey("dbo.ArticleDetail", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.ArticleDetail", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.WebsiteConfigValues", "WebsiteConfigId", "dbo.WebsiteConfigs");
            DropForeignKey("dbo.WebsiteConfigValues", "WebsiteId", "dbo.Websites");
            DropForeignKey("dbo.WebsiteDetail", "WebsiteId", "dbo.Websites");
            DropForeignKey("dbo.WebsiteAddressDetail", "WebsiteAddress_Id", "dbo.WebsiteAddress");
            DropForeignKey("dbo.Websites", "WebsiteAddressId", "dbo.WebsiteAddress");
            DropTable("dbo.KeywordArticles");
            DropTable("dbo.PropertyMultiValueArticles");
            DropTable("dbo.ArticleCategories");
            DropTable("dbo.DiscountCategories");
            DropTable("dbo.DiscountArticles");
            DropTable("dbo.CategoryTypeArticleTypes");
            DropTable("dbo.PropertyArticleTypes");
            DropTable("dbo.WebsiteLanguages");
            DropTable("dbo.LocaleDetails");
            DropTable("dbo.Locales");
            DropTable("dbo.WidgetDetail");
            DropTable("dbo.Widget");
            DropTable("dbo.OrderHistories");
            DropTable("dbo.OrderLines");
            DropTable("dbo.Orders");
            DropTable("dbo.CartItems");
            DropTable("dbo.Carts");
            DropTable("dbo.MenuItemDetails");
            DropTable("dbo.MenuItems");
            DropTable("dbo.WebsitePlugins");
            DropTable("dbo.Price");
            DropTable("dbo.ArticleFileDetail");
            DropTable("dbo.ArticleFile");
            DropTable("dbo.Keywords");
            DropTable("dbo.ArticlePropertyValue");
            DropTable("dbo.PropertyMultiValueDetail");
            DropTable("dbo.PropertyMultiValue");
            DropTable("dbo.Comments");
            DropTable("dbo.Currency");
            DropTable("dbo.Discounts");
            DropTable("dbo.CategoryDetails");
            DropTable("dbo.CategoryTypeDetail");
            DropTable("dbo.CategoryTypes");
            DropTable("dbo.Categories");
            DropTable("dbo.PropertyDetail");
            DropTable("dbo.Properties");
            DropTable("dbo.ArticleTypeDetail");
            DropTable("dbo.ArticleTypes");
            DropTable("dbo.UserProfile");
            DropTable("dbo.Articles");
            DropTable("dbo.ArticleDetail");
            DropTable("dbo.WebsiteConfigs");
            DropTable("dbo.WebsiteConfigValues");
            DropTable("dbo.WebsiteDetail");
            DropTable("dbo.WebsiteAddressDetail");
            DropTable("dbo.WebsiteAddress");
            DropTable("dbo.Websites");
            DropTable("dbo.Languages");
        }
    }
}
