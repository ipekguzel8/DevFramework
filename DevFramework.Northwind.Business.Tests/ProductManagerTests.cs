using DevFramework.Core.DataAccess;
using DevFramework.Northwind.Business.Concrete.Managers;
using DevFramework.Northwind.DataAccess.Abstract;
using DevFramework.Northwind.Entities.Concrete;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace DevFramework.Northwind.Business.Tests
{
    [TestClass]
    public class ProductManagerTests
    {
        //[ExpectedException(typeof(ValidationException))]
        [TestMethod]
        public void Product_validation_check()
        {
            Mock<IProductDal> mock = new Mock<IProductDal>();
            Mock<IQueryableRepository<Product>> mock2 = new Mock<IQueryableRepository<Product>>();
            ProductManager productManager = new ProductManager(mock.Object,mock2.Object);
            productManager.Add(new Product());
        }
    }
}
