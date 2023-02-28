using CommandLine;

namespace SQLCover.Runner;

public class Options
{
    [Option('e', "execute", Required = true, HelpText = "Command to be executed")]
    public string? Execute { get; set; }

    [Option('d', "database", Required = true, HelpText = "")]
    public string? Database { get; set; }

    [Option('c', "connectionstring", Required = true, HelpText = "")]
    public string? ConnectionString { get; set; }

    [Option("include", Required = false, HelpText = "Object inclusion filter")]
    public string IncludeFilter { get; set; } = "";

    [Option("exclude", Required = false, HelpText = "Object exclusion filter")]
    public string ExcludeFilter { get; set; } = "";

    [Option('o', "outputdir", Required = false, Default = "output", HelpText = "Output directory")]
    public string OutputDir { get; set; } = "";

    [Option("coveragefile", Required = false, Default = "coverage.xml", HelpText = "Output directory")]
    public string CoverageFilename { get; set; } = "";

    [Option('r', "reporttypes", Required = false, HelpText = "Report types to generate")]
    public string ReportTypes { get; set; } = "";
}