using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class UIOverlay : MonoBehaviour
{
    public UnityEvent onOpen = new UnityEvent();
    public UnityEvent onClose = new UnityEvent();

    public void Open()
    {
        InputController.Instance.AddInputOverlay(this);
        OpenInternal();
        onOpen.Invoke();
    }
    // actually opens
    protected abstract void OpenInternal();
    public void Close()
    {
        InputController.Instance.RemoveInputOverlay(this);
        CloseInternal();
        onClose.Invoke();
    }
    // actually closes
    protected abstract void CloseInternal();
}
