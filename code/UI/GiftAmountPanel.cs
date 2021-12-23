using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Jambox.UI;

public class GiftAmountPanel : Panel
{
	private Image giftImage;
	
	public GiftAmountPanel()
	{
		giftImage = Add.Image( "ui/gift.png", "gift_image" );
		StyleSheet.Load( "UI/GiftAmountPanel.scss" );
	}

	public override void Tick()
	{
		base.Tick();

		var player = Local.Pawn as JamboxPlayer;

		var game = Game.Current as JamboxGamemode;
		if ( game.IsGameOver | !player.HasGift)
		{
			Style.Display = DisplayMode.None;
		}
		else
		{
			Style.Display = DisplayMode.Flex;
		}
	}
}
