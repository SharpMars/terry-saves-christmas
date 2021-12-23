using System;
using System.Collections.Generic;
using System.Linq;
using Jambox.UI;
using Sandbox;

namespace Jambox;

public partial class JamboxGamemode : Game
{
	private JamboxHUD _hud;
	[Net] public static List<DeliveryZone> Zones { get; set; } = new();
	[Net] public static List<GiftSpawner> Spawners { get; set; } = new();
	
	[Net] public uint ZonesActive { get; set; }
	[Net] public uint SpawnedGifts { get; set; }
	[Net] public uint DeliveredGifts { get; set; }

	public bool multiplyTime => DeliveredGifts >= 10;
	private static float EnableTimerAmount => 3;
	private float EnableTimer { get; set; }

	[Net] public float GameTimer { get; set; } = 30f;
	[Net] public float FinalTime { get; private set; }
	
	[Net, Predicted] public bool IsGameOver { get; private set; } = false;
	[Net] private bool PostEntitySpawn { get; set; } = false;
	[Net]private Sound music { get; set; }

	public JamboxGamemode()
	{
		if ( !IsClient )
			return;

		_hud = new JamboxHUD();
		PostProcess.Add(new StandardPostProcess());
	}
	
	[Event.Hotload]
	public void OnHotload()
	{
		if ( !IsClient) return;
		_hud?.Delete();
		_hud = new JamboxHUD();
	}

	public override void ClientJoined( Client cl )
	{
		Precache();
		base.ClientJoined( cl );

		var player = new JamboxPlayer();
		cl.Pawn = player;

		player.Respawn();
	}

	[Event.Entity.PostSpawn]
	public void PostEntity()
	{
		PostEntitySpawn = true;
	}

	[Event.Tick]
	public void Tick()
	{
		
		var multipliedDelta = multiplyTime ? Time.Delta * 1.4f : Time.Delta;

		if ( !PostEntitySpawn )
			return;

		if ( EnableTimer <= 0 )
		{
			var disabledZones = Zones.FindAll(deliveryZone => deliveryZone.Enabled == false );
			var emptySpawners = Spawners.FindAll(a => a.HasGift == false );
			if ( disabledZones.Any() & ZonesActive < 3 )
			{
				Rand.FromList( disabledZones ).Enabled = true;
			}
			if ( emptySpawners.Any() & SpawnedGifts < 2 )
			{
				Rand.FromList( emptySpawners ).SpawnGift();
			}
			EnableTimer = EnableTimerAmount;
		}
		else
		{
			EnableTimer -= Time.Delta;
		}

		if ( GameTimer > 0 )
		{
			GameTimer -= multipliedDelta;
			FinalTime += multipliedDelta;
		}
		else { IsGameOver = true; }
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();
		LoadMusic();
	}

	private async void LoadMusic()
	{
		music.Stop();
		await GameTask.Delay( 1000 );
		music.Stop();
		
		music = Sound.FromScreen( "music" );
		music.SetVolume( 0.4f );
	}

	private void Precache()
	{
		Sandbox.Precache.Add( "particles/confetti.vpcf" );
		Sandbox.Precache.Add( "particles/explosion_smoke.vpcf" );
		Sandbox.Precache.Add( "particles/fire.vpcf" );
		Sandbox.Precache.Add( "particles/fire_big.vpcf" );
		Sandbox.Precache.Add( "models/citizen/citizen.vmdl" );
		Sandbox.Precache.Add( "models/santasledge.vmdl" );
		Sandbox.Precache.Add( "models/christmas/gift.vmdl" );
	}
}
