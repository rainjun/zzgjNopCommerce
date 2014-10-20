using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Commissions;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.Shipping;
using Nop.Services.Events;
using Nop.Core.Domain.Commissions;
using Nop.Core.Domain.Common;

namespace Nop.Services.Commissions
{
    /// <summary>
    /// Order service
    /// </summary>
    public partial class CommissionService : ICommissionService
    {
        #region Fields

        private readonly IRepository<GenericAttribute> _gaRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Commission> _commissionRepository;
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly IRepository<OrderNote> _orderNoteRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<RecurringPayment> _recurringPaymentRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<ReturnRequest> _returnRequestRepository;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="genericAttribute">genericAttribute repository</param>
        /// <param name="orderRepository">Order repository</param>
        /// <param name="commissionRepository">commission Repository</param>
        /// <param name="orderItemRepository">Order item repository</param>
        /// <param name="orderNoteRepository">Order note repository</param>
        /// <param name="productRepository">Product repository</param>
        /// <param name="recurringPaymentRepository">Recurring payment repository</param>
        /// <param name="customerRepository">Customer repository</param>
        /// <param name="returnRequestRepository">Return request repository</param>
        /// <param name="eventPublisher">Event published</param>
        public CommissionService(
            IRepository<GenericAttribute> genericAttribute,
            IRepository<Order> orderRepository,
            IRepository<Commission> commissionRepository,
            IRepository<OrderItem> orderItemRepository,
            IRepository<OrderNote> orderNoteRepository,
            IRepository<Product> productRepository,
            IRepository<RecurringPayment> recurringPaymentRepository,
            IRepository<Customer> customerRepository,
            IRepository<ReturnRequest> returnRequestRepository,
            IEventPublisher eventPublisher)
        {
            this._gaRepository = genericAttribute;
            this._orderRepository = orderRepository;
            this._commissionRepository = commissionRepository;
            this._orderItemRepository = orderItemRepository;
            this._orderNoteRepository = orderNoteRepository;
            this._productRepository = productRepository;
            this._recurringPaymentRepository = recurringPaymentRepository;
            this._customerRepository = customerRepository;
            this._returnRequestRepository = returnRequestRepository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        #region Orders

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="orderId">The order identifier</param>
        /// <returns>Order</returns>
        public virtual Order GetOrderById(int orderId)
        {
            if (orderId == 0)
                return null;

            return _orderRepository.GetById(orderId);
        }

        /// <summary>
        /// Get orders by identifiers
        /// </summary>
        /// <param name="orderIds">Order identifiers</param>
        /// <returns>Order</returns>
        public virtual IList<Order> GetOrdersByIds(int[] orderIds)
        {
            if (orderIds == null || orderIds.Length == 0)
                return new List<Order>();

            var query = from o in _orderRepository.Table
                        where orderIds.Contains(o.Id)
                        select o;
            var orders = query.ToList();
            //sort by passed identifiers
            var sortedOrders = new List<Order>();
            foreach (int id in orderIds)
            {
                var order = orders.Find(x => x.Id == id);
                if (order != null)
                    sortedOrders.Add(order);
            }
            return sortedOrders;
        }

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="orderGuid">The order identifier</param>
        /// <returns>Order</returns>
        public virtual Order GetOrderByGuid(Guid orderGuid)
        {
            if (orderGuid == Guid.Empty)
                return null;

            var query = from o in _orderRepository.Table
                        where o.OrderGuid == orderGuid
                        select o;
            var order = query.FirstOrDefault();
            return order;
        }

        /// <summary>
        /// Deletes an order
        /// </summary>
        /// <param name="order">The order</param>
        public virtual void DeleteCommission(Commission order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            order.Deleted = true;
            UpdateCommission(order);
        }

        /// <summary>
        /// Search orders
        /// </summary>
        /// <param name="storeId">Store identifier; 0 to load all orders</param>
        /// <param name="vendorId">Vendor identifier; null to load all orders</param>
        /// <param name="customerId">Customer identifier; 0 to load all orders</param>
        /// <param name="productId">Product identifier which was purchased in an order; 0 to load all orders</param>
        /// <param name="affiliateId">Affiliate identifier; 0 to load all orders</param>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="os">Order status; null to load all orders</param>
        /// <param name="ps">Order payment status; null to load all orders</param>
        /// <param name="ss">Order shipment status; null to load all orders</param>
        /// <param name="cs">Order commission status; null to load all orders</param>
        /// <param name="billingEmail">Billing email. Leave empty to load all records.</param>
        /// <param name="orderGuid">Search by order GUID (Global unique identifier) or part of GUID. Leave empty to load all orders.</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Order collection</returns>
        public virtual IPagedList<Order> SearchOrders(int storeId = 0,
            int vendorId = 0, int customerId = 0,
            int productId = 0, int affiliateId = 0,
            DateTime? createdFromUtc = null, DateTime? createdToUtc = null,
            OrderStatus? os = null, PaymentStatus? ps = null, ShippingStatus? ss = null, CommissionStatus? cs = null,
            string billingEmail = null, string orderGuid = null,
            int pageIndex = 0, int pageSize = int.MaxValue, CommissionListType? clt = null, int stateId = 0, int cityId = 0, int districtId = 0)
        {
            int? orderStatusId = null;
            if (os.HasValue)
                orderStatusId = (int)os.Value;

            int? paymentStatusId = null;
            if (ps.HasValue)
                paymentStatusId = (int)ps.Value;

            int? shippingStatusId = null;
            if (ss.HasValue)
                shippingStatusId = (int)ss.Value;

            int? commissionStatusId = null;
            if (cs.HasValue)
                commissionStatusId = (int)cs.Value;

            var query = _orderRepository.Table;
            if (storeId > 0)
                query = query.Where(o => o.StoreId == storeId);
            if (vendorId > 0)
            {
                query = query
                    .Where(o => o.OrderItems
                    .Any(orderItem => orderItem.Product.VendorId == vendorId));
            }
            if (customerId > 0)
                query = query.Where(o => o.CustomerId == customerId);
            if (productId > 0)
            {
                query = query
                    .Where(o => o.OrderItems
                    .Any(orderItem => orderItem.Product.Id == productId));
            }
            if (affiliateId > 0)
                query = query.Where(o => o.AffiliateId == affiliateId);
            if (createdFromUtc.HasValue)
                query = query.Where(o => createdFromUtc.Value <= o.CreatedOnUtc);
            if (createdToUtc.HasValue)
                query = query.Where(o => createdToUtc.Value >= o.CreatedOnUtc);
            if (orderStatusId.HasValue)
                query = query.Where(o => orderStatusId.Value == o.OrderStatusId);
            if (paymentStatusId.HasValue)
                query = query.Where(o => paymentStatusId.Value == o.PaymentStatusId);
            if (shippingStatusId.HasValue)
                query = query.Where(o => shippingStatusId.Value == o.ShippingStatusId);
            if (commissionStatusId.HasValue)
                query = query.Where(o => commissionStatusId.Value == o.CommissionStatusId);
            if (!String.IsNullOrEmpty(billingEmail))
                query = query.Where(o => o.BillingAddress != null && !String.IsNullOrEmpty(o.BillingAddress.Email) && o.BillingAddress.Email.Contains(billingEmail));
            query = query.Where(o => !o.Deleted);
            query = query.OrderByDescending(o => o.CreatedOnUtc);

            //ranAdd 增加省市区的筛选
            int spid = stateId;
            int ctid = cityId;
            int diid = districtId;
            if (spid > 0)
            {
                query = query.Where(o => o.ShippingAddress.StateProvinceId == spid);
            }
            if (ctid > 0)
            {
                query = query.Where(o => o.ShippingAddress.CityId == ctid);
            }
            if (diid > 0)
            {
                query = query.Where(o => o.ShippingAddress.DistrictId == diid);
            }

            DateTime now = DateTime.UtcNow;
            DateTime firstDayOfThisMonth = new DateTime(now.Year, now.Month, 1);
            if (clt.HasValue)
            {
                if (clt.Equals(CommissionListType.Order))
                {
                    //rainAdd 增加待结算佣金列表
                    //佣金未结算，而且订单已经完成（已确认收货），而且是上月的创建的
                    query = query.Where(o => o.CommissionStatusId == (int)CommissionStatus.Pending
                        && o.OrderStatusId == (int)OrderStatus.Complete
                        && o.CreatedOnUtc < firstDayOfThisMonth);
                }
                else if (clt.Equals(CommissionListType.New))
                {
                    //rainAdd 增加新增的佣金列表
                    //佣金未结算
                    query = query.Where(o => o.CommissionStatusId == (int)CommissionStatus.Pending
                        && (o.OrderStatusId == (int)OrderStatus.Pending
                            || o.OrderStatusId == (int)OrderStatus.Processing
                            || (o.OrderStatusId == (int)OrderStatus.Complete
                            && o.CreatedOnUtc >= firstDayOfThisMonth)));
                }
            }

            if (!String.IsNullOrEmpty(orderGuid))
            {
                //filter by GUID. Filter in BLL because EF doesn't support casting of GUID to string
                var orders = query.ToList();
                orders = orders.FindAll(o => o.OrderGuid.ToString().ToLowerInvariant().Contains(orderGuid.ToLowerInvariant()));
                return new PagedList<Order>(orders, pageIndex, pageSize);
            }
            else
            {
                //database layer paging
                return new PagedList<Order>(query, pageIndex, pageSize);
            }

        }


        /// <summary>
        /// search commissions
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="createdFromUtc"></param>
        /// <param name="createdToUtc"></param>
        /// <param name="os"></param>
        /// <param name="ps"></param>
        /// <param name="ss"></param>
        /// <param name="cs"></param>
        /// <param name="customerUserName"></param>
        /// <param name="orderGuid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="clt"></param>
        /// <param name="stateId"></param>
        /// <param name="cityId"></param>
        /// <param name="districtId"></param>
        /// <returns></returns>
        public virtual IPagedList<Commission> SearchCommissions(int customerId = 0,
            DateTime? createdFromUtc = null, DateTime? createdToUtc = null,
            OrderStatus? os = null, PaymentStatus? ps = null, ShippingStatus? ss = null, CommissionStatus? cs = null,
            string customerUserName = null, string orderGuid = null,
            int pageIndex = 0, int pageSize = int.MaxValue,int stateId = 0, int cityId = 0, int districtId = 0)
        {
            int? orderStatusId = null;
            if (os.HasValue)
                orderStatusId = (int)os.Value;

            int? paymentStatusId = null;
            if (ps.HasValue)
                paymentStatusId = (int)ps.Value;

            int? shippingStatusId = null;
            if (ss.HasValue)
                shippingStatusId = (int)ss.Value;

            int? commissionStatusId = null;
            if (cs.HasValue)
                commissionStatusId = (int)cs.Value;

            var query = _commissionRepository.Table;

            if (customerId > 0)
                query = query.Where(o => o.CustomerId == customerId);
            if (createdFromUtc.HasValue)
                query = query.Where(o => createdFromUtc.Value <= o.CreatedOnUtc);
            if (createdToUtc.HasValue)
                query = query.Where(o => createdToUtc.Value >= o.CreatedOnUtc);
            //if (orderStatusId.HasValue)
            //    query = query.Where(o => orderStatusId.Value == o.OrderStatusId);
            //if (paymentStatusId.HasValue)
            //    query = query.Where(o => paymentStatusId.Value == o.PaymentStatusId);
            //if (shippingStatusId.HasValue)
            //    query = query.Where(o => shippingStatusId.Value == o.ShippingStatusId);
            if (commissionStatusId.HasValue)
                query = query.Where(o => commissionStatusId.Value == o.CommissionStatusId);
            if (!String.IsNullOrEmpty(customerUserName))
                query = query.Where(o => o.Customer.Username != null && !String.IsNullOrEmpty(o.Customer.Username) && o.Customer.Username.Contains(customerUserName));
            query = query.Where(o => !o.Deleted);
            

            //ranAdd 增加省市区的筛选
            int spid = stateId;
            int ctid = cityId;
            int diid = districtId;
            if (spid > 0)
            {
                query = query
                   .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { Commission = x, Attribute = y })
                   .Where((z => z.Attribute.KeyGroup == "Customer" &&
                       z.Attribute.Key == SystemCustomerAttributeNames.StateProvinceId &&
                       z.Attribute.Value.Contains(spid.ToString())))
                   .Select(z => z.Commission);
            }
            if (ctid > 0)
            {
                query = query
                  .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { Commission = x, Attribute = y })
                  .Where((z => z.Attribute.KeyGroup == "Customer" &&
                      z.Attribute.Key == SystemCustomerAttributeNames.CityId &&
                      z.Attribute.Value.Contains(ctid.ToString())))
                  .Select(z => z.Commission);
            }
            if (diid > 0)
            {
                query = query
                  .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { Commission = x, Attribute = y })
                  .Where((z => z.Attribute.KeyGroup == "Customer" &&
                      z.Attribute.Key == SystemCustomerAttributeNames.DistrictId &&
                      z.Attribute.Value.Contains(diid.ToString())))
                  .Select(z => z.Commission);
            }
            query = query.OrderByDescending(o => o.CreatedOnUtc);

