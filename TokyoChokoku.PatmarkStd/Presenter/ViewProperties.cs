using System;
using TokyoChokoku.Patmark.RenderKit.Value;
namespace TokyoChokoku.Patmark.Presenter
{
    public interface IViewProperties
    {
        Frame2D Bounds { get; }
        Frame2D Frame  { get; }
    }

}
