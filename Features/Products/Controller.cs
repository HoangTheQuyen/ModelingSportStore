using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Data;
using ECommerce.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Slugify;

namespace ECommerce.Features.Products
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly EcommerceContext _db;

        public ProductsController(EcommerceContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Find(int? sort, string q, string brands, int? minPrice, int? maxPrice, int? minScreen, int? maxScreen, string capacity, string colours, string os, string features)
        {
            var Query = $"%{q?.ToLower()}%";
            var Brands = string.IsNullOrEmpty(brands) ? new List<string>() : brands.Split('|').ToList();
            var Capacity = string.IsNullOrEmpty(capacity) ? new List<string>() : capacity.Split('|').ToList();
            var Colours = string.IsNullOrEmpty(colours) ? new List<string>() : colours.Split('|').ToList();
            var OS = string.IsNullOrEmpty(os) ? new List<string>() : os.Split('|').ToList();
            var Features = string.IsNullOrEmpty(features) ? new List<string>() : features.Split('|').ToList();

            var products = await _db.Products
              .Where(x =>
                string.IsNullOrEmpty(q) ||
                (
                  EF.Functions.Like(x.Name.ToLower(), Query) ||
                  EF.Functions.Like(x.ShortDescription.ToLower(), Query) ||
                  EF.Functions.Like(x.Description.ToLower(), Query) ||
                  EF.Functions.Like(x.Brand.Name.ToLower(), Query)
                )
              )
              .Where(x => Brands.Any() == false || Brands.Contains(x.Brand.Name))
              .Where(x => minPrice.HasValue == false || x.ProductVariants.Any(v => v.Price >= minPrice.Value))
              .Where(x => maxPrice.HasValue == false || x.ProductVariants.Any(v => v.Price <= maxPrice.Value))

              .Where(x => Capacity.Any() == false || x.ProductVariants.Any(v => Capacity.Contains(v.Storage.Capacity)))
              .Where(x => Colours.Any() == false || x.ProductVariants.Any(v => Colours.Contains(v.Colour.Name)))
              .Where(x => Features.Any() == false || Features.All(f => x.ProductFeatures.Any(pf => pf.Feature.Name == f)))
              .Select(x => new ProductListViewModel
              {
                  Id = x.Id,
                  Slug = x.Slug,
                  Name = x.Name,
                  ShortDescription = x.ShortDescription,
                  Thumbnail = x.Thumbnail,
                  Price = x.ProductVariants.OrderBy(v => v.Price).First().Price
              })
              .ToListAsync();

            if(sort == 1)
            {
                products = products.OrderByDescending(x => x.Price).ToList();
            }
            else if(sort == 2)
            {
                products = products.OrderBy(x => x.Name).ToList();
            }
            else if(sort == 3)
            {
                products = products.OrderByDescending(x => x.Name).ToList();
            }
            else
            {
                products = products.OrderBy(x => x.Price).ToList();
            }

            return Ok(products);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> Get(string slug)
        {
            var product = await _db.Products.Select(x => new ProductDetailsViewModel
            {
                Id = x.Id,
                Slug = x.Slug,
                Name = x.Name,
                ShortDescription = x.ShortDescription,
                Description = x.Description,
                Price = x.ProductVariants.OrderBy(v => v.Price).First().Price,
                Thumbnail = x.Thumbnail,
                Images = x.Images.Select(i => i.Url),
                Features = x.ProductFeatures.Select(f => f.Feature.Name),
                Variants = x.ProductVariants
                .OrderBy(v => v.Colour.Name)
                .ThenBy(v => v.Storage.Capacity)
                .Select(v => new ProductVariantViewModel
                {
                    ProductId = x.Id,
                    Name = x.Name,
                    Thumbnail = x.Thumbnail,
                    ColourId = v.ColourId,
                    Colour = v.Colour.Name,
                    StorageId = v.StorageId,
                    Capacity = $"{v.Storage.Capacity}",
                    Price = v.Price
                })
            })
            .FirstOrDefaultAsync(x => x.Slug == slug);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateProductViewModel model)
        {
            var slugHelper = new SlugHelper();

            var brand = await _db.Brands.FirstOrDefaultAsync(x => x.Name == model.Brand);

            if (brand == null)
                brand = new Brand { Name = model.Brand };

            var product = new Product
            {
                Name = model.Name,
                Slug = slugHelper.GenerateSlug(model.Name),
                ShortDescription = model.ShortDescription,
                Description = model.Description,
                Brand = brand,
                Thumbnail = "/assets/images/thumbnail.jpg",
                Images = new List<Image>
        {
          new Image { Url = "/assets/images/1.jpg" },
          new Image { Url = "/assets/images/2.jpg" },
          new Image { Url = "/assets/images/3.jpg" },
          new Image { Url = "/assets/images/4.jpg" },
          new Image { Url = "/assets/images/5.jpg" },
          new Image { Url = "/assets/images/6.jpg" }
        }
            };

            foreach (var feature in model.Features)
            {
                var feat = await _db.Features.SingleAsync(x => x.Name == feature);
                product.ProductFeatures.Add(new ProductFeature { Feature = feat });
            }

            foreach (var variant in model.Variants)
            {
                var colour = await _db.Colours.FirstOrDefaultAsync(x => x.Name == variant.Colour);

                if (colour == null)
                    colour = new Colour { Name = variant.Colour };

                var capacity = variant.Storage;
                var storage = await _db.Storage.FirstOrDefaultAsync(x => x.Capacity == capacity);

                if (storage == null)
                    storage = new Storage { Capacity = capacity };

                product.ProductVariants.Add(new ProductVariant
                {
                    Colour = colour,
                    Storage = storage,
                    Price = variant.Price
                });
            }

            _db.Products.Add(product);

            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("validate"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Validate([FromBody] ValidateProductViewModel model)
        {
            var valid = await _db.Products.AllAsync(x => x.Name.ToLower() != model.Name.ToLower());
            return Ok(valid);
        }
    }
}