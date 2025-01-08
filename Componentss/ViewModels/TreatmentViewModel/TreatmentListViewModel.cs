using Components.Dialog;
using Components.Entities.Treatments;
using System.Collections.ObjectModel;

namespace ViewModels;

public class TreatmentListViewModel : ComponentBaseViewModel
{
    [Inject]
    private ITreatmentService TreatmentService { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }

    protected bool Loading;
    protected ObservableCollection<TreatmentDto> Treatments { get; set; } = new ObservableCollection<TreatmentDto>();
    protected string? SearchTreatmentName { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadTreatments();
        Loading = false;
    }

    protected async Task CreateOrUpdateTreatment(TreatmentDto treatmentDto)
    {
        DialogParameters parameters = new DialogParameters();
        if (treatmentDto.Id == 0)
        {
            var treatmentCreate = treatmentDto.Adapt<TreatmentCreateDto>();
            parameters = new DialogParameters { ["TreatmentCreate"] = treatmentCreate };
        }
        else
        {
            var treatmentUpdate = treatmentDto.Adapt<TreatmentUpdateDto>();
            parameters = new DialogParameters { ["TreatmentUpdate"] = treatmentUpdate };
        }

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium };
        var dialogTitle = treatmentDto.Id == 0 ? "Create Treatment" : "Update Treatment";
        var dialog = await DialogService!.ShowAsync<TreatmentFormComponent>(dialogTitle, parameters, options);

        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            StateHasChanged();
        }
    }

    private async Task LoadTreatments()
    {
        try
        {
            Treatments = await TreatmentService!.GetAll();
            StateHasChanged();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    protected async Task DeleteTreatment(TreatmentDto treatment)
    {
        var parameters = new DialogParameters();
        const string text = "Are you sure you want to delete this treatment?";

        parameters.Add("ContentText", text);
        parameters.Add("ButtonText", "Delete");
        parameters.Add("Color", Color.Success);

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
        var dialog = await DialogService!.ShowAsync<ConfirmComponent>("Delete Treatment", parameters, options);
        var result = await dialog.Result;

        if (result!.Canceled)
        {
            return;
        }

        var response = await TreatmentService!.Delete(treatment.Id);
        if (response.IsSuccess)
        {
            Treatments.Remove(treatment);
        }
        else
        {
            Console.WriteLine("Error deleting treatment");
        }
    }

    protected bool FilterFunc(TreatmentDto element)
    {
        return string.IsNullOrWhiteSpace(SearchTreatmentName) || element.Name.Contains(SearchTreatmentName, StringComparison.OrdinalIgnoreCase);
    }
}
