using CommandLine;

namespace Kata
{
    public class Options
    {
        [Option("jwtdir", Required = true, HelpText = "Directory with jwt")]
        public string JwtDir { get; set; }

        [Option("sutname", Required = true, HelpText = "Name of SUT")]
        public string SutName { get; set; }

        [Option("prompt", Required = true, HelpText = "Whether user input is required. False=> batch-style")]
        public bool UsePrompt { get; set; }

        [Option("sleep", Required = false, HelpText = "Sleep (ms) between steps. Defaults to 0.")]
        public int SleepMs { get; set; } = 0;
    }
}