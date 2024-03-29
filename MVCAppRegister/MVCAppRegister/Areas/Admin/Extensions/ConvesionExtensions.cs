﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using MVCAppRegister.Areas.Admin.Models;
using MVCAppRegister.Entities;
using MVCAppRegister.Models;

namespace MVCAppRegister.Areas.Admin.Extensions
{
    public static class ConvesionExtensions
    {

#region Product
        public static async Task<IEnumerable<ProductModel>> Convert(
                 this IEnumerable<Product> products, ApplicationDbContext db)
        {
            if (products.Count().Equals(0))
                return new List<ProductModel>();

            var texts = await db.ProductLinkTexts.ToListAsync();
            var types = await db.ProductTypes.ToListAsync();

            return from p in products
                   select new ProductModel
                   {
                       Id = p.Id,
                       Title = p.Title,
                       Description = p.Description,
                       ImageUrl = p.ImageUrl,
                       ProductLinkTexteId = p.ProductLinkTexteId,
                       ProductTypeId = p.ProductTypeId,
                       ProductLinkTexts = texts,
                       ProductTypes = types
                   };
        }

        public static async Task<ProductModel> Convert(
      this Product product, ApplicationDbContext db)
        {
            var text = await db.ProductLinkTexts.FirstOrDefaultAsync(
                p => p.Id.Equals(product.ProductLinkTexteId));
            var type = await db.ProductTypes.FirstOrDefaultAsync(
                p => p.Id.Equals(product.ProductTypeId));

            var model = new ProductModel
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                ProductLinkTexteId = product.ProductLinkTexteId,
                ProductTypeId = product.ProductTypeId,
                ProductLinkTexts = new List<ProductLinkText>(),
                ProductTypes = new List<ProductType>()
            };

            model.ProductLinkTexts.Add(text);
            model.ProductTypes.Add(type);

            return model;
        }

        #endregion

#region ProductItem
        public static async Task<IEnumerable<ProductItemModel>> Convert(
          this IQueryable<ProductItem> ProductItems, ApplicationDbContext db)
        {
            if (ProductItems.Count().Equals(0))
                return new List<ProductItemModel>();


            return await (from pi in ProductItems
                          select new ProductItemModel
                          {
                              ProductId = pi.ProductId,
                              ItemId = pi.ItemId,
                              ItemTitle = db.Items.FirstOrDefault(i => i.Id.Equals(pi.ItemId)).Title,
                              ProductTitle = db.Products.FirstOrDefault(i => i.Id.Equals(pi.ProductId)).Title

                          }).ToListAsync();
        }

      


        public static async Task<ProductItemModel> Convert(
        this ProductItem productItems, ApplicationDbContext db, bool addListData = true)
        {


            var model = new ProductItemModel
            {
                ItemId = productItems.ItemId,
                ProductId = productItems.ProductId,
                Items = addListData ? await db.Items.ToListAsync():null,
                Products = addListData ? await db.Products.ToListAsync():null,
                ItemTitle= (await db.Items.FirstOrDefaultAsync(i=>i.Id.Equals(productItems.ItemId))).Title,
                ProductTitle=(await db.Products.FirstOrDefaultAsync(i=>i.Id.Equals(productItems.ProductId))).Title

            };


            return model;
        }


        public static async Task<bool> CanChange(this ProductItem productItem, ApplicationDbContext db)
        {
            var oldPI = await db.ProductItems.CountAsync(pi => pi.ProductId.Equals(productItem.OldProductId) &&
                 pi.ItemId.Equals(productItem.OldItemId));

            var newPI = await db.ProductItems.CountAsync(pi => pi.ProductId.Equals(productItem.ProductId) &&
            pi.ItemId.Equals(productItem.ItemId));

            return oldPI.Equals(1) && newPI.Equals(0);

        }

