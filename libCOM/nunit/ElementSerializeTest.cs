using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using static BitConverter.EndianBitConverter;
using static NUnit.Framework.Assert;

using TokyoChokoku.Communication.CommandGenerateKit;

namespace TokyoChokoku.Communication
{
    [TestFixture()]
    public class ElementSerializeTest
    {
        [Test()]
        public void ElementSerialize()
        {
            var def = TestJsonSupplier.Build();
            var first = def.First();
            var text = JsonSerializer.Serialize(first);
            Console.WriteLine(text);
			// TODO: アサーションの定義
		}

		[Test()]
		public void ElementDeserialize()
		{
			var exp = TestJsonSupplier.Object;
            var obj = JsonSerializer.DeserializeElement(TestJsonSupplier.TestJson);
            Console.WriteLine(exp);
            Console.WriteLine(obj);
			True(obj.ContentEquals(exp));
		}

		[Test()]
		public void ListSerialize()
		{
			var def = TestJsonSupplier.Build();
            var text = JsonSerializer.Serialize(def);
            Console.WriteLine(text);
		}


        [Test()]
        public void ListDeserialize()
        {
            var exp = TestJsonSupplier.Build();
            var res = JsonSerializer.DeserializeList(TestJsonSupplier.TestJsonOfList);
            True(exp.Count() == res.Count());
            exp.Zip(res, (e, r)=>
            {
                True(e == r);
                return None.Instance;
            });
        }
    }

    public class TestJsonSupplier
    {

        public static string TestJson = @"{
    ""Title"":""LoadFieldFromSdCard"",
    ""Arguments"":[],
    ""Address"":""Addresses.CommonOperation"",
    ""Data"":""(byte)151"",
    ""ReturnValueClass"":""Response"",
    ""NeedsResponse"":false,
    ""Timeout"":5000,
    ""NumOfRetry"":3,
    ""Delay"":100,
    ""WaitToFinishWriting"":true
}";
        public static CommandElement Object {
            get {
                return new CommandElement(
                    title: "LoadFieldFromSdCard",
                    arguments: new string[] { },
                    address: "Addresses.CommonOperation",
                    data: "(byte)151",
                    timeout: 5000,
                    needsResponse: false,
                    delay: 100,
                    waitToFinishWriting: true
                );
            }
        }

