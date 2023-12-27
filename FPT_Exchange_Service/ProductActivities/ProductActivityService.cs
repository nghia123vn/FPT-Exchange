using AutoMapper;
using AutoMapper.QueryableExtensions;
using FPT_Exchange_Data;
using FPT_Exchange_Data.DTO.Filters;
using FPT_Exchange_Data.DTO.View;
using FPT_Exchange_Data.Entities;
using FPT_Exchange_Data.Repositories.ProductActivities;
using FPT_Exchange_Data.Repositories.Products;
using FPT_Exchange_Data.Repositories.Transactions;
using FPT_Exchange_Data.Repositories.Users;
using FPT_Exchange_Data.Repositories.Wallets;
using FPT_Exchange_Utility.Constants;
using FPT_Exchange_Utility.Extensions;
using FPT_Exchange_Utility.Helpers;
using FPT_Exchange_Utility.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FPT_Exchange_Service.ProductActivities
{
    public class ProductActivityService : BaseService, IProductActivityService
    {
        private readonly AppSetting _appSettings;

        private readonly IProductActivityRepository _productActivityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IWalletRepository _walletRepository;

        public ProductActivityService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSetting> appSettings) : base(unitOfWork, mapper)
        {
            _appSettings = appSettings.Value;

            _productActivityRepository = unitOfWork.ProductActivity;
            _productRepository = unitOfWork.Product;
            _userRepository = unitOfWork.User;
            _transactionRepository = unitOfWork.Transaction;
            _walletRepository = unitOfWork.Wallet;

        }

        public async Task<IActionResult> GetProductActivity(Guid id)
        {
            var productActivity = await _productActivityRepository.GetMany(productAc => productAc.Id.Equals(id))
                                                                    .ProjectTo<ProductActivityViewModel>(_mapper.ConfigurationProvider)
                                                                    .FirstOrDefaultAsync();
            if (productActivity != null)
            {
                return new JsonResult(productActivity);
            }
            return new StatusCodeResult(StatusCodes.Status404NotFound);
        }

        public async Task<IActionResult> GetProductActivities(ProductActivityFilterModel filter)
        {
            var query = _productActivityRepository.GetAll();
            if (filter.Status != null)
            {
                query = query.Where(activity => activity.NewStatus.Equals(filter.Status));
            }
            if (filter.CustomerId != null)
            {
                query = query.Where(activity => activity.UserId.Equals(filter.CustomerId));
            }
            var activities = await query.ProjectTo<ProductActivityViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return new JsonResult(activities);
        }

        public async Task<IActionResult> CreateOrder(Guid customerId, Guid productId)
        {
            var product = await _productRepository.GetMany(product => product.Id.Equals(productId) && product.StatusId.Equals(Guid.Parse(ProductStatus.Active.GetEnumMember()))).FirstOrDefaultAsync();
            if (product != null)
            {
                var checkUser = await _userRepository.GetMany(user => user.Id.Equals(customerId)).Include(user => user.Wallet).FirstOrDefaultAsync();
                if (checkUser != null && checkUser.Wallet.Score < product.Price)
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }

                var productActivity = new ProductActivy
                {
                    Id = Guid.NewGuid(),
                    UserId = customerId,
                    ProductId = productId,
                    StationsId = product.StationId,
                    OldStatus = product.StatusId,
                    NewStatus = Guid.Parse(ProductStatus.Processing.GetEnumMember()),
                };

                _productActivityRepository.Add(productActivity);

                product.StatusId = Guid.Parse(ProductStatus.Processing.GetEnumMember());
                _productRepository.Update(product);

                var result = await _unitOfWork.SaveChanges();
                if (result > 0)
                {
                    return await GetProductActivity(productActivity.Id);
                }
            }
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }

        public async Task<IActionResult> GetOrderByCustomerId(Guid customerId, Guid statusId)
        {
            var filter = new ProductActivityFilterModel
            {
                Status = statusId,
                CustomerId = customerId
            };
            return await GetProductActivities(filter);
        }




        public async Task<IActionResult> CancelOrder(Guid productActivityId, Guid customerId)
        {
            var productActivity = await _productActivityRepository.GetMany(product => product.Id.Equals(productActivityId)
                                                                                      && product.NewStatus.Equals(Guid.Parse(ProductStatus.Processing.GetEnumMember())))
                                                                   .FirstOrDefaultAsync();
            if (productActivity != null)
            {
                productActivity.OldStatus = (Guid)productActivity.NewStatus;
                productActivity.NewStatus = Guid.Parse(ProductStatus.Canceled.GetEnumMember());
                productActivity.UserId = customerId;
                _productActivityRepository.Update(productActivity);

                //cập nhật lại trại thái sản phẩm
                var product = await _productRepository.GetMany(product => product.Id.Equals(productActivity.ProductId)).FirstOrDefaultAsync();
                if (product != null)
                {
                    product.StatusId = Guid.Parse(ProductStatus.Active.GetEnumMember());
                    _productRepository.Update(product);
                }

                var result = await _unitOfWork.SaveChanges();
                if (result > 0)
                {
                    return await GetProductActivity(productActivityId);
                }
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            return new StatusCodeResult(StatusCodes.Status404NotFound);
        }

        public async Task<IActionResult> RejectOrder(Guid staffId, Guid productActivityId)
        {
            string customerId = null;
            var productActivity = await _productActivityRepository.GetMany(product => product.Id.Equals(productActivityId)
                                                                                      && product.NewStatus.Equals(Guid.Parse(ProductStatus.Processing.GetEnumMember())))
                                                                   .FirstOrDefaultAsync();
            if (productActivity != null)
            {
                customerId = productActivity.UserId.ToString();
                productActivity.OldStatus = (Guid)productActivity.NewStatus;
                productActivity.NewStatus = Guid.Parse(ProductStatus.Rejected.GetEnumMember());
                productActivity.UserId = staffId;
                _productActivityRepository.Update(productActivity);

                //cập nhật lại trại thái sản phẩm
                var product = await _productRepository.GetMany(product => product.Id.Equals(productActivity.ProductId)).FirstOrDefaultAsync();
                if (product != null)
                {
                    product.StatusId = Guid.Parse(ProductStatus.Active.GetEnumMember());
                    _productRepository.Update(product);
                }

                var result = await _unitOfWork.SaveChanges();
                if (result > 0)
                {
                    //send notify for customer
                    ApiHelper.SendNotify(customerId, "Your order is rejected.");

                    return await GetProductActivity(productActivityId);
                }
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            return new StatusCodeResult(StatusCodes.Status404NotFound);
        }

        public async Task<IActionResult> ConfirmOrder(Guid staffId, Guid productActivityId)
        {
            var productActivy = await _productActivityRepository.GetMany(product => product.Id.Equals(productActivityId)
                                                                                    && product.NewStatus.Equals(Guid.Parse(ProductStatus.Processing.GetEnumMember())))
                                                                .Include(product => product.Product)
                                                                    .ThenInclude(seller => seller.Seller)
                                                                        .ThenInclude(seller => seller.Wallet)
                                                                .Include(product => product.User)
                                                                    .ThenInclude(user => user.Wallet)
                                                                .FirstOrDefaultAsync();

            if (productActivy == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }

            if (productActivy.User.Wallet.Score < productActivy.Product.Price)
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            //Fee
            int productPrice = productActivy.Product.Price;
            int fee = int.Parse(_appSettings.Fee);
            int totalFee = productPrice * fee / 100;
            int totalReceive = productPrice - totalFee;

            var trans = new Transaction
            {
                Id = Guid.NewGuid(),
                ProductId = productActivy.ProductId,
                WalletId = productActivy.User.Wallet.Id,
                Amount = productPrice,
                Fee = totalFee,
                Receive = productPrice - totalFee
            };
            _transactionRepository.Add(trans);

            //trừ đ
            //var buyerWallet = await _walletRepository.GetMany(wallet => wallet.Id.Equals(productActivy.User.Wallet.Id)).FirstOrDefaultAsync();
            //if (buyerWallet != null)
            //{
            //buyerWallet.Score -= productPrice;
            //_walletRepository.Update(buyerWallet);

            //}

            //Call API to update score for buyer
            var (currentScore, check) = await ApiHelper.UpdateWallet(productActivy.User.Id.ToString(), -productPrice);


            //cộng đ
            //var sellerWallet = await _walletRepository.GetMany(wallet => wallet.Id.Equals(productActivy.Product.Seller.Wallet.Id)).FirstOrDefaultAsync();
            //if (sellerWallet != null)
            //{
            //    sellerWallet.Score += totalReceive;
            //    _walletRepository.Update(sellerWallet);
            //}


            //Call API to update score for seller
            var (currentScoreOfSeller, checkScore) = await ApiHelper.UpdateWallet(productActivy.Product.SellerId.ToString(), totalReceive);

            var product = await _productRepository.GetMany(product => product.Id.Equals(productActivy.ProductId)).Include(product => product.Station).FirstOrDefaultAsync();
            if (product != null)
            {
                product.BuyerId = productActivy.UserId;
                product.StatusId = Guid.Parse(ProductStatus.Saled.GetEnumMember());
                _productRepository.Update(product);
            }



            //change status and who changed
            productActivy.UserId = staffId;
            productActivy.OldStatus = (Guid)productActivy.NewStatus;
            productActivy.NewStatus = Guid.Parse(ProductStatus.Confirmed.GetEnumMember());
            _productActivityRepository.Update(productActivy);

            var result = await _unitOfWork.SaveChanges();
            if (result > 0)
            {
                //send notify for seller
                ApiHelper.SendNotify(product.SellerId.ToString(), $"Your order {product.Name} is succeed. Your wallet score added is {totalReceive}");

                //send notify for buyer
                ApiHelper.SendNotify(product.BuyerId.ToString(), $"Your order {product.Name} is succed. Your wallet reduce: {productPrice}, please go to {product.Station.Name}.");

                return await GetProductActivity(productActivityId);
            }
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }
    }
}
