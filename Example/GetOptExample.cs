using System;

public class GetOptExample
{
    struct CommandLineOptions
    {
        public bool enable;
        public string name;
    }
    static int Main(string[] args)
    {
        CommandLineOptions options = new CommandLineOptions();
        int argc = Util.GetOpt(args, ref options);
        if (argc < 0)
        {
            // Error already logged by GetOpt
            return 1;
        }

        Console.WriteLine("-enable is {0}", options.enable);
        Console.WriteLine("-name   is \"{0}\"", options.name);

        Console.WriteLine("There are {0} non-option arguments", argc);
        for (int i = 0; i < argc; i++)
        {
            if (args[i].IndexOf(' ') >= 0)
            {
                Console.WriteLine("  [{0}] \"{1}\"", i, args[i]);
            }
            else
            {
                Console.WriteLine("  [{0}] {1}", i, args[i]);
            }
        }
        return 0;
    }
}