using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _StateMachine, string _animBoolName) : base(_player, _StateMachine, _animBoolName)
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

        if (player.isWallDetected()) stateMachine.ChangeState(player.wallSlide);

        if (player.isGroundDetected()) stateMachine.ChangeState(player.idleState);

        if (xInput != 0) player.SetVelocity(player.moveSpeed * .8f * xInput, rb.linearVelocity.y);
    }
}
