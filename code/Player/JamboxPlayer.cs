using System;
using Sandbox;

namespace Jambox;

public partial class JamboxPlayer : Player
{
	private readonly Clothing.Container _clothing = new();
	[Net] private ModelEntity _sledge { get; set; }

	#region Boost
	public bool IsBoosting { get; private set; } = false;
	public float BoostTimerAmount => 3;
	public float BoostTimer { get; private set; }
	public float BoostRechargeTimerAmount => 3;
	public float BoostRechargeTimer { get; private set; }
	private bool canBoost { get; set; } = true;
	private bool boost = false;

	#endregion
	
	[Net] public bool HasGift { get; set; } = false;

	private float VignetteIntensity = 0f;
	private float MotionBlurScale = 0f;

	private Particles Fire1;
	private bool FireSpawned = false;

	private Sound boostSound;

	public JamboxPlayer()
	{
		_clothing.Clothing.Add( Resource.FromPath<Clothing>( "clothing/santa_hat.clothing" ) );
		_clothing.Clothing.Add( Resource.FromPath<Clothing>( "clothing/jacket.clothing" ) );
	}

	public override void Respawn()
	{
		Camera = new JamboxCamera();
		Animator = new JamboxAnimator();
		Controller = new JamboxController();

		SetModel( "models/citizen/citizen.vmdl" );
		Tags.Add( "player" );
		
		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;

		_clothing.DressEntity( this );
		_sledge = new ModelEntity( "models/santasledge.vmdl", this );
		_sledge.Tags.Add( "sledge" );

		base.Respawn();
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		var game = Game.Current as JamboxGamemode;

		if ( Input.Pressed( InputButton.Jump ) & IsServer & HasGift & !game.IsGameOver )
		{
			Create<Gift>().Position = Position;
			HasGift = false;
		}

		if ( Input.Pressed( InputButton.Run ) )
		{
			boost = true;
		}

		if ( (int)BoostTimer == 3 )
		{
			canBoost = true;
		}
		else if ( BoostTimer <= 0f )
		{
			canBoost = false;
			boost = false;
		}

		if ( BoostTimer > 0 & canBoost & boost)
		{
			if ( IsClient )
			{
				VignetteIntensity = VignetteIntensity.LerpTo( 0.75f, Time.Delta * 8f );
				MotionBlurScale = MotionBlurScale.LerpTo( .01f, Time.Delta );
				var pp = PostProcess.Get<StandardPostProcess>();
				pp.Vignette.Enabled = true;
				pp.Vignette.Intensity = VignetteIntensity;
				pp.MotionBlur.Enabled = true;
				pp.MotionBlur.Scale = MotionBlurScale;
				if ( !FireSpawned )
				{
					FireSpawned = true;
					Fire1 = Particles.Create( "particles/fire_big.vpcf", this);
					boostSound = Sound.FromEntity( "boost", this );
				}
			}

			IsBoosting = true;
			new Sandbox.ScreenShake.Perlin(.25f);
			BoostTimer -= Time.Delta;
			BoostRechargeTimer = BoostRechargeTimerAmount;
		}
		else
		{
			if ( IsClient )
			{
				var pp = PostProcess.Get<StandardPostProcess>();
				VignetteIntensity = VignetteIntensity.LerpTo( 0f, Time.Delta * 2 );
				MotionBlurScale = MotionBlurScale.LerpTo( 0f, Time.Delta );
				pp.Vignette.Intensity = VignetteIntensity;
				pp.MotionBlur.Scale = MotionBlurScale;
				boostSound.Stop();
				if ( FireSpawned )
				{
					FireSpawned = false;
					Fire1.Destroy(  );
				}
			}
			
			IsBoosting = false;
			BoostRechargeTimer -= Time.Delta;
			if ( BoostRechargeTimer < 0 )
			{
				BoostTimer += Time.Delta * 2;
				BoostTimer = BoostTimer.Clamp( 0, BoostTimerAmount );
			}
		}
	}

	public override void BuildInput( InputBuilder input )
	{
		base.BuildInput( input );

		input.ViewAngles = Angles.Zero;
	}

	public override void OnKilled()
	{
		base.OnKilled();

		EnableDrawing = false;
		_sledge.Delete();
	}
}
