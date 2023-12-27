
using FPT_Exchange_Data.Repositories.Categories;
using FPT_Exchange_Data.Repositories.ImageProducts;
using FPT_Exchange_Data.Repositories.ProductActivities;
using FPT_Exchange_Data.Repositories.Products;
using FPT_Exchange_Data.Repositories.Stations;
using FPT_Exchange_Data.Repositories.Transactions;
using FPT_Exchange_Data.Repositories.Users;
using FPT_Exchange_Data.Repositories.Wallets;
using Microsoft.EntityFrameworkCore.Storage;

namespace FPT_Exchange_Data
{
    public interface IUnitOfWork
    {
        public IUserRepository User { get; }

        public IWalletRepository Wallet { get; }
        public IStationRepository Station { get; }

        public IProductActivityRepository ProductActivity { get; }
        public ITransactionRepository Transaction { get; }

        public ICategoryRepository Category { get; }
        public IProductRepository Product { get; }
        public IImageProductRepository ImageProduct { get; }

        Task<int> SaveChanges();
        //IDbContextTransaction Transaction();
    }
}
