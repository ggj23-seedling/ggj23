using UnityEngine;
using Fungus;
using InkFungus;

[CommandInfo("Seedling", "Aggressivity", "Sets the enemy AI's aggressivity.")]
public class AggressivityCommand: Command
{
    public IntegerData aggressivity;

    public override Color GetButtonColor()
    {
        return Color.green;
    }

    public override string GetSummary()
    {
        return $"Sets the enemy AI's aggressivity to {aggressivity.GetDescription()}.";
    }

    public override void OnEnter()
    {
        EnemyAi.Instance().SetAggressivity(aggressivity.Value);

        Continue();
    }
}
