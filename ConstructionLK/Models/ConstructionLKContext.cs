using Microsoft.AspNet.Identity.EntityFramework;

namespace ConstructionLK.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    public partial class ConstructionLKContext : DbContext 
    {
        public ConstructionLKContext()
        //: base("name=ConstructionLKContext")
        //: base("name=Techwire")
        : base("name=Azure")
        //: base("name=MRTVS")
        {
        }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<MembershipType> MembershipTypes { get; set; }
        public virtual DbSet<AdministrativeStaff> AdministrativeStaffs { get; set; }
        public virtual DbSet<BlogCategory> BlogCategories { get; set; }
        public virtual DbSet<BlogPostComment> BlogPostComments { get; set; }
        public virtual DbSet<BlogPostImage> BlogPostImages { get; set; }
        public virtual DbSet<BlogPost> BlogPosts { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<ContactType> ContactTypes { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<ItemCategoryIcon> ItemCategoryIcons { get; set; }
        public virtual DbSet<ItemComment> ItemComments { get; set; }
        public virtual DbSet<ItemImage> ItemImages { get; set; }
        public virtual DbSet<ItemMainCategory> ItemMainCategories { get; set; }
        public virtual DbSet<ItemPayment> ItemPayments { get; set; }
        public virtual DbSet<ItemProperty> ItemProperties { get; set; }
        public virtual DbSet<ItemRequest> ItemRequests { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemSubCategory> ItemSubCategories { get; set; }
        public virtual DbSet<ItemType> ItemTypes { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<PublishedItem> PublishedItems { get; set; }
        public virtual DbSet<RatingsCustomer> RatingsCustomers { get; set; }
        public virtual DbSet<RatingsServiceProvider> RatingsServiceProviders { get; set; }
        public virtual DbSet<ServiceProvider> ServiceProviders { get; set; }
        public virtual DbSet<ServiceProviderType> ServiceProviderTypes { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<ItemLocations> ItemLocations { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<Complain> Complains { get; set; }
        public virtual DbSet<ComplainAction> ComplainActions { get; set; }
        public virtual DbSet<ItemStatus> ItemStatus { get; set; }
        public virtual DbSet<ItemRequestStatus> ItemRequestStatuses { get; set; }
        public virtual DbSet<RequestProgress> RequestProgreses { get; set; }
        public virtual DbSet<MetaData>  MetaData { get; set; }
        public virtual DbSet<Orders> Order { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<UserAvatar> UserAvatar { get; set; }
        public virtual DbSet<CustomerCurrentLocation> CustomerCurrentLocation { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdministrativeStaff>()
                .HasMany(e => e.ItemRequests)
                .WithMany(e => e.AdministrativeStaffs)
                .Map(m => m.ToTable("Monitors").MapLeftKey("AdminId").MapRightKey("RequestId"));

            modelBuilder.Entity<BlogCategory>()
                .HasMany(e => e.BlogPosts)
                .WithRequired(e => e.BlogCategory)
                .HasForeignKey(e => e.CategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BlogPost>()
                .HasMany(e => e.BlogPostComments)
                .WithRequired(e => e.BlogPost)
                .HasForeignKey(e => e.PostId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BlogPost>()
                .HasMany(e => e.BlogPostImages)
                .WithRequired(e => e.BlogPost)
                .HasForeignKey(e => e.PostId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.Title)
                .IsFixedLength();

            modelBuilder.Entity<ContactType>()
                .Property(e => e.TypeName)
                .IsFixedLength();

            modelBuilder.Entity<ContactType>()
                .HasMany(e => e.Contacts)
                .WithRequired(e => e.ContactType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Gender)
                .IsFixedLength();

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.BlogPostComments)
                .WithRequired(e => e.Customer)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.ItemComments)
                .WithRequired(e => e.Customer)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.CustomerCurrentLocations)
                .WithRequired(e => e.Customer)
                .HasForeignKey(e => e.CustomerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.ItemRequests)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.RatingsCustomers)
                .WithRequired(e => e.Customer)
                .HasForeignKey(e => e.ReceivedUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.RatingsServiceProviders)
                .WithRequired(e => e.Customer)
                .HasForeignKey(e => e.PostUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ItemCategoryIcon>()
                .HasMany(e => e.ItemMainCategories)
                .WithOptional(e => e.ItemCategoryIcon)
                .HasForeignKey(e => e.IconId);

            modelBuilder.Entity<ItemCategoryIcon>()
                .HasMany(e => e.ItemSubCategories)
                .WithOptional(e => e.ItemCategoryIcon)
                .HasForeignKey(e => e.IconId);

            modelBuilder.Entity<ItemMainCategory>()
                .HasMany(e => e.ItemSubCategories)
                .WithRequired(e => e.ItemMainCategory)
                .HasForeignKey(e => e.MainCategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ItemRequest>()
                .HasMany(e => e.ItemPayments)
                .WithRequired(e => e.ItemRequest)
                .HasForeignKey(e => e.RequestId)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ItemRequest>()
            //    .HasMany(e => e.Locations)
            //    .WithRequired(e => e.ItemRequest)
            //    .HasForeignKey(e => e.RequestId)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<ItemRequest>()
                .HasMany(e => e.RatingsCustomers)
                .WithRequired(e => e.ItemRequest)
                .HasForeignKey(e => e.RequestId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ItemRequest>()
                .HasMany(e => e.RequestProgreses)
                .WithRequired(e => e.ItemRequest)
                .HasForeignKey(e => e.ReqId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ItemRequest>()
                .HasMany(e => e.RatingsServiceProviders)
                .WithRequired(e => e.ItemRequest)
                .HasForeignKey(e => e.RequestId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.ItemComments)
                .WithRequired(e => e.Item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.ItemImages)
                .WithRequired(e => e.Item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ServiceProvider>()
                .HasMany(e => e.UserAvatars)
                .WithRequired(e => e.ServiceProvider)
                .HasForeignKey(e=>e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.ItemProperties)
                .WithRequired(e => e.Item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.ItemRequests)
                .WithRequired(e => e.Item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.PublishedItems)
                .WithRequired(e => e.Item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.ItemLocations)
                .WithRequired(e => e.Item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ItemSubCategory>()
                .HasMany(e => e.Items)
                .WithRequired(e => e.ItemSubCategory)
                .HasForeignKey(e => e.SubCategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ItemType>()
                .Property(e => e.Type)
                .IsFixedLength();

            modelBuilder.Entity<ItemType>()
                .HasMany(e => e.Items)
                .WithRequired(e => e.ItemType)
                .HasForeignKey(e => e.TypeId)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<Location>()
                .HasMany(e => e.ItemRequests)
                .WithRequired(e => e.Location)
                .HasForeignKey(e => e.LocationId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ServiceProvider>()
                .HasMany(e => e.BlogPosts)
                .WithRequired(e => e.ServiceProvider)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ServiceProvider>()
                .HasMany(e => e.Contacts)
                .WithRequired(e => e.ServiceProvider)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ServiceProvider>()
                .HasMany(e => e.ItemRequests)
                .WithRequired(e => e.ServiceProvider)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ServiceProvider>()
                .HasMany(e => e.Items)
                .WithRequired(e => e.ServiceProvider)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ServiceProvider>()
                .HasMany(e => e.PublishedItems)
                .WithRequired(e => e.ServiceProvider)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ServiceProvider>()
                .HasMany(e => e.RatingsCustomers)
                .WithRequired(e => e.ServiceProvider)
                .HasForeignKey(e => e.PostUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ServiceProvider>()
                .HasMany(e => e.RatingsServiceProviders)
                .WithRequired(e => e.ServiceProvider)
                .HasForeignKey(e => e.ReceivedUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ServiceProviderType>()
                .Property(e => e.Type)
                .IsFixedLength();

            modelBuilder.Entity<ServiceProviderType>()
                .HasMany(e => e.ServiceProviders)
                .WithRequired(e => e.ServiceProviderType)
                .HasForeignKey(e => e.TypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AdministrativeStaffs)
                .WithOptional(e => e.AspNetUser)
                .HasForeignKey(e => e.ApplicationUserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.Customers)
                .WithOptional(e => e.AspNetUser)
                .HasForeignKey(e => e.ApplicationUserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.ServiceProviders)
                .WithOptional(e => e.AspNetUser)
                .HasForeignKey(e => e.ApplicationUserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.Complains)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.ComplainedBy);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.Complains)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.ComplainedAbout);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ComplainAction>()
                .HasMany(e => e.Complains)
                .WithRequired(e => e.ComplainAction)
                .HasForeignKey(e => e.ActionId);

            modelBuilder.Entity<OrderStatus>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.OrderStatus)
                .HasForeignKey(e => e.Status);

            modelBuilder.Entity<Status>()
                .HasMany(e => e.Customers)
                .WithRequired(e => e.Status)
                .HasForeignKey(e => e.StatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Status>()
                .HasMany(e => e.ServiceProviders)
                .WithRequired(e => e.Status)
                .HasForeignKey(e => e.StatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ItemStatus>()
                .HasMany(e => e.Items)
                .WithRequired(e => e.ItemStatus)
                .HasForeignKey(e => e.StatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ItemRequestStatus>()
                .HasMany(e => e.ItemRequests)
                .WithRequired(e => e.ItemRequestStatus)
                .HasForeignKey(e => e.StatusId)
                .WillCascadeOnDelete(false);
        }
    }
}
