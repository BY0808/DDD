
using System.Security.Cryptography.X509Certificates;
using System;
using static System.Collections.Specialized.BitVector32;
using System.Reflection.Metadata.Ecma335;
using System.CodeDom.Compiler;

namespace Chapter2_BY
{
    // 아이템 장착 여부 연동할 델리게이트
    public delegate void ItemEquipHandler(bool equippedHandler);

    // 캐릭터
    public class Character
    {
        public int Level = 01;
        public string Name = "Chad";
        public string Job = "전사";
        public int Attack = new Random().Next(8, 13);
        public int Defense = new Random().Next(4, 7);
        public int HP = 100;
        public int Gold = 1500;

        public static Character warrior = new Character(); // 전사 할당

    }

    public class Inventory()
    {
        public static Dictionary<string, Item> playerItems = new Dictionary<string, Item>(); // 플레이어 아이템 딕셔너리 생성
        public static EquipmentManager equip = new EquipmentManager(); // 아이템 장착여부 확인

        public void ShowInventory() // 인벤토리 확인하기
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n인벤토리");
            Console.ResetColor();
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("\n[아이템 목록]");
            if (playerItems != null) // 아이템이 있을 경우
            {
                foreach (var item in playerItems) // 플레이어 아이템 출력
                {
                    Console.Write("- ");
                    if (equip.equipped == true)
                    {
                        Console.Write("[E]");
                    }
                    Console.WriteLine(item.Key + "\t| " + item.Value.ItemType + item.Value.ItemValue + " | " + item.Value.ItemExplain);
                }
            }
        }
        public void OutInventory()
        {
            Console.WriteLine("\n1. 장착 관리");
            Console.WriteLine("0. 나가기\n");
        }
    }

    public class Item
    {
        public string ItemType { get; set; }
        public int ItemValue { get; set; }
        public string ItemExplain { get; set; }
        public int ItemPrice { get; set; }

        // 생성자
        public Item(string itemType, int itemValue, string itemExplain, int itemPrice)
        {
            ItemType = itemType;
            ItemValue = itemValue;
            ItemExplain = itemExplain;
            ItemPrice = itemPrice;
        }

        public static Dictionary<string, Item> items; // 게임 아이템 딕셔너리 선언
        public static Dictionary<string, Item> gameItems = new Dictionary<string, Item>(); // 게임 아이템 딕셔너리 생성
        public static Dictionary<string, Item> equippedItems = new Dictionary<string, Item>(); //장착 아이템 딕셔너리

        // 게임 아이템 셋팅
        public static void ItemSetting()
        {
            int randomArmor = new Random().Next(7, 13);
            int randomWeapon = new Random().Next(5, 7);
            gameItems.Add("수련자 갑옷", new Item("방어력+", 5, "수련에 도움을 주는 갑옷입니다.", 1000));
            gameItems.Add("무쇠갑옷", new Item("방어력+", 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 1800));
            gameItems.Add("스파르타갑옷", new Item("방어력+", 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500));
            gameItems.Add("랜덤 갑옷", new Item("방어력+", randomArmor, "랜덤으로 수치가 결정된 갑옷입니다. ", 1600));
            gameItems.Add("낡은 검", new Item("공격력+", 2, "쉽게 볼 수 있는 낡은 검 입니다.", 600));
            gameItems.Add("청동 도끼", new Item("공격력+", 5, "어디선가 사용됐던거 같은 도끼입니다.", 1500));
            gameItems.Add("스파르타의 창", new Item("공격력+", 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2700));
            gameItems.Add("랜덤 뿅망치", new Item("공격력+", randomWeapon, "랜덤으로 수치가 결정된 뿅망치 입니다.", 1600));
        }

        // 아이템 추가 메서드
        public void AddItem(string key, Item item)
        {
            items.Add(key, item);
        }

        // 아이템 빼기 메서드
        public void RemoveItem(string key, Item item)
        {
            items.Remove(key);
        }
        // 아이템 가져오기 메서드
        public Item GetItem(string key)
        {
            if (items.ContainsKey(key))
            {
                return items[key];
            }
            else
            {
                Console.WriteLine("해당 아이템이 없습니다.");
                return null;
            }
        }
    }

    public class EquipmentManager
    {
        // EquippedHandler 라는 delegate 가짐
        public event ItemEquipHandler EquippedHandler;
        public bool equipped = false;
        public static int plusDefense = 0;
        public static int plusAttack = 0;

        // 이름을 입력 받을시, E 표시 노출, 해제시 사라지기
        public bool EquipItem(string itemName, Dictionary<string, Item> items)
        {
            if (items.ContainsKey(itemName))
            {
                Item item = items[itemName];
                if (!Item.equippedItems.ContainsKey(itemName))
                {
                    Item.equippedItems.Add(itemName, item);
                    // 아이템 장착으로 공격력 & 방어력 증가
                    foreach (var equipitem in Inventory.playerItems.Values.Where(equipitem => equipitem.ItemType == "방어력+"))
                    { plusDefense += equipitem.ItemValue; }
                    foreach (var equipitem in Inventory.playerItems.Values.Where(equipitem => equipitem.ItemType == "공격력+"))
                    { plusAttack += equipitem.ItemValue; }
                    equipped = true;
                    EquippedHandler?.Invoke(equipped);
                    return equipped;
                }
                else
                {
                    // 아이템 해제로 공격력 & 방어력 감소
                    foreach (var equipitem in Inventory.playerItems.Values.Where(equipitem => equipitem.ItemType == "방어력+"))
                    { plusDefense -= equipitem.ItemValue; }
                    foreach (var equipitem in Inventory.playerItems.Values.Where(equipitem => equipitem.ItemType == "공격력+"))
                    { plusAttack -= equipitem.ItemValue; }
                    Item.equippedItems.Remove(itemName);
                    equipped = false;
                    EquippedHandler?.Invoke(equipped);
                    return equipped;
                }
            }
            else
            {
                Console.WriteLine("해당 아이템이 없습니다.");
                return equipped;
            }
        }
    }

    public class Shop
    {
        public void ShowShop() // 상점 보여주기
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n상점");
            Console.ResetColor();
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
            Console.WriteLine("[보유 골드]");
            Console.WriteLine(Character.warrior.Gold + " G");
            Console.WriteLine("\n[아이템 목록]");
            foreach (var items in Item.gameItems)
            {
                Console.WriteLine("- " + items.Key + "\t\t| " + items.Value.ItemType
                    + items.Value.ItemValue + "\t| " + items.Value.ItemExplain + "\t| "
                    + items.Value.ItemPrice + " G");
            }
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.WriteLine("아이템 구매를 원하면 아이템 이름을 입력해주세요.");
            Console.WriteLine("1.아이템 판매");
            Console.WriteLine("0. 나가기");
            Console.Write(">>");
        }
    }

    public class Dungeon
    {
        int requiredDefense = 0;// 권장 방어력
        int totalDefense = Character.warrior.Defense + EquipmentManager.plusDefense; // 플레이어의 방어력 가져오기
        int totalAttack = Character.warrior.Attack + EquipmentManager.plusAttack; // 플레이어의 공격력 가져오기
        int level;
        int nowGold;

        public void ShowDungeon() // 던전 보여주기
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n던전입장");
            Console.ResetColor();
            Console.WriteLine("이곳에서 던전을 선택 할 수 있습니다.\n");
            Console.WriteLine("1. 쉬운 던전\t| 방어력 5 이상 권장");
            Console.WriteLine("2. 일반 던전\t| 방어력 11 이상 권장");
            Console.WriteLine("3. 어려운 던전\t| 방어력 17 이상 권장");
            Console.WriteLine("0. 나가기\n");
        }

        void DungeonResult(int requiredDefense) // 확률로 던전 성공여부 반환
        {
            if (totalDefense < requiredDefense)
            {
                int resultRandom = new Random().Next(0, 10);
                if (resultRandom < 4)
                {
                    Console.WriteLine("던전에 실패 하였습니다.");
                    Console.WriteLine("체력이 반 감소합니다.");
                    Character.warrior.HP = (int)(Character.warrior.HP * 0.5); // 실패시 체력 절반 감소
                }
                else
                {
                    DungeonSuccess(requiredDefense);
                }
            }
            else
            {
                DungeonSuccess(requiredDefense);
            }
        }

        void DungeonSuccess(int requiredDefense) // 던전 성공시 표시
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("던전 클리어");
            Console.ResetColor();
            Console.WriteLine("축하합니다!!\n던전을 클리어 하였습니다.");
            int successRandom = new Random().Next(20, 36);
            int nowHP = Character.warrior.HP + totalDefense - requiredDefense - successRandom; // 깎일 체력 계산 후 반영
            int nowGold = Character.warrior.Gold + DungeonGold(level);
            Console.WriteLine("\n[탐험 결과]");
            Console.WriteLine("체력 " + Character.warrior.HP + " -> " + nowHP);
            Console.WriteLine("Gold " + Character.warrior.Gold + " -> " + nowGold);
            Character.warrior.HP = nowHP;
            Character.warrior.Gold = nowGold;
            Console.WriteLine("\n0. 나가기\n");
        }

        int DungeonGold(int level)
        {
            int randomBonus = new Random().Next(0, 10);
            if (level == 1)
            {
                nowGold = 1000 + totalAttack * (int)(randomBonus * 0.1);
                return nowGold;
            }
            else if (level == 2)
            {
                nowGold = 1700 + totalAttack * (int)(randomBonus * 0.1);
                return nowGold;
            }
            else
            {
                nowGold = 2500 + totalAttack * (int)(randomBonus * 0.1);
                return nowGold;
            }
        }

        public void DoDungeon(int level) // 던전 실행
        {
            if (level == 1)
            {
                DungeonResult(5);

            }
            else if (level == 2)
            {
                DungeonResult(11);
            }
            else if (level == 3)
            {
                DungeonResult(17);
            }
            else Console.WriteLine("잘못된 입력입니다.");
        }
    }

    class Program
    {
        public static int select;

        // 게임 첫 시작
        public static void StartGame()
        {
            Console.WriteLine("\n스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine("\n1. 상태 보기\n2. 인벤토리\n3. 상점\n4. 던전입장\n\n0. 나가기\n");
        }

        // 읽어오기 메소드
        public static int nextMove()
        {
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">>");
            select = int.Parse(Console.ReadLine()); // 선택지 읽어오기
            return select;
        }

        // 상태 보기 메서드
        public static void StatusWindow(Character character)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n상태 보기");
            Console.ResetColor();
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            Console.WriteLine("\nLv. " + character.Level.ToString("D2")); // 레벨
            Console.WriteLine(character.Name + " ( " + character.Job + " ) "); // 이름 + 직업
            Console.Write("공격력 : " + character.Attack); // 공격력
            if (Inventory.equip.equipped && Inventory.playerItems.Values.Any(item => item.ItemType == "공격력+"))
            {
                foreach (var item in Inventory.playerItems.Values.Where(item => item.ItemType == "공격력+"))
                {
                    Console.Write(" (+" + item.ItemValue + ")");
                }
            }
            Console.Write("\n방어력 : " + character.Defense); // 방어력
            if (Inventory.equip.equipped && Inventory.playerItems.Values.Any(item => item.ItemType == "방어력+"))
            {
                foreach (var item in Inventory.playerItems.Values.Where(item => item.ItemType == "방어력+"))
                {
                    Console.Write(" (+" + item.ItemValue + ")");
                }
            }
            Console.WriteLine("\n체 력 : " + character.HP); // 체력
            Console.WriteLine("Gold : " + character.Gold + " G\n"); // 골드
            Console.WriteLine("0. 나가기\n");
        }

        public static void Main(string[] args)
        {
            Item.ItemSetting(); // 아이템 셋팅
            Inventory inventory = new Inventory(); // 인벤토리 생성
            Shop shop = new Shop(); // 상점 생성
            Dungeon dungeon = new Dungeon(); // 던전 생성

            while (true)
            {
                StartGame(); // 게임시작
                nextMove(); // 선택지 입력
                if (select == 1) // 상태 보기
                {
                    while (true)
                    {
                        StatusWindow(Character.warrior);
                        nextMove();
                        if (select == 0) break;
                        else Console.WriteLine("잘못된 입력입니다.");
                    }
                }
                else if (select == 2) // 인벤토리 보기
                {
                    while (true)
                    {
                        inventory.ShowInventory();
                        inventory.OutInventory();
                        nextMove();
                        if (select == 1) // 장착관리 선택시
                        {
                            while (true)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("\n[장착 관리]");
                                Console.ResetColor();
                                inventory.ShowInventory();
                                Console.WriteLine("\n0. 나가기\n");
                                Console.WriteLine("원하시는 행동을 입력해주세요.");
                                Console.WriteLine("아이템 장착/해제 를 원하면 아이템 이름을 입력해주세요.");
                                Console.Write(">>");
                                string action = Console.ReadLine(); // 아이템 이름 입력
                                Inventory.equip.EquipItem(action, Inventory.playerItems); // 장착 & 해제
                                if (action == "0") // 나가기 선택
                                {
                                    break;
                                }
                            }
                        }
                        else break;
                    }
                }
                else if (select == 3) // 상점
                {
                    while (true)
                    {
                        shop.ShowShop();
                        string action2 = Console.ReadLine(); // 아이템 이름 입력
                        if (action2 != "0" && action2 != "1")
                        {
                            if (Item.gameItems.ContainsKey(action2) && Character.warrior.Gold >= Item.gameItems[action2].ItemPrice)
                            {
                                Inventory.playerItems[action2] = Item.gameItems[action2]; // 플레이어 아이템에 추가
                                Character.warrior.Gold -= Item.gameItems[action2].ItemPrice; // 구매한 아이템 가격만큼 Gold 차감
                                Item.gameItems.Remove(action2); // 게임 아이템리스트에서 제거
                                Console.WriteLine("구매를 완료했습니다.");
                            }
                            else if (Character.warrior.Gold < Item.gameItems[action2].ItemPrice)
                            {
                                Console.WriteLine("Gold가 부족합니다.");
                            }
                            else
                            {
                                Console.WriteLine("잘못된 입력입니다.");
                            }
                        }
                        else if (action2 == "1") // 아이템 판매 선택
                        {
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.WriteLine("\n상점 - 아이템 판매");
                            Console.ResetColor();
                            Console.WriteLine("판매를 원하면 이름을 입력해주세요.");
                            Console.WriteLine("[보유 골드]");
                            Console.WriteLine(Character.warrior.Gold + " G");
                            inventory.ShowInventory();
                            Console.WriteLine("원하시는 행동을 입력해주세요.");
                            Console.Write(">>");
                            string action3 = Console.ReadLine(); // 판매할 아이템 이름 입력
                            if (action3 != "0")
                            {
                                if (Inventory.playerItems.ContainsKey(action3)) // 플레이어가 가지고 있는 아이템이라면
                                {
                                    Item.gameItems[action3] = Inventory.playerItems[action3]; // 게임 아이템에 추가
                                    Character.warrior.Gold += (int)(Inventory.playerItems[action3].ItemPrice * 0.85); // 85% 가격으로 판매
                                    if (Inventory.equip.equipped == true) // 아이템 장착이었으면 해제
                                    {
                                        Inventory.equip.EquipItem(action3, Inventory.playerItems);
                                    }
                                    Inventory.playerItems.Remove(action3); // 플레이어 아이템 에서 제거
                                    Console.WriteLine("판매를 완료했습니다.");
                                    nextMove();
                                }
                                else if (Inventory.playerItems.Count == 0) Console.WriteLine("판매 가능한 아이템이 없습니다.");
                                else Console.WriteLine("잘못된 입력입니다.");
                            }
                            else if (action2 == "0") break;
                            else Console.WriteLine("잘못된 입력입니다.");
                        }
                        else if (action2 == "0") break;
                        else Console.WriteLine("잘못된 입력입니다.");
                    }
                }
                else if (select == 4) // 던전 입장
                {
                    while (true)
                    {
                        dungeon.ShowDungeon(); // 던전 리스트 보여주기
                        nextMove();
                        dungeon.DoDungeon(select);
                        nextMove();
                        if (select == 0) break;
                        else Console.WriteLine("잘못된 입력입니다.");
                    }
                }
                else if (select == 0)
                {
                    Console.WriteLine("게임을 종료합니다. ");
                    break;
                }
                else Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }
}