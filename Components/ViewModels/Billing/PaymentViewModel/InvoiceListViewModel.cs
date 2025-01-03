using Components.Dialog;
using Components.Products;
using System.Collections.ObjectModel;

namespace ViewModels;
public class InvoiceListViewModel : ComponentBaseViewModel
{
    protected bool Loading;

    protected ObservableCollection<ProductDto> Products { get; set; } = [];

    protected string? SearchProductName { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadProducts();
        Loading = false;
    }

    protected async Task CreateOrUpdateProduct(ProductDto productDto)
    {
        DialogParameters parameters = [];
        if (productDto.Id == 0)
        {
            var productCreate = productDto.Adapt<ProductCreateDto>();
            parameters = new DialogParameters { ["ProductCreate"] = productCreate };
        }
        else
        {
            var productUpdate = productDto.Adapt<ProductUpdateDto>();
            parameters = new DialogParameters { ["ProductUpdate"] = productUpdate };
        }

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Medium
        };

        var dialogTitle = productDto.Id == 0 ? "Create product" : "Update Product";
        var dialog = await DialogService!.ShowAsync<ProductFormComponent>(dialogTitle, parameters, options);

        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            StateHasChanged();
        }
    }

    private async Task LoadProducts()
    {
        try
        {
            Products = await ProductService!.GetAll();
            StateHasChanged();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    protected async Task DeleteProduct(ProductDto product)
    {
        var parameters = new DialogParameters();
        const string text = "Are you sure you want to delete this product?";

        parameters.Add("ContentText", text);
        parameters.Add("ButtonText", "Delete");
        parameters.Add("Color", Color.Success);

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
        var dialog = await DialogService!.ShowAsync<ConfirmComponent>("Delete product", parameters,
                options);
        var result = await dialog.Result;

        if (result!.Canceled)
        {
            return;
        }

        var response = await ProductService!.Delete(product.Id);
        HandleResponse(response, product);
    }

    protected bool FilterFunc(ProductDto element)
    {
        return string.IsNullOrWhiteSpace(SearchProductName) ||
               element.Name!.Contains(SearchProductName, StringComparison.OrdinalIgnoreCase);
    }

    private void HandleResponse(GeneralResponseDto response, ProductDto product)
    {
        if (response.IsSuccess)
        {
            Products.Remove(product);
            StateHasChanged();
            Snackbar!.Add("Success!", Severity.Success);
        }
        else
        {
            Snackbar!.Add("Error", Severity.Error);
        }
    }
}
