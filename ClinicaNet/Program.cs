using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ClinicaNet.Data;
using System.CodeDom.Compiler;
using DocumentFormat.OpenXml.Spreadsheet;
using ClinicaNet.Models;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;


namespace WebAppEFCore
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Crea el builder que configura la app y registra los servicios antes de arrancar la app
            var builder = WebApplication.CreateBuilder(args);

            // Registrar servicios en el contenedor de dependencias de la aplicaci�n

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("connectionDB")));


            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            //Servicio de Identity
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            })
                 .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();


            // Construir la aplicaci�n con la configuracion registrada
            var app = builder.Build();

            // Configurar el pipeline de solicitud HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
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

            app.MapRazorPages();

            //Seed de datos de roles y usuarios
            using (var scope = app.Services.CreateScope())
            {

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { "Asistente", "Doctor" };

                foreach(var rol in roles)
                {
                    if (!await roleManager.RoleExistsAsync(rol))
                    {
                        await roleManager.CreateAsync(new IdentityRole(rol));
                    }
                }
               
                //Usuario Admin
                var adminEmail = "asistente.Medina@clinicanet.com";
                var adminPassword = "As123456.";

                var adminExists = await userManager.FindByEmailAsync(adminEmail);

                if (adminExists == null)
                {
                    var adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
                    var result = await userManager.CreateAsync(adminUser, adminPassword);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Asistente");
                    }
                }

                //Usuario Afiliado

                var afiliadoEmail = "doctor_Caeiro@clinicanet.com";
                var afiliadoPassword = "Aa123123.";

                var userExists = await userManager.FindByEmailAsync(afiliadoEmail);

                if (userExists == null)
                {
                    var afiliadoUser = new IdentityUser { UserName = afiliadoEmail, Email = afiliadoEmail };

                    var result = await userManager.CreateAsync(afiliadoUser, afiliadoPassword);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(afiliadoUser, "Doctor");
                    }
                }

            }
               app.Run(); // Ejecutar la aplicación
        }
    }
}
/*
 En ASP.NET Core la configuración de una aplicación se puede dividir en dos etapas principales: registrar servicios y configurar el pipeline de solicitud HTTP.

1. Etapa de Registro de Servicios (Configuración de Servicios): en esta etapa, registras los servicios que la aplicación necesita para funcionar. 
Esto se hace utilizando builder.Services. Los servicios son objetos que se pueden inyectar en diferentes partes de la aplicación, como controladores,
middleware, y otros componentes. Algunos de estos servicios pueden ser DbContext, Identity, loggers, etc.

 Ejemplos de lo que se hace en esta etapa:

    - Registrar el DbContext para manejar la base de datos.
    - Registrar Identity para la autenticación de usuarios.
    - Registrar servicios necesarios para MVC (controladores y vistas).
    - Registrar cualquier servicio adicional que la aplicación�n requiera (por ejemplo, servicios personalizados de negocio).

2. Etapa de Configuración del Pipeline de Solicitudes HTTP (Configuración del Middleware): En esta etapa, configuras el pipeline de solicitudes HTTP
(middleware) de la aplicación, que es el conjunto de componentes que procesan las solicitudes y respuestas. Cada middleware puede modificar la 
solicitud o respuesta antes de pasar al siguiente componente en el pipeline.

Esta etapa se hace utilizando el objeto app que se construye con builder.Build(). Aqu� defines c�mo debe manejar la aplicación:

    - Redirigir a HTTPS.
    - Servir archivos est�ticos (como im�genes y archivos CSS).
    - Enrutamiento de solicitudes (rutas a controladores y vistas).
    - Autenticación y autorización.
    - Manejo de excepciones, como errores 404 o errores de base de datos.
 */