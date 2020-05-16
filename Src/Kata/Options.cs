using CommandLine;

namespace Kata
{
    public class Options
    {
        [Option("jwtdir", Required = true, HelpText = "Directory with jwt")]
        public string JwtDir { get; set; }

        [Option("sutname", Required = true, HelpText = "Name of SUT")]
        public string SutName { get; set; }
    }
}