using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TokyoChokoku.Communication.CommandGenerateKit
{
	public static class ReadingCommandsDefinitionBuilder
	{
        public static List<CommandElement> Build ()
        {
            var def = new List<CommandElement> ();

            // Alert
            def.Add (new CommandElement (
                title: "ReadAlert",
                arguments: new string [] { },
                address: "Addresses.Alert",
                data: "1",
                returnValueClass: "ResponseShort"
            ));

            // Calendar day
            def.Add (new CommandElement (
                title: "ReadCalendarDayReplacements",
                arguments: new string [] { },
                address: "Addresses.CalendarDayReplacements",
                data: "Calendar.Consts.CharsOfDayReplacements",
                returnValueClass: "ResponseChars"
            ));

            // Calendar month
            def.Add (new CommandElement (
                title: "ReadCalendarMonthReplacements",
                arguments: new string [] { },
                address: "Addresses.CalendarMonthReplacements",
                data: "Calendar.Consts.CharsOfMonthReplacements",
                returnValueClass: "ResponseChars"
            ));

            // Calendar year
            def.Add (new CommandElement (
                title: "ReadCalendarYearReplacements",
                arguments: new string [] { },
                address: "Addresses.CalendarYearReplacements",
                data: "Calendar.Consts.CharsOfYearReplacements",
                returnValueClass: "ResponseChars"
            ));

            // Calendar shift
            def.Add (new CommandElement (
                title: "ReadCalendarShiftReplacements",
                arguments: new string [] { },
                address: "Addresses.CalendarShiftReplacements",
                data: "Calendar.Consts.NumOfShift * Calendar.Consts.WordsOfShift",
                returnValueClass: "ReponseMBCalendarShiftData"
            ));

            // Serial
            def.Add (new CommandElement (
                title: "ReadSerialSettings",
                arguments: new string [] { },
                address: "Addresses.SerialSettings",
                data: "Serial.Consts.NumOfSerial * Serial.Consts.WordsOfSerialSetting",
                returnValueClass: "ReponseMBSerialSettingsData"
            ));

            // Serial
            def.Add (new CommandElement (
                title: "ReadSerialCounters",
                arguments: new string [] { },
                address: "Addresses.SerialCounters",
                data: "Serial.Consts.NumOfSerial * Serial.Consts.WordsOfSerialCounter",
                returnValueClass: "ReponseMBSerialCountersData"
            ));

            // Command survival
            def.Add (new CommandElement (
                title: "ReadCommandSurvival",
                arguments: new string [] { "WritingCommandBuilder builder" },
                address: "new MemoryAddress(builder.DataType, builder.Addr)",
                data: "1",
                returnValueClass: "ResponseRaw"
            ));

            // Num of field in current file
            def.Add (new CommandElement (
                title: "ReadNumOfFieldInCurrentFile",
                arguments: new string [] { },
                address: "Addresses.NumOfFieldOfCurrentFile",
                data: "1",
                returnValueClass: "ResponseShort"
            ));

            // Field of current file
            def.Add (new CommandElement (
                title: "ReadFieldOfCurrentFile",
                arguments: new string [] { "int indexOfField" },
                address: "Addresses.CurrentFile.Increment(indexOfField * MBFile.Consts.NumofFieldWords)",
                data: "(short)MBFile.Consts.NumofFieldWords",
                returnValueClass: "ResponseMBData"
            ));

            // Permanent marking file number
            def.Add (new CommandElement (
                title: "ReadPermanentMarkingFileNo",
                arguments: new string [] { },
                address: "Addresses.PermanentMarkingFileNo",
                data: "1",
                returnValueClass: "ResponseShort"
            ));

            // Remote file map
            def.Add (new CommandElement (
                title: "ReadFlagFileLoadedFromSdCard",
                arguments: new string [] { },
                address: "Addresses.FilesDidLoadFromSdCard",
                data: "1",
                returnValueClass: "ResponseShort"
            ));

            // Remote file map
            def.Add (new CommandElement (
                title: "ReadRemoteFileMap",
                arguments: new string [] { "int fileIndex" },
                address: "Addresses.RemoteFileMaps.Increment(fileIndex)",
                data: "(short)1",
                returnValueClass: "ResponseShort"
            ));

            // Remote file maps
            def.Add (new CommandElement (
                title: "ReadRemoteFileMaps",
                arguments: new string [] { },
                address: "Addresses.RemoteFileMaps",
                data: "(short)Sizes.NumOfRemoteFile",
                returnValueClass: "ResponseShorts"
            ));

            // Remote file names
            def.Add (new CommandElement (
                title: "ReadRemoteFileNames",
                arguments: new string [] { },
                address: "Addresses.RemoteFileNames",
                data: "(short)(Sizes.NumOfRemoteFile * Sizes.BytesOfRemoteFileName)",
                returnValueClass: "ResponseRemoteFileNames"
            ));

            // Machine model number
            def.Add (new CommandElement (
                title: "ReadMachineModelNo",
                arguments: new string [] { },
                address: "Addresses.MachineModelNo",
                data: "1",
                returnValueClass: "ResponseShort"
            ));

            // Marking head pin is at origin
            def.Add (new CommandElement (
                title: "ReadMarkingHeadPinIsAtOrigin",
                arguments: new string [] { },
                address: "Addresses.MarkingHeadPinIsAtOrigin",
                data: "1",
                returnValueClass: "ResponseShort"
            ));

            // Marking status
            def.Add (new CommandElement (
                title: "ReadMarkingStatus",
                arguments: new string [] { },
                address: "Addresses.MarkingStatus",
                data: "1",
                returnValueClass: "ResponseShort"
            ));

            // Marking pausing status
            def.Add (new CommandElement (
                title: "ReadMarkingPausingStatus",
                arguments: new string [] { },
                address: "Addresses.MarkingPausingStatus",
                data: "1",
                returnValueClass: "ResponseShort"
            ));

            // Marking result
            def.Add (new CommandElement (
                title: "ReadMarkingResult",
                arguments: new string [] { },
                address: "Addresses.MarkingResult",
                data: "1",
                returnValueClass: "ResponseShort"
            ));

            // BSD Enabled
            def.Add (new CommandElement (
                title: "ReadBSDEnabled",
                arguments: new string [] { },
                address: "Addresses.BSDEnabled",
                data: "1",
                returnValueClass: "ResponseShort"
            ));

            // Excusion Settings
            def.Add (new CommandElement (
                title: "ReadExclusion",
                arguments: new string [] { },
                address: "Addresses.Exclusion",
                data: "1",
                returnValueClass: "ResponseShort"
            ));

            return def;
        }
	}
}

