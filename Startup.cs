namespace Event
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseSession();

            // Other middleware configurations
        }
    }
}
