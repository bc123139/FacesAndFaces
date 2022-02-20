﻿using Microsoft.EntityFrameworkCore;
using OrdersApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersApi.Persistence
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrdersContext _context;
        public OrderRepository(OrdersContext context)
        {
            _context = context;

        }

        public Order GetOrder(Guid id)
        {
            return _context.Orders.Include(x=>x.OrderDetails)
                                    .FirstOrDefault(x => x.OrderId == id);
        }

        public async Task<Order> GetOrderAsync(Guid id)
        {
            return await _context.Orders.Include(x => x.OrderDetails)
                                    .FirstOrDefaultAsync(x => x.OrderId == id);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public Task RegisterOrder(Order order)
        {
            //todo: is it good practice?
            _context.Add(order);
            _context.SaveChanges();

            return Task.FromResult(true);
        }

        public void UpdateOrder(Order order)
        {
            //todo: this vs Update
            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
