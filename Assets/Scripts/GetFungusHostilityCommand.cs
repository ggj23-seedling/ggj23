using UnityEngine;
using Fungus;
using InkFungus;

[CommandInfo("Seedling", "Get Hostility", "Sync hostility from Ink/Fungus to Enemy AI.")]
public class GetFungusHostilityCommand: Command
{
    public IntegerData hostility;

    public override Color GetButtonColor()
    {
        return Color.green;
    }

    public override string GetSummary()
    {
        return $"Sets the enemy AI's hostility to {hostility.GetDescription()}.";
    }

    public override void OnEnter()
    {
        EnemyAi.Instance().hostility = hostility.Value;

        Continue();
    }
}
