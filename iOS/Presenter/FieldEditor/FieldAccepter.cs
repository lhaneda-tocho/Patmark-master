using System;
namespace TokyoChokoku.Patmark.iOS.Presenter.FieldEditor
{
    public interface FieldAccepter
    {
        void ReceiveFile(FileContext file);
    }
}
