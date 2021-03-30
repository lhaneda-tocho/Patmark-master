using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TokyoChokoku.MarkinBox.Sketchbook.MetaCommunication
{
	public static class WritingCommandsDefinitionBuilder
	{
        public static List<CommandElement> Build()
        {
            var def = new List<CommandElement>();

            // Load field from SDCard
            def.Add(new CommandElement(
                title: "LoadFieldFromSdCard",
                arguments: new string[] { },
                address: "Addresses.CommonOperation",
                data: "(byte)151",
                needsResponse: false,
                delay: 200,
                waitToFinishWriting: true
            ));

            // Load file names from SDCard
            def.Add(new CommandElement(
                title: "LoadFileNamesFromSdCard",
                arguments: new string[] { },
                address: "Addresses.CommonOperation",
                data: "(byte)19",
                needsResponse: false,
                delay: 200,
                waitToFinishWriting: true
            ));

            // Load file map from SDCard
            def.Add(new CommandElement(
                title: "LoadFileMapFromSdCard",
                arguments: new string[] { },
                address: "Addresses.CommonOperation",
                data: "(byte)13",
                needsResponse: false,
                delay: 200,
                waitToFinishWriting: true
            ));

            // Load file map block from SDCard
            //
            // 事前に下記の設定を行ってください。
            // L[100]=ブロック開始アドレス
            // L[101]=6010
            // L[103]=512
            def.Add(new CommandElement(
                title: "LoadFileMapBlockFromSdCard",
                arguments: new string[] { },
                address: "Addresses.CommonOperation",
                data: "(byte)2",
                needsResponse: false,
                delay: 200,
                waitToFinishWriting: true
            ));

            // Load value from SDCard
            def.Add(new CommandElement(
                title: "LoadValueFromSdCard",
                arguments: new string[] { },
                address: "Addresses.CommonOperation",
                data: "(byte)3",
                needsResponse: false,
                delay: 200,
                waitToFinishWriting: true
            ));

            // Move marking head to origin
            def.Add(new CommandElement(
                title: "MoveMarkingHeadToOrigin",
                arguments: new string[] { },
                address: "Addresses.MovingHeadToOrigin",
                data: "new byte[]{0x00, 0x14}",
                delay: 10
            ));

            // Save file name to SDCard
            def.Add(new CommandElement(
                title: "SaveFileNameToSdCard",
                arguments: new string[] { },
                address: "Addresses.CommonOperation",
                data: "(byte)18",
                needsResponse: false,
                delay: 300,
                waitToFinishWriting: true
            ));

            // 
            def.Add(new CommandElement(
                title: "SetFileMapToWorkSpace",
                arguments: new string[] { "int fileIndex", "int numOfField" },
                address: "Addresses.FileMapWorkSpace.Increment(fileIndex % Sizes.SdCard.FileMapBlock.NumOfMapInBlock)",
				data: "(byte)numOfField",
                delay: 10
            ));

            // Save file map to SDCard
            //
            // 事前に下記の設定を行ってください。
            // 
            // C[6010+n]=フィールド数セット
            // L[100]=ブロック開始アドレス
            // L[101]=6010
            // L[103]=512
            def.Add(new CommandElement(
                title: "SaveFileMapBlockToSdCard",
                arguments: new string[] { },
                address: "Addresses.CommonOperation",
                data: "(byte)4",
                needsResponse: false,
                delay: 200,
                waitToFinishWriting: true
            ));

            // Save basic settings to SDCard
            // serial, calendar, marking direction ... 
            def.Add(new CommandElement(
                title: "SaveBasiceSettingsToSdCard",
                arguments: new string[] { },
                address: "Addresses.CommonOperation",
                data: "(byte)10",
                needsResponse: false,
                delay: 200,
                waitToFinishWriting: true
            ));

            // Load serial settings of file from SDCard
            def.Add(new CommandElement(
                title: "SetSerialSettingsFileNo",
                arguments: new string[] { "short no" },
                address: "Addresses.SerialSettingsFileNo",
				data: "BigEndianBitConverter.GetBytes(no)",
                delay: 10
            ));

            // Load serial settings of file from SDCard
            def.Add(new CommandElement(
                title: "LoadSerialSettingsOfFileFromSdCard",
                arguments: new string[] { },
                address: "Addresses.CommonOperation",
                data: "(byte)22",
                needsResponse: false,
                delay: 200,
                waitToFinishWriting: true
            ));

            // Save serial settings of file to SDCard
            def.Add(new CommandElement(
                title: "SaveSerialSettingsOfFileToSdCard",
                arguments: new string[] { },
                address: "Addresses.CommonOperation",
                data: "(byte)23",
                needsResponse: false,
                delay: 200,
                waitToFinishWriting: true
            ));

            // Save value to SDCard
            // Have to set preferences to L[100]〜[103] before.
            def.Add(new CommandElement(
                title: "SaveValueToSdCard",
                arguments: new string[] { },
                address: "Addresses.CommonOperation",
                data: "(byte)20",
                needsResponse: false,
                timeout: 3000,
                delay: 200,
                waitToFinishWriting: true
            ));

            // Set alert
            def.Add(new CommandElement(
                title: "SetAlert",
                arguments: new string[] { "MBMemories.Alert alert" },
                address: "Addresses.Alert",
				data: "BigEndianBitConverter.GetBytes((short)alert)",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetCalendarShiftReplacements",
                arguments: new string[] { "MBCalendarShiftDataBinarizer binarizer" },
                address: "Addresses.NumOfCalendarShiftReplacements",
				data: "binarizer.GetBytes()",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetCalendarYmdReplacements",
                arguments: new string[] { "MBCalendarDataBinarizer binarizer" },
                address: "Addresses.CalendarYmdReplacements",
				data: "binarizer.GetBytes()",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetSerialSettings",
                arguments: new string[] { "MBSerialSettingsDataBinarizer binarizer" },
                address: "Addresses.SerialSettings",
				data: "binarizer.GetBytes()",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetSerialCounters",
                arguments: new string[] { "MBSerialCountersDataBinarizer binarizer" },
                address: "Addresses.SerialCounters",
				data: "binarizer.GetBytes()",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetCurrentMarkingField",
                arguments: new string[] { "int index", "MBData field" },
                address: "Addresses.CurrentFile.Increment(index * MBFile.Consts.NumofFieldWords)",
				data: "new MBDataBinarizer(field).GetBytes()",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetFieldIndexOfRemoteSdCardFile",
                arguments: new string[] { "short fieldIndex" },
                address: "Addresses.FieldIndexOfRemoteSdCardFile",
				data: "BigEndianBitConverter.GetBytes(fieldIndex)",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetFlagFileLoadedFromSdCard",
                arguments: new string[] { "short value" },
                address: "Addresses.FilesDidLoadFromSdCard",
				data: "BigEndianBitConverter.GetBytes(value)",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetFileToSdCardDataExchangeArea",
                arguments: new string[] { "int fieldIndex", "MBData field" },
                address: "Addresses.SdCardDataExchangeArea.Field(fieldIndex)",
				data: "new MBDataBinarizer(field).GetBytes()",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetHeadButtonMarkingAbility",
                arguments: new string[] { "MBMemories.HeadButtonMarkingAbility ability" },
                address: "Addresses.HeadButtonMarkingAbility",
				data: "BigEndianBitConverter.GetBytes((short)ability)",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetMachineModelNo",
                arguments: new string[] { "short number" },
                address: "Addresses.MachineModelNo",
				data: "BigEndianBitConverter.GetBytes(number)",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetMachineModelNoToSdCard",
                arguments: new string[] { },
                address: "Addresses.SettingMachineModelNoToSdCard",
				data: "new byte[]{0x00, 0x01}",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetMarkingMode",
                arguments: new string[] { "MBMemories.MarkingMode mode" },
                address: "Addresses.MarkingMode",
				data: "(byte)mode",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetMarkingPausingStatus",
                arguments: new string[] { "MBMemories.MarkingPausingStatus status" },
                address: "Addresses.MarkingPausingStatus",
				data: "(byte)status",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetMarkingPowerOfCurrentMarkingField",
                arguments: new string[] { "int index", "short power" },
                address: "Addresses.CurrentFile.Increment((index * MBFile.Consts.NumofFieldWords) + Addresses.MarkingPowerIndexInField)",
				data: "BigEndianBitConverter.GetBytes(power)",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetMarkingSpeedOfCurrentMarkingField",
                arguments: new string[] { "int index", "short speed" },
                address: "Addresses.CurrentFile.Increment((index * MBFile.Consts.NumofFieldWords) + Addresses.MarkingSpeedIndexInField)",
				data: "BigEndianBitConverter.GetBytes(speed)",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetMarkingStatus",
                arguments: new string[] { "MBMemories.MarkingStatus status" },
                address: "Addresses.MarkingStatus",
				data: "BigEndianBitConverter.GetBytes((short)status)",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetPermanentMarkingFileNo",
                arguments: new string[] { "short number" },
                address: "Addresses.PermanentMarkingFileNo",
				data: "BigEndianBitConverter.GetBytes(number)",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "ClearPermanentMarkingFileNo",
                arguments: new string[] {},
                address: "Addresses.PermanentMarkingFileNo",
				data: "BigEndianBitConverter.GetBytes((short)0).ToArray()",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetPermanentMarkingFileNoToSdCard",
                arguments: new string[] {},
                address: "Addresses.SettingPermanentMarkingFileNoToSdCard",
				data: "BigEndianBitConverter.GetBytes((short)11)",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetNumOfFieldOfCurrentMarkingFile",
                arguments: new string[] { "short number" },
                address: "Addresses.NumOfFieldOfCurrentFile",
				data: "BigEndianBitConverter.GetBytes(number)",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetNumOfFieldToSdCardDataExchangeArea",
                arguments: new string[] { "int fileIndex", "short numOfField" },
                address: "Addresses.SdCardDataExchangeArea.FileMap.Increment(Sizes.SdCard.FileMapBlock.FileMapIndexInBlock(fileIndex) * Sizes.BytesOfMemoryUnitC)",
				data: "BigEndianBitConverter.GetBytes(numOfField)",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetRemoteFileMap",
                arguments: new string[] { "int fileIndex", "short num" },
                address: "Addresses.RemoteFileMaps.Increment(fileIndex)",
				data: "BigEndianBitConverter.GetBytes(num)",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetRemoteFileName",
                arguments: new string[] { "short fileIndex", "string name" },
                address: "Addresses.RemoteFileNames.Increment(fileIndex * Sizes.BytesOfRemoteFileName)",
				data: "BigEndianBitConverter.BinarizeText(name, TextEncode.Byte1, 16)",
                delay: 10
            ));

            // 
            def.Add(new CommandElement(
                title: "SetRemoteMarkingFileNo",
                arguments: new string[] { "short no" },
                address: "Addresses.RemoteMarkingFileNo",
				data: "BigEndianBitConverter.GetBytes(no)",
                delay: 10
            ));

            // 
            // L[100]SDカード内アドレス
            // L[101]コントローラ上の作業領域アドレス(C[?]) 
            def.Add(new CommandElement(
                title: "SetSdCardDataWritingInfo",
                arguments: new string[] { "int addressOfSdCard", "int addressOfWorkingSpace", "int writingSize" },
                address: "Addresses.SdCardDataWritingInfo",
                data: string.Join("\n", new string[]{
                    "BigEndianBitConverter.GetBytes(addressOfSdCard).ToArray().Concat(",
                    "    BigEndianBitConverter.GetBytes(addressOfWorkingSpace).ToArray()",
                    ").Concat(",
                    "    BigEndianBitConverter.GetBytes((int)0).ToArray()",
                    ").Concat(",
                    "    BigEndianBitConverter.GetBytes(writingSize).ToArray()",
                    ").ToArray()"
				}),
                delay: 10
            ));

            def.Add (new CommandElement (
                title: "SetBarcode1WayMarkingMode",
                arguments: new string [] { "bool is1Way" },
                address: "Addresses.Barcode1WayMarkingMode",
                data: string.Join("\n", new string []{
                    "(byte) ( (is1Way) ? 0x1 : 0x0 )"
				}),
                delay: 10
            ));

            // BSD Enabled
            def.Add (new CommandElement (
                title: "SetBSDEnabled",
                arguments: new string [] { "bool isEnabled" },
                address: "Addresses.BSDEnabled",
                data: string.Join ("\n", new string [] {
                    @"BigEndianBitConverter.GetBytes (",
                    @"    (short)(isEnabled ? 0x1 : 0x0)",
                    @").ToArray ()"
				}),
                delay: 10
            ));

            // Excusion Settings
            def.Add (new CommandElement (
                title: "SetExclusion",
                arguments: new string [] { "bool isEnabled" },
                address: "Addresses.Exclusion",
                data: string.Join ("\n", new string [] {
                    @"BigEndianBitConverter.GetBytes (",
                    @"    (short)(isEnabled ? 0x1 : 0x0)",
                    @").ToArray ()"
				}),
                delay: 10
            ));

            // Set options of pin moving.
            def.Add(new CommandElement(
                title: "SetOptionsOfPinMoving",
                arguments: new string[] { "short xPulse", "short yPulse", "short speed", "short mode = 1", "short interval = 0" },
                address: "Addresses.OptionsOfPinMoving",
                data: string.Join("\n", new string[] {
                    @"BigEndianBitConverter.GetBytes (xPulse)",
                    @".Concat(BigEndianBitConverter.GetBytes (yPulse))",
                    @".Concat(BigEndianBitConverter.GetBytes (speed))",
                    @".Concat(BigEndianBitConverter.GetBytes (mode))",
                    @".Concat(BigEndianBitConverter.GetBytes (interval))",
                    @".ToArray()"
				}),
                delay: 10
            ));

            // Start to move pin
            def.Add(new CommandElement(
                title: "StartToMovePin",
                arguments: new string[] { },
                address: "Addresses.PinIsMoving",
                data: string.Join("\n", new string[] {
                    @"BigEndianBitConverter.GetBytes (",
                    @"    (short)(14)",
                    @").ToArray ()"
				}),
                delay: 10
            ));

            return def;
        }
	}
}

