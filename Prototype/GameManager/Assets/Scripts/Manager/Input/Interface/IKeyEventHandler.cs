using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Manager.Input
{
    public interface IKeyDownHandler : IEventSystemHandler
    {
        void OnKeyDown(KeyEventData eventData);
    }

    public interface IKeyHoldHandler : IEventSystemHandler
    {
        void OnKeyHold(KeyEventData eventData);
    }

    public interface IKeyBeginHoldHandler : IEventSystemHandler
    {
        void OnKeyBeginHold(KeyEventData eventData);
    }

    public interface IKeyEndHoldHandler : IEventSystemHandler
    {
        void OnKeyEndHold(KeyEventData eventData);
    }

    public interface IKeyUpHandler : IEventSystemHandler
    {
        void OnKeyUp(KeyEventData eventData);
    }
}