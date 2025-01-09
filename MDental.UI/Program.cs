global using Blazored.LocalStorage;
global using Microsoft.AspNetCore.Components.Authorization;
global using Microsoft.AspNetCore.Components.Web;
global using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
global using MudBlazor;
global using MudBlazor.Services;
using MDental.UI;
using ViewModels;
using Services.Billing.PaymentService;
using Services.PatientService;
using Services.AuthenticationService.AuthenticationService.AuthProviders;
using Services.AppointmentService;
using Services.Billing.InvoiceService;
using Services.TreatmentService;
using Services.EmployeeService;
using Services.Common;
using AuthProviders;
using Services;
using Services.AuthenticationService.Dto;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Dodavanje HttpClient sa baznom adresom API-ja
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5275/") }); // Ovde koristite URL vašeg API-ja

// Konfiguracija MudBlazor servisa
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

// Registracija servisa i token autentifikacije
builder.Services.AddScoped<TokenStorage>();
builder.Services.AddScoped<TokenAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<TokenAuthenticationStateProvider>());
builder.Services.AddScoped<IApiService, ApiService>();

// Registracija Blazored Local Storage i autorizacije
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

// Registracija aplikacijskih servisa
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ITreatmentService, TreatmentService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAuthenticationViewModel, AuthenticationViewModel>();

await builder.Build().RunAsync();
