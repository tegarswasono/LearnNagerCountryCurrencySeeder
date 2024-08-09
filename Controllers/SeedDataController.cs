using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nager.Country;

namespace LearnNagerCountrySeeder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedDataController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var result = GetCountryCurrencyCode();
            return Ok(result);
        }
        private static List<GetCountryCurrencyModel> GetCountryCurrencyCode()
        {
            var results = new List<GetCountryCurrencyModel>();

            var countryProvider = new CountryProvider();
            var countries = countryProvider.GetCountries();
            foreach (var country in countries.Where(x => x.CallingCodes.Length >= 1).ToList())
            {
                var currency = country.Currencies.FirstOrDefault();
                string phoneCountryCode = "";
                foreach (var callingCode in country.CallingCodes)
                {
                    phoneCountryCode = "+" + callingCode + ",";
                }
                if (phoneCountryCode.Length > 0) phoneCountryCode = phoneCountryCode.Substring(0, phoneCountryCode.Length - 1);
                var newData = new GetCountryCurrencyModel
                {
                    CountryCode = country.Alpha3Code.ToString(),
                    CountryName = country.CommonName,
                    PhoneCountryCode = phoneCountryCode
                };
                if (currency != null)
                {
                    newData.CurrencyCode = currency.IsoCode;
                    newData.CurrencyName = currency.Name;
                    newData.CurrencySymbol = currency.Symbol;
                }
                results.Add(newData);
            }
            return results;
        }
    }
    class GetCountryCurrencyModel
    {
        public string CountryCode { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public string PhoneCountryCode { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
        public string CurrencyName { get; set; } = string.Empty;
        public string CurrencySymbol { get; set; } = string.Empty;
    }
}