		public static string TestJsonOfList = @"
[{""Title"":""LoadFieldFromSdCard"",""Arguments"":[],""Address"":""Addresses.CommonOperation"",""Data"":""(byte)151"",""ReturnValueClass"":""Response"",""NeedsResponse"":false,""Timeout"":5000,""NumOfRetry"":3,""Delay"":100,""WaitToFinishWriting"":true},{""Title"":""LoadFileNamesFromSdCard"",""Arguments"":[],""Address"":""Addresses.CommonOperation"",""Data"":""(byte)19"",""ReturnValueClass"":""Response"",""NeedsResponse"":false,""Timeout"":5000,""NumOfRetry"":3,""Delay"":150,""WaitToFinishWriting"":true},{""Title"":""LoadFileMapFromSdCard"",""Arguments"":[],""Address"":""Addresses.CommonOperation"",""Data"":""(byte)13"",""ReturnValueClass"":""Response"",""NeedsResponse"":false,""Timeout"":5000,""NumOfRetry"":3,""Delay"":150,""WaitToFinishWriting"":true},{""Title"":""LoadFileMapBlockFromSdCard"",""Arguments"":[],""Address"":""Addresses.CommonOperation"",""Data"":""(byte)2"",""ReturnValueClass"":""Response"",""NeedsResponse"":false,""Timeout"":5000,""NumOfRetry"":3,""Delay"":100,""WaitToFinishWriting"":true},{""Title"":""LoadValueFromSdCard"",""Arguments"":[],""Address"":""Addresses.CommonOperation"",""Data"":""(byte)3"",""ReturnValueClass"":""Response"",""NeedsResponse"":false,""Timeout"":5000,""NumOfRetry"":3,""Delay"":150,""WaitToFinishWriting"":true},{""Title"":""MoveMarkingHeadToOrigin"",""Arguments"":[],""Address"":""Addresses.MovingHeadToOrigin"",""Data"":""new byte[]{0x00, 0x14}"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SaveFileNameToSdCard"",""Arguments"":[],""Address"":""Addresses.CommonOperation"",""Data"":""(byte)18"",""ReturnValueClass"":""Response"",""NeedsResponse"":false,""Timeout"":5000,""NumOfRetry"":3,""Delay"":500,""WaitToFinishWriting"":true},{""Title"":""SetFileMapToWorkSpace"",""Arguments"":[""int fileIndex"",""int numOfField""],""Address"":""Addresses.FileMapWorkSpace.Increment(fileIndex % Sizes.SdCard.FileMapBlock.NumOfMapInBlock)"",""Data"":""(byte)numOfField"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SaveFileMapBlockToSdCard"",""Arguments"":[],""Address"":""Addresses.CommonOperation"",""Data"":""(byte)4"",""ReturnValueClass"":""Response"",""NeedsResponse"":false,""Timeout"":5000,""NumOfRetry"":3,""Delay"":100,""WaitToFinishWriting"":true},{""Title"":""SaveBasiceSettingsToSdCard"",""Arguments"":[],""Address"":""Addresses.CommonOperation"",""Data"":""(byte)10"",""ReturnValueClass"":""Response"",""NeedsResponse"":false,""Timeout"":5000,""NumOfRetry"":3,""Delay"":100,""WaitToFinishWriting"":true},{""Title"":""SetSerialSettingsFileNo"",""Arguments"":[""short no""],""Address"":""Addresses.SerialSettingsFileNo"",""Data"":""BigEndianBitConverter.GetBytes(no)"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""LoadSerialSettingsOfFileFromSdCard"",""Arguments"":[],""Address"":""Addresses.CommonOperation"",""Data"":""(byte)22"",""ReturnValueClass"":""Response"",""NeedsResponse"":false,""Timeout"":10000,""NumOfRetry"":3,""Delay"":100,""WaitToFinishWriting"":true},{""Title"":""SaveSerialSettingsOfFileToSdCard"",""Arguments"":[],""Address"":""Addresses.CommonOperation"",""Data"":""(byte)23"",""ReturnValueClass"":""Response"",""NeedsResponse"":false,""Timeout"":10000,""NumOfRetry"":3,""Delay"":100,""WaitToFinishWriting"":true},{""Title"":""SaveValueToSdCard"",""Arguments"":[],""Address"":""Addresses.CommonOperation"",""Data"":""(byte)20"",""ReturnValueClass"":""Response"",""NeedsResponse"":false,""Timeout"":10000,""NumOfRetry"":3,""Delay"":100,""WaitToFinishWriting"":true},{""Title"":""SetAlert"",""Arguments"":[""MBMemories.Alert alert""],""Address"":""Addresses.Alert"",""Data"":""BigEndianBitConverter.GetBytes((short)alert)"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetCalendarShiftReplacements"",""Arguments"":[""MBCalendarShiftDataBinarizer binarizer""],""Address"":""Addresses.NumOfCalendarShiftReplacements"",""Data"":""binarizer.GetBytes()"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetCalendarYmdReplacements"",""Arguments"":[""MBCalendarDataBinarizer binarizer""],""Address"":""Addresses.CalendarYmdReplacements"",""Data"":""binarizer.GetBytes()"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetSerialSettings"",""Arguments"":[""MBSerialSettingsDataBinarizer binarizer""],""Address"":""Addresses.SerialSettings"",""Data"":""binarizer.GetBytes()"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetSerialCounters"",""Arguments"":[""MBSerialCountersDataBinarizer binarizer""],""Address"":""Addresses.SerialCounters"",""Data"":""binarizer.GetBytes()"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetCurrentMarkingField"",""Arguments"":[""int index"",""MBData field""],""Address"":""Addresses.CurrentFile.Increment(index * MBFile.Consts.NumofFieldWords)"",""Data"":""new MBDataBinarizer(field).GetBytes()"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetFieldIndexOfRemoteSdCardFile"",""Arguments"":[""short fieldIndex""],""Address"":""Addresses.FieldIndexOfRemoteSdCardFile"",""Data"":""BigEndianBitConverter.GetBytes(fieldIndex)"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetFlagFileLoadedFromSdCard"",""Arguments"":[""short value""],""Address"":""Addresses.FilesDidLoadFromSdCard"",""Data"":""BigEndianBitConverter.GetBytes(value)"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetFileToSdCardDataExchangeArea"",""Arguments"":[""int fieldIndex"",""MBData field""],""Address"":""Addresses.SdCardDataExchangeArea.Field(fieldIndex)"",""Data"":""new MBDataBinarizer(field).GetBytes()"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetHeadButtonMarkingAbility"",""Arguments"":[""MBMemories.HeadButtonMarkingAbility ability""],""Address"":""Addresses.HeadButtonMarkingAbility"",""Data"":""BigEndianBitConverter.GetBytes((short)ability)"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetMachineModelNo"",""Arguments"":[""short number""],""Address"":""Addresses.MachineModelNo"",""Data"":""BigEndianBitConverter.GetBytes(number)"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetMachineModelNoToSdCard"",""Arguments"":[],""Address"":""Addresses.SettingMachineModelNoToSdCard"",""Data"":""new byte[]{0x00, 0x01}"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetMarkingMode"",""Arguments"":[""MBMemories.MarkingMode mode""],""Address"":""Addresses.MarkingMode"",""Data"":""(byte)mode"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetMarkingPausingStatus"",""Arguments"":[""MBMemories.MarkingPausingStatus status""],""Address"":""Addresses.MarkingPausingStatus"",""Data"":""(byte)status"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetMarkingPowerOfCurrentMarkingField"",""Arguments"":[""int index"",""short power""],""Address"":""Addresses.CurrentFile.Increment((index * MBFile.Consts.NumofFieldWords) + Addresses.MarkingPowerIndexInField)"",""Data"":""BigEndianBitConverter.GetBytes(power)"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetMarkingSpeedOfCurrentMarkingField"",""Arguments"":[""int index"",""short speed""],""Address"":""Addresses.CurrentFile.Increment((index * MBFile.Consts.NumofFieldWords) + Addresses.MarkingSpeedIndexInField)"",""Data"":""BigEndianBitConverter.GetBytes(speed)"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetMarkingStatus"",""Arguments"":[""MBMemories.MarkingStatus status""],""Address"":""Addresses.MarkingStatus"",""Data"":""BigEndianBitConverter.GetBytes((short)status)"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetPermanentMarkingFileNo"",""Arguments"":[""short number""],""Address"":""Addresses.PermanentMarkingFileNo"",""Data"":""BigEndianBitConverter.GetBytes(number)"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""ClearPermanentMarkingFileNo"",""Arguments"":[],""Address"":""Addresses.PermanentMarkingFileNo"",""Data"":""BigEndianBitConverter.GetBytes((short)0).ToArray()"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetPermanentMarkingFileNoToSdCard"",""Arguments"":[],""Address"":""Addresses.SettingPermanentMarkingFileNoToSdCard"",""Data"":""BigEndianBitConverter.GetBytes((short)11)"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetNumOfFieldOfCurrentMarkingFile"",""Arguments"":[""short number""],""Address"":""Addresses.NumOfFieldOfCurrentFile"",""Data"":""BigEndianBitConverter.GetBytes(number)"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetNumOfFieldToSdCardDataExchangeArea"",""Arguments"":[""int fileIndex"",""short numOfField""],""Address"":""Addresses.SdCardDataExchangeArea.FileMap.Increment(Sizes.SdCard.FileMapBlock.FileMapIndexInBlock(fileIndex) * Sizes.BytesOfMemoryUnitC)"",""Data"":""BigEndianBitConverter.GetBytes(numOfField)"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetRemoteFileMap"",""Arguments"":[""int fileIndex"",""short num""],""Address"":""Addresses.RemoteFileMaps.Increment(fileIndex)"",""Data"":""BigEndianBitConverter.GetBytes(num)"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetRemoteFileName"",""Arguments"":[""short fileIndex"",""string name""],""Address"":""Addresses.RemoteFileNames.Increment(fileIndex * Sizes.BytesOfRemoteFileName)"",""Data"":""BigEndianBitConverter.BinarizeText(name, TextEncode.Byte1, 16)"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetRemoteMarkingFileNo"",""Arguments"":[""short no""],""Address"":""Addresses.RemoteMarkingFileNo"",""Data"":""BigEndianBitConverter.GetBytes(no)"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetSdCardDataWritingInfo"",""Arguments"":[""int addressOfSdCard"",""int addressOfWorkingSpace"",""int writingSize""],""Address"":""Addresses.SdCardDataWritingInfo"",""Data"":""BigEndianBitConverter.GetBytes(addressOfSdCard).ToArray().Concat(\n    BigEndianBitConverter.GetBytes(addressOfWorkingSpace).ToArray()\n).Concat(\n    BigEndianBitConverter.GetBytes((int)0).ToArray()\n).Concat(\n    BigEndianBitConverter.GetBytes(writingSize).ToArray()\n).ToArray()"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetBarcode1WayMarkingMode"",""Arguments"":[""bool is1Way""],""Address"":""Addresses.Barcode1WayMarkingMode"",""Data"":""(byte) ( (is1Way) ? 0x1 : 0x0 )"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetBSDEnabled"",""Arguments"":[""bool isEnabled""],""Address"":""Addresses.BSDEnabled"",""Data"":""BigEndianBitConverter.GetBytes (\n    (short)(isEnabled ? 0x1 : 0x0)\n).ToArray ()"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetExclusion"",""Arguments"":[""bool isEnabled""],""Address"":""Addresses.Exclusion"",""Data"":""BigEndianBitConverter.GetBytes (\n    (short)(isEnabled ? 0x1 : 0x0)\n).ToArray ()"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""SetOptionsOfPinMoving"",""Arguments"":[""short xPulse"",""short yPulse"",""short speed"",""short mode = 1"",""short interval = 0""],""Address"":""Addresses.OptionsOfPinMoving"",""Data"":""BigEndianBitConverter.GetBytes (xPulse)\n.Concat(BigEndianBitConverter.GetBytes (yPulse))\n.Concat(BigEndianBitConverter.GetBytes (speed))\n.Concat(BigEndianBitConverter.GetBytes (mode))\n.Concat(BigEndianBitConverter.GetBytes (interval))\n.ToArray()"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false},{""Title"":""StartToMovePin"",""Arguments"":[],""Address"":""Addresses.PinIsMoving"",""Data"":""BigEndianBitConverter.GetBytes (\n    (short)(14)\n).ToArray ()"",""ReturnValueClass"":""Response"",""NeedsResponse"":true,""Timeout"":5000,""NumOfRetry"":3,""Delay"":0,""WaitToFinishWriting"":false}]
";

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
				delay: 100,
				waitToFinishWriting: true
			));

