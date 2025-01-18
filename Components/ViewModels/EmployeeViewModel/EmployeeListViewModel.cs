using Components.Dialog;
using Components.Entities.Employees;
using Services.Common.Dto;
using System.Collections.ObjectModel;

namespace ViewModels;

public class EmployeeListViewModel : ComponentBaseViewModel
{
    protected bool Loading;
    protected ObservableCollection<EmployeeDto> Employees { get; set; } = new ObservableCollection<EmployeeDto>();
    protected string? SearchEmployeeName { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Loading = true;
        await LoadEmployees();
        Loading = false;
    }

    protected async Task CreateOrUpdateEmployee(EmployeeDto employeeDto)
    {
        DialogParameters parameters;
        if (employeeDto.EmployeeId == 0)
        {
            var employeeCreate = employeeDto.Adapt<EmployeeCreateDto>();
            parameters = new DialogParameters { ["EmployeeCreate"] = employeeCreate };
        }
        else
        {
            var employeeUpdate = employeeDto.Adapt<EmployeeUpdateDto>();
            parameters = new DialogParameters { ["EmployeeUpdate"] = employeeUpdate };
        }

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium };
        var dialogTitle = employeeDto.EmployeeId == 0 ? "Create Employee" : "Update Employee";
        var dialog = await DialogService!.ShowAsync<EmployeeFormComponent>(dialogTitle, parameters, options);

        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await LoadEmployees();
        }
    }

    private async Task LoadEmployees()
    {
        try
        {
            var employees = await EmployeeService!.GetAll();
            Employees.Clear();
            foreach (var employee in employees)
            {
                Employees.Add(employee);
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error loading employees: {ex.Message}");
        }
    }

    protected async Task DeleteEmployee(EmployeeDto employee)
    {
        var parameters = new DialogParameters
        {
            { "ContentText", "Are you sure you want to delete this employee?" },
            { "ButtonText", "Delete" },
            { "Color", Color.Success }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
        var dialog = await DialogService!.ShowAsync<ConfirmComponent>("Delete Employee", parameters, options);
        var result = await dialog.Result;

        if (result!.Canceled)
        {
            return;
        }

        var response = await EmployeeService!.Delete(employee.EmployeeId);
        HandleResponse(response, employee);
    }

    protected bool FilterFunc(EmployeeDto element)
    {
        return string.IsNullOrWhiteSpace(SearchEmployeeName) ||
               element.FirstName!.Contains(SearchEmployeeName, StringComparison.OrdinalIgnoreCase) ||
               element.LastName!.Contains(SearchEmployeeName, StringComparison.OrdinalIgnoreCase);
    }

    private void HandleResponse(GeneralResponseDto response, EmployeeDto employee)
    {
        if (response.IsSuccess)
        {
            Employees.Remove(employee);
            Snackbar!.Add("Success!", Severity.Success);
        }
        else
        {
            Snackbar!.Add("Error", Severity.Error);
        }
    }
}
