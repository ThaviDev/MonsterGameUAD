using UnityEngine;

public class DS_Red : DState
{
    public SpriteRenderer _spr;
    public override void OnInitial()
    {
        _spr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Debug.Log("Cambio a Rojo");
        _spr.color = Color.red;
        //base.OnInitial();
    }

    public override DState OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            return _finitedStateMachine._stateBlue;
        }
        return this;
        //return base.OnUpdate();
    }
    public override void OnExit()
    {
        //base.OnExit();
    }
}
