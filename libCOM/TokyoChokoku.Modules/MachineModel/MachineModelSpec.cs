using System;
namespace TokyoChokoku.MachineModel
{
    /// <summary>
    /// SBの機種番号にあたるものです. 主に打刻領域の範囲を決定します.
    /// </summary>
    public abstract class MachineModelSpec
    {
        //public static MachineModelSpec Patmark1515 { get; } = new MachineModelSpec(0, "1515", "Patmark", 15, 15);
        //public static MachineModelSpec Patmark3315 { get; } = new MachineModelSpec(1, "3315", "Patmark", 33, 15);

        //public static List<MachineModelSpec> PatmarkMachineModelSpecList { get; } = new List<MachineModelSpec>() {
        //    Patmark1515,
        //    Patmark3315
        //};

        public string Name   { get; }
        public string Target { get; }
        public ushort Width  { get; }
        public ushort Height { get; }

        protected MachineModelSpec(string name, string target, ushort width, ushort height)
        {
            Name = name ?? throw new NullReferenceException("\"name\" is must not be null.");
            Target = target ?? throw new NullReferenceException("\"target\" is must not be null.");
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}x{2}(name={3})", Target, Width, Height, Name);
        }
    }
}
