using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace jsobfs
{
    public class Options
    {
        [Option('d', "js_dir", Required = true, HelpText = "The directory containing the javascript files.")]
        public string JsDir { get; set; }

        [Option('b', "backup", Required = false, HelpText = "backup flag", Default = true)]
        public bool Backup { get; set; }

        [Option('e', "jx_ext", Required = false, HelpText = "The javascript extension.", Default=".js")]
        public string JsExtension { get; set; }

        [Usage(ApplicationAlias = "jsobfs")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                return new List<Example>() {
                    new Example("Obfuscates javascript files at a given directory", 
                    new Options { JsDir = "path/to/js", 
                    Backup = true,
                    JsExtension = ".js"})
                };
            }
        }

    }


}