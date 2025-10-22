using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundAdmin.Application.Interfaces.Dtos
{
    public interface IInvestorBase
    {
        string FullName { get; }
        string Email { get; }
        Guid FundId { get; }
    }
}
