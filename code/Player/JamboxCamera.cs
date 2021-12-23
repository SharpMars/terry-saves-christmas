using Sandbox;

namespace Jambox;

public class JamboxCamera : Camera
{
	public override void Update()
	{
		var pawn = Local.Pawn;
		if ( pawn == null ) return;

		if ( Input.Down( InputButton.Score ) )
		{
			Position = Vector3.Up * 3800;
			Rotation = Rotation.From( 90f, 0f, 0f );
		}
		else
		{
			Position = pawn.EyePos + Vector3.Up * 300f + Vector3.Backward * 30f;
			Rotation = Rotation.From( 80f, 0f, 0f );
		}
	}
}
