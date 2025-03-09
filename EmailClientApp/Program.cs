using Application.UseCases;
using EmailClientApp.Components;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using Shared.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<EmailConfiguration>>().Value);

builder.Services.AddScoped<ISmtpEmailService, SmtpEmailService>();
builder.Services.AddScoped<IImapEmailService, ImapEmailService>();

builder.Services.AddScoped<FetchEmailsUseCase>();
builder.Services.AddScoped<SendEmailUseCase>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
