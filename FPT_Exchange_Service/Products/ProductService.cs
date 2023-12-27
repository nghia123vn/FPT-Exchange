using AutoMapper;
using AutoMapper.QueryableExtensions;
using Firebase.Storage;
using FPT_Exchange_Data;
using FPT_Exchange_Data.DTO.Filters;
using FPT_Exchange_Data.DTO.Request.Post;
using FPT_Exchange_Data.DTO.Request.Put;
using FPT_Exchange_Data.DTO.View;
using FPT_Exchange_Data.Entities;
using FPT_Exchange_Data.Repositories.Categories;
using FPT_Exchange_Data.Repositories.ImageProducts;
using FPT_Exchange_Data.Repositories.Products;
using FPT_Exchange_Data.Repositories.Stations;
using FPT_Exchange_Data.Repositories.Users;
using FPT_Exchange_Utility.Constants;
using FPT_Exchange_Utility.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FPT_Exchange_Service.Products
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IStationRepository _stationRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IImageProductRepository _imageProductRepository;
        private readonly IUserRepository _userRepository;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _productRepository = unitOfWork.Product;
            _stationRepository = unitOfWork.Station;
            _categoryRepository = unitOfWork.Category;
            _imageProductRepository = unitOfWork.ImageProduct;
            _userRepository = unitOfWork.User;
        }

        //Get Product By ID
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _productRepository.GetMany(pro => pro.Id.Equals(id))
                                                  .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                                                  .FirstOrDefaultAsync();
            if (product != null)
            {
                return new JsonResult(product);
            }
            return new StatusCodeResult(StatusCodes.Status404NotFound);
        }

        public async Task<IActionResult> GetProductByer(Guid byerId)
        {
            var product = await _productRepository.GetMany(pro => pro.BuyerId.Equals(byerId))
                                                  .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            if (product != null)
            {
                return new JsonResult(product);
            }
            var errorMessage = "No products found for the given buyer ID.";
            return new ObjectResult(errorMessage)
            {
                StatusCode = StatusCodes.Status404NotFound
            };
        }

        public async Task<IActionResult> GetProductSeller(Guid sellerId)
        {
            var product = await _productRepository.GetMany(pro => pro.SellerId.Equals(sellerId))
                                                  .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                                                  .ToListAsync();
            if (product != null)
            {
                return new JsonResult(product);
            }
            var errorMessage = "No products found for the given seller ID.";
            return new ObjectResult(errorMessage)
            {
                StatusCode = StatusCodes.Status404NotFound
            };
        }
        //Get All Product, Search Product by Name, Price, categlory
        public async Task<IEnumerable<ProductViewModel>> GetProducts(ProductFilterModel filter, int skip, int pageSize)
        {
            var query = _productRepository.GetAll().Where(p => p.StatusId != Guid.Parse(ProductStatus.Inactive.GetEnumMember()));

            if (filter.Name != null)
            {
                query = query.Where(pro => pro.Name.Contains(filter.Name));
            }

            if (filter.CategoryName != null)
            {
                query = query.Where(pro => pro.Category.Name.Contains(filter.CategoryName));
            }

            if (filter.MaxPrice != null && filter.MinPrice != null)
            {
                query = query.Where(pro => pro.Price >= filter.MinPrice && pro.Price <= filter.MaxPrice);
            }
            else if (filter.MaxPrice != null)
            {
                query = query.Where(pro => pro.Price <= filter.MaxPrice);
            }
            else if (filter.MinPrice != null)
            {
                query = query.Where(pro => pro.Price >= filter.MinPrice);
            }

            var products = await query
                .Skip(skip)
                .Take(pageSize)
                .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return products;
        }


        // Create Product
        public async Task<IActionResult> CreateProduct(Guid UserId, CreateProductRequest request)
        {
            if (request.ImageProducts.Count > 1)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            if (!_userRepository.Any(user => user.Id.Equals(request.SellerID)))
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }

            if (!_stationRepository.Any(station => station.Id.Equals(request.StationID)))
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            if (!_categoryRepository.Any(category => category.Id.Equals(request.CategoryId)))
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }



            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StationId = request.StationID,
                CategoryId = request.CategoryId,
                SellerId = request.SellerID,
                AddById = UserId,
                StatusId = Guid.Parse(ProductStatus.Active.GetEnumMember()),
                CreatedAt = DateTime.Now
            };
            _productRepository.Add(product);

            foreach (var file in request.ImageProducts)
            {
                var imageProduct = new ImageProduct
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    Url = await UploadProductImageToFirebase(file)
                };
                _imageProductRepository.Add(imageProduct);
            }

            var result = await _unitOfWork.SaveChanges();
            if (result > 0)
            {
                return await GetProduct(product.Id);
            }
            return new StatusCodeResult(400);

        }

        private async Task<String?> UploadProductImageToFirebase(IFormFile file)
        {
            var storage = new FirebaseStorage("prn231-33025.appspot.com");
            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var imageUrl = await storage.Child("images")
                                        .Child(imageName)
                                        .PutAsync(file.OpenReadStream());
            return imageUrl;
        }

        public async Task<IActionResult> UpdateProduct(Guid productID, UpdateProductRequest productRequest)
        {
            var product = await _productRepository.GetMany(pro => pro.Id.Equals(productID) && pro.StatusId == Guid.Parse(ProductStatus.Active.GetEnumMember()))
                                                  .Include(pro => pro.ImageProducts)
                                                  .FirstOrDefaultAsync();
            if (product == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            if (product != null)
            {
                product.Name = productRequest.Name ?? product.Name;
                product.Description = productRequest.Description ?? product.Description;
                product.Price = productRequest.Price ?? product.Price;
                if (productRequest.CategoryId != null)
                {
                    product.CategoryId = (Guid)productRequest.CategoryId;
                }
                if (productRequest.ImageProducts != null && productRequest.ImageProducts.Count != 0)
                {
                    if (productRequest.ImageProducts.Count != 3)
                    {
                        return new StatusCodeResult(400);
                    }
                    var listImage = await _imageProductRepository.GetMany(image => image.ProductId.Equals(productID)).ToListAsync();
                    foreach (var image in listImage)
                    {
                        _imageProductRepository.Remove(image);
                    }
                    foreach (var file in productRequest.ImageProducts)
                    {
                        var imageProduct = new ImageProduct
                        {
                            Id = Guid.NewGuid(),
                            ProductId = product.Id,
                            Url = await UploadProductImageToFirebase(file)
                        };
                        _imageProductRepository.Add(imageProduct);
                    }
                }
                if (productRequest.StationID != null)
                {
                    product.StationId = (Guid)productRequest.StationID;
                }
                if (productRequest.StatusId != null)
                {
                    product.StatusId = (Guid)productRequest.StatusId;
                }

                product.CreatedAt = DateTime.Now;

                _unitOfWork.Product.Update(product);
            }
            return await _unitOfWork.SaveChanges() > 0 ? await GetProduct(productID) : new StatusCodeResult(500);
        }

        public async Task<IActionResult> RemoveProduct(Guid idProduct)
        {
            var product = await _productRepository.GetMany(product => product.Id.Equals(idProduct) && product.StatusId == Guid.Parse(ProductStatus.Active.GetEnumMember())).FirstOrDefaultAsync();
            if (product != null)
            {
                product.StatusId = Guid.Parse(ProductStatus.Inactive.GetEnumMember());
                return await _unitOfWork.SaveChanges() > 0 ? new JsonResult(product) : new JsonResult(new StatusCodeResult(500));
            }

            return new JsonResult(null);
        }

        //list image
        public async Task<IActionResult> GetProductImage(Guid idProduct)
        {
            var product = await _productRepository.FirstOrDefaultAsync(pro => pro.Id == idProduct);
            if (product != null)
            {
                var imageProduct = _unitOfWork.ImageProduct.FirstOrDefaultAsync(pro => pro.Id.Equals(idProduct));
                return new JsonResult(imageProduct);
            }
            return new StatusCodeResult(404);
        }
        public async Task<IActionResult> GetAllProductsSeller()
        {
            var query = _productRepository.GetAll();
            var product = await query.Where(p => p.Seller != null).ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return new JsonResult(product);
        }
        public async Task<IActionResult> GetAllProductsInventory()
        {
            var query = _productRepository.GetAll();
            var product = await query.Where(p => p.Buyer == null).ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return new JsonResult(product);
        }
        public async Task<IActionResult> GetProductInventoryInMonth(DateTime startDate, DateTime endDate)
        {
            var query = await _productRepository.GetMany(t => t.BuyerId == null &&
                t.CreatedAt >= startDate && t.CreatedAt <= endDate).ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return new JsonResult(query);
        }
        public async Task<IActionResult> GetAllProductsSellerInMonth(DateTime startDate, DateTime endDate)
        {
            var query = await _productRepository.GetMany(t => t.SellerId != null && t.CreatedAt >= startDate && t.CreatedAt <= endDate).ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return new JsonResult(query);
        }
    }
}
