using System;
using Sandbox;

namespace Jambox;

public class Gift : ModelEntity
{
	public override void Spawn( )
	{
		base.Spawn();
		SetModel( "models/christmas/gift.vmdl" );
		var color = Color.Random;
		RenderColor = color;
		GlowColor = color;
		GlowActive = true;
		SetMaterialGroup( Random.Shared.Int( 1, 7 ) );
	}

	[Event.Tick.Server]
	public void Tick()
	{
		Position += Vector3.Down * 150f * Time.Delta;
		var tr = Trace.Ray( Position, Position + Vector3.Down * 5f )
			.Ignore( Local.Pawn )
			.Run();
		if ( tr.Hit )
		{
			var game = Game.Current as JamboxGamemode;
			if ( tr.Entity.Tags.Has( "delivery" ) )
			{
				var zone = tr.Entity as DeliveryZone;
				zone.Enabled = false;
				game.DeliveredGifts++;
				Particles.Create( "particles/confetti.vpcf", Position );
				PlaySound( "gift_pickup" );
			}
			else
			{
				Particles.Create( "particles/explosion_smoke.vpcf", Position );
				PlaySound( "gift_break" );
			}
			Delete();
		}
	}
}
