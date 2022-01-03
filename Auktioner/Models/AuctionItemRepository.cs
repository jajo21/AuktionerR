﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Auktioner.Models
{
    public class AuctionItemRepository : IAuctionItemRepository
    {
        private readonly AppDbContext _appDbContext;
        public AuctionItemRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<AuctionItem> AllAuctionItems
        {
            get
            {
                return _appDbContext.AuctionItems.Include(c => c.Category);
            }
        }
        public IEnumerable<AuctionItem> AuctionItemsInStock()
        {
            return _appDbContext.AuctionItems.Where(a => a.InStock == true);
        }
        public IEnumerable<AuctionItem> SoldAuctionItems()
        {
            return _appDbContext.AuctionItems.Where(a => a.InStock == false);
        }

        public AuctionItem GetAuctionItemById(string id)
        {
            return _appDbContext.AuctionItems.FirstOrDefault(item => item.AuctionItemId == id);
        }        
        public void AddToInventory(AuctionItem item)
        {
            if(item.Costs < item.StartingPrice)
            {
                _appDbContext.AuctionItems.Add(item);
                _appDbContext.SaveChanges();
            }
        }
        public void Update(AuctionItem item)
        {
            _appDbContext.AuctionItems.Update(item);
            _appDbContext.SaveChanges();
        }

        public void SetInStock(AuctionItem item)
        {
            if(item.InStock)
            {
                _appDbContext.AuctionItems.FirstOrDefault(auctionItem => auctionItem.Name == item.Name).InStock = false;
            }
            else
            {
                _appDbContext.AuctionItems.FirstOrDefault(auctionItem => auctionItem.Name == item.Name).InStock = true;
            }
        }
    }
}
