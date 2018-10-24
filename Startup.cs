using System.Globalization;
using System.IO.Compression;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Mircroservice.Ioc;
using Swashbuckle.AspNetCore.Swagger;

namespace mircroservice
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
            // // Configura o modo de compressão
            // services.Configure<GzipCompressionProviderOptions>(
            //     options => options.Level = CompressionLevel.Optimal);

            // services.AddResponseCompression(options =>
            // {
            //     options.Providers.Add<GzipCompressionProvider>();
            //     options.EnableForHttps = true;
            // });

            //Configura para nao pegar valor null em retorno
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // .AddJsonOptions(opcoes =>
            // {
            //     opcoes.SerializerSettings.NullValueHandling =
            //         Newtonsoft.Json.NullValueHandling.Ignore;
            // });

            // Ativando o uso de cache via Redis
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration =
                    Configuration.GetConnectionString("ConexaoRedis");
                options.InstanceName = "TesteRedisCache";
            });

            Injector.InitRepository(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "API Values",
                        Version = "v1",
                        Description = "Todos os recursos disponíveis.",
                        Contact = new Contact
                        {
                            Name = "Vinícius Alexandre Saraiva Silva",
                            Url = "valexandre@br.fujitsu.com"
                        }
                    });


                string caminhoAplicacao =
                    PlatformServices.Default.Application.ApplicationBasePath;
                string nomeAplicacao =
                    PlatformServices.Default.Application.ApplicationName;

                string caminhoXmlDoc = System.IO.Path.Combine(caminhoAplicacao, "api-microservice.xml");

                c.IncludeXmlComments(caminhoXmlDoc);


                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            var cultureInfo = new CultureInfo("pt-BR");
            cultureInfo.NumberFormat.CurrencySymbol = "R$";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pessoas");
                c.RoutePrefix = "api/docs";
            });
            app.UseHttpsRedirection();
            //app.UseResponseCompression();
            app.UseMvc();
        }
    }
}
