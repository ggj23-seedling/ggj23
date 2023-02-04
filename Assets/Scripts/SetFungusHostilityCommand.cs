using UnityEngine;
using Fungus;
using InkFungus;

[CommandInfo("Seedling", "Set Hostility", "Sync hostility from Enemy AI to Ink/Fungus.")]
public class SetFungusHostilityCommand : Command
{
    [VariableProperty(typeof(IntegerVariable))]
    public IntegerVariable hostility;

    public override Color GetButtonColor()
    {
        return Color.green;
    }

    public override string GetSummary()
    {        
        return $"Sets variable {hostility?.name ?? "(undefined)"} to the Enemy AI's hostility value.";
    }


    public override void OnEnter()
    {
        hostility.Value = EnemyAi.Instance().hostility;

        Continue();
    }
}