        public static async Task Change(this ProductItem productItem, ApplicationDbContext db)
        {
            var oldProductItem = await db.ProductItems.FirstOrDefaultAsync(pi => pi.ProductId.Equals(productItem.OldProductId) &&
                 pi.ItemId.Equals(productItem.OldItemId));

            var newProductItem = await db.ProductItems.FirstOrDefaultAsync(pi => pi.ProductId.Equals(productItem.ProductId) &&
          pi.ItemId.Equals(productItem.ItemId));

            if (oldProductItem != null && newProductItem == null)
            {
                newProductItem = new ProductItem
                {
                    ItemId = productItem.ItemId,
                    ProductId = productItem.ProductId
                };

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        db.ProductItems.Remove(oldProductItem);
                        db.ProductItems.Add(newProductItem);
                        await db.SaveChangesAsync();
                        transaction.Complete();


                    }
                    catch
                    {
                        transaction.Dispose();
                    }
                }



            }


        }

        #endregion

        #region Subscription Product Item



        public static async Task<IEnumerable<SubscriptionProductModel>> Convert(
        this IQueryable<SubscriptionProduct> subscriptionProducts, ApplicationDbContext db)
        {
            if (subscriptionProducts.Count().Equals(0))
                return new List<SubscriptionProductModel>();


            return await (from pi in subscriptionProducts
                          select new SubscriptionProductModel
                          {
                              ProductId = pi.ProductId,
                              SubscriptionId = pi.SubscriptionId,
                              SubscriptionTitle = db.Items.FirstOrDefault(i => i.Id.Equals(pi.SubscriptionId)).Title,
                              ProductTitle = db.Products.FirstOrDefault(i => i.Id.Equals(pi.ProductId)).Title

                          }).ToListAsync();
        }




        public static async Task<SubscriptionProductModel> Convert(
        this SubscriptionProduct subscriptionProducts, ApplicationDbContext db, bool addListData = true)
        {


            var model = new SubscriptionProductModel
            {
                SubscriptionId = subscriptionProducts.SubscriptionId,
                ProductId = subscriptionProducts.ProductId,
                Subscriptions = addListData ? await db.Subscriptions.ToListAsync() : null,
                Products = addListData ? await db.Products.ToListAsync() : null,
                SubscriptionTitle = (await db.Subscriptions.FirstOrDefaultAsync(i => i.Id.Equals(subscriptionProducts.SubscriptionId))).Title,
                ProductTitle = (await db.Products.FirstOrDefaultAsync(i => i.Id.Equals(subscriptionProducts.ProductId))).Title

            };


            return model;
        }


        public static async Task<bool> CanChange(this SubscriptionProduct subscriptionProduct, ApplicationDbContext db)
        {
            var oldSP = await db.SubscriptionProducts.CountAsync(sp => sp.ProductId.Equals(subscriptionProduct.OldProductId) &&
               sp.SubscriptionId.Equals(subscriptionProduct.OldSubscriptionId));

            var newSP = await db.SubscriptionProducts.CountAsync(sp => sp.ProductId.Equals(subscriptionProduct.ProductId) &&
            sp.SubscriptionId.Equals(subscriptionProduct.OldSubscriptionId));

            return oldSP.Equals(1) && newSP.Equals(0);

        }

        public static async Task Change(this SubscriptionProduct subscriptionProduct, ApplicationDbContext db)
        {
            var oldsubscriptionProduct = await db.SubscriptionProducts.FirstOrDefaultAsync(sp => sp.ProductId.Equals(subscriptionProduct.OldProductId) &&
                 sp.SubscriptionId.Equals(subscriptionProduct.OldSubscriptionId));

            var newsubscriptionProduct = await db.SubscriptionProducts.FirstOrDefaultAsync(sp => sp.ProductId.Equals(subscriptionProduct.ProductId) &&
          sp.SubscriptionId.Equals(subscriptionProduct.SubscriptionId));

            if (oldsubscriptionProduct != null && newsubscriptionProduct == null)
            {
                newsubscriptionProduct = new SubscriptionProduct
                {
                    SubscriptionId = subscriptionProduct.SubscriptionId,
                    ProductId = subscriptionProduct.ProductId
                };

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        db.SubscriptionProducts.Remove(oldsubscriptionProduct);
                        db.SubscriptionProducts.Add(newsubscriptionProduct);
                        await db.SaveChangesAsync();
                        transaction.Complete();


                    }
                    catch
                    {
                        transaction.Dispose();
                    }
                }



            }


        }

       #endregion


    }
}
