using System;
using System.Threading;
using static System.Console;

namespace Practise
{
    class Color
    {
        public static void Black() => ForegroundColor = ConsoleColor.Black;
        public static void Blue() => ForegroundColor = ConsoleColor.Blue;
        public static void Cyan() => ForegroundColor = ConsoleColor.Cyan;
        public static void DarkBlue() => ForegroundColor = ConsoleColor.DarkBlue;
        public static void DarkCyan() => ForegroundColor = ConsoleColor.DarkCyan;
        public static void DarkGray() => ForegroundColor = ConsoleColor.DarkGray;
        public static void DarkGreen() => ForegroundColor = ConsoleColor.DarkGreen;
        public static void DarkMagenta() => ForegroundColor = ConsoleColor.DarkMagenta;
        public static void DarkRed() => ForegroundColor = ConsoleColor.DarkRed;
        public static void DarkYellow() => ForegroundColor = ConsoleColor.DarkYellow;
        public static void Gray() => ForegroundColor = ConsoleColor.Gray;
        public static void Green() => ForegroundColor = ConsoleColor.Green;
        public static void Magenta() => ForegroundColor = ConsoleColor.Magenta;
        public static void Red() => ForegroundColor = ConsoleColor.Red;
        public static void White() => ForegroundColor = ConsoleColor.White;
        public static void Yellow() => ForegroundColor = ConsoleColor.Yellow;
        public static void Reset() => ResetColor();
    }

    class Helper
    {
        public static void Option(int no, string str, string op = "")
        {
            Color.Red(); Write("["); Color.Yellow(); Write(no); Color.Red(); Write("] "); Color.White(); Write(str);
        }
    }

    class MyException : Exception
    {
        public string message = "Yanlis melumat daxil etdiniz";
        public void PrintMessage()
        {
            Clear();
            Color.Red();
            WriteLine(message);
            Thread.Sleep(1500);
            Clear();
            Color.Reset();
        }
    }

    class Card
    {
        public string Pan { get; set; }
        public decimal Balance { get; set; }
        public string Cvc { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Pin { get; set; }

        public Card(string pin)
        {
            int panNumber; string pan = "";
            Random rand = new Random();
            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(10);
                panNumber = rand.Next(1000, 10000);
                pan += Convert.ToString(panNumber) + " ";
            }
            Thread.Sleep(25);
            Pan = pan;

            Cvc = Convert.ToString(rand.Next(100, 1000));

            ExpireDate = DateTime.Now.AddYears(2);

            Balance = rand.Next(0, 1000);

            Pin = pin;
        }

        public void ShowCard()
        {
            WriteLine($"PAN : {Pan}");
            WriteLine($"CVC : {Cvc}");
            WriteLine($"Balance : {Balance} azn");
            WriteLine($"Expire Date : {ExpireDate.Month}/{ExpireDate.Year % 100}");
        }
    }

    class User
    {
        private int id;
        private static int s_id = default;
        public int Id
        {
            get { return id; }
            set { id = ++s_id; }
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Card CreditCard { get; set; }

        public User() { }
        public User(string name, string surname, Card creditCard)
        {
            Name = name;
            Surname = surname;
            CreditCard = creditCard;
        }
    }

    class Bank
    {
        private static string[] operations = new string[] { };
        public static string[] Operations
        {
            get { return operations; }
            set { operations = value; }
        }

        public static void AddOperation(string operation)
        {
            var temp = new string[Operations.Length + 1];
            for (int i = 0; i < Operations.Length; i++)
            {
                temp[i] = Operations[i];
            }
            temp[Operations.Length] = operation;
            Operations = temp;
        }

