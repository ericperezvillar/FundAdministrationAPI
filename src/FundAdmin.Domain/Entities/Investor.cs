using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FundAdmin.Domain.Entities;

public class Investor
{
    [Key]
    public int InvestorId { get; set; }

    [Required, MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required, MaxLength(320)]
    public string Email { get; set; } = string.Empty;

    [ForeignKey(nameof(Fund))]
    public int FundId { get; set; }

    public Fund? Fund { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
