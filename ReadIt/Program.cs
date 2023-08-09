using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReadIt.BLL.Managers.AuthManager;
using ReadIt.BLL.Managers.BookManager;
using ReadIt.BLL.Managers.ReadSessionManager;
using ReadIt.BLL.Managers.UserBookManager;
using ReadIt.BLL.Managers.UserManager;
using ReadIt.DAL.Entities;
using ReadIt.DAL.Persistance;
using ReadIt.DAL.Persistance.Services;
using ReadIt.DAL.Persistance.Settings;
using ReadIt.WebApi.TokenProvider;
using SendGrid.Extensions.DependencyInjection;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddFluentValidation(c =>
    c.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "ReadIt", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddSendGrid(options =>
{
    options.ApiKey = builder.Configuration.GetSection("SendGridSettings")
    .GetValue<string>("ApiKey");
});

builder.Services.AddDbContext<ReadItDbContext>(opts =>
                opts.UseSqlServer(builder.Configuration["ConnectionStrings:ReadItDatabaseConnection"]));


builder.Services.AddScoped<BookManager, BookManager>();
builder.Services.AddScoped<ReadSessionManager, ReadSessionManager>();
builder.Services.AddScoped<UserManager, UserManager>();
builder.Services.AddScoped<UserBookManager, UserBookManager>();
builder.Services.AddScoped<AuthManager>();
builder.Services.AddScoped<IdentityManager>();
builder.Services.AddScoped<EmailSenderService>();

builder.Services.AddIdentity<User, IdentityRole<Guid>>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.SignIn.RequireConfirmedEmail = true;
    opt.Tokens.EmailConfirmationTokenProvider = "EmailConfirmation";
}).
    AddEntityFrameworkStores<ReadItDbContext>().
    AddTokenProvider("ReadIt", typeof(DataProtectorTokenProvider<User>)).
    AddDefaultTokenProviders().
    AddTokenProvider<EmailConfirmationTokenProvider<User>>("EmailConfirmation");

builder.Services.Configure<DefaultAdminSettings>(builder.Configuration.GetSection(nameof(DefaultAdminSettings)));
builder.Services.Configure<AccessTokenSettings>(builder.Configuration.GetSection(nameof(AccessTokenSettings)));
builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection(nameof(SendGridSettings)));

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("RequireAdmin", policy =>
    policy.RequireRole("Admin"));
    option.AddPolicy("RequireUser", policy =>
    policy.RequireRole("User"));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,

        ValidAudience = builder.Configuration["AccessTokenSettings:Audience"],
        ValidIssuer = builder.Configuration["AccessTokenSettings:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AccessTokenSettings:SigningKey"]))
    };
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
