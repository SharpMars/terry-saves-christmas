using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Jambox.UI;

public class TimerPanel : Panel
{
	private Label _label;

	public TimerPanel()
	{
		_label = Add.Label("0:00", "timer_label");
		StyleSheet.Load( "UI/TimerPanel.scss" );
	}

	public override void Tick()
	{
		base.Tick();

		var game = Game.Current as JamboxGamemode;

		int seconds = (int) (game.GameTimer % 60f);
		int minutes = (int) (game.GameTimer / 60);

		_label.Text = $"{minutes}:{seconds}";
		
		if ( game.IsGameOver )
		{
			Style.Display = DisplayMode.None;
		}
	}
}
