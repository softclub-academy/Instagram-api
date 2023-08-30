using System.Text;
using Infrastructure.AutoMapper;
using Infrastructure.Data;
using Infrastructure.Services.AccountService;
using Infrastructure.Services.CategoryService;
using Infrastructure.Services.ExternalAccountService;
using Infrastructure.Services.FileService;
using Infrastructure.Services.FollowingRelationShipService;
using Infrastructure.Services.LocationDto;
using Infrastructure.Services.LocationService;
using Infrastructure.Services.PostCategoryService;
using Infrastructure.Services.PostCommentService;
using Infrastructure.Services.PostFavoriteService;
using Infrastructure.Services.PostService;
using Infrastructure.Services.PostStatService;
using Infrastructure.Services.PostTagService;
using Infrastructure.Services.TagService;
using Infrastructure.Services.UserProfileService;
using Infrastructure.Services.UserService;
using Infrastructure.Services.UserSettingService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// connection to database
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(conf => conf.UseNpgsql(connection));


// dependency injection
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IExternalAccountService, ExternalAccountService>();
builder.Services.AddScoped<IFollowingRelationShipService, FollowingRelationShipService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IPostCategoryService, PostCategoryService>();
builder.Services.AddScoped<IPostCommentService, PostCommentService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IPostFavoriteService, PostFavoriteService>();
builder.Services.AddScoped<IPostStatService, PostStatService>();
builder.Services.AddScoped<IPostTagService, PostTagService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IUserSettingService, UserSettingService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IAccountService, AccountService>();

// automapper
builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;            
}).AddJwtBearer(o =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sample web API",
        Version = "v1",
        Description = "Sample API Services.",
        Contact = new OpenApiContact
        {
            Name = "John Doe"
        },
    });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
            
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
