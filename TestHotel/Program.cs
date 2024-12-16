using Autofac;
using Autofac.Extensions.DependencyInjection;
using BhoomiGlobalAPI;
using BhoomiGlobalAPI.Common;
using BhoomiGlobalAPI.Entities;
using BhoomiGlobalAPI.Service;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Reflection;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.Host.ConfigureContainer<ContainerBuilder>(ConfigureContainer);


        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

        //builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        //builder.Services.AddScoped<IDbFactory, DbFactory>();
        builder.Services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Add services to the container.

        static void ConfigureContainer(ContainerBuilder builder)
        {
            var mapper = AutoMapping.Automapperinitializer();
            builder.RegisterInstance(mapper);


            // Register services
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerLifetimeScope();

            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();


            builder.RegisterAssemblyTypes(Assembly.Load("BhoomiGlobalAPI")).
                Where(x => x.Name.EndsWith("Service", StringComparison.OrdinalIgnoreCase)).AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.Load("BhoomiGlobalAPI")).
               Where(x => x.Name.EndsWith("Repository", StringComparison.OrdinalIgnoreCase)).AsImplementedInterfaces();

        }

        builder.Services.AddIdentity<User, IdentityRole>(opt =>
        {
            opt.Password.RequiredLength = 8;
            opt.Password.RequireUppercase = true;
            opt.Password.RequireDigit = false;
        })
            .AddEntityFrameworkStores<RepositoryContext>()
            .AddDefaultTokenProviders(); ;

        builder.Services.AddHttpClient<OpenAIService>(client =>
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", builder.Configuration.GetConnectionString("secret"));
        });


        builder.Services.Configure<FormOptions>(o =>
        {
            o.ValueLengthLimit = int.MaxValue;
            o.MultipartBodyLengthLimit = int.MaxValue;
            o.MemoryBufferThreshold = int.MaxValue;
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors(builder =>
        {
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
        app.UseStaticFiles();
        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
            RequestPath = new PathString("/Resources")
        });

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}