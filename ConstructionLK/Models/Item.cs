namespace ConstructionLK.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Item()
        {
            ItemLocations = new HashSet<ItemLocations>();
            ItemComments = new HashSet<ItemComment>();
            ItemImages = new HashSet<ItemImage>();
            ItemProperties = new HashSet<ItemProperty>();
            ItemRequests = new HashSet<ItemRequest>();
            PublishedItems = new HashSet<PublishedItem>();
            
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [StringLength(50)]
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Item Description")]
        public string Description { get; set; }

        public int? StatusId { get; set; }
        [Display(Name = "Availability")]
        public virtual ItemStatus ItemStatus { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Item Sub Category")]
        public int SubCategoryId { get; set; }

        [Display(Name = "Item Type")]
        public int TypeId { get; set; }

        public int UserId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemComment> ItemComments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemImage> ItemImages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemProperty> ItemProperties { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemRequest> ItemRequests { get; set; }

        public virtual ItemSubCategory ItemSubCategory { get; set; }

        public virtual ItemType ItemType { get; set; }

        public virtual ServiceProvider ServiceProvider { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PublishedItem> PublishedItems { get; set; }

        public virtual ICollection<ItemLocations> ItemLocations { get; set; }
        //[Required]
        //public float Price { get; set; }
        //public float? Tax { get; set; }
    }
}
