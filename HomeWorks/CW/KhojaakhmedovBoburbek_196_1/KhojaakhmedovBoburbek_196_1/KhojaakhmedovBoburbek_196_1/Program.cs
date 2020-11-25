using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;

namespace KhojaakhmedovBoburbek_196_1
{
    class Program
    {
        static void Main(string[] args)
        {
            string path_to_test = @"input/test1.txt";
            string path_to_answer = @"output/asd.txt";
            string path_to_data = @"data";

            //if (args.Length != 3)
            //{
            //    Console.WriteLine("Неверные аргументы");
            //    return;
            //}

            string[] input = dataProg(path_to_test);

            if (input != null)
            {
                string val = input[0];
                int n;
                if (int.TryParse(input[1], out n) && n >= 0 && n <= 1000)
                {
                    List<string> predicates = RetPredicate(input);
                    if (n == predicates.Count)
                    {
                        Dictionary<string, string[]> listOfValues = FactData(val, path_to_data); //Факты 
                        Dictionary<string, List<string>> listOfPredicates = DimData(predicates, path_to_data); //Предикаты
                        DateTime m = DateTime.Now;
                        InvisibleJoin_StepOne(listOfValues, listOfPredicates, val, m, path_to_answer, path_to_data);
                    }
                    else
                    {
                        Console.WriteLine("Неверное количество предикатов!");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Вторая сторока не парсится или находится вне [0,1000]!");
                    return;
                }
            }
        }
        public static string[] dataProg(string path_tests)
        {
            string[] input = File.ReadAllLines(path_tests);
            return input;
        }
        public static List<string> RetPredicate(string[] input)
        {
            List<string> li = new List<string>();
            for (int i = 2; i < input.Length; i++)
            {
                li.Add(input[i]);
            }
            return li;
        }
        public static Dictionary<string, string[]> FactData(string val, string data_path)
        {
            Dictionary<string, string[]> listOfValues = new Dictionary<string, string[]>();
            var facts = val.Split(',');
            for (int i = 0; i < facts.Length; i++)
            {
                ReadDim(listOfValues, data_path, facts[i]);
            }
            return listOfValues;
        }
        public static Dictionary<string, List<string>> DimData(List<string> val, string data_path)
        {
            Dictionary<string, List<string>> listOfPredicates = new Dictionary<string, List<string>>();
            List<string> tableNames = new List<string>();
            List<string> columnNames = new List<string>();
            List<string> operations = new List<string>();
            List<string> matters = new List<string>();
            for (int i = 0; i < val.Count; i++)
            {
                if (val[i].Contains('\''))
                {
                    var result = val[i].Split('.', '\'');
                    tableNames.Add(result[0]);
                    matters.Add(result[2]);

                    var additional_res = result[1].Split(' ');
                    columnNames.Add(additional_res[0]);
                    operations.Add(additional_res[1]);
                }
                else
                {
                    var result = val[i].Split('.', ' ');
                    tableNames.Add(result[0]);
                    columnNames.Add(result[1]);
                    operations.Add(result[2]);
                    matters.Add(result[3]);
                }
            }

            ReadFact(listOfPredicates, tableNames, columnNames, data_path);
            listOfPredicates.Add("tableNames", tableNames);
            listOfPredicates.Add("columnNames", columnNames);
            listOfPredicates.Add("operations", operations);
            listOfPredicates.Add("matters", matters);
            return listOfPredicates;
        }
        public static void ReadDim(Dictionary<string, string[]> dic, string path, string element)
        {
            string[] li = new string[60855];
            using (StreamReader reader = new StreamReader(path + "/" + element + ".csv"))
            {
                string str;
                int count = 0;
                while ((str = reader.ReadLine()) != null)
                {
                    li[count] = (str.Split('\n')[0]);
                    count++;
                }
            }
            dic.Add(element, li);
        }
        public static void ReadFact(Dictionary<string, List<string>> listOfPredicates, List<string> tableNames, List<string> columnNames, string data_path)
        {
            string[] q1 = new string[] { "CurrencyAlternateKey", "SalesTerritoryAlternateKey", "PromotionAlternateKey", "ProductAlternateKey",
            "FullDateAlternateKey","ResellerAlternateKey","FirstName"};
            string[] q2 = new string[] { "CurrencyName","SalesTerritoryRegion", "EnglishPromotionName","EnglishProductName",
            "DayNumberOfWeek","Phone","LastName"};
            string[] q3 = new string[] { "SalesTerritoryCountry", "EnglishPromotionType", "Color", "EnglishDayNameOfWeek", "BusinessType", "Title" };
            string[] q4 = new string[] { "SalesTerritoryGroup", "EnglishPromotionCategory", "SafetyStockLevel", "DayNumberOfMonth", "ResellerName", "BirthDate" };
            string[] q5 = new string[] { "StartDate", "ReorderPoint", "DayNumberOfYear", "NumberEmployees", "LoginID" };
            string[] q6 = new string[] { "EndDate", "SizeRange", "WeekNumberOfYear", "OrderFrequency", "EmailAddress" };
            string[] q7 = new string[] { "MinQty", "DaysToManufacture", "EnglishMonthName", "ProductLine", "Phone" };
            string[] q8 = new string[] { "StartDate", "MonthNumberOfYear", "AddressLine1", "MaritalStatus" };
            string[] q9 = new string[] { "CalendarQuarter", "BankName", "Gender" };
            string[] q10 = new string[] { "CalendarYear", "YearOpened", "PayFrequency" };

            //string data_path = @"../../data/";
            for (int i = 0; i < tableNames.Count; i++)
            {
                List<string> li = new List<string>();
                if (tableNames[i] == "FactResellerSales")
                {
                    using (StreamReader reader = new StreamReader(data_path + "/" + tableNames[i] + "." + columnNames[i] + ".csv"))
                    {
                        string str;
                        while ((str = reader.ReadLine()) != null)
                        {
                            li.Add(str.Split('\n')[0]);
                        }
                        if (!listOfPredicates.ContainsKey(tableNames[i] + "." + columnNames[i]))
                            listOfPredicates.Add(tableNames[i] + "." + columnNames[i], li);
                    }
                }
                else
                {
                    string[] arr = File.ReadAllLines(data_path + "/" + tableNames[i] + ".csv");
                    if (columnNames[i] == "StartDate")
                    {
                        if (tableNames[i] == "DimEmployee")
                        {
                            KeyAdding(listOfPredicates, arr, tableNames, i);
                            List<string> list = Wtry(arr, 14);
                            if (!listOfPredicates.ContainsKey(columnNames[i]))
                                listOfPredicates.Add(columnNames[i], list);
                        }
                        if (tableNames[i] == "DimPromotion")
                        {
                            KeyAdding(listOfPredicates, arr, tableNames, i);
                            List<string> list = Wtry(arr, 5);
                            if (!listOfPredicates.ContainsKey(columnNames[i]))
                                listOfPredicates.Add(columnNames[i], list);
                        }
                        if (tableNames[i] == "DimProduct")
                        {
                            KeyAdding(listOfPredicates, arr, tableNames, i);
                            List<string> list = Wtry(arr, 8);
                            if (!listOfPredicates.ContainsKey(columnNames[i]))
                                listOfPredicates.Add(columnNames[i], list);
                        }
                    }
                    else
                    {
                        if (q1.Contains(columnNames[i]))
                        {
                            KeyAdding(listOfPredicates, arr, tableNames, i);
                            List<string> list = Wtry(arr, 1);
                            if (!listOfPredicates.ContainsKey(columnNames[i]))
                                listOfPredicates.Add(columnNames[i], list);
                        }
                        if (q2.Contains(columnNames[i]))
                        {
                            KeyAdding(listOfPredicates, arr, tableNames, i);
                            List<string> list = Wtry(arr, 2);
                            if (!listOfPredicates.ContainsKey(columnNames[i]))
                                listOfPredicates.Add(columnNames[i], list);
                        }
                        if (q3.Contains(columnNames[i]))
                        {
                            KeyAdding(listOfPredicates, arr, tableNames, i);
                            List<string> list = Wtry(arr, 3);
                            if (!listOfPredicates.ContainsKey(columnNames[i]))
                                listOfPredicates.Add(columnNames[i], list);
                        }
                        if (q4.Contains(columnNames[i]))
                        {
                            KeyAdding(listOfPredicates, arr, tableNames, i);
                            List<string> list = Wtry(arr, 4);
                            if (!listOfPredicates.ContainsKey(columnNames[i]))
                                listOfPredicates.Add(columnNames[i], list);
                        }
                        if (q5.Contains(columnNames[i]))
                        {
                            KeyAdding(listOfPredicates, arr, tableNames, i);
                            List<string> list = Wtry(arr, 5);
                            if (!listOfPredicates.ContainsKey(columnNames[i]))
                                listOfPredicates.Add(columnNames[i], list);
                        }
                        if (q6.Contains(columnNames[i]))
                        {
                            KeyAdding(listOfPredicates, arr, tableNames, i);
                            List<string> list = Wtry(arr, 6);
                            if (!listOfPredicates.ContainsKey(columnNames[i]))
                                listOfPredicates.Add(columnNames[i], list);
                        }
                        if (q7.Contains(columnNames[i]))
                        {
                            KeyAdding(listOfPredicates, arr, tableNames, i);
                            List<string> list = Wtry(arr, 7);
                            if (!listOfPredicates.ContainsKey(columnNames[i]))
                                listOfPredicates.Add(columnNames[i], list);
                        }
                        if (q8.Contains(columnNames[i]))
                        {
                            KeyAdding(listOfPredicates, arr, tableNames, i);
                            List<string> list = Wtry(arr, 8);
                            if (!listOfPredicates.ContainsKey(columnNames[i]))
                                listOfPredicates.Add(columnNames[i], list);
                        }
                        if (q9.Contains(columnNames[i]))
                        {
                            KeyAdding(listOfPredicates, arr, tableNames, i);
                            List<string> list = Wtry(arr, 9);
                            if (!listOfPredicates.ContainsKey(columnNames[i]))
                                listOfPredicates.Add(columnNames[i], list);
                        }
                        if (q10.Contains(columnNames[i]))
                        {
                            KeyAdding(listOfPredicates, arr, tableNames, i);
                            List<string> list = Wtry(arr, 10);
                            if (!listOfPredicates.ContainsKey(columnNames[i]))
                                listOfPredicates.Add(columnNames[i], list);
                        }
                        if (columnNames[i] == "CalendarSemester" || columnNames[i] == "VacationHours") //11
                        {
                            KeyAdding(listOfPredicates, arr, tableNames, i);
                            List<string> list = Wtry(arr, 11);
                            if (!listOfPredicates.ContainsKey(columnNames[i]))
                                listOfPredicates.Add(columnNames[i], list);
                        }
                        if (columnNames[i] == "FiscalQuarter" || columnNames[i] == "SickLeaveHours") //12
                        {
                            KeyAdding(listOfPredicates, arr, tableNames, i);
                            List<string> list = Wtry(arr, 12);
                            if (!listOfPredicates.ContainsKey(columnNames[i]))
                                listOfPredicates.Add(columnNames[i], list);
                        }
                        if (columnNames[i] == "FiscalYear" || columnNames[i] == "DepartmentName") //13
                        {
                            KeyAdding(listOfPredicates, arr, tableNames, i);
                            List<string> list = Wtry(arr, 13);
                            if (!listOfPredicates.ContainsKey(columnNames[i]))
                                listOfPredicates.Add(columnNames[i], list);
                        }
                        if (columnNames[i] == "FiscalSemester" || columnNames[i] == "StartDate") //14
                        {
                            KeyAdding(listOfPredicates, arr, tableNames, i);
                            List<string> list = Wtry(arr, 14);
                            if (!listOfPredicates.ContainsKey(columnNames[i]))
                                listOfPredicates.Add(columnNames[i], list);
                        }
                    }
                }
            }
        }
        public static List<string> Wtry(string[] arr, int index)
        {
            List<string> listArr = new List<string>();
            for (int k = 0; k < arr.Length; k++)
            {
                var rez = arr[k].Split('|');
                listArr.Add(rez[index]);
            }
            return listArr;
        }
        public static void KeyAdding(Dictionary<string, List<string>> listOfPredicates, string[] arr, List<string> tableNames, int index)
        {
            List<string> listArr = new List<string>();
            for (int k = 0; k < arr.Length; k++)
            {
                var rez = arr[k].Split('|');
                //Console.WriteLine(rez[0]);
                listArr.Add(rez[0]);
            }
            if (!listOfPredicates.ContainsKey(tableNames[index] + "Key"))
            {
                listOfPredicates.Add(tableNames[index] + "Key", listArr);
            }
        }
        public static string Parseble(string pia)
        {
            try
            {
                int.Parse(pia);
                return "Int";
            }
            catch (Exception)
            {
                try
                {
                    DateTime.Parse(pia);
                    return "DateTime";
                }
                catch (Exception)
                {
                    return "No";
                }
            }
        }
        public static void InvisibleJoin_StepOne(Dictionary<string, string[]> listOfValues, Dictionary<string, List<string>> listOfPredicates, string first_line, DateTime m, string path_to_answer,string path_to_data)
        {
            Dictionary<string[], List<string>> results = new Dictionary<string[], List<string>>();

            List<string> tableNames = new List<string>();
            List<string> columnNames = new List<string>();
            List<string> operations = new List<string>();
            List<string> matters = new List<string>();

            foreach (var item in listOfPredicates)
            {
                foreach (var p in item.Value)
                {
                    //Получаю операции и значения
                    if (item.Key.Equals("columnNames"))
                        columnNames.Add(p);
                    if (item.Key.Equals("operations"))
                        operations.Add(p);
                    if (item.Key.Equals("matters"))
                        matters.Add(p);
                    if (item.Key.Equals("tableNames"))
                        tableNames.Add(p);
                }
            }
            for (int i = 0; i < operations.Count; i++)
            {
                if (operations[i] == "<") //1
                {
                    Repeating(listOfPredicates, tableNames, i, columnNames, matters, results, 1, operations);
                }
                if (operations[i] == ">") //2
                {
                    Repeating(listOfPredicates, tableNames, i, columnNames, matters, results, 2, operations);
                }
                if (operations[i] == "<=") //3
                {
                    Repeating(listOfPredicates, tableNames, i, columnNames, matters, results, 3, operations);
                }
                if (operations[i] == ">=") //4
                {
                    Repeating(listOfPredicates, tableNames, i, columnNames, matters, results, 4, operations);
                }
                if (operations[i] == "=") //5
                {
                    Repeating(listOfPredicates, tableNames, i, columnNames, matters, results, 5, operations);
                }
                if (operations[i] == "<>") //6
                {
                    Repeating(listOfPredicates, tableNames, i, columnNames, matters, results, 6, operations);
                }
            }

            InvisibleJoin_StepTwoThree(results, first_line, m, path_to_data, path_to_answer);
        }
        public static void Repeating(Dictionary<string, List<string>> listOfPredicates, List<string> tableNames, int i, List<string> columnNames, List<string> matters, Dictionary<string[], List<string>> results, int oper, List<string> operations)
        {
            List<int> listOfIndexes = new List<int>();
            List<string> listOfKeys = new List<string>();

            foreach (var item in listOfPredicates)
            {
                int count = 0;
                foreach (var item2 in item.Value)
                {
                    if (oper == 5 || oper == 6)
                    {
                        if (oper == 5)
                        {
                            if (item.Key == columnNames[i])
                            {
                                if (Parseble(item2) == "DateTime")
                                {
                                    DateTime n = DateTime.Parse(item2);
                                    DateTime matt = DateTime.Parse(matters[i]);
                                    if (n.Equals(matt))
                                    {
                                        listOfIndexes.Add(count);
                                    }
                                    count++;
                                }
                                if (Parseble(item2) == "Int")
                                {
                                    int n = int.Parse(item2);
                                    int matt = int.Parse(matters[i]);
                                    if (n.Equals(matt))
                                    {
                                        listOfIndexes.Add(count);
                                    }
                                    count++;
                                }
                                if (Parseble(item2) == "No")
                                {
                                    if (item2.Equals(matters[i]))
                                    {
                                        listOfIndexes.Add(count);
                                    }
                                    count++;
                                }
                            }
                            else
                            {
                                if (item2.Equals(matters[i]) && item.Key == tableNames[i] + "." + columnNames[i])
                                {
                                    listOfKeys.Add(item2);
                                }
                            }
                        }
                        else
                        {
                            if (item.Key == columnNames[i])
                            {
                                if (!item2.Equals(matters[i]))
                                {
                                    listOfIndexes.Add(count);
                                }
                                count++;
                            }
                            else
                            {
                                if (!(item2.Equals(matters[i])) && item.Key == tableNames[i] + "." + columnNames[i])
                                {
                                    listOfKeys.Add(item2);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (oper == 1)
                        {
                            if (item.Key == columnNames[i])
                            {
                                if (Parseble(matters[i]).Equals("Int"))
                                {
                                    int matt = int.Parse(matters[i]);
                                    int n = int.Parse(item2);
                                    if (n < matt)
                                    {
                                        listOfIndexes.Add(count);
                                    }
                                    count++;
                                }
                                if (Parseble(matters[i]).Equals("DateTime"))
                                {
                                    DateTime matt = DateTime.Parse(matters[i]);
                                    DateTime n = DateTime.Parse(item2);
                                    if (n < matt)
                                    {
                                        listOfIndexes.Add(count);
                                    }
                                    count++;
                                }
                            }
                            else
                            {
                                if (Parseble(matters[i]).Equals("Int") && item.Key == tableNames[i] + "." + columnNames[i])
                                {
                                    int matt = int.Parse(matters[i]);
                                    int n = int.Parse(item2);
                                    if (n < matt)
                                    {
                                        listOfKeys.Add(item2);
                                    }
                                    count++;
                                }
                                if (Parseble(matters[i]).Equals("DateTime") && item.Key == tableNames[i] + "." + columnNames[i])
                                {
                                    DateTime matt = DateTime.Parse(matters[i]);
                                    DateTime n = DateTime.Parse(item2);
                                    if (n < matt)
                                    {
                                        listOfKeys.Add(item2);
                                    }
                                    count++;
                                }
                            }

                        }
                        if (oper == 2)
                        {
                            if (item.Key == columnNames[i])
                            {
                                if (Parseble(matters[i]).Equals("Int"))
                                {
                                    int matt = int.Parse(matters[i]);
                                    int n = int.Parse(item2);
                                    if (n > matt)
                                    {
                                        listOfIndexes.Add(count);
                                    }
                                    count++;
                                }
                                if (Parseble(matters[i]).Equals("DateTime"))
                                {
                                    DateTime matt = DateTime.Parse(matters[i]);
                                    DateTime n = DateTime.Parse(item2);
                                    if (n > matt)
                                    {
                                        listOfIndexes.Add(count);
                                    }
                                    count++;
                                }
                            }
                            else
                            {
                                if (Parseble(matters[i]).Equals("Int") && item.Key == tableNames[i] + "." + columnNames[i])
                                {
                                    int matt = int.Parse(matters[i]);
                                    int n = int.Parse(item2);
                                    if (n > matt)
                                    {
                                        listOfKeys.Add(item2);
                                    }
                                    count++;
                                }
                                if (Parseble(matters[i]).Equals("DateTime") && item.Key == tableNames[i] + "." + columnNames[i])
                                {
                                    DateTime matt = DateTime.Parse(matters[i]);
                                    DateTime n = DateTime.Parse(item2);
                                    if (n > matt)
                                    {
                                        listOfKeys.Add(item2);
                                    }
                                    count++;
                                }
                            }
                        }
                        if (oper == 3)
                        {
                            if (item.Key == columnNames[i])
                            {
                                if (Parseble(matters[i]).Equals("Int"))
                                {
                                    int matt = int.Parse(matters[i]);
                                    int n = int.Parse(item2);
                                    if (n <= matt)
                                    {
                                        listOfIndexes.Add(count);
                                    }
                                    count++;
                                }
                                if (Parseble(matters[i]).Equals("DateTime"))
                                {
                                    DateTime matt = DateTime.Parse(matters[i]);
                                    DateTime n = DateTime.Parse(item2);
                                    if (n <= matt)
                                    {
                                        listOfIndexes.Add(count);
                                    }
                                    count++;
                                }
                            }
                            else
                            {
                                if (Parseble(matters[i]).Equals("Int") && item.Key == tableNames[i] + "." + columnNames[i])
                                {
                                    int matt = int.Parse(matters[i]);
                                    int n = int.Parse(item2);
                                    if (n <= matt)
                                    {
                                        listOfKeys.Add(item2);
                                    }
                                    count++;
                                }
                                if (Parseble(matters[i]).Equals("DateTime") && item.Key == tableNames[i] + "." + columnNames[i])
                                {
                                    DateTime matt = DateTime.Parse(matters[i]);
                                    DateTime n = DateTime.Parse(item2);
                                    if (n <= matt)
                                    {
                                        listOfKeys.Add(item2);
                                    }
                                    count++;
                                }
                            }
                        }
                        if (oper == 4)
                        {
                            if (item.Key == columnNames[i])
                            {
                                if (Parseble(matters[i]).Equals("Int"))
                                {
                                    int matt = int.Parse(matters[i]);
                                    int n = int.Parse(item2);
                                    if (n >= matt)
                                    {
                                        listOfIndexes.Add(count);
                                    }
                                    count++;
                                }
                                if (Parseble(matters[i]).Equals("DateTime"))
                                {
                                    DateTime matt = DateTime.Parse(matters[i]);
                                    DateTime n = DateTime.Parse(item2);
                                    if (n >= matt)
                                    {
                                        listOfIndexes.Add(count);
                                    }
                                    count++;
                                }
                            }
                            else
                            {
                                if (Parseble(matters[i]).Equals("Int") && item.Key == tableNames[i] + "." + columnNames[i])
                                {
                                    int matt = int.Parse(matters[i]);
                                    int n = int.Parse(item2);
                                    if (n >= matt)
                                    {
                                        listOfKeys.Add(item2);
                                    }
                                    count++;
                                }
                                if (Parseble(matters[i]).Equals("DateTime") && item.Key == tableNames[i] + "." + columnNames[i])
                                {
                                    DateTime matt = DateTime.Parse(matters[i]);
                                    DateTime n = DateTime.Parse(item2);
                                    if (n >= matt)
                                    {
                                        listOfKeys.Add(item2);
                                    }
                                    count++;
                                }
                            }
                        }
                    }
                }
            }

            //переделываю из лист индексов в лист ключей
            if (listOfKeys.Count == 0)
            {
                int index_count = 0;
                foreach (var item in listOfPredicates)
                {
                    int count = 0;
                    foreach (var item2 in item.Value)
                    {
                        if (listOfIndexes.Count == index_count)
                            break;
                        else
                        {
                            if (item.Key.Equals(tableNames[i] + "Key"))
                            {
                                if (count == listOfIndexes[index_count])
                                {
                                    listOfKeys.Add(item2);
                                    index_count++;
                                }
                                count++;
                            }
                        }
                    }
                }
            }

            if (tableNames[i].Equals("DimPromotion"))
            {
                results.Add(new string[] { "FactResellerSales.PromotionKey.csv", tableNames[i], columnNames[i], operations[i], matters[i] }, listOfKeys);
            }

            if (tableNames[i].Equals("DimProduct"))
            {
                results.Add(new string[] { "FactResellerSales.ProductKey.csv", tableNames[i], columnNames[i], operations[i], matters[i] }, listOfKeys);
            }

            if (tableNames[i].Equals("DimDate"))
            {
                results.Add(new string[] { "FactResellerSales.OrderDateKey.csv", tableNames[i], columnNames[i], operations[i], matters[i] }, listOfKeys);
            }

            if (tableNames[i].Equals("DimReseller"))
            {
                results.Add(new string[] { "FactResellerSales.ResellerKey.csv", tableNames[i], columnNames[i], operations[i], matters[i] }, listOfKeys);
            }

            if (tableNames[i].Equals("DimEmployee"))
            {
                results.Add(new string[] { "FactResellerSales.EmployeeKey.csv", tableNames[i], columnNames[i], operations[i], matters[i] }, listOfKeys);
            }

            if (tableNames[i].Equals("DimCurrency"))
            {
                results.Add(new string[] { "FactResellerSales.CurrencyKey.csv", tableNames[i], columnNames[i], operations[i], matters[i] }, listOfKeys);
            }

            if (tableNames[i].Equals("DimSalesTerritory"))
            {
                results.Add(new string[] { "FactResellerSales.SalesTerritoryKey.csv", tableNames[i], columnNames[i], operations[i], matters[i] }, listOfKeys);
            }

            if (tableNames[i].Equals("FactResellerSales"))
            {
                results.Add(new string[] { tableNames[i] + "." + columnNames[i] + ".csv", tableNames[i], columnNames[i], operations[i], matters[i] }, listOfKeys);
            }
        }
        public static void InvisibleJoin_StepTwoThree(Dictionary<string[], List<string>> results, string first_line, DateTime m, string data_path,string path_to_answer)
        {
            //string data_path = @"../../data/";
            //List<MyBitmap> listOfBitmaps = new List<MyBitmap>();
            MyBitmap lastBitmap = null;
            foreach (var item in results)
            {
                MyBitmap bitmap = new MyBitmap(60855);
                string[] arrFacts = new string[60855];
                getFactColumn(data_path + "/" + item.Key[0], arrFacts);
                for (int i = 0; i < arrFacts.Length; i++)
                {
                    if (item.Value.Contains(arrFacts[i]))
                        bitmap.Set(i, true);
                    else
                        bitmap.Set(i, false);
                }
                if (lastBitmap == null)
                {
                    lastBitmap = bitmap;
                }
                else
                {
                    lastBitmap.And(bitmap);
                }
            }

            //Console.WriteLine(lastBitmap.GetOne());

            var vals = first_line.Split(',');
            for (int i = 0; i < vals.Length; i++)
            {
                List<string> list = new List<string>();
                getFactColumn2(data_path + "/" + vals[i] + ".csv", list);
                lastBitmap.AddList(list);
            }

            exportResult(lastBitmap, m, path_to_answer);
        }
        public static void getFactColumn(string path, string[] li)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string str;
                int count = 0;
                while ((str = reader.ReadLine()) != null)
                {
                    li[count] = (str.Split('\n')[0]);
                    count++;
                }
            }
        }
        public static void getFactColumn2(string path, List<string> li)
        {
            li.Clear();
            using (StreamReader reader = new StreamReader(path))
            {
                string str;
                while ((str = reader.ReadLine()) != null)
                {
                    li.Add(str.Split('\n')[0]);
                }
            }
        }
        public static void exportResult(MyBitmap lastBitmap, DateTime m, string path)
        {
            foreach (var item in lastBitmap)
            {
                using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(item);
                }
                Console.WriteLine(item);
            }
            Console.WriteLine("\n\n" + "RUNTIME: " + (DateTime.Now - m).Seconds + " seconds");
        }
    }

