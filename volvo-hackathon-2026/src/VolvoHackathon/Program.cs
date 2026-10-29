using VolvoHackathon.Components;
using VolvoHackathon.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// App services
builder.Services.AddSingleton<AgentChatService>();  // factory monitoring chat, shared by all users
builder.Services.AddSingleton<AgentStatusService>(); // agents' running state from the factory API
builder.Services.AddHttpClient();
builder.Services.AddHostedService<FactoryPollingService>(); // polls the factory API into the chat

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    // Only outside Development: locally the app runs on plain http, so there is no https port to redirect to
    app.UseHttpsRedirection();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
