using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Jambox.UI;

public class BoostPanel : Panel
{
	public Label BoostLabel;

	public BoostPanel()
	{
		BoostLabel = Add.Label( "Boost: xxx", "boost_label" );
		StyleSheet.Load( "UI/BoostPanel.scss" );
	}

	public override void Tick()
	{
		base.Tick();

		var player = Local.Pawn as JamboxPlayer;

		BoostLabel.Text = $"Boost: {(player.BoostTimer).ToString("##.00")}";
		
		var game = Game.Current as JamboxGamemode;
		if ( game.IsGameOver )
		{
			Style.Display = DisplayMode.None;
		}
	}
}
