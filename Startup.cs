using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;
using System.Security.Cryptography;

namespace weather
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
            string validIssuer = Configuration.GetSection("Authorization:Issuer").Get<string>();
            string[] validAudiences = Configuration.GetSection("Authorization:Audience").Get<string[]>();
            string jwksUri = Configuration.GetSection("Authorization:JWKS").Get<string>();
            string keyId = Configuration.GetSection("Authorization:Kid").Get<string>();
            string alg = Configuration.GetSection("Authorization:Alg").Get<string>();
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(async options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = false,
                        ValidAudiences = validAudiences,
                        ValidIssuer = validIssuer,
                        IssuerSigningKey = await GetIssuerSigningKeyAsync(jwksUri, keyId, alg)
                    };
                });
                
            services.AddAuthorization(options =>
            {
                options.AddPolicy("lowRisk", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(claim =>
                            claim.Type == "risk" && int.Parse(claim.Value) < 50
                        )
                    )
                );
                options.AddPolicy("developer", policy =>
                    policy.RequireClaim("title", "junior developer", "senior developer")
                    .RequireClaim("department", "development")
                );
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "weather", Version = "v1" });
            });
        }

        static private async Task<RsaSecurityKey> GetIssuerSigningKeyAsync(string jwksUri, string keyId, string alg)
        {
            using var httpClient = new HttpClient();
            
            // Fetch JWKS JSON from URI
            var response = await httpClient.GetStringAsync(jwksUri);            
            JsonWebKeySet jwks = new(response);
            
            // Find the key with the specified Key ID (kid) and algorithm (alg)
            var key = jwks.Keys.Single(k => k.Kid == keyId && k.Alg == alg) ?? throw new Exception($"Key with ID '{keyId}' not found.");

            // Extract parameters from the JWK
            var n = Base64UrlEncoder.DecodeBytes(key.N.ToString());
            var e = Base64UrlEncoder.DecodeBytes(key.E.ToString());

            // Create an RSA security key
            var rsa = RSA.Create();
            rsa.ImportParameters(new RSAParameters
            {
                Modulus = n,
                Exponent = e
            });

            return new RsaSecurityKey(rsa) { KeyId = keyId };
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "weather v1"));
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
