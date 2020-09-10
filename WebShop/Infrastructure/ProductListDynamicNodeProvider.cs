using MvcSiteMapProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebShop.DAL;
using WebShop.Models;

namespace WebShop.Infrastructure
{
    public class ProductListDynamicNodeProvider : DynamicNodeProviderBase
    {
        private StoreContext db = new StoreContext();
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            var returnValue = new List<DynamicNode>();

            foreach (Kategoria a in db.Kategorie)
            {
                DynamicNode n = new DynamicNode();
                n.Title = a.Name;
                n.Key = "Kategoria_" + a.KategoriaId;
                n.RouteValues.Add("type", a.Name);
                returnValue.Add(n);

            }

            return returnValue;
        }
    }
}