using System.Text.Json.Serialization;
using portfolio_backend.Services.Interfaces;
using portfolio_backend.Models.Repository;
using portfolio_backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace portfolio_backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /*
        secret: QuarterPounderSevenBirAdamimBen22021999
        token: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6InNja21rIiwicHdvcmQiOiIzMTc2NjU0NTQyOGVjODAwOGZiMDYwNTZiODY1ZDAxMDEwMzZkNjk4MWQzMjExMzMwNzU2NmE0MWU2NWFkYzE3In0.k7jZW591cEnN4DDqlqDiju06CFYjuVI108cp7d3aHMI
        */
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("LocalPolicy", builder => { builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader(); }));

            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
            services.AddDbContext<AppDatabaseContext>(optionsAction =>
            {
                optionsAction.UseSqlServer(Configuration.GetConnectionString("Database"));
            });
            services.AddSingleton<IMailService, MailService>();
            services.AddSingleton<IImageService, ImageService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "backend", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "backend v1"));
            }
            app.UseCors("LocalPolicy");
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
