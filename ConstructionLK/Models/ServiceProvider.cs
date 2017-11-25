namespace ConstructionLK.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ServiceProvider
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ServiceProvider()
        {
            BlogPosts = new HashSet<BlogPost>();
            Contacts = new HashSet<Contact>();
            ItemRequests = new HashSet<ItemRequest>();
            Items = new HashSet<Item>();
            PublishedItems = new HashSet<PublishedItem>();
            RatingsCustomers = new HashSet<RatingsCustomer>();
            RatingsServiceProviders = new HashSet<RatingsServiceProvider>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Based City")]
        public string BasedCity { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Mailing Address")]
        public string MailingAddress { get; set; }

        [Required]
        [Display(Name = "About You")]
        public string Bio { get; set; }

        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:y}")]
        [Column(TypeName = "date")]
        [Display(Name = "Date of Birth")]
        [Min18YearsIfAMember]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [StringLength(50)]
        public string Telephone { get; set; }

        [StringLength(50)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [StringLength(20)]
        [Display(Name = "Company Registration Number")]
        public string CompanyRegNo { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Started Date of the Company")]
        public DateTime StartedDate { get; set; }

        public string Avatar { get; set; }

        public int? Status { get; set; }

        public int? CreatedBy { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? ModifiedDate { get; set; }

        public int TypeId { get; set; }


        public MembershipType MembershipType { get; set; }
        [Display(Name = "Membership Type")]
        public byte MembershipTypeId { get; set; }

        [StringLength(128)]
        public string ApplicationUserId { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BlogPost> BlogPosts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contact> Contacts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemRequest> ItemRequests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item> Items { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PublishedItem> PublishedItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RatingsCustomer> RatingsCustomers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RatingsServiceProvider> RatingsServiceProviders { get; set; }
        [Display(Name = "Service Provider Type")]
        public virtual ServiceProviderType ServiceProviderType { get; set; }
    }
}
