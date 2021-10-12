using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repos
{
    class EmployeeRepo : GenericRepository<ApiUser>, IEmployeeRepo
    {
        public EmployeeRepo(IUnitOfWork<CustomerDbEntities> unitOfWork)
            : base(unitOfWork)
        {
        }

        public EmployeeRepo(CustomerDbEntities context)
            : base(context)
        {
        }
    }

    public interface IEmployeeRepo
    {

    }
}
