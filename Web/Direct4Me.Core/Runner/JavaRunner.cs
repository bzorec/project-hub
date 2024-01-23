using System.Diagnostics;

namespace Direct4Me.Core.Runner;

public interface IJavaRunner
{
    Task<string> RunJarAsync(string jarPath, string args = "");
}

public class JavaRunner : IJavaRunner
{
    public async Task<string> RunJarAsync(string jarPath, string args = "")
    {
        const string javaExecutablePath = "java";

        var command = $"{javaExecutablePath} -jar \"{jarPath}\" {args}";

        var processStartInfo = new ProcessStartInfo
        {
            FileName = command,
            Arguments = args,
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

        if (!string.IsNullOrEmpty(errors)) throw new Exception(errors);

        return result;
    }
}