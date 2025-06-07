using ArtSight.AppService.Setup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args)
.ConfigureService();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    })
        .Services.AddFluentValidationAutoValidation()
        .AddFluentValidationClientsideAdapters()
        .AddValidatorsFromAssemblyContaining<Program>();

// This is needed to make HttpClient work in the processors
builder.Services.AddHttpClient();

// This is needed to make HttpContext work in the processors
builder.Services.AddHttpContextAccessor();

// This is needed to exclude the nulls in the responses
builder.Services.AddMvc()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidIssuer = "yourdomain.com",
            ValidateAudience = false,
            ValidAudience = "yourdomain.com",
            ValidateLifetime = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Keys:JwtSettingsSecretKey"]!)),
            ValidateIssuerSigningKey = false,
            NameClaimType = "sub"
        };
    });

var app = builder.Build();

app.UseExceptionHandler("/error");

app.Map("/error", (HttpContext context) =>
{
    var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
    return Results.Problem(exception?.Message);
});

app.UseCors("AllowAll");

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ArtSight API");
    });
}

app.UseRouting();

// Authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Static files
var mediaFolderPath = builder.Configuration["Paths:MediaPath"];
if (string.IsNullOrEmpty(mediaFolderPath))
{
    throw new InvalidOperationException("Media folder path is not configured.");
}

var absoluteMediaPath = Path.GetFullPath(mediaFolderPath, Directory.GetCurrentDirectory());
if (!Directory.Exists(absoluteMediaPath))
{
    throw new DirectoryNotFoundException($"The Media folder path does not exist: {absoluteMediaPath}");
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(absoluteMediaPath),
    RequestPath = "/artsight/media",
    OnPrepareResponse = ctx =>
    {
        var headers = ctx.Context.Response.GetTypedHeaders();
        headers.CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
        {
            Public = true,
            MaxAge = TimeSpan.FromDays(365)
        };
        headers.Expires = DateTime.UtcNow.AddYears(1);
    }
});

app.Run();
