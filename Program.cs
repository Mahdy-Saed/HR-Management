using HR_Carrer.Authentication;
using HR_Carrer.Authntication;
using HR_Carrer.Data;
using HR_Carrer.Data.Repositery;
using HR_Carrer.Services.AttachmentService;
using HR_Carrer.Services.AuthService;
using HR_Carrer.Services.EmployeeService;
using HR_Carrer.Services.FileService;
using HR_Carrer.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ------------------- Logging -------------------
// Enable Serilog from appsettings.json
builder.Host.UseSerilog((context, configurations) =>
    configurations.ReadFrom.Configuration(context.Configuration));

// ------------------- Services -------------------
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("jwtsettings"));

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IAttachmentService, AttachmentService>();
builder.Services.AddScoped<ITokenGenerater, TokenGenerater>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IEmployeeRepo, EmployeeRepo>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddCors();
builder.Services.AddAutoMapper(typeof(Program));

// ------------------- Data Connection -------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PSQL_Connection")));

// ------------------- JWT Setting -------------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("jwtsettings");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)),
            ClockSkew = TimeSpan.Zero
        };
        options.IncludeErrorDetails = true;
    });

// ------------------- Swagger -------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API v1", Version = "v1", Description = "An API for beautiful HR system" });

    // Bearer Token Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        In = ParameterLocation.Header
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }},
            Array.Empty<string>()
        }
    });


    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

});

var app = builder.Build();

// ------------------- Middlewares -------------------
app.Use(async (context, next) =>
{
    Console.WriteLine($" Request: {context.Request.Method} {context.Request.Path}");
    await next();
    Console.WriteLine($"⬅ Response: {context.Response.StatusCode}");
});

// Enable CORS
app.UseRouting();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Enable Serilog HTTP request logging
app.UseSerilogRequestLogging();

// ------------------- Configure HTTP pipeline -------------------
app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.InjectStylesheet("/Custom.css"); c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");   });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
