using UnityEngine;

public class C_DoorOnlyPlayer : C_Door
{
    public override void Start()
    {
        base.Start();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void CloseDoor()
    {
        base.CloseDoor();
    }
    public override void OpenDoor()
    {
        base.OpenDoor();
    }
    public override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_CurOpenTime = m_OpenTime;
        }
    }
}
