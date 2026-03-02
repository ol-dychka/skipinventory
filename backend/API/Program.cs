using Application.Core;
using Application.Interfaces;
using Application.Organizations.Queries;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<PsqlDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddMediatR(x =>
    x.RegisterServicesFromAssemblyContaining<GetOrganizationList.Handler>());
builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);

builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();

var app = builder.Build();

app.MapControllers();

app.Run();
