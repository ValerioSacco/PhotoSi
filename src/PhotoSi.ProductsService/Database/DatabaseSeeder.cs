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
                        Code = 1,
                        Name = "Stampe",
                        Description = "Stampa semplice di una foto"
                    },
                    new Category
                    {
                        Code = 2,
                        Name = "Gadgets",
                        Description = "Gadget per il tempo libero"
                    },
                    new Category
                    {
                        Code = 3,
                        Name = "Bigletti",
                        Description = "Biglietti auguri per ricorrenze"
                    }
                );
            }
            if (!dbContext.Products.Any())
            {
                dbContext.Products.AddRange(
                    new Product
                    {
                        Code = 1,
                        Name = "Stampa 10x15",
                        Description = "Stampa di una foto in formato 10x15",
                        CategoryCode = 1
                    },
                    new Product
                    {
                        Code = 2,
                        Name = "Tazza personalizzata",
                        Description = "Tazza con foto personalizzata",
                        CategoryCode = 2
                    },
                    new Product
                    {
                        Code = 3,
                        Name = "Biglietto di auguri",
                        Description = "Biglietto di auguri personalizzato",
                        CategoryCode = 3
                    },
                    new Product
                    {
                        Code = 4,
                        Name = "Stampa 20x30",
                        Description = "Stampa di una foto in formato 20x30",
                        CategoryCode = 1
                    },
                    new Product
                    {
                        Code = 5,
                        Name = "Calendario personalizzato",
                        Description = "Calendario con foto personalizzate",
                        CategoryCode = 2
                    },
                    new Product
                    {
                        Code = 6,
                        Name = "Cartolina di auguri",
                        Description = "Cartolina di auguri personalizzata",
                        CategoryCode = 3
                    }
                );
            }
            dbContext.SaveChanges();
        }
    }
}
