using System.ComponentModel.DataAnnotations;

namespace PromoCodeFactory.WebHost.Models
{
    public class GivePromoCodeRequest
    {
        [Required]
        public string ServiceInfo { get; set; }

        [Required]
        public string PartnerName { get; set; }

        [Required]
        public string PromoCode { get; set; }

        [Required]
        public string Preference { get; set; }
    }
}