			// Load file names from SDCard
			def.Add(new CommandElement(
				title: "LoadFileNamesFromSdCard",
				arguments: new string[] { },
				address: "Addresses.CommonOperation",
				data: "(byte)19",
				needsResponse: false,
				delay: 150,
				waitToFinishWriting: true
			));

			// Load file map from SDCard
			def.Add(new CommandElement(
				title: "LoadFileMapFromSdCard",
				arguments: new string[] { },
				address: "Addresses.CommonOperation",
				data: "(byte)13",
				needsResponse: false,
				delay: 150,
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
				delay: 100,
				waitToFinishWriting: true
			));

			// Load value from SDCard
			def.Add(new CommandElement(
				title: "LoadValueFromSdCard",
				arguments: new string[] { },
				address: "Addresses.CommonOperation",
				data: "(byte)3",
				needsResponse: false,
				delay: 150,
				waitToFinishWriting: true
			));

			// Move marking head to origin
			def.Add(new CommandElement(
				title: "MoveMarkingHeadToOrigin",
				arguments: new string[] { },
				address: "Addresses.MovingHeadToOrigin",
				data: "new byte[]{0x00, 0x14}"
			));

			// Save file name to SDCard
			def.Add(new CommandElement(
				title: "SaveFileNameToSdCard",
				arguments: new string[] { },
				address: "Addresses.CommonOperation",
				data: "(byte)18",
				needsResponse: false,
				delay: 500,
				waitToFinishWriting: true
			));

