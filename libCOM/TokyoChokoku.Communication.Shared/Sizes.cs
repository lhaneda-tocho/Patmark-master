using System;

namespace TokyoChokoku.Communication
{
    public static class Sizes
    {
        static Sizes ()
        {
        }

        public const int BytesOfMemoryUnitC = 2;

        // ファイル

        public const int MaxNumOfFieldInFile = 51;
        public const int BytesOfField = 176; 
        public const int BytesOfFile = BytesOfField * MaxNumOfFieldInFile;

        public const int MaxReadingBytes = 88;
        public const int MaxWritingDataSize = 44;

        // リモートファイル

        public const int NumOfRemoteFile = 255;
        public const int IndexOfRemotePermanentFile = 255;
        public const int BytesOfRemoteFileName = 16;
        public const int CharsOfRemoteFileName = BytesOfRemoteFileName / 2;

        // SdCard
        public static class SdCard
        {
            // 転送バイト数(MB1の縛りにより、このサイズでしかデータを転送できない)
            public const int BytesOfTransferUnit = 512;

            public static class FileBlock
            {
                public const int NumOfFileInBlock = 24;
                public const int BytesOfBlock = 262144;
                public const int BytesOfFileMap = 512;

                public static int BlockIndex(int fileIndex)
                {
                    return (int)Math.Floor((decimal)(fileIndex / NumOfFileInBlock));
                }

                public static int FileIndexInBlock(int fileIndex)
                {
                    return fileIndex % NumOfFileInBlock;
                }
            }

            public static class FileMapBlock
            {
                public const int NumOfMapInBlock = FileBlock.NumOfFileInBlock;
                public const int BytesOfBlock = FileBlock.BytesOfBlock;

                public static int FileMapIndexInBlock(int fileIndex)
                {
                    return fileIndex % NumOfMapInBlock;
                }
            }
        }
    }
}

