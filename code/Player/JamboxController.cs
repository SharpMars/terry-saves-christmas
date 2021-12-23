using Sandbox;

namespace Jambox;

public partial class JamboxController : PawnController
{
	public JamboxController() { }
	
	public override void Simulate()
	{
		base.Simulate();

		Rotation = Rotation.RotateAroundAxis( Vector3.Up, Input.Left * 200f * Time.Delta );

		var tr_forward = Trace.Ray( Position, Position + Rotation.Forward * 70f )
			.WithTag( "bounds" )
			.Run();
		
		var tr_left = Trace.Ray( Position, Position + Vector3.Left * Rotation * 90f )
			.WithTag( "bounds" )
			.Run();
		
		var tr_right = Trace.Ray( Position, Position + Vector3.Right * Rotation * 90f )
			.WithTag( "bounds" )
			.Run();
		
		//DebugOverlay.Line( Position, Position + Rotation.Forward * 70f, tr_forward.Hit ? Color.Green : Color.Red );
		//DebugOverlay.Line( Position, Position + Vector3.Left * Rotation * 90f, tr_left.Hit ? Color.Green : Color.Red );
		//DebugOverlay.Line( Position, Position + Vector3.Right * Rotation * 90f, tr_right.Hit ? Color.Green : Color.Red );

		if ( tr_forward.Hit && tr_left.Hit)
		{
			Rotation = Rotation.RotateAroundAxis( Vector3.Up, 460f * Time.Delta );
		}
		else if ( tr_forward.Hit && tr_right.Hit)
		{
			Rotation = Rotation.RotateAroundAxis( Vector3.Up, -460f * Time.Delta );
		}

		var dir = ((JamboxPlayer)Pawn).IsBoosting ? Rotation.Forward * 800 : Rotation.Forward * 300;
		
		// Apply the move
		Position += dir * Time.Delta;

		SetTag( "sitting" );
		Input.Rotation = Rotation;
	}
}
