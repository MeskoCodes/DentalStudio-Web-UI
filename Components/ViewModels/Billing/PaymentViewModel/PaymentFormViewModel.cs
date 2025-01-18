using Services.Common.Dto;
using Services.Common.Dto.Billing;

namespace ViewModels
{
    public class PaymentFormViewModel : ComponentBaseViewModel
    {
        [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
        protected const string ValidationMessage = "Field is required.";

        [Parameter]
        public PaymentCreateDto? PaymentCreate { get; set; }

        [Parameter]
        public PaymentUpdateDto? PaymentUpdate { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (PaymentUpdate != null)
            {
                StateHasChanged();
            }
        }

        public async Task CreateOrUpdate()
        {
            try
            {
                var response = new GeneralResponseDto();
                if (PaymentCreate != null && PaymentCreate.PaymentId == 0)
                {
                    response = await PaymentService!.Create(PaymentCreate);
                }
                else
                {
                    response = await PaymentService!.Update(PaymentUpdate!.PaymentId, PaymentUpdate);
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
            (PaymentCreate != null && string.IsNullOrWhiteSpace(PaymentCreate.PaymentMethod)) ||
            (PaymentUpdate != null && string.IsNullOrWhiteSpace(PaymentUpdate.PaymentMethod));
    }
}
