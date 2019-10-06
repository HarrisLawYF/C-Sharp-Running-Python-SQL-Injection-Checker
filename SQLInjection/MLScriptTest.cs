using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SQLInjection
{
    [TestClass()]
    public class MLSriptTest
    {
        private static string filePythonNamePath = ConfigurationManager.AppSettings["filePythonNamePath"];
        [TestMethod()]
        public void ExecutePythonScriptTest()
        {
            string standardError;
            string expectedOutputText = "Possible SQL Injection";
            MLScript mlScript = new MLScript();
            string testingText = "SELECT * FROM app_client_farm_devices";
            string fileNameParameter = $"{filePythonNamePath} {testingText}";
            string actualOutputText = mlScript.ExecutePythonScript(fileNameParameter, out standardError);
            string testResult = "";
            if (actualOutputText.Contains("[1]"))
            {
                testResult = "Possible SQL Injection";
            }
            else if (actualOutputText.Contains("[0]"))
            {
                testResult = "Normal comment";
            }
            else
            {
                testResult = "N/A";
            }
            Assert.AreEqual(expectedOutputText, testResult);
        }
    }
}
