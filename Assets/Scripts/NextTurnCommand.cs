using UnityEngine;
using Fungus;
using InkFungus;

[CommandInfo("Seedling", "Next Turn", "Proceeds to the game's next turn.")]
public class NextTurnCommand: Command
{
    public override Color GetButtonColor()
    {
        return Color.green;
    }

    public override string GetSummary()
    {
        return "Proceed to the game's next turn";
    }

    public override void OnEnter()
    {
        Clock.Instance().NextTurn();

        Continue();
    }
}
