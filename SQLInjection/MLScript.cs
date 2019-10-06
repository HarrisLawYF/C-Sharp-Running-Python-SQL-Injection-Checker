using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace SQLInjection
{
    public class MLScript : IMLSharpPython
    {
        /// <summary>
        /// ML Sharp Python class constructor
        /// </summary>
        /// <param name="exePythonPath">Python EXE file path</param>
        public MLScript()
        {
        }
        /// <summary>
        /// Execute Python script file
        /// </summary>
        /// <param name="filePythonScript">Python script file and input parameter(s)</param>
        /// <param name="standardError">Output standard error</param>
        /// <returns>Output text result</returns>
        public string ExecutePythonScript(string filePythonScript, out string standardError)
        {
            string outputText = string.Empty;
            standardError = string.Empty;
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/K \"C:\\Program Files (x86)\\Microsoft Visual Studio\\Shared\\Anaconda3_64\\Scripts\\activate.bat\"",
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                using (var sw = process.StandardInput)
                {
                    if (sw.BaseStream.CanWrite)
                    {
                        sw.WriteLine("python "+filePythonScript);
                    }
                }
                outputText = process.StandardOutput.ReadToEnd();
                //To remove new line
                outputText = outputText.Replace(Environment.NewLine, string.Empty);
                //To remove the excessive words in the cmd result
                outputText = outputText.Replace("python " + filePythonScript, string.Empty);
                standardError = process.StandardError.ReadToEnd();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return outputText;
        }
    }

    public interface IMLSharpPython
    {
        string ExecutePythonScript(string filePythonScript, out string standardError);
    }
}
