using Components.Dialog;
using System.Collections.ObjectModel;
using Services.Common.Dto.Billing;
using Components.Entities.Billing.Invoices;
using Services.Common.Dto;
namespace ViewModels
{
    public class InvoiceListViewModel : ComponentBaseViewModel
    {
        protected bool Loading;
        protected ObservableCollection<InvoiceDto> Invoices { get; set; } = new ObservableCollection<InvoiceDto>(); // Ispravljeno
        protected string? SearchInvoiceNumber { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadInvoices();
            Loading = false;
        }

        protected async Task CreateOrUpdateInvoice(InvoiceDto invoiceDto)
        {
            DialogParameters parameters = new DialogParameters(); // Ispravljeno
            if (invoiceDto.InvoiceId == 0)
            {
                var invoiceCreate = invoiceDto.Adapt<InvoiceCreateDto>();
                parameters = new DialogParameters { ["InvoiceCreate"] = invoiceCreate };
            }
            else
            {
                var invoiceUpdate = invoiceDto.Adapt<InvoiceUpdateDto>();
                parameters = new DialogParameters { ["InvoiceUpdate"] = invoiceUpdate };
            }

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium };
            var dialogTitle = invoiceDto.InvoiceId == 0 ? "Create Invoice" : "Update Invoice";
            var dialog = await DialogService!.ShowAsync<InvoiceFormComponent>(dialogTitle, parameters, options);

            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                StateHasChanged();
            }
        }

        private async Task LoadInvoices()
        {
            try
            {
                Invoices = await InvoiceService!.GetAll();
                StateHasChanged();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected async Task DeleteInvoice(InvoiceDto invoice)
        {
            var parameters = new DialogParameters(); // Ispravljeno
            const string text = "Are you sure you want to delete this invoice?";

            parameters.Add("ContentText", text);
            parameters.Add("ButtonText", "Delete");
            parameters.Add("Color", Color.Success);

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
            var dialog = await DialogService!.ShowAsync<ConfirmComponent>("Delete Invoice", parameters, options);

            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                await LoadInvoices(); // Osvežavanje liste nakon brisanja
                StateHasChanged();
            }
        }
        protected bool FilterFunc(InvoiceDto element)
        {
            return  element.InvoiceNumber == (SearchInvoiceNumber);
        }
    }

}
