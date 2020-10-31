public class AClient
{
    private Discord.DiscordClient discordClient;
    public AClient(string token)
    {   
        discordClient = new Discord.DiscordClient(token);
    }
    public Discord.DiscordClient GetClient()
    {
        return discordClient;
    }
}