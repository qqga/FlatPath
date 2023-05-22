using CommandLine;

namespace FlatPath.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Parser.Default.ParseArguments<Options>(args)
            .WithParsed(RunOptions)
            .WithNotParsed(HandleParseError);
        }

        static void RunOptions(Options opts)
        {
            FlatPathManager flatPathManager = new FlatPathManager();

            if (opts.NestedFolders && Confirm($"Move all files to root in subdirectories from direcotries in {opts.Path}"))
            {
                flatPathManager.FlatNested(opts.Path, opts.Prefix, opts.UseFolderAsPrefix);
            }
            else if(Confirm($"Move all files from subdirectories to root: {opts.Path}"))
            {
                flatPathManager.Flat(opts.Path, opts.Prefix, opts.UseFolderAsPrefix);
            }
        }
        static void HandleParseError(IEnumerable<Error> errs)
        {
            foreach (var err in errs)
            {
                Console.WriteLine(err);
            }
        }
        static bool Confirm(string msg)
        {
            Console.WriteLine(msg);
            Console.WriteLine("Y/N");
            return Console.ReadKey().Key == ConsoleKey.Y;
        }
    }


    class Options
    {
        [Option('n', "nested", Default = false, HelpText = "Iterate other nested folder of spicefied directory and flatten them")]
        public bool NestedFolders { get; set; }

        [Option('f',"folderPrefix", Default = false, HelpText = "Add folder name to new fileName")]
        public bool UseFolderAsPrefix { get; set; }

        [Option('p', "prefix", HelpText = "Prefix for new file names")]
        public string Prefix { get; set; }

        [Value(0, MetaName = "Path", HelpText = "Path to root folder")]
        public string Path { get; set; }


    }
}