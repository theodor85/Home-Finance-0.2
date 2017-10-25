using System;
using System.Collections.Generic;
using System.Globalization;

namespace Home_Finance_02
{
    public class MainWindow
    {

        public MainWindow(HF_BD DB)
        {
        }

        public static void Start(HF_BD CurrentDB)
        {
            do
            {
                Console.Clear();

                // выводим шапку
                Console.WriteLine("*****************************************************************");
                Console.WriteLine("*****************************************************************");
                Console.WriteLine("Home Finance v0.2.001 10/10/2017 by Teddy Coder.");
                Console.WriteLine("Добро пожаловать!");
                Console.WriteLine();

                // выводим отчет об остатках
                DateTime DT = DateTime.Now;

                Console.WriteLine("-----------------------------------------------------------------");
                Console.WriteLine("На {0} у вас денег в кошельках: ", DT);
                Console.WriteLine();

                List<TItemState> ReportMoneyInWallets = CurrentDB.ReportBalance(DT);

                ReportOutput(ReportMoneyInWallets);

                // выводим отчет о расходах
                Console.WriteLine("-----------------------------------------------------------------");
                Console.WriteLine("Ваши РАСХОДЫ за период {0}: ", "Сентябрь 2017");
                Console.WriteLine();

                DateTime DT1 = new DateTime(2017, 10, 1);
                DateTime DT2 = new DateTime(2017, 10, 30);

                List<TItemState> ReportExpenses = CurrentDB.ReportExpenses(DT1, DT2);
                ReportOutput(ReportExpenses);

                // выводим отчет о доходах
                Console.WriteLine("-----------------------------------------------------------------");
                Console.WriteLine("Ваши ДОХОДЫ за период {0}: ", "Октябрь 2017");
                Console.WriteLine();

                DT1 = new DateTime(2017, 10, 1);
                DT2 = new DateTime(2017, 10, 30);
                List<TItemState> ReportIncomes = CurrentDB.ReportIncomes(DT1, DT2);
                ReportOutput(ReportIncomes);


                // приглашение 
                Console.WriteLine("-----------------------------------------------------------------");
                Console.WriteLine("Список доступных команд:");
                Console.WriteLine("0 - Выход; 1 - Ввод операций; 2 - Добавление элементов справочников; 3 - Список операций/Удалить операцию");
                Console.WriteLine("-----------------------------------------------------------------");
                Console.Write("--->");
                string InputString = Console.ReadLine();

                switch (InputString)
                {
                    case "0":
                        return;
                    case "1":
                        WindowEnterOperation(CurrentDB);
                        continue;
                    case "2":
                        WindowAddReferenceItems(CurrentDB);
                        continue;
                    case "3":
                        WindowOperationsList(CurrentDB);
                        continue;
                    default:
                        Console.WriteLine("Введена недопустимая команда!");
                        continue;
                }

            } while (true);
        }

        static void ReportOutput(List<TItemState> Report)
        {

            double Itogo = 0;
            foreach (TItemState item in Report)
            {
                Itogo = Itogo + item.Sum;

                string ItemName = item.ItemName;
                string strSum = item.Sum.ToString("#####.00");

                int Tab = 56 - ItemName.Length - strSum.Length;
                string strTab = "";
                for (int i = 0; i < Tab; i++) strTab = strTab + " ";

                Console.WriteLine("\t{0}{1}{2}", ItemName, strTab, strSum);
            }

            int tab2 = 52 - Itogo.ToString("#####.00").Length;
            string strtab1 = "     ";
            string strtab2 = "";
            for (int i = 0; i < tab2; i++) strtab2 = strtab2 + " ";

            Console.WriteLine();
            Console.WriteLine("{1}Всего: {2}{0}", Itogo.ToString("#####.00"), strtab1, strtab2);
            Console.WriteLine();
        }

        static void ReportOutput(List<string> Report)
        {
            foreach (string item in Report)
                Console.WriteLine("\t{0}", item);
            Console.WriteLine();
        }

