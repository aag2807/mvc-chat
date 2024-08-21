using ChatApplication.Persistence.Chat;

namespace ChatApplication.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddChatServices(this IServiceCollection services)
    {
        services.AddScoped<IChatRepository, ChatRepository>();
    }
}