			// 
			def.Add(new CommandElement(
				title: "SetFileMapToWorkSpace",
				arguments: new string[] { "int fileIndex", "int numOfField" },
				address: "Addresses.FileMapWorkSpace.Increment(fileIndex % Sizes.SdCard.FileMapBlock.NumOfMapInBlock)",
				data: "(byte)numOfField"
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
				delay: 100,
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
				delay: 100,
				waitToFinishWriting: true
			));

			// Load serial settings of file from SDCard
			def.Add(new CommandElement(
				title: "SetSerialSettingsFileNo",
				arguments: new string[] { "short no" },
				address: "Addresses.SerialSettingsFileNo",
				data: "BigEndianBitConverter.GetBytes(no)"
			));

			// Load serial settings of file from SDCard
			def.Add(new CommandElement(
				title: "LoadSerialSettingsOfFileFromSdCard",
				arguments: new string[] { },
				address: "Addresses.CommonOperation",
				data: "(byte)22",
				needsResponse: false,
				timeout: 10000,
				delay: 100,
				waitToFinishWriting: true
			));

			// Save serial settings of file to SDCard
			def.Add(new CommandElement(
				title: "SaveSerialSettingsOfFileToSdCard",
				arguments: new string[] { },
				address: "Addresses.CommonOperation",
				data: "(byte)23",
				needsResponse: false,
				timeout: 10000,
				delay: 100,
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
				timeout: 10000,
				delay: 100,
				waitToFinishWriting: true
			));

