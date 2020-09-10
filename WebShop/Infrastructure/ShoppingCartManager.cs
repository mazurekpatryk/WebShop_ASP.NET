using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebShop.DAL;
using WebShop.Models;

namespace WebShop.Infrastructure
{
    public class ShoppingCartManager
    {
        private StoreContext Db;
        private ISessionManager session;
        public const string CartSessionKey = "CartData";

        public ShoppingCartManager(ISessionManager session, StoreContext Db)
        {
            this.session = session;
            this.Db = Db;
        }

        public void AddToCart(int produktid)
        {
            var cart = this.GetCart();

            var cartItem = cart.Find(c => c.Produkt.ProduktId == produktid);

            if (cartItem != null)
                cartItem.Quantity++;
            else
            {
                var produkToAdd = Db.Produkty.Where(a => a.ProduktId == produktid).SingleOrDefault();
                if (produkToAdd != null)
                {
                    var newCartItem = new CartItem()
                    {
                        Produkt = produkToAdd,
                        Quantity = 1,
                        TotalPrice = produkToAdd.Cena
                    };

                    cart.Add(newCartItem);
                }
            }

            session.Set(CartSessionKey, cart);
        }

        public List<CartItem> GetCart()
        {
            List<CartItem> cart;

            if (session.Get<List<CartItem>>(CartSessionKey) == null)
            {
                cart = new List<CartItem>();
            }
            else
            {
                cart = session.Get<List<CartItem>>(CartSessionKey) as List<CartItem>;
            }
            return cart;
        }
    
        public int RemoveFromCart(int produkid)
        {
            var cart = this.GetCart();

            var cartItem = cart.Find(c => c.Produkt.ProduktId== produkid);

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    return cartItem.Quantity;
                }
                else
                {
                    cart.Remove(cartItem);
                }
            }
            return 0;
        }

        public decimal GetCartTotalPrice()
        {
            var cart = this.GetCart();

            return cart.Sum(c => (c.Quantity * c.Produkt.Cena));
        }

        public int GetCartItemCount()
        {
            var cart = this.GetCart();
            int count = cart.Sum(c => c.Quantity);

            return count;
        }

        public Zamowienie CreateOrder(Zamowienie newOrder, string userId)
        {
            var cart = this.GetCart();

            newOrder.DataZamowienia = DateTime.Now;
            // newOrder.UserId = userId;

            this.Db.Zamowienia.Add(newOrder);

            if (newOrder.ZamowioneProdukty == null)
                newOrder.ZamowioneProdukty = new List<ZamowionyProdukt>();

            decimal cartTotal = 0;

            foreach (var cartItem in cart)
            {
                var newOrderItem = new ZamowionyProdukt()
                {
                    ProduktId = cartItem.Produkt.ProduktId,
                    Ilosc = cartItem.Quantity,
                    CenaJednostkowa = cartItem.Produkt.Cena
                };

                cartTotal += (cartItem.Quantity * cartItem.Produkt.Cena);

                newOrder.ZamowioneProdukty.Add(newOrderItem);
            }

            newOrder.CenaZbiorcza = cartTotal;

            this.Db.SaveChanges();

            return newOrder;
        }

        public void EmptyCart()
        {
            session.Set<List<CartItem>>(CartSessionKey, null);
        }

    }
}