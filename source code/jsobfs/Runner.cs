using System;
using System.IO;
using System.IO.Compression;

namespace jsobfs
{
    public class Runner
    {
        Options _opts;

        public Runner(Options opts)
        {
            _opts = opts;
        }

        public void Run()
        {
            DirectoryInfo d = new DirectoryInfo(_opts.JsDir);
            FileInfo[] Files = d.GetFiles(string.Format("*{0}", _opts.JsExtension));
            if (_opts.Backup)
                AddFilesToZip(Files);
            else
                ConsoleLog("WARNING: backup is set to false. Codes will be overwritten directly", ConsoleColor.DarkYellow);
            Obfuscate(Files);
        }

        void AddFilesToZip(FileInfo[] files)
        {
            if (files == null || files.Length == 0)
            {
                ConsoleLog(string.Format("ERROR: No javascript files were found at the given directory"), ConsoleColor.Red);
                return;
            }
            ConsoleLog(string.Format("INFO:Creating ZIP file for the backup"), ConsoleColor.Green);
            string zip_path = string.Format(@"{0}\{1}.zip", _opts.JsDir, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
            using (var zipArchive = ZipFile.Open(zip_path, ZipArchiveMode.Update))
            {
                foreach (var file in files)
                {
                    zipArchive.CreateEntryFromFile(file.FullName,  file.Name);
                    ConsoleLog(string.Format("INFO:\tAdding File: {0}", file.Name), ConsoleColor.Green);
                }
            }
            ConsoleLog(string.Format("INFO:Backup zip file created successfully at: {0}", zip_path), ConsoleColor.Green);
        }

        void ConsoleLog(string text, ConsoleColor clr)
        {
            Console.ForegroundColor = clr;
            Console.Write(text);
            Console.WriteLine();
            Console.ResetColor();
        }
        
        void Obfuscate(FileInfo[] files)
        {
            if (Environment.GetEnvironmentVariable("node") != null)
                {
                    ConsoleLog("ERROR: node is not found. Please add node to the environmental variables", ConsoleColor.Red);
                    return;
                }
            if (Directory.Exists(@"javascript-obfuscator\obfuscate.bat"))
            {
                ConsoleLog("ERROR: javascript-obfuscator binary is not found. Makesure it exists at the correct path", ConsoleColor.Red);
                return;
            }
            ConsoleLog("INFO: Starting obfuscation", ConsoleColor.Green);
            string exe_path = AppDomain.CurrentDomain.BaseDirectory;
            string cmd = string.Format(@"{0}\javascript-obfuscator\obfuscate.bat", exe_path);
            string bin = string.Format(@"{0}\javascript-obfuscator\bin\javascript-obfuscator", exe_path);
            foreach (var file in files)
            {
                using (var p = new System.Diagnostics.Process())
                {
                    p.StartInfo.CreateNoWindow = false;
                    p.StartInfo.RedirectStandardOutput = false;
                    p.StartInfo.RedirectStandardInput = false;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardError = false;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.Arguments = string.Format("/c {0} {1} {2}", cmd, bin, file.FullName);
                    p.Start();
                    p.WaitForExit();
                    ConsoleLog(string.Format("INFO:\tObfuscated file: {0}", file.Name), ConsoleColor.Green);
                }
            }
            ConsoleLog("INFO: All files obfuscated successfully", ConsoleColor.Green);
        }
    }

}