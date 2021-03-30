using System;
using TokyoChokoku.SerialModule.Ast;
namespace TokyoChokoku.FieldTextKit
{

    public class FTSerialNode : SerialNode, IFieldTextNode
    {
        public FieldTextType FieldTextType => FieldTextType.Serial;

        public FTSerialNode(int current, int serial) : base(current, serial)
        {
        }
    }
}
