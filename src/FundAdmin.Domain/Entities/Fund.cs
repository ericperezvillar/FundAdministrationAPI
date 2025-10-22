using System.ComponentModel.DataAnnotations;

namespace FundAdmin.Domain.Entities;

public class Fund
{
    [Key]
    public Guid FundId { get; set; }

    [Required, MaxLength(200)]
    public string FundName { get; set; } = string.Empty;

    [Required, MaxLength(3)]
    public string CurrencyCode { get; set; } = "USD";

    public DateTime LaunchDate { get; set; } = DateTime.UtcNow.Date;

    public ICollection<Investor> Investors { get; set; } = new List<Investor>();
}
