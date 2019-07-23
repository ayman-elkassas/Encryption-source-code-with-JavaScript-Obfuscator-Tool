using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace jsobfs
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       var r = new Runner(o);
                       r.Run();

                   });
        }
    }
}
