using System;
using System.Collections.Generic;

using Microsoft.Extensions.PlatformAbstractions;

using Mono.Options;

namespace wsfind.Bll
{
    public class Options
    {
        public List<KeyValuePair<string, string>> ContainsProperties { get; set; } = new List<KeyValuePair<string, string>>();

        public List<KeyValuePair<string, string>> FreetextProperties { get; set; } = new List<KeyValuePair<string, string>>();

        public string Scope { get; set; }
        public string Directory { get; set; }

        public bool Verbose { get; set; }

        public bool HelpRequested { get; set; }
        public bool VersionRequested { get; set; }

        public void ParseOptions(string[] args)
        {
            OptionSet optionSet = new OptionSet { 
                {
                    "path=", "Add contains filter on ItemPathDisplay", contains =>
                    {
                        ContainsProperties.Add(new KeyValuePair<string, string>("System.ItemPathDisplay", contains));
                    }
                },

                {
                    "freetext=", "Add freetext document search", freetext =>
                    {
                        FreetextProperties.Add(new KeyValuePair<string, string>("*", freetext));
                    }
                },

                { "scope=", "Limit search to specific folder and its sub-folders", scope => Scope = scope },
                { "directory=", "Limit search to a specific folder", directory => Directory = directory },

                { "v|verbose", "Enable verbose messages", verbose => Verbose = verbose != null },
                { "h|help", "Show this message and exit", help => HelpRequested = help != null },
                { "version", "Show version message", version => VersionRequested = version != null },
            };            

            try {
                optionSet.Parse(args);

            } catch (OptionException e) {
                Console.Write("Error: ");

                Console.WriteLine(e.Message);
                Console.WriteLine($"Try {PlatformServices.Default.Application.ApplicationName} --help' for more information.");

                Environment.Exit(1);
            }   

            if (HelpRequested) {
                Help(optionSet);

                Environment.Exit(0);
            } else if (VersionRequested) {
                Console.WriteLine($"Version: {WhenBuilt.ItWas.ToString("s")}");

                Environment.Exit(0);
            }
        }

        public void Help (OptionSet optionSet)
        {
            Console.WriteLine($"Usage: {PlatformServices.Default.Application.ApplicationName} [OPTIONS]");
            Console.WriteLine($"Version: {WhenBuilt.ItWas.ToString("s")}");
            Console.WriteLine();

            // output the options
            Console.WriteLine("Options:");
            optionSet.WriteOptionDescriptions (Console.Out);
        }
    }
}
