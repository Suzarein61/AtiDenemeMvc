using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Product: IEntity
    {
        [Key]
        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public int CategoryID { get; set; }

        public decimal UnitPrice { get; set; }

        public short UnitsInStock { get; set; }
    }
}
