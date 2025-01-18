using Components.Dialog;
using Components.Entities.Patients;
using Services.Common.Dto;
using System.Collections.ObjectModel;

namespace ViewModels;

public class PatientListViewModel : ComponentBaseViewModel
{
    protected bool Loading;
    protected ObservableCollection<PatientDto> Patients { get; set; } = new ObservableCollection<PatientDto>();
    protected string? SearchPatientName { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Loading = true;
        await LoadPatients();
        Loading = false;
    }

    protected async Task CreateOrUpdatePatient(PatientDto patientDto)
    {
        DialogParameters parameters;
        if (patientDto.PatientId == 0)
        {
            var patientCreate = patientDto.Adapt<PatientCreateDto>();
            parameters = new DialogParameters { ["PatientCreate"] = patientCreate };
        }
        else
        {
            var patientUpdate = patientDto.Adapt<PatientUpdateDto>();
            parameters = new DialogParameters { ["PatientUpdate"] = patientUpdate };
        }

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium };
        var dialogTitle = patientDto.PatientId == 0 ? "Create Patient" : "Update Patient";
        var dialog = await DialogService!.ShowAsync<PatientFormComponent>(dialogTitle, parameters, options);

        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await LoadPatients();
        }
    }

    private async Task LoadPatients()
    {
        try
        {
            var patients = await PatientService!.GetAll();
            Patients.Clear();
            foreach (var patient in patients)
            {
                Patients.Add(patient);
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error loading patients: {ex.Message}");
        }
    }

    protected async Task DeletePatient(PatientDto patient)
    {
        var parameters = new DialogParameters
        {
            { "ContentText", "Are you sure you want to delete this patient?" },
            { "ButtonText", "Delete" },
            { "Color", Color.Success }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
        var dialog = await DialogService!.ShowAsync<ConfirmComponent>("Delete Patient", parameters, options);
        var result = await dialog.Result;

        if (result!.Canceled)
        {
            return;
        }

        var response = await PatientService!.Delete(patient.PatientId);
        HandleResponse(response, patient);
    }

    protected bool FilterFunc(PatientDto element)
    {
        return string.IsNullOrWhiteSpace(SearchPatientName) ||
               element.FirstName!.Contains(SearchPatientName, StringComparison.OrdinalIgnoreCase) ||
               element.LastName!.Contains(SearchPatientName, StringComparison.OrdinalIgnoreCase);
    }

    private void HandleResponse(GeneralResponseDto response, PatientDto patient)
    {
        if (response.IsSuccess)
        {
            Patients.Remove(patient);
            Snackbar!.Add("Success!", Severity.Success);
        }
        else
        {
            Snackbar!.Add("Error", Severity.Error);
        }
    }
}
