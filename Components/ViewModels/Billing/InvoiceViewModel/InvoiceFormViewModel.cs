using Services.Common.Dto;
using Services.Common.Dto.Billing;

namespace ViewModels
{
    public class InvoiceFormViewModel : ComponentBaseViewModel
    {
        [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
        protected const string ValidationMessage = "Field is required.";

        [Parameter]
        public InvoiceCreateDto? InvoiceCreate { get; set; }

        [Parameter]
        public InvoiceUpdateDto? InvoiceUpdate { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (InvoiceUpdate != null)
            {
                StateHasChanged();
            }
        }

        public async Task CreateOrUpdate()
        {
            try
            {
                var response = new GeneralResponseDto();
                if (InvoiceCreate != null && InvoiceCreate.InvoiceId == 0)
                {
                    response = await InvoiceService!.Create(InvoiceCreate);
                }
                else
                {
                    response = await InvoiceService!.Update(InvoiceUpdate!.InvoiceId, InvoiceUpdate);
                }

                HandleResponse(response);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                MudDialog!.Close(DialogResult.Ok(true));
            }
        }

        private void HandleResponse(GeneralResponseDto response)
        {
            var isSuccess = response?.IsSuccess == true;
            Snackbar!.Add(isSuccess ? "Success!" : "Error!", isSuccess ? Severity.Success : Severity.Error);
            MudDialog!.Close(DialogResult.Ok(true));
        }

        public void Cancel() => MudDialog!.Cancel();

        public bool Disabled =>
            (InvoiceCreate != null && string.IsNullOrWhiteSpace(InvoiceCreate.InvoiceNumber)) ||
            (InvoiceUpdate != null && string.IsNullOrWhiteSpace(InvoiceUpdate.InvoiceNumber));
    }
}

