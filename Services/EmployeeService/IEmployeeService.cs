using Services.Common.Dto;

namespace Services.EmployeeService
{
    public interface IEmployeeService
    {
        Task<GeneralResponseDto> Create(EmployeeCreateDto employeeDto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Delete(int employeeId, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Update(int employeeId, EmployeeUpdateDto employeeDto, CancellationToken cancellationToken = default);
        Task<ObservableCollection<EmployeeDto>> GetAll(CancellationToken cancellationToken = default);
        Task<EmployeeDto> GetById(int employeeId, CancellationToken cancellationToken = default);
    }
}
