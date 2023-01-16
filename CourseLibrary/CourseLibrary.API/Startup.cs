using AutoMapper;
using CourseLibrary.API.DbContexts;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CourseLibrary.API
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(setupAction =>
            {
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));

                setupAction.OutputFormatters.Add(new XmlSerializerOutputFormatter());

                var jsonOutputFormatter=setupAction.OutputFormatters.OfType<NewtonsoftJsonOutputFormatter>().FirstOrDefault();
                if(jsonOutputFormatter != null)
                {
                    if (jsonOutputFormatter.SupportedMediaTypes.Contains("text/jsom"))
                    {
                        jsonOutputFormatter.SupportedMediaTypes.Remove("text/json");
                    }
                }
            });
            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                

            }).AddNewtonsoftJson(setupAction =>
             {
                 setupAction.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
             })
            .AddXmlDataContractSerializerFormatters()
            .ConfigureApiBehaviorOptions(setupAction =>
            {
                setupAction.InvalidModelStateResponseFactory = context =>
                {
                    // create a problem details object
                    var problemDetailsFactory = context.HttpContext.RequestServices
                        .GetRequiredService<ProblemDetailsFactory>();
                    var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                            context.HttpContext,
                            context.ModelState);

                    // add additional info not added by default
                    problemDetails.Detail = "See the errors field for details.";
                    problemDetails.Instance = context.HttpContext.Request.Path;

                    // find out which status code to use
                    var actionExecutingContext =
                          context as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

                    // if there are modelstate errors & all keys were correctly
                    // found/parsed we're dealing with validation errors
                    if ((context.ModelState.ErrorCount > 0) &&
                        (actionExecutingContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count))
                    {
                        problemDetails.Type = "https://courselibrary.com/modelvalidationproblem";
                        problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                        problemDetails.Title = "One or more validation errors occurred.";

                        return new UnprocessableEntityObjectResult(problemDetails)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    }

                    // if one of the keys wasn't correctly found / couldn't be parsed
                    // we're dealing with null/unparsable input
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "One or more errors on input occurred.";
                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ICourseLibraryRepository, CourseLibraryRepository>();

            services.AddDbContext<CourseLibraryContext>(options =>
            {
                options.UseSqlServer(
                    @"Server=(localdb)\mssqllocaldb;Database=CourseLibraryDB;Trusted_Connection=True;");
            });
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("CourseLibraryOpenApiSpecification",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "CourseLibrary API",
                        Version = "v1",
                        Description = "Through this API you can access authors and their courses.",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact() 
                        { 
                            Name="Omkar Kadam",
                            Email="omkar.kadam@klingelnberg.com",
                            Url=new Uri("https://www.twitter.com/OmkarKadam"),
                            
                        },
                        License=new Microsoft.OpenApi.Models.OpenApiLicense()
                        {
                            Name="MIT License",
                            Url=new Uri("https://opensource.org/licenses/MIT")
                        },
                     
                    });
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                setupAction.IncludeXmlComments(xmlCommentsFullPath);
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });

            }
            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/CourseLibraryOpenApiSpecification/swagger.json", "CourseLibrary Api");
               
            
            });
            

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
