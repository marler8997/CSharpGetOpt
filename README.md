C# GetOpt
================================================================================
This code is meant to be copy/pasted into your project.  It's inspired by
getopt but redesigned for C#.

### Example

```C#
struct CommandLineOptions
{
    public readonly bool enable;
    public readonly string name;
}
static int Main(string[] args)
{
    CommandLineOptions options;
    int argc = Util.GetOpt(args, out options);
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
```


### Options are Case Sensitive

For now, options are case sensitive.  Justification is to normalize the look of
the command's on the command line which makes it easier to read.  Forcing everyone
to use the same case makes all the examples look the same so it's easier for humans
to read.

This may or may not be good enough justification though, maybe there should be an
option to disable case-sensitivity in the future.

### Performance

Command line parsing is done one time at the beginning of the program.  No
matter how performant the command-line-parsing is, it's almost definitely not
going to make a noticeable difference in performance.

That being said, if there is a time vs memory decision to make, command line
parsing should trade more time for less memory, because parsing only happens
once.  The memory left over from parsing will be put pressure on the garbage
collector, but spending more cycles in the beginning to avoid allocating memory
is a one-time cost that's a drop in the bucket in comparison to adding more
pressure on the garbage collector.