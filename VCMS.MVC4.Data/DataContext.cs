using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace VCMS.MVC4.Data
{

    public class DataContext : System.Data.Entity.DbContext
    {
        public DataContext()
            : base("VCMS.DataConnection")
        {
            //this.Configuration.AutoDetectChangesEnabled = false;
            
        }
        public void Initialize()
        {
            if (!this.Database.CompatibleWithModel(false))
            {
            }
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<Article>().HasMany(a => a.Categories).WithMany(c => c.Articles);

            modelBuilder.Entity<ArticleDetail>().HasKey(d => new { d.ArticleId, d.LanguageId });

            modelBuilder.Entity<ArticleDetail>().HasRequired(d => d.Body).WithRequiredPrincipal(b => b.ArticleDetail);

            //modelBuilder.Entity<ArticleDetail>()
            //.HasRequired(a => a.Body)
            //.WithRequiredPrincipal();
            //Map(mc =>
            //{
            //    mc.ToTable("ArticleCategory");
            //    mc.MapLeftKey("ArticleId");
            //    mc.MapRightKey("CategoryId");
            //});
        }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleDetail> ArticleDetails { get; set; }
        public DbSet<ArticlePropertyValue> ArticlePropertyValues { get; set; }
        public DbSet<PropertyMultiValue> PropertyMultiValues { get; set; }
        public DbSet<Website> Websites { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryDetail> CategoryDetails { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<WebsitePlugin> WebsitePlugins { get; set; }
        public DbSet<ArticleType> ArticleTypes { get; set; }
        public DbSet<ArticleTypeDetail> ArticleTypeDetails { get; set; }
        public DbSet<CategoryType> CategoryTypes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<UserProfile> Users { get; set; }
        public DbSet<ArticleFile> ArticleFiles { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuItemDetail> MenuItemDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<UserArticleTypeCate> UserArticleTypeCates { get; set; }
        public DbSet<CartItemFile> CartItemFiles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<OrderHistory> OrderHistories { get; set; }
        public DbSet<Widget> Widget { get; set; }
        public DbSet<WidgetDetail> WidgetDetail { get; set; }
        public DbSet<WidgetGroup> WidgetGroup { get; set; }
        public DbSet<WebsiteConfig> WebsiteConfigs { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<Locale> Locales { get; set; }
        public DbSet<LocaleDetail> LocaleDetails { get; set; }
        public DbSet<Discount> Discounts { get; set; }
    }
}


