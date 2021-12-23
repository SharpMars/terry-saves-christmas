using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Jambox.UI;

public class GameOver : Panel
{
	private Label _gameOverLabel;
	private Label _finalTimeLabel;
	private bool Once = false;

	public GameOver()
	{
		_gameOverLabel = Add.Label( "GAMEOVER", "gameover_label" );
		_finalTimeLabel = Add.Label( "Final Time: 123 sec", "finaltime" );
		StyleSheet.Load( "UI/GameOver.scss" );
		Style.Display = DisplayMode.None;
	}

	public override void Tick()
	{
		base.Tick();

		var game = Game.Current as JamboxGamemode;

		if ( game.IsGameOver )
		{
			Style.Display = DisplayMode.Flex;
			var root = FindRootPanel();
			root.Style.BackdropFilterBlur = 16;
			root.Style.Set( "pointer-events", "visible" );

			int seconds = (int) (game.FinalTime % 60f);
			int minutes = (int) (game.FinalTime / 60);

			_finalTimeLabel.Text = $"Final Time: {minutes}:{seconds} minutes";
			
			if ( !Once )
			{
				Sound.FromScreen( "yay" );
				Once = true;
			}
		}
	}
}
