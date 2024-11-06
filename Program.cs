using Syncfusion.Blazor;
using SyncfusionSemanticKernelBlazor.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents().AddHubOptions(
        o =>
        {
            o.MaximumReceiveMessageSize = 102400000;
        });

builder.Services.AddMemoryCache();
//HACK:Add Syncfusion Blazor service to the container.
builder.Services.AddSyncfusionBlazor();
//HACK: Register SyncFusion 27.x.x
Syncfusion.Licensing.SyncfusionLicenseProvider
    .RegisterLicense("");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
