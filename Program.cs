using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace ASP.Net_MVC_Firebase
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

            //Google 이외의 환경에서 초기화할 때
            var firebaseApp = FirebaseApp.Create(new AppOptions()	
            {
                Credential = GoogleCredential.FromFile(@"D:\GitHub\ASP.Net MVC-Firebase\wwwroot\project-servergame-firebase-adminsdk-umeje-31335b9b38.json"),
                ProjectId = builder.Configuration["project -servergame"],
                ServiceAccountId = builder.Configuration["firebase-adminsdk-umeje@project-servergame.iam.gserviceaccount.com"]
            });

            builder.Services.AddSingleton(firebaseApp);

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
            app.UseAuthentication(); //추가한거

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();

		} //end main

	}
}
