using UnityEngine;

public class DS_Blue : DState
{
    public SpriteRenderer _spr;
    public override void OnInitial()
    {
        _spr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Debug.Log("Cambio a azul");
        _spr.color = Color.blue;
        //base.OnInitial();
    }

    public override DState OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            return _finitedStateMachine._stateRed;
        }
        return this;
        //return base.OnUpdate();
    }
    public override void OnExit()
    {
        //base.OnExit();
    }
}
