using MvcSiteMapProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WebShop.DAL;
using WebShop.Models;

namespace WebShop.Infrastructure
{
    public class ProductDetailsDynamicNodeProvider : DynamicNodeProviderBase
    {
        private StoreContext db = new StoreContext();
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            var returnValue = new List<DynamicNode>();

            foreach (Produkt a in db.Produkty)
            {
                DynamicNode n = new DynamicNode();
                n.Title = a.Nazwa;
                n.Key = "Produkt_" + a.ProduktId;
                n.ParentKey = "Kategoria_" + a.KategoriaId;
                n.RouteValues.Add("id", a.ProduktId);
                returnValue.Add(n);

            }
            return returnValue;
        }
    }
}