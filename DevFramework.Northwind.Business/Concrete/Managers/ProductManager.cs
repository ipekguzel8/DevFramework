
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

namespace DevFramework.Northwind.Business.Concrete.Managers
{
    public class ProductManager : IProductService
    {
        public IProductDal _productDal;
        private readonly IQueryableRepository<Product> _queryable;
        public ProductManager(IProductDal productDal, IQueryableRepository<Product> queryable)
        {
            _queryable = queryable;
            _productDal = productDal ;
        }
        //[FluentValidationAspect(typeof(ProductValidator))]
        public Product Add(Product product)
        {
            return _productDal.Add(product);
        }
        [CacheAspect(typeof(MemoryCacheManager))]
        public List<Product> GetAll()
        {
            return _productDal.GetList();
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
