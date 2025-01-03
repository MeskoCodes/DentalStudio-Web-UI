using Components.Dialog;
using Components.Entities.Employees;
using System.Collections.ObjectModel;

namespace ViewModels;

public class EmployeeListViewModel : ComponentBaseViewModel
{
    protected bool Loading;
    protected ObservableCollection<EmployeeDto> Employees { get; set; } = [];
    protected string? SearchEmployeeName { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadEmployees();
        Loading = false;
    }

    protected async Task CreateOrUpdateEmployee(EmployeeDto employeeDto)
    {
        DialogParameters parameters = [];
        if (employeeDto.Id == 0)
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
        var dialogTitle = employeeDto.Id == 0 ? "Create Employee" : "Update Employee";
        var dialog = await DialogService!.ShowAsync<EmployeeFormComponent>(dialogTitle, parameters, options);

        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            StateHasChanged();
        }
    }

    private async Task LoadEmployees()
    {
        try
        {
            Employees = await EmployeeService!.GetAll();
            StateHasChanged();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    protected async Task DeleteEmployee(EmployeeDto employee)
    {
        var parameters = new DialogParameters();
        const string text = "Are you sure you want to delete this employee?";

        parameters.Add("ContentText", text);
        parameters.Add("ButtonText", "Delete");
        parameters.Add("Color", Color.Success);

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
        var dialog = await DialogService!.ShowAsync<ConfirmComponent>("Delete Employee", parameters, options);
        var result = await dialog.Result;

        if (result!.Canceled)
        {
            return;
        }

        var response = await EmployeeService!.Delete(employee.Id);
        HandleResponse(response, employee);
    }

    protected bool FilterFunc(EmployeeDto element)
    {
        return string.IsNullOrWhiteSpace(SearchEmployeeName) ||
               element.Name!.Contains(SearchEmployeeName, StringComparison.OrdinalIgnoreCase);
    }

    private void HandleResponse(GeneralResponseDto response, EmployeeDto employee)
    {
        if (response.IsSuccess)
        {
            Employees.Remove(employee);
            StateHasChanged();
            Snackbar!.Add("Success!", Severity.Success);
        }
        else
        {
            Snackbar!.Add("Error", Severity.Error);
        }
    }
}
