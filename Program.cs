using System;
using Microsoft.Win32;

const string BACKGROUND_PATH = "Software\\Classes\\Directory\\Background\\shell";
const string FOLDER_PATH = "Software\\Classes\\Directory\\shell";
const string FILE_PATH = "Software\\Classes\\{0}\\shell";

string? operation = null;
string? name = null;
string? icon = null;
string? command = null;
string? target = null;

void AddRegistryKey()
{
    var path = BACKGROUND_PATH;
    if (!string.IsNullOrEmpty(target))
    {
        path = target == "b" ? BACKGROUND_PATH : target == "f" ? FOLDER_PATH : string.Format(FILE_PATH, target);
    }
    var registryKey = Registry.CurrentUser.OpenSubKey(path, true);
    var newKey = registryKey.CreateSubKey(name, true);

    if (newKey == null)
    {
        Console.WriteLine($"Error when creating new registry key.");
        return;
    }

    newKey.SetValue(null, name);

    if (icon != null)
    {
        newKey.SetValue("icon", icon);
    }

    if (command != null)
    {
        var commandKey = newKey.CreateSubKey("command", true);
        commandKey.SetValue(null, command);
    }
}

void RemoveRegistryKey()
{
    var path = BACKGROUND_PATH;
    if (!string.IsNullOrEmpty(target))
    {
        path = target == "b" ? BACKGROUND_PATH : target == "f" ? FOLDER_PATH : string.Format(FILE_PATH, target);
    }
    var registryKey = Registry.CurrentUser.OpenSubKey(path, true);
    registryKey.DeleteSubKey(name);
}

for (int i = 0; i < args.Length; i++)
{
    switch (args[i])
    {
        case "add":
            operation = "add";
            break;
        case "remove":
            operation = "remove";
            break;
        case "-i":
            icon = args[++i];
            break;
        case "-c":
            command = args[++i];
            break;
        case "-t":
            target = args[++i];
            break;
        default:
            if (name == null)
            {
                name = args[i];
            }
            else
            {
                Console.WriteLine($"Unknow argument {args[i]}.");
                return;
            }
            break;
    }
}

if (name == null)
{
    Console.WriteLine($"Must indicate a name.");
    return;
}

if (operation == null)
{
    Console.WriteLine($"Must indicate an operation.");
    return;
}

if (operation == "add")
{
    AddRegistryKey();
} else if (operation == "remove")
{
    RemoveRegistryKey();
}
