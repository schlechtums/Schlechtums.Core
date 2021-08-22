using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Schlechtums.Core.Common.Extensions
{
    public static class ProcessExtensions
    {
        /// <summary>
        /// Runs a process in a hidden command line window.
        /// </summary>
        /// <param name="process">The full path to the process.</param>
        /// <param name="arguments">Any arguments.</param>
        /// <param name="workingDirectory">The working directory in which to run the process.  Defaults to the current directory if null.</param>
        /// <param name="waitForExit">True or false to wait for the process to exit</param>
        /// <returns>The process object.</returns>
        public static Process RunProcessInBackground(String process, String arguments, String workingDirectory, Boolean waitForExit = true)
        {
            var psi = new ProcessStartInfo(process, arguments);
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.WorkingDirectory = workingDirectory ?? psi.WorkingDirectory;

            var p = new Process();
            p.StartInfo = psi;

            p.Start();
            if (waitForExit)
                p.WaitForExit();

            return p;
        }

        /// <summary>
        /// Runs a process in a hidden command line window synchronously and returns the standard output
        /// </summary>
        /// <param name="process">The full path to the process.</param>
        /// <param name="arguments">Any arguments.</param>
        /// <param name="workingDirectory">The working directory in which to run the process.  Defaults to the current directory if null.</param>
        /// <returns>The standard out from the process</returns>
        public static String RunProcessInBackgroundAndGetStandardOutput(String process, String arguments, String workingDirectory = null)
        {
            var psi = new ProcessStartInfo(process, arguments);
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.WorkingDirectory = workingDirectory ?? psi.WorkingDirectory;

            var p = new Process();
            p.StartInfo = psi;

            p.Start();

            var sb = new StringBuilder();

            while (!p.HasExited)
            {
                sb.Append(p.StandardOutput.ReadToEnd());
                Thread.Sleep(1);
            }

            sb.Append(p.StandardOutput.ReadToEnd());

            return sb.ToString();
        }
    }
}