using Contract.Account;

namespace Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IApiService _apiService;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public EmployeeService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<GeneralResponseDto> Create(EmployeeCreateDto employeeDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Post($"{ApiEndpoints.EmployeeController}/create", employeeDto);
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<GeneralResponseDto>(responseStream, _options, cancellationToken);
                return res ?? null!;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<GeneralResponseDto> Delete(int employeeId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Delete($"{ApiEndpoints.EmployeeController}/{employeeId}");
                return response.IsSuccessStatusCode
                    ? new GeneralResponseDto { IsSuccess = true }
                    : new GeneralResponseDto { IsSuccess = false };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return new GeneralResponseDto { IsSuccess = false };
            }
        }

        public async Task<GeneralResponseDto> Update(int employeeId, EmployeeUpdateDto employeeDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Put($"{ApiEndpoints.EmployeeController}/update/{employeeId}", employeeDto);
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<GeneralResponseDto>(responseStream, _options, cancellationToken);
                return res ?? null!;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<ObservableCollection<EmployeeDto>> GetAll(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Get($"{ApiEndpoints.EmployeeController}");
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<ObservableCollection<EmployeeDto>>(responseStream, _options, cancellationToken);
                return res ?? null!;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<EmployeeDto> GetById(int employeeId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _apiService.Get($"{ApiEndpoints.EmployeeController}/details/{employeeId}");
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<EmployeeDto>(responseStream, _options, cancellationToken);
                return res ?? null!;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }
    }
}
