using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace TokyoChokoku.Communication.CommandGenerateKit
{
    /// <summary>
    /// Command Element を Jsonに変換する．
    /// </summary>
    public static class JsonSerializer
    {
        public static string Serialize(CommandElement element)
		{
			return JsonConvert.SerializeObject(element);
		}

		public static CommandElement DeserializeElement(String text)
		{
			var element = JsonConvert.DeserializeObject<CommandElement>(text);
            element.FixAndValidate();
            return element;
		}

		public static List<CommandElement> DeserializeList(String text)
		{
			var elements = JsonConvert.DeserializeObject<List<CommandElement>>(text);
            foreach(var element in elements)
                element.FixAndValidate();
			return elements;
		}

        public static string Serialize(IList<CommandElement> elements)
        {
            return JsonConvert.SerializeObject(elements);
        }
    }
}
