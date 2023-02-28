using CommandLine;
using Palmmedia.ReportGenerator.Core;

namespace SQLCover.Runner;

public class Program
{
    public static int Main(string[] args)
    {
        var result = 0;
        try
        {
            Parser.Default
                .ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(o));
                    result = Run(o);
                });
        }
        catch (Exception ex)
        {
            var exception = ex;
            while (exception != null)
            {
                Console.WriteLine(exception.Message);
                exception = exception.InnerException;
            }

            result = 1;
        }
        return result;
    }
    
    public static int Run(Options options)
    {
        var logging = true;
        var debugger = false;
        var codeCoverage = new CodeCoverage(
            options.ConnectionString,
            options.Database, 
            options.IncludeFilter.Split(';', StringSplitOptions.RemoveEmptyEntries), 
            options.ExcludeFilter.Split(';', StringSplitOptions.RemoveEmptyEntries),
            logging, 
            debugger);

        try
        {
            var basePath = Directory.GetCurrentDirectory();
            var outputDir = options.OutputDir;
            if (!Path.IsPathRooted(outputDir))
                outputDir = Path.Join(basePath, outputDir);

            Console.WriteLine("Covering tests...");
            var result = codeCoverage.Cover(options.Execute);

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            Console.WriteLine("Saving source files...");
            var sourceDir = Path.Join(outputDir, "source");
            if (!Directory.Exists(sourceDir))
                Directory.CreateDirectory(sourceDir);

            result.SaveSourceFiles(sourceDir);

            Console.WriteLine("Generating OpenCover XML...");
            var coverageFilename = Path.Join(outputDir, options.CoverageFilename);
            var openCoverXml = result.OpenCoverXml();
            File.WriteAllText(coverageFilename, openCoverXml);

            var generator = new Generator();

            IEnumerable<string> reportFilePatterns = new[] { coverageFilename };
            var targetDirectory = Path.Join(outputDir, "report");
            IEnumerable<string> sourceDirectories = new[] { sourceDir };
            var historyDirectory = Path.Join(outputDir, "history");
            IEnumerable<string> reportTypes = options.ReportTypes.Split(';', StringSplitOptions.RemoveEmptyEntries);
            IEnumerable<string> plugins = new List<string>();
            IEnumerable<string> assemblyFilters = new List<string>();
            IEnumerable<string> classFilters = new List<string>();
            IEnumerable<string> fileFilters = new List<string>();
            string? verbosityLevel = null;
            string? tag = null;

            Console.WriteLine("Generating report...");
            if (!generator.GenerateReport(
                    new ReportConfiguration(
                        reportFilePatterns,
                        targetDirectory,
                        sourceDirectories,
                        historyDirectory,
                        reportTypes,
                        plugins,
                        assemblyFilters,
                        classFilters,
                        fileFilters,
                        verbosityLevel,
                        tag)))
            {
                Console.WriteLine("Report generation failed!");
                return 2;
            };

            Console.WriteLine("Finished.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Exception? exception = codeCoverage.Exception;
            while (exception != null)
            {
                Console.WriteLine(exception.Message);
                exception = exception.InnerException;
            }
            return 1;
        }

        return 0;
    }
}
