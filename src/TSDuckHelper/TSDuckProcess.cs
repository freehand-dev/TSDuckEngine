using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TSDuckHelper
{
    public static class TSDuckProcess
    {

        static public string? TSPApp { get; set; }

        static private string GetExecutableFile()
        {
            if (!string.IsNullOrEmpty(TSPApp)) 
            { 
                if (File.Exists(TSPApp))
                {
                    return TSPApp;
                }
            }

            if (OperatingSystem.IsLinux())
            {
                return File.Exists(Path.Combine(@"/usr/bin/", "tsp"))
                    ? Path.Combine(@"/usr/bin/", "tsp")
                    : Path.Combine(@"/usr/local/bin/", "tsp");
            }
            else if (OperatingSystem.IsWindows())
            {
                return Path.Combine(@"C:\Program Files\TSDuck\bin\", "tsp.exe");
            }
            else
            {
                throw new Exception("tsp command not found");
            }
        }

        static public async Task<int?> Run(string argumets, DataReceivedEventHandler errorDataReceived, CancellationToken cancellationToken = default, Action<Process>? returnProcess = null)
        {
            string tspApp = TSDuckProcess.GetExecutableFile();
            int? exitCode;

            if (!File.Exists(tspApp))
            {
                throw new Exception($"tsduck could not be found {tspApp}");
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = tspApp,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true, 
            };

            startInfo.Arguments = argumets;

            Process tsduckProcess = new Process();
            try
            {
                tsduckProcess.StartInfo = startInfo;
                tsduckProcess.ErrorDataReceived += errorDataReceived;
                bool ffmpegStarted = tsduckProcess.Start();
                tsduckProcess.BeginErrorReadLine();
                try
                {
                    returnProcess?.Invoke(tsduckProcess);
                    await tsduckProcess.WaitForExitAsync(cancellationToken);
                }
                finally
                {
                    if (tsduckProcess != null && !tsduckProcess.HasExited)
                    {
                        try
                        {
                            tsduckProcess.Kill();
                        }
                        catch { }
                    }
                }
            }
            finally
            {
                exitCode = tsduckProcess.ExitCode;
                tsduckProcess.ErrorDataReceived -= errorDataReceived;
                tsduckProcess?.Dispose();
            }
            return exitCode;
        }
    }
}
