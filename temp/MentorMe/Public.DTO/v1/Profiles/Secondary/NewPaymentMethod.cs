using System.ComponentModel.DataAnnotations;

namespace Public.DTO.v1.Profiles.Secondary;

public class NewPaymentMethod
{
    [MinLength(2)] 
    [MaxLength(128)] 
    public string Details { get; set; } = default!;
    [MinLength(1)]
    [MaxLength(32)]
    public string CardCvv { get; set; } = default!;
    [MinLength(1)]
    [MaxLength(32)]
    public string CardExpirationDate { get; set; } = default!;
    [MinLength(1)]
    [MaxLength(32)]
    public string CardNumber { get; set; } = default!;
}