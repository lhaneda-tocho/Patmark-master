using System;

namespace TokyoChokoku.Communication
{
    public class MemoryAddress
    {
        public CommandDataType DataType;
        public int Address;

        public MemoryAddress(CommandDataType dataType, int address){
            this.DataType = dataType;
            this.Address = address;
        }

        public MemoryAddress Increment(int delta){
            return new MemoryAddress (this.DataType, this.Address + delta);
        }
    }

	public static class Addresses
	{
        private static class AddressFactory
        {
            public static MemoryAddress CreateR(int address){
                return new MemoryAddress (CommandDataType.R, address);
            }

            public static MemoryAddress CreateD(int address){
                return new MemoryAddress (CommandDataType.D, address);
            }

            public static MemoryAddress CreateC(int address){
                return new MemoryAddress (CommandDataType.C, address);
            }

            public static MemoryAddress CreateL(int address){
                return new MemoryAddress (CommandDataType.L, address);
            }

            public static MemoryAddress CreateF(int address){
                return new MemoryAddress (CommandDataType.F, address);
            }
        }

        // 相対位置

        public const int MarkingSpeedIndexInField = 70;
        public const int MarkingPowerIndexInField = 72;

        // C

        public static readonly MemoryAddress MarkingPausingStatus = AddressFactory.CreateC(36);

        public static readonly MemoryAddress MarkingMode = AddressFactory.CreateC(41);

        public static readonly MemoryAddress Barcode1WayMarkingMode = AddressFactory.CreateC (495);

        public static readonly MemoryAddress CalendarYmdReplacements = AddressFactory.CreateC(1840);
        public static readonly MemoryAddress CalendarYearReplacements = AddressFactory.CreateC(1840);
        public static readonly MemoryAddress CalendarMonthReplacements = AddressFactory.CreateC(1850);
        public static readonly MemoryAddress CalendarDayReplacements = AddressFactory.CreateC(1862);

        public static readonly MemoryAddress CommonOperation = AddressFactory.CreateC(6000);

        public static readonly MemoryAddress FileMapWorkSpace = AddressFactory.CreateC(6010);

        public static readonly MemoryAddress RemoteFileNames = AddressFactory.CreateC(14200);

        public static readonly MemoryAddress SavedFiles = AddressFactory.CreateC(20000);

		// D

        public static readonly MemoryAddress Versions  = AddressFactory.CreateD(10);
        public static readonly MemoryAddress Revision  = AddressFactory.CreateD( 9);

        public static readonly MemoryAddress Alert = AddressFactory.CreateD(11);

        public static readonly MemoryAddress MarkingResult = AddressFactory.CreateD(19);

        public static readonly MemoryAddress PinIsMoving = AddressFactory.CreateD(30);

        public static readonly MemoryAddress MarkingStatus = AddressFactory.CreateD(91);

        public static readonly MemoryAddress RemoteMarkingFileNo = AddressFactory.CreateD(94);

        public static readonly MemoryAddress FilesDidLoadFromSdCard = AddressFactory.CreateD(97);

        public static readonly MemoryAddress SettingPermanentMarkingFileNoToSdCard = AddressFactory.CreateD(120);

        public static readonly MemoryAddress MarkingHeadPinIsAtOrigin = AddressFactory.CreateD(130);

        public static readonly MemoryAddress SpeedOfMarkingHeadPinMovingToOrigin = AddressFactory.CreateD(132);

        public static readonly MemoryAddress OptionsOfPinMoving = AddressFactory.CreateD(164);

        public static readonly MemoryAddress Exclusion = AddressFactory.CreateD(530);

        public static readonly MemoryAddress MovingHeadToOrigin = AddressFactory.CreateD(30);

        public static readonly MemoryAddress HeadButtonMarkingAbility = AddressFactory.CreateD(19);

        public static readonly MemoryAddress PermanentMarkingFileNo = AddressFactory.CreateD(98);

