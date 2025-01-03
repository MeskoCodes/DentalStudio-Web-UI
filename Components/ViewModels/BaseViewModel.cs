namespace ViewModels
{
    public abstract class BaseViewModel : ComponentBase
    {
        [Inject] public IDialogService? DialogService { get; set; }

        public static MudTheme DefaultTheme = new()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = new MudBlazor.Utilities.MudColor("#1976D2"), 
                Secondary = new MudBlazor.Utilities.MudColor("#42A5F5"),
                Tertiary = new MudBlazor.Utilities.MudColor("#90CAF9"),
                AppbarBackground = new MudBlazor.Utilities.MudColor("#1565C0"), 
                AppbarText = Colors.Shades.White, 
                Background = Colors.Shades.White, 
                DrawerBackground = Colors.BlueGray.Lighten5, 
                DrawerText = Colors.BlueGray.Darken3,
                Success = Colors.LightGreen.Default, 
                Warning = Colors.Amber.Default, 
                Error = Colors.Red.Darken2,
                Info = Colors.LightBlue.Default 
            },
            Typography = new Typography()
            {
                Default = new Default()
                {
                    FontSize = "14px",
                    FontFamily = ["Roboto", "Helvetica", "Arial", "sans-serif"]
                },
                H6 = new H6()
                {
                    FontFamily = ["Roboto", "Helvetica", "Arial", "sans-serif"],
                    FontSize = "1.2rem",
                    FontWeight = 500,
                    LineHeight = 1.3,
                    LetterSpacing = ".0075em"
                }
            },
            LayoutProperties = new LayoutProperties()
            {
                DrawerWidthLeft = "300px",
                DrawerWidthRight = "250px", 
                DefaultBorderRadius = "12px",
            }
        };
    }
}
