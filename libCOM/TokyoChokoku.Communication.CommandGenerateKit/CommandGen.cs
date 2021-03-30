using System;
using TokyoChokoku.Communication;
using System.Collections.Generic;
using System.IO;
namespace TokyoChokoku.Communication.CommandGenerateKit
{
    public static class CommandGen
    {
        public static string GenArguments(this CommandElement cmd)
		{
            var args = string.Join(", ", cmd.Arguments);
            if (cmd.Arguments.Length == 0)
                return "bool enableBeforeExcluding";
            else
                return args + ", bool enableBeforeExcluding";
        }
    }

	public static class CommandLoad
	{
        public static IList<CommandElement> File(string file)
        {
            using (StreamReader sr = new StreamReader(new FileStream(file, FileMode.Open)))
            {
                var text = sr.ReadToEnd();
                return JsonSerializer.DeserializeList(text);
            }
        }
	}

}
