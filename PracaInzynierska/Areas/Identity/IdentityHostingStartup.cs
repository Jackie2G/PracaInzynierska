using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PracaInzynierska.Areas.Identity.Data;
using PracaInzynierska.Data;
using PracaInzynierska.Services;

[assembly: HostingStartup(typeof(PracaInzynierska.Areas.Identity.IdentityHostingStartup))]
namespace PracaInzynierska.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<PracaInzynierskaContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("AccountConnection")));

                services.AddDefaultIdentity<PracaInzynierskaUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<PracaInzynierskaContext>();

                //services.AddSingleton<IEmailSender, EmailSender>();
                //services.Configure<AuthMessageSenderOptons>
                //services.AddTransient<IEmailSender, EmailSender>();
                //services.Configure<AuthMessageSenderOptions>(Configuration);
            });
        }
    }

    public class EmailSender: IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }
    }
}