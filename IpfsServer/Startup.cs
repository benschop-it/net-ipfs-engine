using System;
using System.IO;
using System.Text.Json;
using Ipfs.CoreApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Ipfs.Server {
    /// <summary>
    ///   Startup steps.
    /// </summary>
    class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ICoreApi>(Program.IpfsEngine);
            services.AddCors();
            services.AddMvc()
                .AddJsonOptions(jo =>
                {
                    jo.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v0",
                    new OpenApiInfo
                    {
                        Title = "IPFS HTTP API",
                        Description = "The API for interacting with IPFS nodes.",
                        Version = "v0",
                        Contact = new OpenApiContact
                        {
                            Email = "af@o2.services",
                            Name = "o2.services",
                            Url = new Uri("https://o2.services")
                        },
                        License = new OpenApiLicense
                        {
                            Name = "TBD",
                            Url = new Uri("https://o2.services/licenses")
                        },
                        TermsOfService = new Uri("https://o2.services/terms-of-service")
                    }
                );

                var path = System.Reflection.Assembly.GetExecutingAssembly().Location;
                path = Path.ChangeExtension(path, ".xml");
                c.IncludeXmlComments(path);
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
                app.UseExceptionHandler("/Error");
            }
            app.UseCors(c => c
                .AllowAnyOrigin() // TODO: This is NOT SAFE
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithExposedHeaders("X-Stream-Output", "X-Chunked-Output", "X-Content-Length")
            );
            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v0/swagger.json", "IPFS HTTP API");
            });

            app.UseMvc();
        }
    }
}
