using Hammer;
using Sandbox;

namespace Jambox;

[Library("bounds_marker", Title = "Bounds Marker")]
[Solid, RenderFields]
public class BoundsMarker : ModelEntity
{
	public override void Spawn()
	{
		base.Spawn();

		SetupPhysicsFromModel( PhysicsMotionType.Static );
		EnableAllCollisions = true;
		EnableDrawing = false;
		Tags.Add( "bounds" );
	}
}
