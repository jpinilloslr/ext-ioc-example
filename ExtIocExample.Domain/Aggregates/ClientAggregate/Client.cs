using System.Collections.Generic;
using Domain.Seedwork;
using ExtIocExample.Domain.Aggregates.ProductAggregate;

namespace ExtIocExample.Domain.Aggregates.ClientAggregate
{
    public class Client : Entity<int>
    {
        private HashSet<Product> _products;

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Product> Products
        {
            get { return _products ?? (_products = new HashSet<Product>()); }
            set { _products = new HashSet<Product>(value); }
        }
    }
}