            if (!String.IsNullOrEmpty(orderGuid))
            {
                //filter by GUID. Filter in BLL because EF doesn't support casting of GUID to string
                var commissions = query.ToList();
                commissions = commissions.FindAll(o => o.OrderGuid.ToString().ToLowerInvariant().Contains(orderGuid.ToLowerInvariant()));
                return new PagedList<Commission>(commissions, pageIndex, pageSize);
            }
            else
            {
                //database layer paging
                return new PagedList<Commission>(query, pageIndex, pageSize);
            }

        }

        /// <summary>
        /// Inserts an order
        /// </summary>
        /// <param name="order">Order</param>
        public virtual void InsertCommission(Commission commission)
        {
            if (commission == null)
                throw new ArgumentNullException("commission");

            _commissionRepository.Insert(commission);

            //event notification
            _eventPublisher.EntityInserted(commission);
        }

        /// <summary>
        /// Updates the order
        /// </summary>
        /// <param name="order">The order</param>
        public virtual void UpdateCommission(Commission commission)
        {
            if (commission == null)
                throw new ArgumentNullException("commission");

            _commissionRepository.Update(commission);

            //event notification
            _eventPublisher.EntityUpdated(commission);
        }

        /// <summary>
        /// Get an order by authorization transaction ID and payment method system name
        /// </summary>
        /// <param name="authorizationTransactionId">Authorization transaction ID</param>
        /// <param name="paymentMethodSystemName">Payment method system name</param>
        /// <returns>Order</returns>
        public virtual Order GetOrderByAuthorizationTransactionIdAndPaymentMethod(string authorizationTransactionId,
            string paymentMethodSystemName)
        {
            var query = _orderRepository.Table;
            if (!String.IsNullOrWhiteSpace(authorizationTransactionId))
                query = query.Where(o => o.AuthorizationTransactionId == authorizationTransactionId);

            if (!String.IsNullOrWhiteSpace(paymentMethodSystemName))
                query = query.Where(o => o.PaymentMethodSystemName == paymentMethodSystemName);

            query = query.OrderByDescending(o => o.CreatedOnUtc);
            var order = query.FirstOrDefault();
            return order;
        }

        #endregion

        #region Orders items

        /// <summary>
        /// Gets an order item
        /// </summary>
        /// <param name="orderItemId">Order item identifier</param>
        /// <returns>Order item</returns>
        public virtual OrderItem GetOrderItemById(int orderItemId)
        {
            if (orderItemId == 0)
                return null;

            return _orderItemRepository.GetById(orderItemId);
        }

        /// <summary>
        /// Gets an item
        /// </summary>
        /// <param name="orderItemGuid">Order identifier</param>
        /// <returns>Order item</returns>
        public virtual OrderItem GetOrderItemByGuid(Guid orderItemGuid)
        {
            if (orderItemGuid == Guid.Empty)
                return null;

            var query = from orderItem in _orderItemRepository.Table
                        where orderItem.OrderItemGuid == orderItemGuid
                        select orderItem;
            var item = query.FirstOrDefault();
            return item;
        }

        /// <summary>
        /// Gets all order items
        /// </summary>
        /// <param name="orderId">Order identifier; null to load all records</param>
        /// <param name="customerId">Customer identifier; null to load all records</param>
        /// <param name="createdFromUtc">Order created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Order created date to (UTC); null to load all records</param>
        /// <param name="os">Order status; null to load all records</param>
        /// <param name="ps">Order payment status; null to load all records</param>
        /// <param name="ss">Order shipment status; null to load all records</param>
        /// <param name="loadDownloableProductsOnly">Value indicating whether to load downloadable products only</param>
        /// <returns>Order collection</returns>
        public virtual IList<OrderItem> GetAllOrderItems(int? orderId,
            int? customerId, DateTime? createdFromUtc, DateTime? createdToUtc,
            OrderStatus? os, PaymentStatus? ps, ShippingStatus? ss,
            bool loadDownloableProductsOnly)
        {
            int? orderStatusId = null;
            if (os.HasValue)
                orderStatusId = (int)os.Value;

            int? paymentStatusId = null;
            if (ps.HasValue)
                paymentStatusId = (int)ps.Value;

            int? shippingStatusId = null;
            if (ss.HasValue)
                shippingStatusId = (int)ss.Value;


            var query = from orderItem in _orderItemRepository.Table
                        join o in _orderRepository.Table on orderItem.OrderId equals o.Id
                        join p in _productRepository.Table on orderItem.ProductId equals p.Id
                        where (!orderId.HasValue || orderId.Value == 0 || orderId == o.Id) &&
                        (!customerId.HasValue || customerId.Value == 0 || customerId == o.CustomerId) &&
                        (!createdFromUtc.HasValue || createdFromUtc.Value <= o.CreatedOnUtc) &&
                        (!createdToUtc.HasValue || createdToUtc.Value >= o.CreatedOnUtc) &&
                        (!orderStatusId.HasValue || orderStatusId == o.OrderStatusId) &&
                        (!paymentStatusId.HasValue || paymentStatusId.Value == o.PaymentStatusId) &&
                        (!shippingStatusId.HasValue || shippingStatusId.Value == o.ShippingStatusId) &&
                        (!loadDownloableProductsOnly || p.IsDownload) &&
                        !o.Deleted
                        orderby o.CreatedOnUtc descending, orderItem.Id
                        select orderItem;

            var orderItems = query.ToList();
            return orderItems;
        }

        /// <summary>
        /// Delete an order item
        /// </summary>
        /// <param name="orderItem">The order item</param>
        public virtual void DeleteOrderItem(OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException("orderItem");

            _orderItemRepository.Delete(orderItem);

            //event notification
            _eventPublisher.EntityDeleted(orderItem);
        }

        #endregion

        #region Orders

        /// <summary>
        /// Gets an order note
        /// </summary>
        /// <param name="orderNoteId">The order note identifier</param>
        /// <returns>Order note</returns>
        public virtual OrderNote GetOrderNoteById(int orderNoteId)
        {
            if (orderNoteId == 0)
                return null;

            return _orderNoteRepository.GetById(orderNoteId);
        }

        /// <summary>
        /// Deletes an order note
        /// </summary>
        /// <param name="orderNote">The order note</param>
        public virtual void DeleteOrderNote(OrderNote orderNote)
        {
            if (orderNote == null)
                throw new ArgumentNullException("orderNote");

            _orderNoteRepository.Delete(orderNote);

            //event notification
            _eventPublisher.EntityDeleted(orderNote);
        }

        #endregion

        #region Recurring payments

        /// <summary>
        /// Deletes a recurring payment
        /// </summary>
        /// <param name="recurringPayment">Recurring payment</param>
        public virtual void DeleteRecurringPayment(RecurringPayment recurringPayment)
        {
            if (recurringPayment == null)
                throw new ArgumentNullException("recurringPayment");

            recurringPayment.Deleted = true;
            UpdateRecurringPayment(recurringPayment);
        }

        /// <summary>
        /// Gets a recurring payment
        /// </summary>
        /// <param name="recurringPaymentId">The recurring payment identifier</param>
        /// <returns>Recurring payment</returns>
        public virtual RecurringPayment GetRecurringPaymentById(int recurringPaymentId)
        {
            if (recurringPaymentId == 0)
                return null;

            return _recurringPaymentRepository.GetById(recurringPaymentId);
        }

        /// <summary>
        /// Inserts a recurring payment
        /// </summary>
        /// <param name="recurringPayment">Recurring payment</param>
        public virtual void InsertRecurringPayment(RecurringPayment recurringPayment)
        {
            if (recurringPayment == null)
                throw new ArgumentNullException("recurringPayment");

            _recurringPaymentRepository.Insert(recurringPayment);

            //event notification
            _eventPublisher.EntityInserted(recurringPayment);
        }

        /// <summary>
        /// Updates the recurring payment
        /// </summary>
        /// <param name="recurringPayment">Recurring payment</param>
        public virtual void UpdateRecurringPayment(RecurringPayment recurringPayment)
        {
            if (recurringPayment == null)
                throw new ArgumentNullException("recurringPayment");

            _recurringPaymentRepository.Update(recurringPayment);

            //event notification
            _eventPublisher.EntityUpdated(recurringPayment);
        }

        /// <summary>
        /// Search recurring payments
        /// </summary>
        /// <param name="storeId">The store identifier; 0 to load all records</param>
        /// <param name="customerId">The customer identifier; 0 to load all records</param>
        /// <param name="initialOrderId">The initial order identifier; 0 to load all records</param>
        /// <param name="initialOrderStatus">Initial order status identifier; null to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Recurring payment collection</returns>
        public virtual IPagedList<RecurringPayment> SearchRecurringPayments(int storeId,
            int customerId, int initialOrderId, OrderStatus? initialOrderStatus,
            int pageIndex, int pageSize, bool showHidden = false)
        {
            int? initialOrderStatusId = null;
            if (initialOrderStatus.HasValue)
                initialOrderStatusId = (int)initialOrderStatus.Value;

            var query1 = from rp in _recurringPaymentRepository.Table
                         join c in _customerRepository.Table on rp.InitialOrder.CustomerId equals c.Id
                         where
                         (!rp.Deleted) &&
                         (showHidden || !rp.InitialOrder.Deleted) &&
                         (showHidden || !c.Deleted) &&
                         (showHidden || rp.IsActive) &&
                         (customerId == 0 || rp.InitialOrder.CustomerId == customerId) &&
                         (storeId == 0 || rp.InitialOrder.StoreId == storeId) &&
                         (initialOrderId == 0 || rp.InitialOrder.Id == initialOrderId) &&
                         (!initialOrderStatusId.HasValue || initialOrderStatusId.Value == 0 || rp.InitialOrder.OrderStatusId == initialOrderStatusId.Value)
                         select rp.Id;

            var query2 = from rp in _recurringPaymentRepository.Table
                         where query1.Contains(rp.Id)
                         orderby rp.StartDateUtc, rp.Id
                         select rp;

            var recurringPayments = new PagedList<RecurringPayment>(query2, pageIndex, pageSize);
            return recurringPayments;
        }

        #endregion

        #region Return requests

        /// <summary>
        /// Deletes a return request
        /// </summary>
        /// <param name="returnRequest">Return request</param>
        public virtual void DeleteReturnRequest(ReturnRequest returnRequest)
        {
            if (returnRequest == null)
                throw new ArgumentNullException("returnRequest");

            _returnRequestRepository.Delete(returnRequest);

            //event notification
            _eventPublisher.EntityDeleted(returnRequest);
        }

        /// <summary>
        /// Gets a return request
        /// </summary>
        /// <param name="returnRequestId">Return request identifier</param>
        /// <returns>Return request</returns>
        public virtual ReturnRequest GetReturnRequestById(int returnRequestId)
        {
            if (returnRequestId == 0)
                return null;

            return _returnRequestRepository.GetById(returnRequestId);
        }

        /// <summary>
        /// Search return requests
        /// </summary>
        /// <param name="storeId">Store identifier; 0 to load all entries</param>
        /// <param name="customerId">Customer identifier; null to load all entries</param>
        /// <param name="orderItemId">Order item identifier; 0 to load all entries</param>
        /// <param name="rs">Return request status; null to load all entries</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Return requests</returns>
        public virtual IPagedList<ReturnRequest> SearchReturnRequests(int storeId, int customerId,
            int orderItemId, ReturnRequestStatus? rs, int pageIndex, int pageSize)
        {
            var query = _returnRequestRepository.Table;
            if (storeId > 0)
                query = query.Where(rr => storeId == rr.StoreId);
            if (customerId > 0)
                query = query.Where(rr => customerId == rr.CustomerId);
            if (rs.HasValue)
            {
                int returnStatusId = (int)rs.Value;
                query = query.Where(rr => rr.ReturnRequestStatusId == returnStatusId);
            }
            if (orderItemId > 0)
                query = query.Where(rr => rr.OrderItemId == orderItemId);

            query = query.OrderByDescending(rr => rr.CreatedOnUtc).ThenByDescending(rr => rr.Id);

            var returnRequests = new PagedList<ReturnRequest>(query, pageIndex, pageSize);
            return returnRequests;
        }

        #endregion

        #endregion
    }
}
