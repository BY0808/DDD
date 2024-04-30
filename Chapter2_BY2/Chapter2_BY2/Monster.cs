namespace Chapter2_BY2
{
    internal class Monster
    {
        public string Name { get; }
        public int Level { get; }
        public int Atk { get; }
        public int Def {  get; }
        public int Hp { get; }

        

        public Monster(string name, int level, int hp, int atk, int def)
        {
            Name = name;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
        }
    }
}