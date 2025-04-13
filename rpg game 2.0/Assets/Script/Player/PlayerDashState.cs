using UnityEditor.Toolbars;
using UnityEngine;

public class PlayerDashState : PlayerState
{

    public PlayerDashState(Player _player, PlayerStateMachine _StateMachine, string _animBoolName) : base(_player, _StateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.dash.CloneOnDash();

        stateTimer = player.dashDuration;

        //player.stats.MakeInvincible(true);

    }

    public override void Exit()
    {
        base.Exit();

        player.skill.dash.CloneOnArrival();

        player.SetVelocity(0, rb.linearVelocity.y);

        //player.stats.MakeInvincible(false);
    }
    
    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);

        if (!player.isGroundDetected() && player.isWallDetected()) stateMachine.ChangeState(player.wallSlide);
        if (stateTimer < 0) stateMachine.ChangeState(player.idleState);

        player.fx.CreateAfterImage();
    }
}
