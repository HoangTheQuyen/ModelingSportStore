using System;
using System.Collections.Generic;
using System.Linq;
using ECommerce.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data
{
    public static class DbContextExtensions
    {
        public static RoleManager<AppRole> RoleManager { get; set; }
        public static UserManager<AppUser> UserManager { get; set; }

        public static void EnsureSeeded(this EcommerceContext context)
        {
            AddRoles(context);
            AddUsers(context);
            AddColoursFeaturesAndStorage(context);
            AddBrands(context);
            AddProducts(context);
        }

        private static void AddRoles(EcommerceContext context)
        {
            if (RoleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult() == false)
            {
                RoleManager.CreateAsync(new AppRole("Admin")).GetAwaiter().GetResult();
            }

            if (RoleManager.RoleExistsAsync("Customer").GetAwaiter().GetResult() == false)
            {
                RoleManager.CreateAsync(new AppRole("Customer")).GetAwaiter().GetResult();
            }
        }

        private static void AddUsers(EcommerceContext context)
        {
            if (UserManager.FindByEmailAsync("HoangTheQuyen01@gmail.com").GetAwaiter().GetResult() == null)
            {
                var user = new AppUser
                {
                    FirstName = "hoang",
                    LastName = "quyen",
                    UserName = "HoangTheQuyen01@gmail.com",
                    Email = "HoangTheQuyen01@gmail.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };

                UserManager.CreateAsync(user, "Password1*").GetAwaiter().GetResult();
            }

            var admin = UserManager.FindByEmailAsync("trangbinhnam@gmail.com").GetAwaiter().GetResult();

            if (UserManager.IsInRoleAsync(admin, "Admin").GetAwaiter().GetResult() == false)
            {
                UserManager.AddToRoleAsync(admin, "Admin");
            }
        }

        private static void AddColoursFeaturesAndStorage(EcommerceContext context)
        {
            if (context.Colours.Any() == false)
            {
                var colours = new List<string>() { "Trắng", "Xanh", "Đen", "Xám than" };

                colours.ForEach(c => context.Add(new Colour
                {
                    Name = c
                }));

                context.SaveChanges();
            }

            if (context.Features.Any() == false)
            {
                var features = new List<string>() { "Cách nhiệt", "Hút ẩm", "Bay hơi thoáng khí" };

                features.ForEach(f => context.Add(new Feature
                {
                    Name = f
                }));

                context.SaveChanges();
            }

            if (context.Storage.Any() == false)
            {
                var storage = new List<string>() { "S", "M", "L", "XL", "XXL" };

                storage.ForEach(s => context.Storage.Add(new Storage
                {
                    Capacity = s
                }));

                context.SaveChanges();
            }
        }

        private static void AddBrands(EcommerceContext context)
        {

            if (context.Brands.Any() == false)
            {
                var brands = new List<string>() { "Nike", "ESPN", "Adidas", "Sky Sports", "Gatorade", "Reebok", "Under Armour", "EA Sports", "YES Network", "MSG" };

                brands.ForEach(b => context.Brands.Add(new Brand
                {
                    Name = b
                }));

                context.SaveChanges();
            }

            context.SaveChanges();
        }

        private static void AddProducts(EcommerceContext context)
        {
            if (context.Products.Any() == false)
            {
                var products = new List<Product>()
        {
          new Product
          {
            Name = "Danco 01",
            Slug = "danco-01",
            Thumbnail = "/assets/images/thumbnail.jpg",
            ShortDescription = "Áo Thể Thao Nam Danco Chính Hãng",
            Description = "Chất liệu vải Lì Chun với độ mềm mại nổi trội không bị nhăn nhàu khi sử dụng lâu ngày",
            Brand = context.Brands.Single(b => b.Name == "Nike"),
            Images = new List<Image>
            {
              new Image { Url = "/assets/images/1.jpg" },
              new Image { Url = "/assets/images/2.jpg" },
              new Image { Url = "/assets/images/3.jpg" },
              new Image { Url = "/assets/images/4.jpg" },
              new Image { Url = "/assets/images/5.jpg" },
              new Image { Url = "/assets/images/6.jpg" }
            },
            ProductFeatures = new List<ProductFeature>
            {
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Cách nhiệt")
              },
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Hút ẩm")
              },
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Bay hơi thoáng khí")
              }
            },
            ProductVariants = new List<ProductVariant>
            {
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Đen"),
                Storage = context.Storage.Single(s => s.Capacity == "S"),
                Price = 299M
              },
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Xanh"),
                Storage = context.Storage.Single(s => s.Capacity == "XL"),
                Price = 349M
              },
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Trắng"),
                Storage = context.Storage.Single(s => s.Capacity == "M"),
                Price = 319M
              }

            }
          },

          new Product
          {
            Name = "Sero 01",
            Slug = "sero-01",
            Thumbnail = "/assets/images/thumbnail1.jpg",
            ShortDescription = "Áo Thể Thao Nam Sero Chính Hãng",
            Description = "Áo thể thao bền thoáng khí số 1 hải ngoại",
            Brand = context.Brands.Single(b => b.Name == "Nike"),
            Images = new List<Image>
            {
              new Image { Url = "/assets/images/1.jpg" },
              new Image { Url = "/assets/images/2.jpg" },
              new Image { Url = "/assets/images/3.jpg" },
              new Image { Url = "/assets/images/4.jpg" },
              new Image { Url = "/assets/images/5.jpg" },
              new Image { Url = "/assets/images/6.jpg" }
            },
            ProductFeatures = new List<ProductFeature>
            {
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Cách nhiệt")
              },
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Hút ẩm")
              },
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Bay hơi thoáng khí")
              }
            },
            ProductVariants = new List<ProductVariant>
            {
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Đen"),
                Storage = context.Storage.Single(s => s.Capacity == "S"),
                Price = 499M
              },
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Xanh"),
                Storage = context.Storage.Single(s => s.Capacity == "M"),
                Price = 349M
              },
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Trắng"),
                Storage = context.Storage.Single(s => s.Capacity == "M"),
                Price = 500M
              }

            }
          },

          new Product
          {
            Name = "Mamo 01",
            Slug = "mamo-01",
            Thumbnail = "/assets/images/thumbnail2.jpg",
            ShortDescription = "Áo Thể Thao Nam Mamo Chính Hãng",
            Description = "Chất liệu vải Lì Chun với độ mềm mại nổi trội không bị nhăn nhàu khi sử dụng lâu ngày",
            Brand = context.Brands.Single(b => b.Name == "ESPN"),
            Images = new List<Image>
            {
              new Image { Url = "/assets/images/1.jpg" },
              new Image { Url = "/assets/images/2.jpg" },
              new Image { Url = "/assets/images/3.jpg" },
              new Image { Url = "/assets/images/4.jpg" },
              new Image { Url = "/assets/images/5.jpg" },
              new Image { Url = "/assets/images/6.jpg" }
            },
            ProductFeatures = new List<ProductFeature>
            {
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Cách nhiệt")
              },
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Hút ẩm")
              }
            },
            ProductVariants = new List<ProductVariant>
            {
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Xanh"),
                Storage = context.Storage.Single(s => s.Capacity == "XL"),
                Price = 199M
              },
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Trắng"),
                Storage = context.Storage.Single(s => s.Capacity == "M"),
                Price = 299M
              }

            }
          },

          new Product
          {
            Name = "Loto 01",
            Slug = "loto-01",
            Thumbnail = "/assets/images/thumbnail3.jpg",
            ShortDescription = "Áo Thể Thao Nam Loto Chính Hãng",
            Description = "Chất liệu vải Lì Chun với độ mềm mại nổi trội không bị nhăn nhàu khi sử dụng lâu ngày",
            Brand = context.Brands.Single(b => b.Name == "Adidas"),
            Images = new List<Image>
            {
              new Image { Url = "/assets/images/1.jpg" },
              new Image { Url = "/assets/images/2.jpg" },
              new Image { Url = "/assets/images/3.jpg" },
              new Image { Url = "/assets/images/4.jpg" },
              new Image { Url = "/assets/images/5.jpg" },
              new Image { Url = "/assets/images/6.jpg" }
            },
            ProductFeatures = new List<ProductFeature>
            {
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Hút ẩm")
              },
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Bay hơi thoáng khí")
              }
            },
            ProductVariants = new List<ProductVariant>
            {
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Đen"),
                Storage = context.Storage.Single(s => s.Capacity == "M"),
                Price = 699M
              },
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Trắng"),
                Storage = context.Storage.Single(s => s.Capacity == "M"),
                Price = 599M
              }

            }
          },

          new Product
          {
            Name = "BoBo",
            Slug = "bobo",
            Thumbnail = "/assets/images/thumbnail4.jpg",
            ShortDescription = "Áo Thể Thao Nam BoBo Chính Hãng",
            Description = "Chất liệu vải Lì Chun với độ mềm mại nổi trội không bị nhăn nhàu khi sử dụng lâu ngày",
            Brand = context.Brands.Single(b => b.Name == "Sky Sports"),
            Images = new List<Image>
            {
              new Image { Url = "/assets/images/1.jpg" },
              new Image { Url = "/assets/images/2.jpg" },
              new Image { Url = "/assets/images/3.jpg" },
              new Image { Url = "/assets/images/4.jpg" },
              new Image { Url = "/assets/images/5.jpg" },
              new Image { Url = "/assets/images/6.jpg" }
            },
            ProductFeatures = new List<ProductFeature>
            {
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Cách nhiệt")
              },
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Hút ẩm")
              },
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Bay hơi thoáng khí")
              }
            },
            ProductVariants = new List<ProductVariant>
            {
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Đen"),
                Storage = context.Storage.Single(s => s.Capacity == "S"),
                Price = 899M
              },
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Xanh"),
                Storage = context.Storage.Single(s => s.Capacity == "XL"),
                Price = 799M
              }
            }
          },

          new Product
          {
            Name = "OLoTo 01",
            Slug = "oloto-01",
            Thumbnail = "/assets/images/thumbnail5.jpg",
            ShortDescription = "Áo Thể Thao Nam OLoTo Chính Hãng",
            Description = "Chất liệu vải Lì Chun với độ mềm mại nổi trội không bị nhăn nhàu khi sử dụng lâu ngày",
            Brand = context.Brands.Single(b => b.Name == "Gatorade"),
            Images = new List<Image>
            {
              new Image { Url = "/assets/images/1.jpg" },
              new Image { Url = "/assets/images/2.jpg" },
              new Image { Url = "/assets/images/3.jpg" },
              new Image { Url = "/assets/images/4.jpg" },
              new Image { Url = "/assets/images/5.jpg" },
              new Image { Url = "/assets/images/6.jpg" }
            },
            ProductFeatures = new List<ProductFeature>
            {
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Cách nhiệt")
              },
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Hút ẩm")
              },
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Bay hơi thoáng khí")
              }
            },
            ProductVariants = new List<ProductVariant>
            {
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Đen"),
                Storage = context.Storage.Single(s => s.Capacity == "S"),
                Price = 600M
              },
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Xanh"),
                Storage = context.Storage.Single(s => s.Capacity == "XL"),
                Price = 400M
              },
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Trắng"),
                Storage = context.Storage.Single(s => s.Capacity == "M"),
                Price = 450M
              }

            }
          },

          new Product
          {
            Name = "Naruto 01",
            Slug = "naruto-01",
            Thumbnail = "/assets/images/thumbnail6.jpg",
            ShortDescription = "Áo Thể Thao Nam Naruto Chính Hãng",
            Description = "Chất liệu vải Lì Chun với độ mềm mại nổi trội không bị nhăn nhàu khi sử dụng lâu ngày",
            Brand = context.Brands.Single(b => b.Name == "Reebok"),
            Images = new List<Image>
            {
              new Image { Url = "/assets/images/1.jpg" },
              new Image { Url = "/assets/images/2.jpg" },
              new Image { Url = "/assets/images/3.jpg" },
              new Image { Url = "/assets/images/4.jpg" },
              new Image { Url = "/assets/images/5.jpg" },
              new Image { Url = "/assets/images/6.jpg" }
            },
            ProductFeatures = new List<ProductFeature>
            {
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Cách nhiệt")
              },
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Hút ẩm")
              },
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Bay hơi thoáng khí")
              }
            },
            ProductVariants = new List<ProductVariant>
            {
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Đen"),
                Storage = context.Storage.Single(s => s.Capacity == "S"),
                Price = 700M
              },
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Xanh"),
                Storage = context.Storage.Single(s => s.Capacity == "XL"),
                Price = 800M
              },
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Trắng"),
                Storage = context.Storage.Single(s => s.Capacity == "M"),
                Price = 800M
              }

            }
          },

          new Product
          {
            Name = "Sasuke 01",
            Slug = "sasuke-01",
            Thumbnail = "/assets/images/thumbnail7.jpg",
            ShortDescription = "Áo Thể Thao Nam Sasuke Chính Hãng",
            Description = "Chất liệu vải Lì Chun với độ mềm mại nổi trội không bị nhăn nhàu khi sử dụng lâu ngày",
            Brand = context.Brands.Single(b => b.Name == "Under Armour"),
            Images = new List<Image>
            {
              new Image { Url = "/assets/images/1.jpg" },
              new Image { Url = "/assets/images/2.jpg" },
              new Image { Url = "/assets/images/3.jpg" },
              new Image { Url = "/assets/images/4.jpg" },
              new Image { Url = "/assets/images/5.jpg" },
              new Image { Url = "/assets/images/6.jpg" }
            },
            ProductFeatures = new List<ProductFeature>
            {
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Cách nhiệt")
              },
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Bay hơi thoáng khí")
              }
            },
            ProductVariants = new List<ProductVariant>
            {
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Xanh"),
                Storage = context.Storage.Single(s => s.Capacity == "XL"),
                Price = 900M
              },
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Trắng"),
                Storage = context.Storage.Single(s => s.Capacity == "M"),
                Price = 900M
              }

            }
          },

          new Product
          {
            Name = "Sakura",
            Slug = "sakura",
            Thumbnail = "/assets/images/thumbnail8.jpg",
            ShortDescription = "Áo Thể Thao Nam Sakura Chính Hãng",
            Description = "Chất liệu vải Lì Chun với độ mềm mại nổi trội không bị nhăn nhàu khi sử dụng lâu ngày",
            Brand = context.Brands.Single(b => b.Name == "EA Sports"),
            Images = new List<Image>
            {
              new Image { Url = "/assets/images/1.jpg" },
              new Image { Url = "/assets/images/2.jpg" },
              new Image { Url = "/assets/images/3.jpg" },
              new Image { Url = "/assets/images/4.jpg" },
              new Image { Url = "/assets/images/5.jpg" },
              new Image { Url = "/assets/images/6.jpg" }
            },
            ProductFeatures = new List<ProductFeature>
            {
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Cách nhiệt")
              },
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Hút ẩm")
              },
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Bay hơi thoáng khí")
              }
            },
            ProductVariants = new List<ProductVariant>
            {
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Đen"),
                Storage = context.Storage.Single(s => s.Capacity == "S"),
                Price = 1000M
              },
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Xanh"),
                Storage = context.Storage.Single(s => s.Capacity == "XL"),
                Price = 1000M
              },
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Trắng"),
                Storage = context.Storage.Single(s => s.Capacity == "M"),
                Price = 1000M
              }

            }
          },

          new Product
          {
            Name = "Minato",
            Slug = "minato",
            Thumbnail = "/assets/images/thumbnail9.jpg",
            ShortDescription = "Áo Thể Thao Nam Minato Chính Hãng",
            Description = "Chất liệu vải Lì Chun với độ mềm mại nổi trội không bị nhăn nhàu khi sử dụng lâu ngày",
            Brand = context.Brands.Single(b => b.Name == "YES Network"),
            Images = new List<Image>
            {
              new Image { Url = "/assets/images/1.jpg" },
              new Image { Url = "/assets/images/2.jpg" },
              new Image { Url = "/assets/images/3.jpg" },
              new Image { Url = "/assets/images/4.jpg" },
              new Image { Url = "/assets/images/5.jpg" },
              new Image { Url = "/assets/images/6.jpg" }
            },
            ProductFeatures = new List<ProductFeature>
            {
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Cách nhiệt")
              },
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Hút ẩm")
              },
              new ProductFeature
              {
                Feature = context.Features.Single(f => f.Name == "Bay hơi thoáng khí")
              }
            },
            ProductVariants = new List<ProductVariant>
            {
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Đen"),
                Storage = context.Storage.Single(s => s.Capacity == "S"),
                Price = 299M
              },
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Xanh"),
                Storage = context.Storage.Single(s => s.Capacity == "XL"),
                Price = 349M
              },
              new ProductVariant
              {
                Colour = context.Colours.Single(c => c.Name == "Trắng"),
                Storage = context.Storage.Single(s => s.Capacity == "M"),
                Price = 400M
              }

            }
            },

            new Product
            {
              Name = "Saratobi",
              Slug = "saratobi",
              Thumbnail = "/assets/images/thumbnail10.jpg",
              ShortDescription = "Áo Thể Thao Nam Saratobi Chính Hãng",
              Description = "Chất liệu vải Lì Chun với độ mềm mại nổi trội không bị nhăn nhàu khi sử dụng lâu ngày",
              Brand = context.Brands.Single(b => b.Name == "MSG"),
              Images = new List<Image>
              {
                new Image { Url = "/assets/images/1.jpg" },
                new Image { Url = "/assets/images/2.jpg" },
                new Image { Url = "/assets/images/3.jpg" },
                new Image { Url = "/assets/images/4.jpg" },
                new Image { Url = "/assets/images/5.jpg" },
                new Image { Url = "/assets/images/6.jpg" }
              },
              ProductFeatures = new List<ProductFeature>
              {
                new ProductFeature
                {
                  Feature = context.Features.Single(f => f.Name == "Cách nhiệt")
                }
              },
              ProductVariants = new List<ProductVariant>
              {
                new ProductVariant
                {
                  Colour = context.Colours.Single(c => c.Name == "Đen"),
                  Storage = context.Storage.Single(s => s.Capacity == "S"),
                  Price = 299M
                }
              }
            },

            new Product
            {
              Name = "Kyubi",
              Slug = "kuybi",
              Thumbnail = "/assets/images/thumbnail11.jpg",
              ShortDescription = "Áo Thể Thao Nam Kyubi Chính Hãng",
              Description = "Chất liệu vải Lì Chun với độ mềm mại nổi trội không bị nhăn nhàu khi sử dụng lâu ngày",
              Brand = context.Brands.Single(b => b.Name == "Nike"),
              Images = new List<Image>
              {
                new Image { Url = "/assets/images/1.jpg" },
                new Image { Url = "/assets/images/2.jpg" },
                new Image { Url = "/assets/images/3.jpg" },
                new Image { Url = "/assets/images/4.jpg" },
                new Image { Url = "/assets/images/5.jpg" },
                new Image { Url = "/assets/images/6.jpg" }
              },
              ProductFeatures = new List<ProductFeature>
              {
                new ProductFeature
                {
                  Feature = context.Features.Single(f => f.Name == "Cách nhiệt")
                },
                new ProductFeature
                {
                  Feature = context.Features.Single(f => f.Name == "Hút ẩm")
                },
                new ProductFeature
                {
                  Feature = context.Features.Single(f => f.Name == "Bay hơi thoáng khí")
                }
              },
              ProductVariants = new List<ProductVariant>
              {
                new ProductVariant
                {
                  Colour = context.Colours.Single(c => c.Name == "Đen"),
                  Storage = context.Storage.Single(s => s.Capacity == "S"),
                  Price = 299M
                },
                new ProductVariant
                {
                  Colour = context.Colours.Single(c => c.Name == "Xanh"),
                  Storage = context.Storage.Single(s => s.Capacity == "XL"),
                  Price = 349M
                },
                new ProductVariant
                {
                  Colour = context.Colours.Single(c => c.Name == "Trắng"),
                  Storage = context.Storage.Single(s => s.Capacity == "M"),
                  Price = 319M
                }

              }
            

            //new Product
            //{
            //  Name = "Tsunade",
            //  Slug = "tsunade",
            //  Thumbnail = "/assets/images/thumbnail12.jpg",
            //  ShortDescription = "Áo Thể Thao Nam Tsunade Chính Hãng",


            //  Brand = context.Brands.Single(b => b.Name == "Nike"),
            //  Images = new List<Image>
            //  {
            //    new Image { Url = "/assets/images/1.jpg" },
            //    new Image { Url = "/assets/images/2.jpg" },
            //    new Image { Url = "/assets/images/3.jpg" },
            //    new Image { Url = "/assets/images/4.jpg" },
            //    new Image { Url = "/assets/images/5.jpg" },
            //    new Image { Url = "/assets/images/6.jpg" }
            //  },
            //  ProductFeatures = new List<ProductFeature>
            //  {
            //    new ProductFeature
            //    {
            //      Feature = context.Features.Single(f => f.Name == "Cách nhiệt")
            //    },
            //    new ProductFeature
            //    {
            //      Feature = context.Features.Single(f => f.Name == "Hút ẩm")
            //    },
            //    new ProductFeature
            //    {
            //      Feature = context.Features.Single(f => f.Name == "Bay hơi thoáng khí")
            //    }
            //  },
            //  ProductVariants = new List<ProductVariant>
            //  {
            //    new ProductVariant
            //    {
            //      Colour = context.Colours.Single(c => c.Name == "Đen"),
            //      Storage = context.Storage.Single(s => s.Capacity == "S"),
            //      Price = 299M
            //    },
            //    new ProductVariant
            //    {
            //      Colour = context.Colours.Single(c => c.Name == "Xanh"),
            //      Storage = context.Storage.Single(s => s.Capacity == "XL"),
            //      Price = 349M
            //    },
            //    new ProductVariant
            //    {
            //      Colour = context.Colours.Single(c => c.Name == "Trắng"),
            //      Storage = context.Storage.Single(s => s.Capacity == "M"),
            //      Price = 319M
            //    }

            //  }
            //}
                
          }
        };
                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }



    }
}