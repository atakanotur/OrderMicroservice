using Business.Abstract;
using Business.Constants;
using Business.RabbitMQ.Producer;
using Core.Utilities.Business;
using Core.Utilities.Results;
using Core.Utilities.Logger;
using DataAccess.Abstract;
using Entities.Concrete;


namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        public IResult Add(Product product)
        {
            var result = BusinessRules.Run(
                CheckIfProductIsAlreadyExist(product.Name));
            if (result != null)
            {
                Logger.LogError(Messages.ProductNotCreated, new Exception(Messages.BusinessRulesNotComply));
                return new ErrorResult(Messages.ProductNotCreated);
            }
            _productDal.Add(product);
            Logger.LogAuditEvent(Messages.ProductCreated);
            RabbitMQProducer.SendMessage(product);
            return new SuccessResult(Messages.ProductCreated);
        }

        public IResult Delete(Product product)
        {
            var result = BusinessRules.Run();
            if (result != null)
            {
                Logger.LogError(Messages.ProductNotDeleted, new Exception(Messages.BusinessRulesNotComply));
                return new ErrorResult(Messages.ProductNotDeleted);
            }
            _productDal.Delete(product);
            Logger.LogAuditEvent(Messages.ProductDeleted);
            return new SuccessResult(Messages.ProductDeleted);
        }

        public IDataResult<List<Product>> GetAll()
        {
            var result = BusinessRules.Run();
            if (result != null)
            {
                Logger.LogError(Messages.ProductsNotListed, new Exception(Messages.BusinessRulesNotComply));
                RabbitMQProducer.SendMessage(Messages.ProductsListed);
                return new ErrorDataResult<List<Product>>(Messages.ProductsNotListed);
            }
            Logger.LogAuditEvent(Messages.ProductsListed);

            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.OrdersListed);
        }

        public IDataResult<Product> GetById(int id)
        {
            var result = BusinessRules.Run();
            if (result != null)
            {
                Logger.LogError(Messages.ProductsNotListed, new Exception(Messages.BusinessRulesNotComply));
                return new ErrorDataResult<Product>(Messages.ProductsNotListed);
            }
            Logger.LogAuditEvent(Messages.ProductsListed);
            return new SuccessDataResult<Product>(_productDal.Get(o => o.Id == id), Messages.ProductsListed);
        }

        public IResult Update(Product product)
        {
            var result = BusinessRules.Run();
            if (result != null)
            {
                Logger.LogError(Messages.ProductNotUpdated, new Exception(Messages.BusinessRulesNotComply));
                return new ErrorResult(Messages.ProductNotUpdated);
            }
            Logger.LogAuditEvent(Messages.ProductUpdated);
            _productDal.Update(product);
            return new SuccessResult(Messages.ProductUpdated);
        }

        private IResult CheckIfProductIsAlreadyExist(string name)
        {
            var product = _productDal.Get(p => p.Name == name);
            if (product != null) return new ErrorResult(Messages.ProductAlreadyExist);
            return new SuccessResult();
        }
    }
}