        static void WindowEnterOperation(HF_BD CurrDB)
        {
            Console.Clear();

            Console.WriteLine("*****************************************************************");
            Console.WriteLine("Вы находитесь в окне ввода операций");
            Console.WriteLine();
            do
            {

                Console.WriteLine("-----------------------------------------------------------------");
                Console.WriteLine("Список доступных команд:");
                Console.WriteLine("0 - Выход; 1 - Ввод расходов; 2 - Ввод доходов; 3 - Перемещение; 4 - Ввод начальных остатков");
                Console.WriteLine("-----------------------------------------------------------------");
                Console.Write("--->");

                string InputString = Console.ReadLine();


                switch (InputString)
                {
                    case "0":
                        return;
                    case "1":

                        Console.WriteLine();

                        // вывести на экран список кошельков и список статей расходов

                        // выводим список кошельков
                        Console.WriteLine("-----------------------------------------------------------------");
                        Console.WriteLine("Доступные кошельки:");
                        Console.WriteLine();

                        List<string> WalletsList = CurrDB.GetReferenceItemsList("Wallets");
                        ReportOutput(WalletsList);

                        // выводим список статей расходов
                        Console.WriteLine("-----------------------------------------------------------------");
                        Console.WriteLine("Доступные статьи расходов:");
                        Console.WriteLine();

                        List<string> EspensesList = CurrDB.GetReferenceItemsList("Expenses");
                        ReportOutput(EspensesList);

                        DateTime DT = new DateTime();
                        string Wallet = "";
                        string Expense = "";
                        double Sum = 0;
                        
                        Console.Write("Дата ------------->");
                        try
                        {
                            DT = DateTime.Parse(Console.ReadLine());
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine("Недопустимый формат даты. Системное сообщение: " + e.Message);
                            Console.WriteLine("Нажмите Enter для продложения.");
                            Console.ReadLine();
                            continue;
                        }


                        Console.Write("Кошелёк ---------->");
                        Wallet = Console.ReadLine();
                        Console.Write("Статья расходов -->");
                        Expense = Console.ReadLine();
                        Console.Write("Сумма ------------>");
                        try
                        {
                            Sum = double.Parse(Console.ReadLine());
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine("Недопустимый формат числа. Системное сообщение: " + e.Message);
                            Console.WriteLine("Нажмите Enter для продложения.");
                            Console.ReadLine();
                            continue;
                        } 
                        // для отладки
                        /*DT = new DateTime(2017, 10, 6);
                        Wallet = "Основной";
                        Expense = "Еда";
                        Sum = 10.3;
                        Console.Write("Дата ------------->");
                        Console.WriteLine(DT);
                        Console.Write("Кошелёк ---------->");
                        Console.WriteLine(Wallet);
                        Console.Write("Статья расходов ---------->");
                        Console.WriteLine(Expense);
                        Console.Write("Сумма ------------>");
                        Console.WriteLine(Sum);
                        Console.ReadLine();*/
                        // для отладки

                        try
                        {
                            CurrDB.EnterOperation(OperationTypes.Expense, DT, Wallet, Expense, Sum);
                            Console.WriteLine();
                            Console.WriteLine("Операция успешно завершена! Нажмите Enter для продложения.");
                            Console.WriteLine();
                            Console.ReadLine();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine();
                            Console.WriteLine("ERROR: Во время совершения операции проихошла ошибка!");
                            Console.WriteLine("Источник ошибки: {0}\n Cообщение: {1}\n Стэк вызовов: {2}", e.Source, e.Message, e.StackTrace);
                            Console.WriteLine("Нажмите Enter для продложения.");
                            Console.ReadLine();
                        }



                        continue;
                    case "2":
                        Console.WriteLine();

                        // вывести на экран список кошельков и список статей доходов

                        // выводим список кошельков
                        Console.WriteLine("-----------------------------------------------------------------");
                        Console.WriteLine("Доступные кошельки:");
                        Console.WriteLine();

                        List<string> WalletsListForIncomes = CurrDB.GetReferenceItemsList("Wallets");
                        ReportOutput(WalletsListForIncomes);

                        // выводим список статей доходов
                        Console.WriteLine("-----------------------------------------------------------------");
                        Console.WriteLine("Доступные статьи доходов:");
                        Console.WriteLine();

                        List<string> IncomesList = CurrDB.GetReferenceItemsList("Incomes");
                        ReportOutput(IncomesList);

                        DateTime DT2 = new DateTime();
                        string Wallet2 = "";
                        string Income = "";
                        double Sum2 = 0;

                        Console.Write("Дата ------------->");
                        try
                        {
                            DT2 = DateTime.Parse(Console.ReadLine());
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine("Недопустимый формат даты. Системное сообщение: " + e.Message);
                            Console.WriteLine("Нажмите Enter для продложения.");
                            Console.ReadLine();
                            continue;
                        }


                        Console.Write("Кошелёк ---------->");
                        Wallet2 = Console.ReadLine();
                        Console.Write("Статья доходов --->");
                        Income = Console.ReadLine();
                        Console.Write("Сумма ------------>");
                        try
                        {
                            Sum2 = double.Parse(Console.ReadLine());

                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine("Недопустимый формат числа. Системное сообщение: " + e.Message);
                            Console.WriteLine("Нажмите Enter для продложения.");
                            Console.ReadLine();
                            continue;
                        }

                        try
                        {
                            CurrDB.EnterOperation(OperationTypes.Income, DT2, Income, Wallet2, Sum2);
                            Console.WriteLine();
                            Console.WriteLine("Операция успешно завершена! Нажмите Enter для продложения.");
                            Console.WriteLine();
                            Console.ReadLine();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine();
                            Console.WriteLine("ERROR: Во время совершения операции проихошла ошибка!");
                            Console.WriteLine("Источник ошибки: {0}\n Cообщение: {1}", e.Source, e.Message, e.StackTrace);
                            Console.WriteLine("Нажмите Enter для продложения.");
                            Console.ReadLine();
                        }
                        
                        continue;
                    case "3":
                        Console.WriteLine();

                        // вывести на экран список кошельков и список статей доходов

                        // выводим список кошельков
                        Console.WriteLine("-----------------------------------------------------------------");
                        Console.WriteLine("Доступные кошельки:");
                        Console.WriteLine();

                        List<string> WalletsListForTransfer = CurrDB.GetReferenceItemsList("Wallets");
                        ReportOutput(WalletsListForTransfer);

                        DateTime DT3 = new DateTime();
                        string WalletSource = "";
                        string WalletReciever = "";
                        double Sum3 = 0;

                        Console.Write("Дата ------------->");
                        try
                        {
                            DT3 = DateTime.Parse(Console.ReadLine());
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine("Недопустимый формат даты. Системное сообщение: " + e.Message);
                            Console.WriteLine("Нажмите Enter для продложения.");
                            Console.ReadLine();
                            continue;
                        }


                        Console.Write("Кошелёк источник ->");
                        WalletSource = Console.ReadLine();
                        Console.Write("Кошелёк приёмник ->");
                        WalletReciever = Console.ReadLine();
                        Console.Write("Сумма ------------>");
                        try
                        {
                            Sum3 = double.Parse(Console.ReadLine());
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine("Недопустимый формат числа. Системное сообщение: " + e.Message);
                            Console.WriteLine("Нажмите Enter для продложения.");
                            Console.ReadLine();
                            continue;
                        }

                        try
                        {
                            CurrDB.EnterOperation(OperationTypes.Transfer, DT3, WalletSource, WalletReciever, Sum3);
                            Console.WriteLine();
                            Console.WriteLine("Операция успешно завершена! Нажмите Enter для продложения.");
                            Console.WriteLine();
                            Console.ReadLine();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine();
                            Console.WriteLine("ERROR: Во время совершения операции проихошла ошибка!");
                            Console.WriteLine("Источник ошибки: {0}\n Cообщение: {1}", e.Source, e.Message);
                            Console.WriteLine("Нажмите Enter для продложения.");
                            Console.ReadLine();
                        }
                        
                        continue;
                    case "4":

                        Console.WriteLine();

                        // вывести на экран список кошельков

                        // выводим список кошельков
                        Console.WriteLine("-----------------------------------------------------------------");
                        Console.WriteLine("Доступные кошельки:");
                        Console.WriteLine();

                        List<string> WalletsListForBalance = CurrDB.GetReferenceItemsList("Wallets");
                        ReportOutput(WalletsListForBalance);

                        DateTime DT4 = new DateTime();
                        string Wallet4 = "";
                        double Sum4 = 0;

                        
                        Console.Write("Дата ------------->");
                        try
                        {
                            DT4 = DateTime.Parse(Console.ReadLine());
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine("Недопустимый формат даты. Системное сообщение: " + e.Message);
                            Console.WriteLine("Нажмите Enter для продложения.");
                            Console.ReadLine();
                            continue;
                        }


                        Console.Write("Кошелёк ---------->");
                        Wallet4 = Console.ReadLine();
                        Console.Write("Сумма ------------>");
                        try
                        {
                            Sum4 = double.Parse(Console.ReadLine());
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine("Недопустимый формат числа. Системное сообщение: " + e.Message);
                            Console.WriteLine("Нажмите Enter для продложения.");
                            Console.ReadLine();
                            continue;
                        }


                        // для отладки
                        /*Console.Write("Дата ------------->");
                        Console.WriteLine(DT4);
                        Console.Write("Кошелёк ---------->");
                        Console.WriteLine(Wallet4);
                        Console.Write("Сумма ------------>");
                        Console.WriteLine(Sum4);
                        Console.ReadLine();*/
                        // для отладки
                        try
                        {
                            CurrDB.EnterOperation(OperationTypes.Rest, DT4, "", Wallet4, Sum4);
                            Console.WriteLine();
                            Console.WriteLine("Операция успешно завершена! Нажмите Enter для продложения.");
                            Console.WriteLine();
                            Console.ReadLine();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine();
                            Console.WriteLine("ERROR: Во время совершения операции проихошла ошибка!");
                            Console.WriteLine("Источник ошибки: {0}\n Cообщение: {1}", e.Source, e.Message);
                            Console.WriteLine("Нажмите Enter для продложения.");
                            Console.ReadLine();
                        }
                        
                        continue;

                    default:
                        Console.WriteLine("Введена недопустимая команда!");
                        continue;
                }
            } while (true);
        }

