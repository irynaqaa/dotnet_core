```csharp
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace MyCompany.MyProject.Security
{
    public class SecurityConfig
    {
        private readonly ILogger<SecurityConfig> _logger;

        public SecurityConfig(ILogger<SecurityConfig> logger)
        {
            _logger = logger;
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = Environment.GetEnvironmentVariable("AUTHORITY");
                options.RequireHttpsMetadata = true;
                options.ClientId = Environment.GetEnvironmentVariable("CLIENT_ID");
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;

                options.Scope.Add("openid");
                options.Scope.Add("profile");

                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    NameClaimType = "name",
                    ValidateIssuer = true,
                    ValidIssuers = Environment.GetEnvironmentVariable("VALID_ISSUERS").Split(','),
                    ValidateAudience = true,
                    ValidAudiences = Environment.GetEnvironmentVariable("VALID_AUDIENCES").Split(','),
                    ValidateLifetime = true, // validate token expiration
                    ClockSkew = TimeSpan.FromMinutes(5) // allow for some clock skew
                };

                options.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProvider = context =>
                    {
                        if (context.Request.Path != "/signin-oidc")
                        {
                            context.Response.StatusCode = 401;
                            context.HandleResponse();
                        }
                        return Task.CompletedTask;
                    },
                    OnRemoteFailure = context =>
                    {
                        _logger.LogError($"Authentication failed: {context.Failure.Message}");
                        context.Response.StatusCode = 401;
                        context.Response.Headers["Location"] = "/login";
                        context.HandleResponse();
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = async context =>
                    {
                        var principal = context.Principal;
                        if (principal == null || !principal.Identity.IsAuthenticated)
                        {
                            _logger.LogError("Authentication failed: no principal or not authenticated");
                            context.Fail("No principal or not authenticated");
                        }
                        else
                        {
                            var claimsPrincipal = principal as System.Security.Claims.ClaimsPrincipal;
                            if (claimsPrincipal != null)
                            {
                                var nameClaim = claimsPrincipal.FindFirstValue("name");
                                if (string.IsNullOrEmpty(nameClaim))
                                {
                                    _logger.LogError("Authentication failed: no 'name' claim");
                                    context.Fail("No 'name' claim");
                                }
                                else
                                {
                                    _logger.LogInformation($"Authenticated user {nameClaim}");
                                }
                            }
                        }
                        await Task.CompletedTask;
                    },
                    OnUserInformationReceived = async context =>
                    {
                        var claimsPrincipal = context.Principal as System.Security.Claims.ClaimsPrincipal;
                        if (claimsPrincipal != null)
                        {
                            var nameClaim = claimsPrincipal.FindFirstValue("name");
                            _logger.LogInformation($"Authenticated user {nameClaim}");
                        }
                        else
                        {
                            _logger.LogError("Authentication failed: no claims principal");
                            context.Fail("No claims principal");
                        }
                        await Task.CompletedTask;
                    },
                };
            });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
        }
    }
}
```