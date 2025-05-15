using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Linq;

namespace Demo
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }


        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        
        public void ConfigureServices(IServiceCollection services)
        {
            // All API requests require JWT access token validation
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // Use OAuth claim names to prevent the claims principal recxiving Microsoft-specific claim names
                    options.MapInboundClaims = false;

                    // Use JWT best practices to check the expected parameters of issuer, audience and algorithm
                    options.Authority = Configuration["Authorization:Issuer"];
                    options.Audience = Configuration["Authorization:Audience"];
                    options.TokenValidationParameters.ValidAlgorithms = [Configuration["Authorization:Algorithm"]];

                    if (Environment.IsDevelopment())
                    {
                        options.RequireHttpsMetadata = false;
                    }
                });

            services.AddAuthorization(options =>
            {
                // Normal sensitivity endpoints require at least a read scope in the access token
                options.AddPolicy("has_required_scope", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(claim =>
                            claim.Type == "scope" && claim.Value.Split(' ').Any(c => c == "read")
                        )
                    )
                );

                // Higher sensitivity endpoints also require a qualifying risk score as a custom claim in the access token
                options.AddPolicy("has_low_risk", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(claim =>
                            claim.Type == "risk" && Int32.Parse(claim.Value) < 50
                        )
                    )
                );
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo API v1"));
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
