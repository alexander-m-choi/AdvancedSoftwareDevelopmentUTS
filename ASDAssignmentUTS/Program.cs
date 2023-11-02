using ASDAssignmentUTS.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Antiforgery;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<UserRepository>(serviceProvider =>
    new UserRepository(DBConnector.GetConnectionString()));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
});




builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

/*builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Name = "X-CSRF-TOKEN";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.HeaderName = "X-CSRF-TOKEN";
});*/




var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();




app.UseRouting();

app.UseCookiePolicy();

app.UseAuthorization();

//app.UseMiddleware<ValidateAntiForgeryTokenMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

/*builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "RequestVerificationToken"; // Custom header name if needed
    options.Cookie.Name = "MyAntiforgeryCookie"; // Optional: Customize the cookie name
    options.FormFieldName = "__RequestVerificationToken"; // Optional: Customize the form field name
    options.SuppressXFrameOptionsHeader = false; // Optional: Set to true if necessary
});

app.Use(next => context =>
{
    // Custom validation logic
    var antiforgery = context.RequestServices.GetRequiredService<IAntiforgery>();
    antiforgery.ValidateRequestAsync(context).Wait();

    return next(context);
});*/

app.Run();

