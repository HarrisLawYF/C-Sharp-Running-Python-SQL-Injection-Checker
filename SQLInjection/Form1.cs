using System;
using System.Windows.Forms;
using System.Configuration;

namespace SQLInjection
{
    public partial class Form1 : Form
    {
        private static string filePythonExePath = ConfigurationManager.AppSettings["filePythonExePath"];
        private static string filePythonNamePath = ConfigurationManager.AppSettings["filePythonNamePath"];
        private IMLSharpPython mlScript;
        private MLSriptTest testApp;
        public Form1()
        {
            InitializeComponent();
            mlScript = new MLScript();
            testApp = new MLSriptTest();
            testApp.ExecutePythonScriptTest();
        }

        private void checkBtn_Click(object sender, EventArgs e)
        {
            string outputText, standardError;
            string fileNameParameter = $"{filePythonNamePath} {insertStringTxt.Text}";
            // Execute the python script file 
            outputText = mlScript.ExecutePythonScript(fileNameParameter, out standardError);
            if (outputText.Contains("[1]"))
            {
                resultLbl.Text = "Possible SQL Injection";
            }
            else if (outputText.Contains("[0]"))
            {
                resultLbl.Text = "Normal comment";
            }
            else
            {
                resultLbl.Text = "N/A";
            }
        }
    }
}
