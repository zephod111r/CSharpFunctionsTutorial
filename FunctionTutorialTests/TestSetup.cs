using System;
using System.IO;
using Microsoft.Extensions.Configuration;

public class TestSetup
{
    public TestSetup()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .Build();

        foreach (var setting in configuration.GetSection("Values").GetChildren())
        {
            Environment.SetEnvironmentVariable(setting.Key, setting.Value);
        }
    }
}
