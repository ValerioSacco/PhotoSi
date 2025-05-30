using PhotoSi.ProductsService.Models;

namespace PhotoSi.ProductsService.Database
{
    public static class DatabaseSeeder
    {
        public static void Seed(ProductsDbContext dbContext)
        {
            if (!dbContext.Categories.Any())
            {
                dbContext.Categories.AddRange(
                    new Category
                    {
                        Id = Guid.Parse("b3eaf4bd-2e57-4041-8cf0-6a19a55c9fb9"),
                        Name = "Stampe",
                        Description = "Stampa semplice di una foto"
                    },
                    new Category
                    {
                        Id = Guid.Parse("59cf0ad7-89a8-4dd5-83da-9cb50608080b"),
                        Name = "Gadgets",
                        Description = "Gadget per il tempo libero"
                    },
                    new Category
                    {
                        Id = Guid.Parse("1c3e24c8-b0c5-4c9b-8b28-f5c5ec2c819a"),
                        Name = "Biglietti",
                        Description = "Biglietti auguri per ricorrenze"
                    }
                );
            }

            if (!dbContext.Products.Any())
            {
                dbContext.Products.AddRange(
                    new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = "Stampa 10x15",
                        Description = "Stampa di una foto in formato 10x15",
                        CategoryId = Guid.Parse("b3eaf4bd-2e57-4041-8cf0-6a19a55c9fb9")
                    },
                    new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = "Tazza personalizzata",
                        Description = "Tazza con foto personalizzata",
                        CategoryId = Guid.Parse("59cf0ad7-89a8-4dd5-83da-9cb50608080b")
                    },
                    new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = "Biglietto di auguri",
                        Description = "Biglietto di auguri personalizzato",
                        CategoryId = Guid.Parse("1c3e24c8-b0c5-4c9b-8b28-f5c5ec2c819a")
                    },
                    new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = "Stampa 20x30",
                        Description = "Stampa di una foto in formato 20x30",
                        CategoryId = Guid.Parse("b3eaf4bd-2e57-4041-8cf0-6a19a55c9fb9")
                    },
                    new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = "Calendario personalizzato",
                        Description = "Calendario con foto personalizzate",
                        CategoryId = Guid.Parse("59cf0ad7-89a8-4dd5-83da-9cb50608080b")
                    },
                    new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = "Cartolina di auguri",
                        Description = "Cartolina di auguri personalizzata",
                        CategoryId = Guid.Parse("1c3e24c8-b0c5-4c9b-8b28-f5c5ec2c819a")
                    }
                );
            }

            dbContext.SaveChanges();
        }
    }
}