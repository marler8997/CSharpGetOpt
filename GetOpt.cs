using System;
using System.Reflection;

public static class Util
{
    public static int GetOpt<T>(string[] args, ref T options) where T : struct
    {
        FieldInfo[] optionFields = typeof(T).GetFields();

        int newArgCount = 0;
        for (int argIndex = 0; argIndex < args.Length; argIndex++)
        {
            string currentArg = args[argIndex];
            if (currentArg.Length == 0 || currentArg[0] != '-')
            {
                args[newArgCount++] = currentArg;
            }
            else
            {
                if (currentArg.Length == 1)
                {
                    Console.WriteLine("Error: found empty '-' without an option name");
                    return -1; // fail
                }
                if (currentArg[1] == '-')
                {
                    Console.WriteLine("Error: double-dash options not supported '{0}'", currentArg);
                    return -1; // fail
                }
                bool foundMatch = false;
                for (int fieldIndex = 0; fieldIndex < optionFields.Length; fieldIndex++)
                {
                    FieldInfo optionField = optionFields[fieldIndex];
                    if (optionField.Name.Length + 1 == currentArg.Length)
                    {
                        foundMatch = true;
                        for (int i = 0; i < optionField.Name.Length; i++)
                        {
                            if (currentArg[i + 1] != optionField.Name[i])
                            {
                                foundMatch = false;
                                break;
                            }
                        }
                        if (foundMatch)
                        {
                            if (optionField.FieldType == typeof(bool))
                            {
                                optionField.SetValueDirect(__makeref(options), true);
                            }
                            else 
                            {
                                argIndex++;
                                if (argIndex >= args.Length)
                                {
                                    Console.WriteLine("Error: option '{0}' is missing an argument", currentArg);
                                    return -1; // fail
                                }
                                String optionArg = args[argIndex];
                                if (optionField.FieldType == typeof(string))
                                {
                                    optionField.SetValueDirect(__makeref(options), optionArg);
                                }
                                else
                                {
                                    Console.WriteLine("Error: option '{0}' has unhandled type '{1}'",
                                        currentArg, optionField.FieldType.Name);
                                }
                            }
                            break;
                        }
                    }
                }
                if (!foundMatch)
                {
                    Console.WriteLine("Error: unknown option '{0}'", currentArg);
                    return -1; // fail
                }
            }
        }
        return newArgCount;
    }
}