			// Set alert
			def.Add(new CommandElement(
				title: "SetAlert",
				arguments: new string[] { "MBMemories.Alert alert" },
				address: "Addresses.Alert",
				data: "BigEndianBitConverter.GetBytes((short)alert)"
			));

			// 
			def.Add(new CommandElement(
				title: "SetCalendarShiftReplacements",
				arguments: new string[] { "MBCalendarShiftDataBinarizer binarizer" },
				address: "Addresses.NumOfCalendarShiftReplacements",
				data: "binarizer.GetBytes()"
			));

			// 
			def.Add(new CommandElement(
				title: "SetCalendarYmdReplacements",
				arguments: new string[] { "MBCalendarDataBinarizer binarizer" },
				address: "Addresses.CalendarYmdReplacements",
				data: "binarizer.GetBytes()"
			));

			// 
			def.Add(new CommandElement(
				title: "SetSerialSettings",
				arguments: new string[] { "MBSerialSettingsDataBinarizer binarizer" },
				address: "Addresses.SerialSettings",
				data: "binarizer.GetBytes()"
			));

			// 
			def.Add(new CommandElement(
				title: "SetSerialCounters",
				arguments: new string[] { "MBSerialCountersDataBinarizer binarizer" },
				address: "Addresses.SerialCounters",
				data: "binarizer.GetBytes()"
			));

			// 
			def.Add(new CommandElement(
				title: "SetCurrentMarkingField",
				arguments: new string[] { "int index", "MBData field" },
				address: "Addresses.CurrentFile.Increment(index * MBFile.Consts.NumofFieldWords)",
				data: "new MBDataBinarizer(field).GetBytes()"
			));

			// 
			def.Add(new CommandElement(
				title: "SetFieldIndexOfRemoteSdCardFile",
				arguments: new string[] { "short fieldIndex" },
				address: "Addresses.FieldIndexOfRemoteSdCardFile",
				data: "BigEndianBitConverter.GetBytes(fieldIndex)"
			));

			// 
			def.Add(new CommandElement(
				title: "SetFlagFileLoadedFromSdCard",
				arguments: new string[] { "short value" },
				address: "Addresses.FilesDidLoadFromSdCard",
				data: "BigEndianBitConverter.GetBytes(value)"
			));

			// 
			def.Add(new CommandElement(
				title: "SetFileToSdCardDataExchangeArea",
				arguments: new string[] { "int fieldIndex", "MBData field" },
				address: "Addresses.SdCardDataExchangeArea.Field(fieldIndex)",
				data: "new MBDataBinarizer(field).GetBytes()"
			));

			// 
			def.Add(new CommandElement(
				title: "SetHeadButtonMarkingAbility",
				arguments: new string[] { "MBMemories.HeadButtonMarkingAbility ability" },
				address: "Addresses.HeadButtonMarkingAbility",
				data: "BigEndianBitConverter.GetBytes((short)ability)"
			));

			// 
			def.Add(new CommandElement(
				title: "SetMachineModelNo",
				arguments: new string[] { "short number" },
				address: "Addresses.MachineModelNo",
				data: "BigEndianBitConverter.GetBytes(number)"
			));

			// 
			def.Add(new CommandElement(
				title: "SetMachineModelNoToSdCard",
				arguments: new string[] { },
				address: "Addresses.SettingMachineModelNoToSdCard",
				data: "new byte[]{0x00, 0x01}"
			));

			// 
			def.Add(new CommandElement(
				title: "SetMarkingMode",
				arguments: new string[] { "MBMemories.MarkingMode mode" },
				address: "Addresses.MarkingMode",
				data: "(byte)mode"
			));

			// 
			def.Add(new CommandElement(
				title: "SetMarkingPausingStatus",
				arguments: new string[] { "MBMemories.MarkingPausingStatus status" },
				address: "Addresses.MarkingPausingStatus",
				data: "(byte)status"
			));

			// 
			def.Add(new CommandElement(
				title: "SetMarkingPowerOfCurrentMarkingField",
				arguments: new string[] { "int index", "short power" },
				address: "Addresses.CurrentFile.Increment((index * MBFile.Consts.NumofFieldWords) + Addresses.MarkingPowerIndexInField)",
				data: "BigEndianBitConverter.GetBytes(power)"
			));

