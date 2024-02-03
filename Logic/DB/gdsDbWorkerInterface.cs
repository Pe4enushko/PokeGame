using System.Reflection;
using Godot;
using Poke.Logic.DB;
using Poke.Logic.Player;

public partial class gdsDbWorkerInterface : GodotObject
{
    public long buffer;
    /// <summary>
    /// Executes method from DbWorker
    /// </summary>
    /// <param name="methodName"></param>
    /// <returns></returns>
    public void executeMethod()
    {
        buffer = (long)typeof(DBWorker)
            .GetMethod("gdsCreaturesCount")
            .Invoke(null, null);
        GD.Print($"{buffer} (creaturecount from C#)");
    }
    public void SetPlayerCreature(int id)
    {
        PlayerSingleton.GetPlayer()
            .SetCreature(DBWorker.GetCreature(id));
    }
    public void SetOpponentCreature(int id)
    {
        PlayerSingleton.GetPlayer()
            .OpponentToFightId = id;
    }
}