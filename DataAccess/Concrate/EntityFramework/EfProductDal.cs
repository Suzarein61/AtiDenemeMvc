using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrate.EntityFramework.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrate.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<Product, DenemeContext>, IProductDal
    {

    }
}
