using Microsoft.Extensions.Options;
using PhotoSi.UsersService.Models;
using System.Net.Http;

namespace PhotoSi.UsersService.Services
{
    public interface IAddressChecker
    {
        Task<bool> IsAddressValidAsync(ShipmentAddress address, CancellationToken cancellationToken);
    }


    public class AddressChecker : IAddressChecker
    {
        private readonly HttpClient _addressBookClient;
        public AddressChecker(
            HttpClient addressBookClient, 
            IOptions<UsersServiceOptions> options
        )
        {
            _addressBookClient = addressBookClient;
            _addressBookClient.BaseAddress = new Uri(options.Value.AddressBookBaseUrl);
        }

        public async Task<bool> IsAddressValidAsync(
            ShipmentAddress address, 
            CancellationToken cancellationToken
        )
        {
            var country = Uri.EscapeDataString(address.Country);
            var city = Uri.EscapeDataString(address.City);
            var postalCode = Uri.EscapeDataString(address.PostalCode);
            var street = Uri.EscapeDataString(address.Street);

            var response = await _addressBookClient
                .GetAsync($"addresses/{country}/{city}/{postalCode}/{street}", cancellationToken);

            return response.IsSuccessStatusCode ? true : false;
        }
    }
 
}
