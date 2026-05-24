using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, GameInput.IPlayerActions, GameInput.IUIActions
{
    GameInput gameInput;

    private void OnEnable()
    {
        if(gameInput == null)
        {
            gameInput = new GameInput();

            gameInput.Player.SetCallbacks(this);
            gameInput.UI.SetCallbacks(this);

            SetGameplay(); 
            // maybe change this to something done in a game manager script 
            // so we can decide if we want to start in menu or gameplay
        }
    }
    void OnDisable()
    {   if(gameInput != null)
        {
            gameInput.Disable();
        }
    }
    void OnDestroy()
    {
        OnDisable();
    }

    // Set Gameplay
    public void SetGameplay()
    {
        gameInput.Player.Enable();
        gameInput.UI.Disable();
    }
    // Set UI
    public void SetUI()
    {
        gameInput.Player.Disable();
        gameInput.UI.Enable();
    }

    public event Action<Vector2> MoveEvent;







    public void OnMove(InputAction.CallbackContext context) // Move 
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }


    ///////// Not Implemented ////////////////////////////////////////////////////////////////////////
    /// 
    public void OnAttack(InputAction.CallbackContext context)
    {
        
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
       //throw new System.NotImplementedException();
    }

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    // template 
    /*
    if (context.phase == InputActionPhase.Performed)
    {
        BlankEvent?.Invoke();
    }
    if (context.phase == InputActionPhase.Canceled)
    {
        BlankEventCanelled?.Invoke();
    }
    */
}