        public static readonly MemoryAddress TextValidationStore  = AddressFactory.CreateD(387);

        public static readonly MemoryAddress TextValidationSize   = AddressFactory.CreateD(396);
        
        public static readonly MemoryAddress TextValidationStart  = AddressFactory.CreateD(397);

        public static readonly MemoryAddress TextValidationError  = AddressFactory.CreateD(398);


        // F

        public static readonly MemoryAddress PulseInfo = AddressFactory.CreateF(0);

		// R

        public static readonly MemoryAddress MachineModelNo = AddressFactory.CreateR(29);
        public static readonly MemoryAddress SettingMachineModelNoToSdCard = AddressFactory.CreateR(28);

        public static readonly MemoryAddress NumOfCalendarShiftReplacements = AddressFactory.CreateR(70);
        public static readonly MemoryAddress CalendarShiftReplacements = AddressFactory.CreateR(71);

        public static readonly MemoryAddress RemoteFileMaps = AddressFactory.CreateR(1000);

        public static readonly MemoryAddress SerialSettings = AddressFactory.CreateR(4000);
        public static readonly MemoryAddress SerialCounters = AddressFactory.CreateR(4100);
        public static readonly MemoryAddress SerialRemoteCounters = AddressFactory.CreateR(4200);

        public static readonly MemoryAddress SerialSettingsFileNo = AddressFactory.CreateR(6299);

        public static readonly MemoryAddress BSDEnabled = AddressFactory.CreateR (8020);

        public static readonly MemoryAddress FieldIndexOfRemoteSdCardFile = AddressFactory.CreateR(9998);
        public static readonly MemoryAddress NumOfFieldOfCurrentFile = AddressFactory.CreateR(9999);

        public static readonly MemoryAddress CurrentFile = AddressFactory.CreateR(10000);
       

        // L

        public static readonly MemoryAddress SdCardDataWritingInfo = AddressFactory.CreateL(100);

        // SdCard
        public static class SdCard
        {
            public static class FileBlock
            {
                public const int Start = 4194304;

                public static int StartOfFileMap(int fileIndex)
                {
                    return Start + (Sizes.SdCard.FileBlock.BlockIndex(fileIndex) * Sizes.SdCard.FileBlock.BytesOfBlock);
                }

                public static int StartOfFile(int fileIndex)
                {
                    return
                        (StartOfFileMap(fileIndex)) +
                        (Sizes.SdCard.FileBlock.BytesOfFileMap) +
                        (Sizes.SdCard.FileBlock.FileIndexInBlock(fileIndex) * Sizes.BytesOfFile);
                }

                public static int StartOfField(int fileIndex, int fieldIndex)
                {
                    return
                        (StartOfFile(fileIndex)) +
                        (Sizes.BytesOfField * fieldIndex);
                }
            }

            public static class FileMapBlock
            {
                public const int Start = FileBlock.Start;

                public static int BlockIndexWithMapIndex(int mapIndex)
                {
                    return (int)Math.Floor((decimal)mapIndex / Sizes.SdCard.FileMapBlock.NumOfMapInBlock);
                }

                public static int FileIndexOfCurrentBlock(int mapIndex)
                {
                    return mapIndex % Sizes.SdCard.FileMapBlock.NumOfMapInBlock;
                }

                public static int AddressWithMapIndex(int mapIndex)
                {
                    return
                        (BlockIndexWithMapIndex(mapIndex) * Sizes.SdCard.FileMapBlock.BytesOfBlock) +
                        mapIndex;
                }
            }
        }

        public static class SdCardDataExchangeArea
        {
            public static readonly MemoryAddress FileMap = AddressFactory.CreateC(20000);
            public static readonly MemoryAddress File = FileMap.Increment(Sizes.SdCard.FileBlock.BytesOfFileMap);
            public static MemoryAddress Field(int fieldIndex){
                return File.Increment(fieldIndex * Sizes.BytesOfField);
            }
        }
	}
}

