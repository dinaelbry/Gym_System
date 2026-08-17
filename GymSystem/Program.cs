using GymSystem.BLL.Services.Attachment;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.DAL.Data.Contexts;
using GymSystem.DAL.Repository.Classes;
using GymSystem.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using GymSystem.DAL.Entities;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using GymSystem.BLL.Services.Classes;
using GymSystem.BLL.Utilites;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;



namespace GymSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>(options => {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
             });



            // Repos

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();


            // Services
            builder.Services.AddScoped<IMemberServices, MemberServices>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IMembershipService, MembershipService>();
            builder.Services.AddScoped<IHomeStateService, HomeStateService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();


            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());


            // Configure Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                options.User.RequireUniqueEmail = false;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
            }).AddEntityFrameworkStores<GymDbContext>() // enable DI
                .AddDefaultTokenProviders();


            // Configure cookie settings
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });



            var app = builder.Build();
            await app.MigrationAndSeedAsync();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
