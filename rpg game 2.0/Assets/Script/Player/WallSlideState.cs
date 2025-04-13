using UnityEngine;

public class WallSlideState : PlayerState
{
    public WallSlideState(Player _player, PlayerStateMachine _StateMachine, string _animBoolName) : base(_player, _StateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.isWallDetected() == false)
            stateMachine.ChangeState(player.airState);
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }
        if ( (xInput != 0 && player.facingDir != xInput) || player.isGroundDetected()) stateMachine.ChangeState(player.idleState);

        if(yInput < 0) rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        else rb.linearVelocity = new Vector2(0, rb.linearVelocity.y * .7f);

        
    }
}
