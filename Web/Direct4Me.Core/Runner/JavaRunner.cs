using System.Diagnostics;
using System.Reflection;

namespace Direct4Me.Core.Runner;

public interface IJavaRunner
{
    Task<string> RunJarAsync(string args = "");
}

public class JavaRunner : IJavaRunner
{
    public async Task<string> RunJarAsync(string args = "")
    {
        const string javaExecutablePath = "java";

        // Correctly setting FileName and Arguments
        var processStartInfo = new ProcessStartInfo
        {
            FileName = javaExecutablePath,
            Arguments = $"D:\\workspace\\project-hub\\MakineLearning\\DeliveryEstimator_JarBuilder3000\\src\\main\\java\\org\\example\\MakineSmartAI.java MakineSmartAI {args}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = processStartInfo;

        process.Start();

        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();

        await Task.WhenAll(process.WaitForExitAsync(), outputTask, errorTask);

        var result = outputTask.Result;
        var errors = errorTask.Result;

        if (!string.IsNullOrEmpty(errors))
        {
            throw new Exception(errors);
        }

        return result;
    }
}