using Avalonia.Controls;
using Avalonia.Media;


namespace Treee;

public class AppSettings
{
    public string FieldColor { get; set; } = "#000000";
    public string CellColor { get; set; } = "#00FF00";
    public int MinSpeed { get; set; } = 100;
    public int MaxSpeed { get; set; } = 2000;

    public static AppSettings Default()
    {
        return new AppSettings
        {
            FieldColor = "#000000",
            CellColor = "#00FF00",
            MinSpeed = 100,
            MaxSpeed = 2000
        };

    }
}

public partial class SettingsWindow : Window
{
    public AppSettings Settings { get; private set; }

    public SettingsWindow(AppSettings currentSettings)
    {
        InitializeComponent();
        Settings = currentSettings;

        FieldColorPicker.Color = Color.Parse(Settings.FieldColor);
        CellColorPicker.Color = Color.Parse(Settings.CellColor);
        MinSpeedBox.Value = Settings.MinSpeed;
        MaxSpeedBox.Value = Settings.MaxSpeed;

        OkButton.Click += (_, __) =>
        {
            Settings.FieldColor = FieldColorPicker.Color.ToString() ?? "#000000";
            Settings.CellColor = CellColorPicker.Color.ToString() ?? "#00FF00";
            Settings.MinSpeed = (int)MinSpeedBox.Value;
            Settings.MaxSpeed = (int)MaxSpeedBox.Value;
            Close(Settings);
        };

        CancelButton.Click += (_, __) => Close(null);

        ResetButton.Click += (_, __) =>
        {
            var def = AppSettings.Default();
            FieldColorPicker.Color = Color.Parse(def.FieldColor);
            CellColorPicker.Color = Color.Parse(def.CellColor);
            MinSpeedBox.Value = def.MinSpeed;
            MaxSpeedBox.Value = def.MaxSpeed;
        };
    }
}

