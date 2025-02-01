using RannaTask.DAL;
//using RannaTask.Business;

namespace RannaTask.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddDALRegistiration(builder.Configuration);

            //builder.Services.AddBusinessServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();
            //app.UseAuthentication(); Burasý da Kodlanacak

            app.MapControllers();

            app.Run();
        }
    }
}