			// 
			def.Add(new CommandElement(
				title: "SetMarkingSpeedOfCurrentMarkingField",
				arguments: new string[] { "int index", "short speed" },
				address: "Addresses.CurrentFile.Increment((index * MBFile.Consts.NumofFieldWords) + Addresses.MarkingSpeedIndexInField)",
				data: "BigEndianBitConverter.GetBytes(speed)"
			));

			// 
			def.Add(new CommandElement(
				title: "SetMarkingStatus",
				arguments: new string[] { "MBMemories.MarkingStatus status" },
				address: "Addresses.MarkingStatus",
				data: "BigEndianBitConverter.GetBytes((short)status)"
			));

			// 
			def.Add(new CommandElement(
				title: "SetPermanentMarkingFileNo",
				arguments: new string[] { "short number" },
				address: "Addresses.PermanentMarkingFileNo",
				data: "BigEndianBitConverter.GetBytes(number)"
			));

			// 
			def.Add(new CommandElement(
				title: "ClearPermanentMarkingFileNo",
				arguments: new string[] { },
				address: "Addresses.PermanentMarkingFileNo",
				data: "BigEndianBitConverter.GetBytes((short)0).ToArray()" // clear D[98]
			));

			// 
			def.Add(new CommandElement(
				title: "SetPermanentMarkingFileNoToSdCard",
				arguments: new string[] { },
				address: "Addresses.SettingPermanentMarkingFileNoToSdCard",
				data: "BigEndianBitConverter.GetBytes((short)11)"
			));

			// 
			def.Add(new CommandElement(
				title: "SetNumOfFieldOfCurrentMarkingFile",
				arguments: new string[] { "short number" },
				address: "Addresses.NumOfFieldOfCurrentFile",
				data: "BigEndianBitConverter.GetBytes(number)"
			));

			// 
			def.Add(new CommandElement(
				title: "SetNumOfFieldToSdCardDataExchangeArea",
				arguments: new string[] { "int fileIndex", "short numOfField" },
				address: "Addresses.SdCardDataExchangeArea.FileMap.Increment(Sizes.SdCard.FileMapBlock.FileMapIndexInBlock(fileIndex) * Sizes.BytesOfMemoryUnitC)",
				data: "BigEndianBitConverter.GetBytes(numOfField)"
			));

			// 
			def.Add(new CommandElement(
				title: "SetRemoteFileMap",
				arguments: new string[] { "int fileIndex", "short num" },
				address: "Addresses.RemoteFileMaps.Increment(fileIndex)",
				data: "BigEndianBitConverter.GetBytes(num)"
			));

			// 
			def.Add(new CommandElement(
				title: "SetRemoteFileName",
				arguments: new string[] { "short fileIndex", "string name" },
				address: "Addresses.RemoteFileNames.Increment(fileIndex * Sizes.BytesOfRemoteFileName)",
				data: "BigEndianBitConverter.BinarizeText(name, TextEncode.Byte1, 16)"
			));

			// 
			def.Add(new CommandElement(
				title: "SetRemoteMarkingFileNo",
				arguments: new string[] { "short no" },
				address: "Addresses.RemoteMarkingFileNo",
				data: "BigEndianBitConverter.GetBytes(no)"
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
				})
			));

			def.Add(new CommandElement(
				title: "SetBarcode1WayMarkingMode",
				arguments: new string[] { "bool is1Way" },
				address: "Addresses.Barcode1WayMarkingMode",
				data: string.Join("\n", new string[]{
					"(byte) ( (is1Way) ? 0x1 : 0x0 )"
				})
			));

			// BSD Enabled
			def.Add(new CommandElement(
				title: "SetBSDEnabled",
				arguments: new string[] { "bool isEnabled" },
				address: "Addresses.BSDEnabled",
				data: string.Join("\n", new string[] {
					@"BigEndianBitConverter.GetBytes (",
					@"    (short)(isEnabled ? 0x1 : 0x0)",
					@").ToArray ()"
				})
			));

			// Excusion Settings
			def.Add(new CommandElement(
				title: "SetExclusion",
				arguments: new string[] { "bool isEnabled" },
				address: "Addresses.Exclusion",
				data: string.Join("\n", new string[] {
					@"BigEndianBitConverter.GetBytes (",
					@"    (short)(isEnabled ? 0x1 : 0x0)",
					@").ToArray ()"
				})
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
				})
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
				})
			));

			return def;
		}
    }


}
