
using System.Security.Cryptography.X509Certificates;
using System;

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
        public void ShowInventory() // 인벤토리 확인하기
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n인벤토리");
            Console.ResetColor();
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("\n[아이템 목록]");
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

        // 게임 아이템 딕셔너리 선언
        public static Dictionary<string, Item> items;

        public static Dictionary<string, Item> gameItems = new Dictionary<string, Item>(); // 게임 아이템 딕셔너리 생성
        // 게임 아이템 셋팅
        public static void ItemSetting()
        {
            gameItems.Add("수련자 갑옷", new Item("방어력+", 5, "수련에 도움을 주는 갑옷입니다.", 1000));
            gameItems.Add("무쇠갑옷", new Item("방어력+", 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 1800));
            gameItems.Add("스파르타갑옷", new Item("방어력+", 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500));
            gameItems.Add("낡은 검", new Item("공격력+", 2, "쉽게 볼 수 있는 낡은 검 입니다.", 600));
            gameItems.Add("청동 도끼", new Item("공격력+", 5, "어디선가 사용됐던거 같은 도끼입니다.", 1500));
            gameItems.Add("스파르타의 창", new Item("공격력+", 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2700));
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
    
    class EquipmentManager
    {
        //장착 아이템 딕셔너리
        Dictionary<string, Item> equippedItems = new Dictionary<string, Item>();
        // EquippedHandler 라는 delegate 가짐
        public event ItemEquipHandler EquippedHandler;
        public bool equipped = false;

        // 이름을 입력 받을시, E 표시 노출, 해제시 사라지기
        public bool EquipItem(string itemName, Dictionary<string, Item> items)
        {
            if (items.ContainsKey(itemName))
            {
                Item item = items[itemName];
                if (!equippedItems.ContainsKey(itemName))
                {
                    equippedItems.Add(itemName, item);
                    equipped = true;
                    EquippedHandler?.Invoke(equipped);
                    return equipped;
                }
                else
                {
                    equippedItems.Remove(itemName);
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
            Console.WriteLine("0. 나가기");
            Console.Write(">>");
        }
    }

    class Program
    {
        public static int select;
        public static EquipmentManager equip = new EquipmentManager(); // 아이템 장착여부 확인
        public static Dictionary<string, Item> playerItems = new Dictionary<string, Item>(); // 플레이어 아이템 딕셔너리 생성

        // 게임 첫 시작
        public static void StartGame()
        {
            Console.WriteLine("\n스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine("\n1. 상태 보기\n2. 인벤토리\n3. 상점\n\n0. 나가기\n");
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
            if (equip.equipped && playerItems.Values.Any(item => item.ItemType == "공격력+"))
            {
                foreach (var item in playerItems.Values.Where(item => item.ItemType == "공격력+"))
                {
                    Console.Write(" (+" + item.ItemValue + ")");
                }
            }
            Console.Write("\n방어력 : " + character.Defense); // 방어력
            if (equip.equipped && playerItems.Values.Any(item => item.ItemType == "방어력+"))
            {
                foreach (var item in playerItems.Values.Where(item => item.ItemType == "방어력+"))
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

            while (true)
            {
                StartGame(); // 게임시작
                nextMove(); // 선택지 입력
                if (select == 1) // 상태 보기
                {
                    StatusWindow(Character.warrior);
                    nextMove();
                }
                else if (select == 2) // 인벤토리 보기
                {
                    while (true)
                    {
                        inventory.ShowInventory();
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
                                foreach (var item in playerItems) // 플레이어 아이템 출력
                                {
                                    int i = 1;
                                    Console.Write("- " + i + " ");
                                    if (equip.equipped == true)
                                    {
                                        Console.Write("[E]");
                                    }
                                    Console.WriteLine(item.Key + "\t| " + item.Value.ItemType + item.Value.ItemValue + " | " + item.Value.ItemExplain);
                                    i++;
                                }
                                Console.WriteLine("\n0. 나가기\n");
                                Console.WriteLine("원하시는 행동을 입력해주세요.");
                                Console.WriteLine("아이템 장착/해제 를 원하면 아이템 이름을 입력해주세요.");
                                Console.Write(">>");
                                string action = Console.ReadLine(); // 아이템 이름 입력
                                equip.EquipItem(action, playerItems); // 장착 & 해제
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
                        if (action2 != "0")
                        {
                            if (Item.gameItems.ContainsKey(action2) && Character.warrior.Gold >= Item.gameItems[action2].ItemPrice)
                            {
                                playerItems[action2] = Item.gameItems[action2]; // 플레이어 아이템에 추가
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
                        else if (action2 == "0") // 나가기 선택
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다.");
                        }
                    }
                }
                else if (select == 0)
                {
                    Console.WriteLine("게임을 종료합니다. ");
                    break;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }
    }
}