    public class MyBitmap : Bitmap, IEnumerable<string>
    {
        int[] arr;
        List<List<string>> list = new List<List<string>>();
        public MyBitmap(int length)
        {
            arr = new int[length];
        }

        public override void And(Bitmap other)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (other.Get(i) == true)
                    arr[i] = arr[i] & 1;
                else
                    arr[i] = arr[i] & 0;
            }
        }

        public override bool Get(int i)
        {
            return arr[i] == 1;
        }

        public IEnumerator<string> GetEnumerator()
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == 1)
                {
                    string str = "";
                    int amount = list.Count;
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (amount != 1)
                        {
                            str += (list[j])[i] + "|";
                            amount--;
                        }
                        else
                        {
                            str += (list[j])[i];
                            amount--;
                        }
                    }
                    yield return str;
                }
            }
        }
        public override void Set(int i, bool value)
        {
            if (value)
                arr[i] = 1;
            else
                arr[i] = 0;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddList(List<string> list)
        {
            this.list.Add(list);
        }

        public int GetOne()
        {
            int count = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == 1)
                    count++;
            }
            return count;
        }
    }

    public abstract class Bitmap
    {
        public abstract void And(Bitmap other);

        public abstract void Set(int i, bool value);

        public abstract bool Get(int i);
    }
}

