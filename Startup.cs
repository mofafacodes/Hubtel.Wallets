using Hubtel.Wallets.Data;
using Hubtel.Wallets.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using AutoMapper;
using Newtonsoft.Json.Serialization;
using FluentValidation;
using Hubtel.Wallets.Dtos;
using Hubtel.Wallets.Utilities;

namespace Hubtel.Wallets
{
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
            //configuring sql dbcontext class to be used in application
            services.AddDbContext<WalletContext>(options => options.UseSqlServer(Configuration.GetConnectionString("WalletConnection")));

            services.AddScoped<IValidator<WalletCreateDto>, CreateValidation>();

            services.AddControllers().AddNewtonsoftJson(s =>
            {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddControllers();

            //adding automapper to map our internal domain commands to the various DTOs, making availvable throught out the application
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //would be temporarily used but swapped out for a peristent DB
            //services.AddScoped<IWalletRepo, MockWalletRepo>();
            //services.AddScoped<IWalletRepo, SqlWalletRepo>();
            //add async version
            services.AddScoped<IWalletRepoAsync, SqlWalletRepoAsync>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
