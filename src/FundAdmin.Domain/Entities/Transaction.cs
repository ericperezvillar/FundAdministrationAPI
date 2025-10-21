using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FundAdmin.Domain.Enums;

namespace FundAdmin.Domain.Entities;

public class Transaction
{
    [Key]
    public int TransactionId { get; set; }

    [ForeignKey(nameof(Investor))]
    public int InvestorId { get; set; }

    public Investor? Investor { get; set; }

    [Required]
    public TransactionType Type { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
}
