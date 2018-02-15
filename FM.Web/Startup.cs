using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using FM.DAL;
using FM.DAL.Models;
using FM.DAL.Repositories;
using FM.Services;
using FM.Services.Models;
using FM.Web.ExceptionHandler;
using FM.Web.Models;
using NLog.Extensions.Logging;
using NLog.Web;

namespace FM.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<FMContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<FMContextSeedData>();
            services.AddTransient<IFMService, FMService>();

            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();

            services.AddLogging();

            services.AddMvc(config => { config.Filters.Add(typeof(CustomExceptionFilter)); });

            services.AddMvc().AddJsonOptions(config =>
            {
                config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
           FMContextSeedData seedData, ILoggerFactory factory)
        {
            env.ConfigureNLog("nlog.config");
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<FMContext>().Database.Migrate();
            }
            Mapper.Initialize(config =>
            {
                config.CreateMap<CreatePlayerViewModel, PlayerDTO>().ReverseMap();
                config.CreateMap<CreateTeamViewModel, TeamDTO>().ReverseMap();
                config.CreateMap<UpdateTeamViewModel, TeamDTO>().ReverseMap();
                config.CreateMap<EntityPlayer, PlayerDTO>().ReverseMap();
                config.CreateMap<EntityTeam, TeamDTO>().ReverseMap();
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            factory.AddNLog();
            app.AddNLogWeb();

            app.UseMvc();

            seedData.EnsureSeedData().Wait();

        }
    }
}