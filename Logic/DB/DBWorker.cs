using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using static System.Environment;

namespace Poke.Logic.DB
{
    public static class DBWorker
    {
        //SQLiteConnection Connection = new SQLiteConnection($"Data Source={GetFolderPath(SpecialFolder.MyDocuments)}/uwu/Poke.db");
        static SQLiteConnection Connection = new SQLiteConnection($"Data Source=Poke.db");
        public static int CreaturesCount {
        get
        {
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Creatures";
            return (int)cmd.ExecuteScalar();
        }}

        public static List<Attack> GetAttacks(int creatureId)
        {
            if(Connection.State == System.Data.ConnectionState.Closed)
                Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Creature_Attack WHERE Creature_Id = " + creatureId;
            System.Collections.Generic.List<int> AtkIds = new System.Collections.Generic.List<int>();
            var reader2 = cmd.ExecuteReader();
            while(reader2.Read())
            {
                AtkIds.Add(int.Parse(reader2.GetValue(2).ToString()));
            }
            reader2.Close();

            List<Attack> Attacks = new List<Attack>();
            string ids = "";
            AtkIds.ForEach((e) => ids += $"{e},");
            cmd.CommandText = $"SELECT * FROM Attacks WHERE Id IN({ids.Substring(0,ids.Length -1)})";
            var reader3 = cmd.ExecuteReader();
            object[] atkValues = new object[6];
            while(reader3.Read())
            {
                reader3.GetValues(atkValues);
                var buffcmd = Connection.CreateCommand();
                buffcmd.CommandText = $"SELECT * FROM Buffs WHERE Attack_Id = {atkValues[0]}";
                var buffreader = buffcmd.ExecuteReader();
                List<Buff> buffs = new List<Buff>();
                while(buffreader.Read())
                {
                    object[] buffObj = new object[5];
                    buffreader.GetValues(buffObj);
                    buffs.Add(new Buff(
                        int.Parse(buffObj[2].ToString()),
                        buffObj[1].ToString(),
                        int.Parse(buffObj[3].ToString()),
                        (Targets)int.Parse(buffObj[4].ToString())));
                }
                buffreader.Close();
                Attacks.Add(new Attack(atkValues[5].ToString(),
                                    float.Parse(atkValues[1].ToString()),
                                    int.Parse(atkValues[3].ToString()),
                                    int.Parse(atkValues[2].ToString()),
                                    int.Parse(atkValues[4].ToString()),
                                    buffs.ToArray()
                                    ));
            }
            reader3.Close();
            if(Connection.State == System.Data.ConnectionState.Open)
                Connection.Close();
            return Attacks;
        }
        
        public static Creature GetCreature(int id)
        {
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Creatures WHERE Id = " +  id;
            Connection.Open();
            var reader = cmd.ExecuteReader();
            reader.Read();
            object[] values = new object[10];

            reader.GetValues(values);
            reader.Close();

            var Attacks = GetAttacks(id);

            var Result = new Creature(int.Parse(values[0].ToString()),
                                int.Parse(values[1].ToString()),
                                int.Parse(values[2].ToString()),
                                int.Parse(values[3].ToString()),
                                int.Parse(values[4].ToString()),
                                int.Parse(values[5].ToString()),
                                int.Parse(values[6].ToString()),
                                int.Parse(values[7].ToString()),
                                Attacks,
                                values[8].ToString(),
                                values[9].ToString()
                                );
            return Result;
        }

        public static List<Creature> GetAllCreaturesWithoutAttacks()
        {
            var Result = new List<Creature>();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Creatures;";
            Connection.Open();
            var reader = cmd.ExecuteReader();
            object[] values = new object[10];
            while(reader.Read())
            {
                reader.GetValues(values);

                var cr = new Creature(int.Parse(values[0].ToString()),
                                    int.Parse(values[1].ToString()),
                                    int.Parse(values[2].ToString()),
                                    int.Parse(values[3].ToString()),
                                    int.Parse(values[4].ToString()),
                                    int.Parse(values[5].ToString()),
                                    int.Parse(values[6].ToString()),
                                    int.Parse(values[7].ToString()),
                                    null,
                                    values[8].ToString(),
                                    values[9].ToString()
                                    );
                Result.Add(cr);
            }
            reader.Close();
            Connection.Close();
            return Result;
        }
    }
}