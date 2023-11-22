namespace She_He_Store.Middleware
{
    public class SessionDataMiddleWare
    {
        private readonly RequestDelegate _next;
        public SessionDataMiddleWare(RequestDelegate next)
        {
            _next = next; 
        }
        public async Task InvokeAsync(HttpContext context)
        {
            int? userId = context.Session.GetInt32("UserId");
            string fname = context.Session.GetString("Fname");
            string userName= context.Session.GetString("userName");
            string profileImage = context.Session.GetString("ProfileImage");
            context.Items["UserId"] = userId;
            context.Items["Fname"] = fname;
            context.Items["userName"] = userName;
            context.Items["profileImage"] = profileImage;
            await _next(context);
        }
    }
}
