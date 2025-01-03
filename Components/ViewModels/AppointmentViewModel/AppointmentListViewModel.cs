using Components.Dialog;
using Components.Entities.Appointments;
using System.Collections.ObjectModel;

namespace ViewModels;

public class AppointmentListViewModel : ComponentBaseViewModel
{
    protected bool Loading;
    protected ObservableCollection<AppointmentDto> Appointments { get; set; } = [];
    protected string? SearchPatientName { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadAppointments();
        Loading = false;
    }

    protected async Task CreateOrUpdateAppointment(AppointmentDto appointmentDto)
    {
        DialogParameters parameters = [];
        if (appointmentDto.Id == 0)
        {
            var appointmentCreate = appointmentDto.Adapt<AppointmentCreateDto>();
            parameters = new DialogParameters { ["AppointmentCreate"] = appointmentCreate };
        }
        else
        {
            var appointmentUpdate = appointmentDto.Adapt<AppointmentUpdateDto>();
            parameters = new DialogParameters { ["AppointmentUpdate"] = appointmentUpdate };
        }

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium };
        var dialogTitle = appointmentDto.Id == 0 ? "Create Appointment" : "Update Appointment";
        var dialog = await DialogService!.ShowAsync<AppointmentFormComponent>(dialogTitle, parameters, options);

        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            StateHasChanged();
        }
    }

    private async Task LoadAppointments()
    {
        try
        {
            Appointments = await AppointmentService!.GetAll();
            StateHasChanged();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    protected async Task DeleteAppointment(AppointmentDto appointment)
    {
        var parameters = new DialogParameters();
        const string text = "Are you sure you want to delete this appointment?";

        parameters.Add("ContentText", text);
        parameters.Add("ButtonText", "Delete");
        parameters.Add("Color", Color.Success);

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
        var dialog = await DialogService!.ShowAsync<ConfirmComponent>("Delete Appointment", parameters, options);
        var result = await dialog.Result;

        if (result!.Canceled)
        {
            return;
        }

        var response = await AppointmentService!.Delete(appointment.Id);
        HandleResponse(response, appointment);
    }

    protected bool FilterFunc(AppointmentDto element)
    {
        return string.IsNullOrWhiteSpace(SearchPatientName) ||
               element.PatientName!.Contains(SearchPatientName, StringComparison.OrdinalIgnoreCase);
    }

    private void HandleResponse(GeneralResponseDto response, AppointmentDto appointment)
    {
        if (response.IsSuccess)
        {
            Appointments.Remove(appointment);
            StateHasChanged();
            Snackbar!.Add("Success!", Severity.Success);
        }
        else
        {
            Snackbar!.Add("Error", Severity.Error);
        }
    }
}
