using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

namespace #cs#;

static class Program
{
    private static readonly AppInfo AppInfo = BuildAppInfo();

    public static Version Version             => AppInfo.Version;
    public static string FileVersion          => AppInfo.FileVersion;
    public static string InformationalVersion => AppInfo.InformationalVersion;
    public static string ProductVersion       => Application.ProductVersion;

    public static string Company              => Application.CompanyName;
    public static string Product              => Application.ProductName;
    public static string Description          => AppInfo.Description;

    static Program()
    {
        AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
    }

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
    #if NET472
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
    #else
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
    #endif
        Application.Run(new MainForm());
    }

    private static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
    {
        var asmName = new AssemblyName(args.Name).Name;
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var searchRoot = Path.Combine(baseDir, "Assemblies");
        if (!Directory.Exists(searchRoot)) return null;

        foreach (var dll in Directory.EnumerateFiles(searchRoot, asmName + ".dll", SearchOption.AllDirectories))
        {
            try { return Assembly.LoadFrom(dll); }
            catch { continue; }
        }
        return null;
    }

    private static AppInfo BuildAppInfo()
    {
        var asm = Assembly.GetExecutingAssembly();

        return new AppInfo{
            Version = asm.GetName().Version,
            FileVersion = FileVersionInfo.GetVersionInfo(asm.Location).FileVersion,
            InformationalVersion = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "",

            Description = asm.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ?? "",
        };
    }
}

class AppInfo
{
    public Version Version { get; set; }
    public string FileVersion { get; set; } = "";
    public string InformationalVersion { get; set; } = "";

    public string Description { get; set; } = "";
}
