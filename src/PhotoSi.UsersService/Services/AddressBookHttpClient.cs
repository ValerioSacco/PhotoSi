using Microsoft.Extensions.Options;

namespace PhotoSi.UsersService.Services
{
    public class AddressBookHttpClient
    {
        private readonly HttpClient _httpClient;

        public AddressBookHttpClient(
            HttpClient httpClient, 
            IOptions<UsersServiceOptions> options
        )
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(options.Value.AddressBookBaseUrl);
        }
    }
}
