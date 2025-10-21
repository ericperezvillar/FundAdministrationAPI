using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundAdmin.Application.Interfaces.Dtos
{
    public interface IFundBase
    {
        string FundName { get; }
        string CurrencyCode { get; }
        DateTime LaunchDate { get; }
    }
}
