using System.Collections.Generic;

namespace ECommerce.Features.Products
{
  public class CreateProductViewModel
  {
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public string Brand { get; set; }
    public List<string> Features { get; set; }
    public List<CreateProductVariantViewModel> Variants { get; set; }
  }
}