        static void WindowAddReferenceItems(HF_BD CurrDB)
        {           
            do
            {
                Console.Clear();

                Console.WriteLine("*****************************************************************");
                Console.WriteLine("Вы находитесь в окне добавления элементов справочников");
                Console.WriteLine();

                Console.WriteLine("-----------------------------------------------------------------");
                Console.WriteLine("Список доступных команд:");
                Console.WriteLine("0 - Выход; 1 - Добавление кошельков; 2 - Добавление статей расходов; 3 - Добавление статей доходов ");
                Console.WriteLine("-----------------------------------------------------------------");
                Console.Write("--->");

                string InputString = Console.ReadLine();


                switch (InputString)
                {
                    case "0":
                        return;
                    case "1":
                        Console.WriteLine();

                        // вывести на экран список кошельков

                        // выводим список кошельков
                        Console.WriteLine("-----------------------------------------------------------------");
                        Console.WriteLine("Доступные кошельки:");
                        Console.WriteLine();

                        List<string> Wallets = CurrDB.GetReferenceItemsList("Wallets");
                        ReportOutput(Wallets);

                        string Wallet = "";
                        Console.Write("Новый кошелёк ----->");
                        Wallet = Console.ReadLine();

                        try
                        {
                            CurrDB.AddReferenceItem("Wallets", Wallet);
                            Console.WriteLine();
                            Console.WriteLine("Кошелёк успешно добавлен! Нажмите Enter для продложения.");
                            Console.WriteLine();
                            Console.ReadLine();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine();
                            Console.WriteLine("ERROR: Во время добавления кошелька произошла ошибка: " + e.Message + "\nНажмите Enter для продложения.");
                            Console.WriteLine();
                            Console.ReadLine();
                        }
                    continue;
                    case "2":
                        Console.WriteLine();

                        // вывести на экран список статей расходов

                        // выводим список статей расходов
                        Console.WriteLine("-----------------------------------------------------------------");
                        Console.WriteLine("Доступные статьи расходов:");
                        Console.WriteLine();

                        List<string> Expenses = CurrDB.GetReferenceItemsList("Expenses");
                        ReportOutput(Expenses);

                        string Expense = "";
                        Console.Write("Новая статья расходов ----->");
                        Expense = Console.ReadLine();

                        try
                        {
                            CurrDB.AddReferenceItem("Expenses", Expense);
                            Console.WriteLine();
                            Console.WriteLine("Статья расходов успешно добавлена! Нажмите Enter для продложения.");
                            Console.WriteLine();
                            Console.ReadLine();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine();
                            Console.WriteLine("ERROR: Во время добавления статьи расходов произошла ошибка: " + e.Message + "\nНажмите Enter для продложения.");
                            Console.WriteLine();
                            Console.ReadLine();
                        }

                        continue;
                    case "3":
                        Console.WriteLine();

                        // вывести на экран список статей доходов

                        // выводим список статей доходов
                        Console.WriteLine("-----------------------------------------------------------------");
                        Console.WriteLine("Доступные статьи доходов:");
                        Console.WriteLine();

                        List<string> Incomes = CurrDB.GetReferenceItemsList("Incomes");
                        ReportOutput(Incomes);

                        string Income = "";
                        Console.Write("Новая статья доходов ----->");
                        Income = Console.ReadLine();

                        try
                        {
                            CurrDB.AddReferenceItem("Incomes", Income);
                            Console.WriteLine();
                            Console.WriteLine("Статья доходов успешно добавлена! Нажмите Enter для продложения.");
                            Console.WriteLine();
                            Console.ReadLine();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine();
                            Console.WriteLine("ERROR: Во время добавления статьи доходов произошла ошибка: " + e.Message + "\nНажмите Enter для продложения.");
                            Console.WriteLine();
                            Console.ReadLine();
                        }
                        
                        continue;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Введена недопустимая команда!");
                        Console.ReadLine();
                        continue;
                }

            } while (true);

        }

