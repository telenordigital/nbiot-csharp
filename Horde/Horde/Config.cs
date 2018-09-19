using System;
using System.IO;

namespace Horde
{
    public static class Config
    {
        public const string DefaultFileName = ".horde";
        public const string DefaultAddress = "https://api.nbiot.engineering";

        internal static (string addr, string token) addressTokenFromConfig(string fileName)
        {
            (var addr, var token) = readConfig(fileName);
            return (addr, token);
        }

        static (string addr, string token) readConfig(string fileName)
        {
            var addr = DefaultAddress;
            var token = "";

            var path = Path.Combine(home(), fileName);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Horde config file {path} not found.");
            }

            var lines = File.ReadAllLines(path);
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (line.Length == 0 || line[0] == '#')
                {
                    // ignore comments and empty lines
                    continue;
                }
                var elements = line.Split(new char[] { '=' }, 2);
                if (elements.Length != 2)
                {
                    throw new Exception($"Not a key value expression on line {i} in {path}");
                }
                switch (elements[0])
                {
                    case "address":
                        addr = elements[1];
                        break;
                    case "token":
                        token = elements[1];
                        break;
                    default:
                        throw new Exception($"Unknown keyword on line {i} in {path}");
                }
            }

            return (addr, token);
        }

        static string home()
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix ||
                Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                return Environment.GetEnvironmentVariable("HOME");
            }
            return Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
        }
    }
}
