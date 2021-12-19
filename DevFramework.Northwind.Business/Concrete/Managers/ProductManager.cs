
using DevFramework.Core.CrossCuttingConcerns.Validation.FluentValidation;
using DevFramework.Northwind.Business.Abstract;
using DevFramework.Northwind.Business.ValidationRules.FluentValidation;
using DevFramework.Northwind.DataAccess.Abstract;
using DevFramework.Northwind.DataAccess.Concrete.EntityFramework;
using DevFramework.Northwind.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevFramework.Core.Aspect.Postsharp;
using DevFramework.Core.DataAccess;
using System.Transactions;
using DevFramework.Core.Aspect.Postsharp.ValidationAspects;
using DevFramework.Core.Aspect.Postsharp.TransactionAspects;
using DevFramework.Core.Aspect.Postsharp.CacheAspect;
using DevFramework.Core.CrossCuttingConcerns.Caching.Microsoft;
using DevFramework.Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using DevFramework.Core.Aspect.Postsharp.LogAspects;
using DevFramework.Core.Aspect.Postsharp.PerformanceAspects;
using System.Threading;
using DevFramework.Core.Aspect.Postsharp.AuthorizationAspects;
using AutoMapper;
using DevFramework.Core.Utilities.Mappings;

namespace DevFramework.Northwind.Business.Concrete.Managers
{
    public class ProductManager : IProductService
    {
        public IProductDal _productDal;
        private readonly IMapper _mapper;
        public ProductManager(IProductDal productDal,IMapper mapper)
        {
            _productDal = productDal ;
            _mapper = mapper;
        }
        //[FluentValidationAspect(typeof(ProductValidator))]
        [LogAspect(typeof(FileLogger))]
        public Product Add(Product product)
        {
            return _productDal.Add(product);
        }
        [CacheAspect(typeof(MemoryCacheManager))]
        //[LogAspect(typeof(DatabaseLogger))]
        //[LogAspect(typeof(FileLogger))]
        [PerformanceCounterAspect(2)]
        //[SecuredOperation(Roles="Admin,Editor")]
        public List<Product> GetAll()
        {
            var products = _mapper.Map<List<Product>>(_productDal.GetList());
            //var products= AutoMapperHelper.MapToSameTypeList<Product>(_productDal.GetList());
            return products;
            /*return _productDal.GetList().Select(p => new Product { 
                CategoryId=p.CategoryId,
                ProductId=p.ProductId,
                ProductName=p.ProductName,
                QuantityPerUnit=p.QuantityPerUnit,
                UnitPrice=p.UnitPrice
            }).ToList();*/

        }

        

        public Product GetById(int id)
        {
            return _productDal.Get(p=>p.ProductId==id);
        }
        [FluentValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect(typeof(MemoryCacheManager))]
        public Product Update(Product product)
        {
            ValidatorTool.FluentValidate(new ProductValidator(), product);
            return _productDal.Update(product);
        }
        [TransactionScopeAspect]
        public void TransactionalOperation(Product product1, Product product2)
        {   
                    _productDal.Add(product1);
                    _productDal.Update(product2);
            
        }
    }
}