        static void WindowOperationsList(HF_BD CurrDB)
        {
            string S;
            DateTime StartDate, EndDate;

            Console.Clear();

            Console.WriteLine("*****************************************************************");
            Console.WriteLine("Вы находитесь в окне просмотра списка операций");
            Console.WriteLine();

            do
            {
                Console.WriteLine("-----------------------------------------------------------------");
                Console.WriteLine("Список доступных команд:");
                Console.WriteLine("0 - Выход; 1 - Смотреть список операций; 2 - Удалить операцию");
                Console.WriteLine("-----------------------------------------------------------------");
                Console.Write("--->");
                S = Console.ReadLine();

                switch (S)
                {
                    case "0":
                        return;
                    case "1":
                        Console.WriteLine("Введите даты начала и конца периода, за который выводить список операций");
                        Console.Write("Дата начала --->");
                        
                        try
                        {
                            StartDate = DateTime.Parse(Console.ReadLine());
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine("Недопустимый формат даты. Системное сообщение: " + e.Message);
                            Console.WriteLine("Нажмите Enter для продложения.");
                            Console.ReadLine();
                            continue;
                        }


                        Console.Write("Дата конца ---->");
                        try
                        {
                            EndDate = DateTime.Parse(Console.ReadLine());
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine("Недопустимый формат даты. Системное сообщение: " + e.Message);
                            Console.WriteLine("Нажмите Enter для продложения.");
                            Console.ReadLine();
                            continue;
                        }
                        ReportOperationList(StartDate, EndDate, CurrDB);
                        Console.WriteLine("Нажмите Enter для продолжения");
                        Console.ReadLine();
                        continue;
                    case "2":
                        Console.WriteLine("");
                        Console.WriteLine("Введите ID операции, которую нужно удалить");
                        Console.Write("ID ----->");
                        
                        int ID;
                        try
                        {
                            ID = int.Parse(Console.ReadLine());
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine("Недопустимый формат числа. Системное сообщение: " + e.Message);
                            Console.WriteLine("Нажмите Enter для продложения.");
                            Console.ReadLine();
                            continue;
                        }

                        int Err = CurrDB.DeleteOperation(ID);
                        if (Err == 0)
                        {
                            Console.WriteLine("Операция удалена успешно. Нажмите Enter");
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("Во время выполнения проихошла ошибка! Нажмите Enter");
                            Console.ReadLine();
                        }

                        continue;
                    default:
                        Console.WriteLine("Выбрана недопустимая операция! Нажмите Enter");
                        Console.ReadLine();
                        continue;

                }

            } while (true);
        }

        static void ReportOperationList(DateTime StartDate, DateTime EndDate, HF_BD CurrDB)
        {
            List<TOperation> OperationsList = CurrDB.GetOperationsList(StartDate, EndDate);

            Console.WriteLine("");
            Console.WriteLine("ID\tДата\t\tТип\tИсточник\tПриемник\tСумма");
            Console.WriteLine("----------------------------------------------------------------------");

            foreach(TOperation Op in OperationsList)
            {
                string tabs1 = "\t";
                string tabs2 = "\t\t";
                string Source;
                string Dest;

                if (Op.Source.Length < 8)
                    Source = Op.Source + tabs2;
                else
                    Source = Op.Source + tabs1;
                if (Op.Destination.Length < 8)
                    Dest = Op.Destination + tabs2;
                else
                    Dest = Op.Destination + tabs1;

                Console.WriteLine("{0}\t{1}\t{2}\t" + Source + Dest +"{3}", Op.ID.ToString(), Op.Date.ToString("dd.MM.yyyy"), Op.OperationType.ToString(), Op.Sum.ToString("#####.00", CultureInfo.CreateSpecificCulture("en-US")) );
                
            };

            Console.WriteLine("----------------------------------------------------------------------");
        }
    }
}

