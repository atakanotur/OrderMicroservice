using Business.Abstract;
using Business.Constants;
using Core.Utilities.Business;
using Core.Utilities.Results;
using Core.Utilities.Logger;
using Core.Aspects.Autofac.Validation;
using DataAccess.Abstract;
using Entities.Concrete;
using Business.ValidationRules.FluentValidation;
using Business.RabbitMQ.Producer;


namespace Business.Concrete
{
    public class OrderManager : IOrderService
    {
        IOrderDal _orderDal;
        public OrderManager (IOrderDal orderDal)
        {
            _orderDal = orderDal;
        }

        [ValidationAspect(typeof(OrderValidator))]
        public IResult Add(Order order)
        {
            var result = BusinessRules.Run();
            if (result != null)
            {
                Logger.LogError(Messages.OrderNotCreated, new Exception(Messages.BusinessRulesNotComply));
                return new ErrorResult(Messages.OrderNotCreated);
            }
            _orderDal.Add(order);
            Logger.LogAuditEvent(Messages.OrderCreated);
            RabbitMQProducer.SendMessage(order);
            return new SuccessResult(Messages.OrderCreated);
        }

        public IResult ChangeStatus(int id, string status)
        {
            var result = BusinessRules.Run();
            if (result != null)
            {
                Logger.LogError(Messages.OrderStatusNotChanged, new Exception(Messages.BusinessRulesNotComply));
                return new ErrorResult(Messages.OrderStatusNotChanged);
            }
            Logger.LogAuditEvent(Messages.OrderStatusChanged);
            return new SuccessResult(Messages.OrderStatusChanged);
        }

        public IResult Delete(Order order)
        {
            var result = BusinessRules.Run();
            if (result != null)
            {
                Logger.LogError(Messages.OrderNotDeleted, new Exception(Messages.BusinessRulesNotComply));
                return new ErrorResult(Messages.OrderNotDeleted);
            }
            _orderDal.Delete(order);
            Logger.LogAuditEvent(Messages.OrderDeleted);
            return new SuccessResult(Messages.OrderDeleted);
        }

        public IDataResult<List<Order>> GetAll()
        {
            var result = BusinessRules.Run();
            if (result != null)
            {
                Logger.LogError(Messages.OrdersNotListed, new Exception(Messages.BusinessRulesNotComply));
                RabbitMQProducer.SendMessage(Messages.OrdersListed);
                return new ErrorDataResult<List<Order>>(Messages.OrdersNotListed);
            }
            Logger.LogAuditEvent(Messages.OrdersListed);
            
            return new SuccessDataResult<List<Order>>(_orderDal.GetAll(), Messages.OrdersListed);
        }

        public IDataResult<List<Order>> GetByCustomerId(int customerId)
        {
            var result = BusinessRules.Run();
            if (result != null)
            {
                Logger.LogError(Messages.OrdersNotListed, new Exception(Messages.BusinessRulesNotComply));
                return new ErrorDataResult<List<Order>>(Messages.OrdersNotListed);
            }
            Logger.LogAuditEvent(Messages.OrdersListed);
            return new SuccessDataResult<List<Order>>(_orderDal.GetAll(o => o.CustomerId == customerId), Messages.OrdersListed);
        }

        public IDataResult<Order> GetById(int id)
        {
            var result = BusinessRules.Run();
            if (result != null)
            {
                Logger.LogError(Messages.OrdersNotListed, new Exception(Messages.BusinessRulesNotComply));
                return new ErrorDataResult<Order>(Messages.OrdersNotListed);
            }
            Logger.LogAuditEvent(Messages.OrdersListed);
            return new SuccessDataResult<Order>(_orderDal.Get(o => o.Id == id), Messages.OrdersListed);
        }

        public IResult Update(Order order)
        {
            var result = BusinessRules.Run();
            if (result != null)
            {
                Logger.LogError(Messages.OrderNotUpdated, new Exception(Messages.BusinessRulesNotComply));
                return new ErrorResult(Messages.OrderNotUpdated);
            }
            Logger.LogAuditEvent(Messages.OrderUpdated);
            _orderDal.Update(order);
            return new SuccessResult(Messages.OrderUpdated);
        }
    }
}
