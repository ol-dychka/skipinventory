using System.Text;
using API.Filters;
using API.Hubs;
using Application.Interfaces;
using Application.Organizations.Queries;
using Infrastructure.Auth;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<List.Handler>());
builder.Services.AddSignalR();

// postgresql
builder.Services.AddDbContext<PsqlDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IJoinRequestRepository, JoinRequestRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISaleRecordRepository, SaleRecordRepository>();
builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//redis
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect("localhost:6379")
);
builder.Services.AddScoped<IRedisTokenService, RedisTokenService>();
builder.Services.AddScoped<RedisTokenValidationFilter>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<RedisTokenValidationFilter>();
});

// token + hashing
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// auth
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;

        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings!.Issuer,
            ValidAudience = jwtSettings.Audience,

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
        };
    });
builder.Services.AddAuthorization();

// interlayer communication (frontend, ml service)
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAngular",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    );
});
builder.Services.AddHttpClient(
    "mlservice",
    client =>
    {
        client.BaseAddress = new Uri("http://localhost:8000");
    }
);

var app = builder.Build();

app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// signal r
app.MapHub<ChatHub>("/hubs/chat");
app.MapHub<NotificationsHub>("/hubs/notifications");

app.Run();
