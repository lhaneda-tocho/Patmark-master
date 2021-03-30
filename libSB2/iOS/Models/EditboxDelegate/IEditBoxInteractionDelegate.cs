using System;
namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public interface IEditBoxInteractionDelegate
    {
        // interaction identifier, sender
        event Action<string, IEditBoxInteractionDelegate> RequestInteraction;
        void Interact (string interactionId, PropertyEditBoxSource source);
        void ClearListeners ();
    }
}

