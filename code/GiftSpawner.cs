using System;
using Hammer;
using Sandbox;

namespace Jambox;

[Library("gift_spawner", Title = "Gift Spawner"), EditorModel("models/christmas/gift.vmdl")]
[EntityTool( "Gift Spawner", "Jambox", "Spawns poggers gifts" )]
public partial class GiftSpawner : Entity
{
	public ModelEntity Model { get; set; }
	//public float RespawnTime { get { return 10f; } }
	//public float RespawnTimer { get; protected set; }
	[Net] public bool HasGift { get; set; }

	public override void Spawn()
	{
		base.Spawn();
		
		JamboxGamemode.Spawners.Add( this );
		//SpawnGift();
	}

	public void SpawnGift()
	{
		var game = Game.Current as JamboxGamemode;

		game.SpawnedGifts++;
		Model = Create<GiftPickup>();
		Model.Parent = this;
		Model.Position = Position;
	}

	[Event.Tick]
	public void Tick()
	{
		if ( Model != null )
		{
			if ( Model.IsValid )
			{
				HasGift = true;
				var tr = Trace.Box( Model.CollisionBounds, Position, Model.Position).Run();
			
				Model.Position = Position + Vector3.Up * (MathF.Sin( Time.Now * 3f ));
				Model.Rotation = Model.Rotation.RotateAroundAxis( Vector3.Up, 100f * Time.Delta );
				
				var game = Game.Current as JamboxGamemode;
				
				if ( tr.Hit )
				{
					if ( tr.Entity.Tags.Has( "player" ) )
					{
						var player = tr.Entity as JamboxPlayer;
						if ( !player.HasGift )
						{
							player.HasGift = true;
							//RespawnTimer = RespawnTime;
							PlaySound( "gift_pickup" );
							Model.Delete();
							game.SpawnedGifts--;
						}
					}
					if ( tr.Entity.Tags.Has( "sledge" ) )
					{
						var player = tr.Entity.Parent as JamboxPlayer;
						if ( !player.HasGift )
						{
							player.HasGift = true;
							//RespawnTimer = RespawnTime;
							PlaySound( "gift_pickup" );
							Model.Delete();
							game.SpawnedGifts--;
						}
					}
				}
			}
			else
			{
				HasGift = false;
				//RespawnTimer -= Time.Delta;
				//if(RespawnTimer < 0){ SpawnGift(); }
			}
		}
	}
}

public class GiftPickup : ModelEntity
{
	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/christmas/gift.vmdl" );
		var color = Color.Random;
		RenderColor = color;
		GlowColor = color;
		GlowActive = true;
		SetMaterialGroup( Random.Shared.Int( 1, 7 ) );
		Scale = 2f;
	}
}
