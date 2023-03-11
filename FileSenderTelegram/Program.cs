using Deployf.Botf;

namespace TISBIHelperBot
{
    public class Program : BotfProgram
    {
        public static void Main(string[] args)
        {
            StartBot(args, onConfigure: (svc, _) =>
            {
            }, onRun: (_, _) =>
            {
            });
        }
    }
}