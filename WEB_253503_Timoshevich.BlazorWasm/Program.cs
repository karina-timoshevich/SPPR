using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WEB_253503_Timoshevich.BlazorWasm;
using WEB_253503_Timoshevich.BlazorWasm.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["UriData:ApiUri"]) });
builder.Services.AddScoped<IDataService, DataService>();

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Keycloak", options.ProviderOptions);
});

await builder.Build().RunAsync();
