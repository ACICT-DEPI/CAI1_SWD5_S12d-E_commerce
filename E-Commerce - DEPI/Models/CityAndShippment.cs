using System.ComponentModel.DataAnnotations;

namespace E_Commerce___DEPI.Models
{
    public class CityAndShippment
    {
        public int Id { get; set; }
        public string CityName { get; set; }

        public int ShppmentFee { get; set; }
    }
}
