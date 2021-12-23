using Sandbox;
using Sandbox.UI;

namespace Jambox.UI;

public class JamboxHUD : HudEntity<RootPanel>
{
	public JamboxHUD()
	{
		RootPanel.AddChild<GiftAmountPanel>();
		RootPanel.AddChild<BoostPanel>();
		RootPanel.AddChild<GameOver>("gameover");
		RootPanel.AddChild<TimerPanel>("timer_panel");
	}
}
