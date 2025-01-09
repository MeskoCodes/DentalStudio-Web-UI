global using Blazored.LocalStorage;
global using Mapster;
global using Microsoft.AspNetCore.Components;
global using MudBlazor;
using Services.TreatmentService;
using Services.Billing.InvoiceService;
using Services.PatientService;
using Services.Billing.PaymentService;
using Services.EmployeeService;
using Services.AppointmentService;
using Services;
using AuthProviders;

namespace ViewModels
{
    public class ComponentBaseViewModel : ComponentBase
    {
        [Inject] public IPatientService? PatientService { get; set; }
        [Inject] public IAccountService? AccountService { get; set; }

        [Inject] public IEmployeeService? EmployeeService { get; set; }

        [Inject] public ITreatmentService? TreatmentService { get; set; }

        [Inject] public IInvoiceService? InvoiceService { get; set; }

        [Inject] public IPaymentService? PaymentService { get; set; }

        [Inject] public IAppointmentService? AppointmentService { get; set; }

        [Inject] public IAuthenticationService? AuthenticationService { get; set; }

        [Inject] public NavigationManager? NavigationManager { get; set; }

        [Inject] public TokenAuthenticationStateProvider? StateProvider { get; set; }

        [Inject] public IDialogService? DialogService { get; set; }

        [Inject] public ISnackbar? Snackbar { get; set; }

        [Inject] public IAuthenticationViewModel? AuthenticationViewModel { get; set; }

        [Inject] public ILocalStorageService? LocalStorage { get; set; }

        protected const string InfoFormat = "{first_item}-{last_item} of {all_items}";
    }
}
