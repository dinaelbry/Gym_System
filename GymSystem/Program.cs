using GymSystem.BLL.Services.Classes;
using GymSystem.BLL.Services.Attachment;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.DAL.Data.Contexts;
using GymSystem.DAL.Repository.Classes;
using GymSystem.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using GymManagementBLL.Services.Classes;
namespace GymSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
    builder.Services.AddDbContext<GymDbContext>(options => {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                    });


            // Repos
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            builder.Services.AddScoped<IMemberServices, MemberServices>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddScoped<IPlanService,PlanService>();
            builder.Services.AddScoped<IMembershipService, MembershipService>();

            var app = builder.Build();

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
