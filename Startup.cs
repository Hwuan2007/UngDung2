// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using Microsoft.AspNetCore.Http;
// using Newtonsoft.Json;
// using System;

// namespace hocvieccuccangMVC
// {
//     public class Startup
//     {
//         public Startup(IConfiguration configuration)
//         {
//             Configuration = configuration;
//         }

//         public IConfiguration Configuration { get; }

//         public void ConfigureServices(IServiceCollection services)
// {
//         services.AddHttpContextAccessor();
//         services.AddSession(options =>
//         {
//             options.IdleTimeout = TimeSpan.FromMinutes(30); // Thiết lập thời gian chờ phiên
//             options.Cookie.HttpOnly = true; // Chỉ cho phép cookie được truy cập bởi HTTP
//             options.Cookie.IsEssential = true; // Đảm bảo phiên không bị vô hiệu hóa khi người dùng vô hiệu hóa cookie
//         });

//         services.AddControllersWithViews();
// }

//         public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//         {
//             if (env.IsDevelopment())
//             {
//                 app.UseDeveloperExceptionPage();
//             }
//             else
//             {
//                 app.UseExceptionHandler("/Home/Error");
//                 app.UseHsts();
//             }

//             app.UseHttpsRedirection();
//             app.UseStaticFiles();

//             app.UseRouting();

//             app.UseAuthorization();

//             app.UseSession();

//             app.UseEndpoints(endpoints =>
//             {
//                 endpoints.MapControllerRoute(
//                     name: "default",
//                     pattern: "{controller=Staff}/{action=Index}/{id?}");
//             });
//         }
//     }
//     public static class SessionExtensions
// {
//     public static void SetObject<T>(this ISession session, string key, T value)
//     {
//         session.SetString(key, JsonConvert.SerializeObject(value));
//     }

//     public static T GetObject<T>(this ISession session, string key)
//     {
//         var value = session.GetString(key);
//         return value == null ? default : JsonConvert.DeserializeObject<T>(value);
//     }
// }
// }
