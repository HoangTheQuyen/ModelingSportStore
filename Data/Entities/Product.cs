using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Data.Entities
{
  public class Product
  {
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Slug { get; set; }
    [Required]
    public string Thumbnail { get; set; }
    [Required]
    public string ShortDescription { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int BrandId { get; set; }
    public List<Image> Images { get; set; }
    public Brand Brand { get; set; }
    public List<ProductFeature> ProductFeatures { get; set; } = new List<ProductFeature>();
    public List<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
  }
}