using Hammer;
using Sandbox;

namespace Jambox;

[Library("delivery_zone", Title = "Delivery Zone")]
[Solid, RenderFields, Text("delivery_zone")]
public partial class DeliveryZone: ModelEntity
{
	public bool Enabled
	{
		get => _enabled;
		set
		{
			_enabled = value;
			
			var game = Game.Current as JamboxGamemode;
			if ( _enabled ) { game.ZonesActive++; }
			else
			{
				game.ZonesActive--;
				if (!game.IsGameOver)
				{
					game.GameTimer += game.multiplyTime ? 14f : 10f;
				}
			}
		}
	}

	private bool _enabled = false;

	public override void Spawn()
	{
		base.Spawn();

		SetupPhysicsFromModel( PhysicsMotionType.Static );
		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableShadowReceive = false;
		Tags.Add( "delivery" );
		JamboxGamemode.Zones.Add( this );
	}

	[Event.Entity.PostSpawn]
	public void PostSpawn()
	{
		
	}

	[Event.Tick.Server]
	public void Tick()
	{
		RenderColor = Color.Yellow.WithAlpha( .4f );
		EnableDrawing = _enabled;
		EnableAllCollisions = _enabled;
	}
}
