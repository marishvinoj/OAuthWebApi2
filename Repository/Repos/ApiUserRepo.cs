using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repos
{
    public class ApiUserRepo : GenericRepository<ApiUser>, IApiUserRepo
    {
        public ApiUserRepo(IUnitOfWork<CustomerDbEntities> unitOfWork)
            : base(unitOfWork)
        {
        }

        public ApiUserRepo(CustomerDbEntities context)
            : base(context)
        {
        }
    }

    public interface IApiUserRepo
    {
            
    }
}
