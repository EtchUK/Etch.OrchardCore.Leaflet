using OrchardCore.Modules.Manifest;

[assembly: Module(
    Author = "Etch UK Ltd.",
    Category = "Content",
    Dependencies = new string[] { "OrchardCore.BackgroundTasks", "OrchardCore.Media", "OrchardCore.Title" },
    Description = "Create interactive maps utilising Leaflet.",
    Name = "Leaflet Maps",
    Version = "1.6.1",
    Website = "https://etchuk.com"
)]