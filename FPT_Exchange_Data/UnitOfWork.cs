using FPT_Exchange_Data.Entities;
using FPT_Exchange_Data.Repositories.Categories;
using FPT_Exchange_Data.Repositories.ImageProducts;
using FPT_Exchange_Data.Repositories.ProductActivities;
using FPT_Exchange_Data.Repositories.Products;
using FPT_Exchange_Data.Repositories.Stations;
using FPT_Exchange_Data.Repositories.Transactions;
using FPT_Exchange_Data.Repositories.Users;
using FPT_Exchange_Data.Repositories.Wallets;

namespace FPT_Exchange_Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FptExchangeDbContext _context;

        private IUserRepository _user = null!;

        private IWalletRepository _wallet = null!;

        private IProductActivityRepository _productActivity = null!;
        private ITransactionRepository _transaction = null!;

        private IStationRepository _station = null!;
        private ICategoryRepository _category = null!;
        private IProductRepository _product = null!;
        private IImageProductRepository _imageProduct = null!;
        public UnitOfWork(FptExchangeDbContext context)
        {
            _context = context;
        }

        public IUserRepository User
        {
            get { return _user ??= new UserRepository(_context); }
        }

        public IWalletRepository Wallet
        {
            get { return _wallet ??= new WalletRepository(_context); }
        }

        public IStationRepository Station
        {
            get { return _station ??= new StationRepository(_context); }
        }

        public ICategoryRepository Category
        {
            get { return _category ??= new CategoryRepository(_context); }
        }

        public IProductRepository Product
        {
            get { return _product ??= new ProductRepository(_context); }
        }

        public IImageProductRepository ImageProduct
        {
            get { return _imageProduct ??= new ImageProductRepository(_context); }
        }

        public IProductActivityRepository ProductActivity
        {
            get { return _productActivity ??= new ProductActivityRepository(_context); }
        }

        public ITransactionRepository Transaction
        {
            get { return _transaction ??= new TransactionRepository(_context); }
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        //public IDbContextTransaction Transaction()
        //{
            //return _context.Database.BeginTransaction();
        //}
    }
}