        public static void Welcome(ref User[] users)
        {
            Clear();
            bool find = false;
            do
            {
                try
                {
                    for (int i = 0; i < users.Length; i++)
                    {
                        WriteLine(users[i].CreditCard.Pan);
                    }
                    WriteLine(); Color.White();
                    Write("PAN daxil edin : ");
                    Color.Cyan(); string pan = ReadLine(); Color.White();
                    for (int i = 0; i < users.Length; i++)
                    {
                        if (users[i].CreditCard.Pan == pan + " ")
                        {
                            Color.White(); Write("PIN daxil edin : "); Color.Cyan();
                            string pin = ReadLine();
                            if (users[i].CreditCard.Pin == pin)
                            {
                                find = true;
                                Operation(ref users[i], ref users);
                            }
                            else throw new MyException { message = "PIN yanlisdir !" };
                        }
                    }
                    if (!find)
                    {
                        throw new MyException { message = "PAN yanlisdir !" };
                    }
                }
                catch (MyException ex)
                {
                    ex.PrintMessage();
                }
            } while (!find);
        }

        public static void Operation(ref User user, ref User[] users)
        {
            string choice;
            do
            {
                Clear(); Color.Blue();
                Write($"{user.Name} {user.Surname}"); Color.Reset();
                WriteLine($", Xos gelmisiniz zehmet olmasa\nasagidakilardan birini secerdiniz\n");
                Helper.Option(1, "Balans\n");
                Helper.Option(2, "Nagd pul\n");
                Helper.Option(3, "Emeliyyatlar\n");
                Helper.Option(4, "Karta kocurme\n\n");
                Helper.Option(0, "Geri\n\n");
                Color.White(); Write("Secminizi daxil edin : "); Color.Cyan();
                choice = ReadLine(); Color.White();
                try
                {
                    if (choice != "0" && choice != "1" && choice != "2" && choice != "3" && choice != "4")
                        throw new MyException();
                }
                catch (MyException ex)
                {
                    ex.PrintMessage();
                }
                if (choice == "0")
                {
                    Welcome(ref users);
                }
                else if (choice == "1")
                {
                    try
                    {
                        ShowBalance(ref user, ref users);
                    }
                    catch (MyException ex)
                    {
                        ex.PrintMessage();
                        Clear();
                        ShowBalance(ref user, ref users);
                    }
                }
                else if (choice == "2")
                {
                    try
                    {
                        GetCash(ref user, ref users);
                    }
                    catch (MyException ex)
                    {
                        ex.PrintMessage();
                        Clear();
                        Operation(ref user, ref users);
                    }
                }
                else if (choice == "3")
                {
                    string enter = "";
                    do
                    {
                        try
                        {
                            Clear();
                            foreach (var item in Operations)
                            {
                                WriteLine(item);
                            }
                            WriteLine();
                            Helper.Option(0, "Geri\n\n");
                            Write("Seciminizi daxil edin : ");
                            Color.Cyan(); enter = ReadLine(); Color.White();
                            if (enter != "0") throw new MyException();
                            else Operation(ref user, ref users);
                        }
                        catch (MyException ex)
                        {
                            ex.PrintMessage();
                        }
                    } while (enter != "0");
                }
                else if (choice == "4")
                {
                    try
                    {
                        SendMoney(ref user, ref users);
                    }
                    catch (MyException ex)
                    {
                        ex.PrintMessage();
                        Clear();
                        Operation(ref user, ref users);
                    }
                }
            } while (choice != "0" && choice != "1" && choice != "2" && choice != "3" && choice != "4");
        }
        private static void ShowBalance(ref User user, ref User[] users)
        {
            DateTime date = DateTime.Now;
            AddOperation($"========================================================\n" +
                $"{user.CreditCard.Pan} - PAN'li istifadeci balansina baxdi\nTarix : {date}");
            string enter;
            do
            {
                Clear();
                Color.White(); Write("Balans : "); Color.Green(); Write(user.CreditCard.Balance); Color.White(); Write(" azn\n\n");
                Helper.Option(0, "Geri\n\n");
                Write("Seciminizi daxil edin : ");
                Color.Cyan(); enter = ReadLine(); Color.White();
                if (enter == "0")
                {
                    Operation(ref user, ref users);
                }
                else
                {
                    throw new MyException();
                }
            } while (enter != "0");
        }
        private static void GetCash(ref User user, ref User[] users)
        {
            void Check(decimal cash, ref User user2, ref User[] users2)
            {
                if (user2.CreditCard.Balance >= cash)
                {
                    DateTime date = DateTime.Now;
                    AddOperation($"========================================================\n" +
                        $"{user2.CreditCard.Pan} - PAN'li istifadeci pul cixardi\nTarix : {date}");
                    user2.CreditCard.Balance -= cash;
                    Write("Balansinizdan "); Color.Green(); Write(cash); Color.White(); Write(" azn cixildi !");
                    Thread.Sleep(1500);
                    Operation(ref user2, ref users2);
                }
                else
                {
                    throw new MyException { message = "Balansinizda kifayet qeder mebleg yoxdur !" };
                }
            }
            Clear();
            Helper.Option(1, "10 azn\n");
            Helper.Option(2, "20 azn\n");
            Helper.Option(3, "50 azn\n");
            Helper.Option(4, "100 azn\n");
            Helper.Option(5, "Diger\n\n");
            Helper.Option(0, "Geri\n\n");
            Write("Seciminizi daxil edin : ");
            Color.Cyan(); string choice = ReadLine(); Color.White();
            switch (choice)
            {
                case "0":
                    Operation(ref user, ref users);
                    break;
                case "1":
                    Check(10, ref user, ref users);
                    break;
                case "2":
                    Check(20, ref user, ref users);
                    break;
                case "3":
                    Check(50, ref user, ref users);
                    break;
                case "4":
                    Check(100, ref user, ref users);
                    break;
                case "5":
                    Write("Ne qeder pul cixarmaq isteyirsiniz ? : ");
                    Color.Cyan(); decimal.TryParse(ReadLine(), out decimal result); Color.White();
                    Check(result, ref user, ref users);
                    break;
                default:
                    throw new MyException();
            }
        }
        private static void SendMoney(ref User user, ref User[] users)
        {
            bool find = false;
            do
            {
                Clear();
                Write("Pul gonderilecek kartin PAN'ini daxil edin : ");
                Color.Cyan(); string pan = ReadLine(); Color.White();
                foreach (var item in users)
                {
                    if (item.CreditCard.Pan == pan + " ")
                    {
                        find = true;
                        Write("PIN daxil edin : ");
                        Color.Cyan(); string pin = ReadLine(); Color.White();
                        if (pin == user.CreditCard.Pin)
                        {
                            Write("Gonderilecek meblegi daxil edin : ");
                            Color.Cyan(); decimal.TryParse(ReadLine(), out decimal amount); Color.White();
                            if (amount <= user.CreditCard.Balance)
                            {
                                DateTime date = DateTime.Now;
                                AddOperation($"========================================================\n" +
                                    $"{user.CreditCard.Pan} - PAN'li istifadeci pul gonderdi\nTarix : {date}");
                                user.CreditCard.Balance -= amount;
                                item.CreditCard.Balance += amount;
                                Color.Green(); WriteLine("Odeme ugurlu sekilde gerceklesdi !"); Color.White();
                                Thread.Sleep(1500);
                                Operation(ref user, ref users);
                            }
                            else
                            {
                                throw new MyException { message = "Balansinizda kifayet qeder mebleg yoxdur !" };
                            }
                        }
                        else
                        {
                            throw new MyException { message = "PIN yanlisdir !" };
                        }
                    }
                }
                if (!find)
                {
                    throw new MyException { message = "Bu PAN'a aid istifadeci tapilmadi !" };
                }
            } while (!find);
        }
    }

    class Program
    {
        public static void Print(string str)
        {
            WriteLine();
        }

        static void Main(string[] args)
        {
            User[] users = new User[2]
            {
                new User("samir","mammadov",new Card("2458")),
                new User("tural","novruzlu",new Card("1367")),
            };
            Bank.Welcome(ref users);

            Console.ReadKey();
        }
    }
}