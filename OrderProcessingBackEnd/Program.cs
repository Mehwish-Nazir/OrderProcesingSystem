using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Text.Json.Serialization;
using Serilog;
using OrderProcessingBackEnd.Data;
using OrderProcessingBackEnd.Repository;
using OrderProcessingBackEnd.Services;
using AutoMapper;
using OrderProcessingBackEnd.AutoMapper;
using Microsoft.OpenApi.Models;
using OrderProcessingBackEnd.Middleware;

var builder = WebApplication.CreateBuilder(args);

//  Load Configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

//  Setup Database Context
builder.Services.AddDbContext<OrderProcessingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("mydb"))
);
builder.Services.AddLogging();  //add loggin service
//  Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins(builder.Configuration["AllowedOrigins"] ?? "http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

//  Configure JSON Serializer
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

//  Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,  //as my front end project is not setup
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) //this name must match with name used in Uservice file in JWTGenerated function
        };
    });   ///Add jwt section in 'appsettings.json' file 

builder.Services.AddAuthorization();
//Add genearl repo Repositry and use it for any kind of entity
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
//Register user servicee
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IOrderPlaceService, OrderPlaceService>();
//Add auto mapper
builder.Services.AddAutoMapper(typeof(MappingProfile)); //  register the profile class

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//to view authorization button
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Customer API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                        Enter 'Bearer' [space] and then your token in the text input below.
                        Example: Bearer abc123",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        //Type = SecuritySchemeType.Http,  //most professional type
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
                // Apply Security to All Endpoints

            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
           // new string[] {}
           Array.Empty<string>()
        }
    });
});



// ✅ Configure Serilog (Corrected)
builder.Host.UseSerilog((context, config) =>
{
    config.WriteTo.Console();
});


builder.Services.AddLogging();

var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();

//app.UseDeveloperExceptionPage(); // Shows detailed error in browser (works only in Development)

app.UseRouting();
app.UseCors("AllowAngular");

// ✅ Configure Swagger for ALL Environments
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Hello World!");



app.Run();
