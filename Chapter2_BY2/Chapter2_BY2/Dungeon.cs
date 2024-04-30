
namespace Chapter2_BY2
{
    //던전 타입 어떤것 있는지 정의
    public enum dungeonType
    {
        EASY,
        NORMAL,
        HARD
    }

    internal class Dungeon
    {

        public void ShowDungeon()
        {
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 던전입장 ■");
            Console.WriteLine("이곳에서 던전을 선택 할 수 있습니다.\n");
            Console.WriteLine("1. 쉬운 던전\t| 방어력 5 이상 권장");
            Console.WriteLine("2. 일반 던전\t| 방어력 11 이상 권장");
            Console.WriteLine("3. 어려운 던전\t| 방어력 17 이상 권장");
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
        }

        internal void DoDungeon(int playerDef)
        {
        }
    }
}