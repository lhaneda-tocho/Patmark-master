using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TokyoChokoku.Communication.CommandGenerateKit
{
	public static class MetaCommands
	{
        public static ReadOnlyCollection<CommandElement> Readings { get; }
        public static ReadOnlyCollection<CommandElement> Writings { get; }

        static MetaCommands()
        {
            Readings = ReadingCommandsDefinitionBuilder.Build().AsReadOnly();
            Writings = WritingCommandsDefinitionBuilder.Build().AsReadOnly();
        }


	}
}

