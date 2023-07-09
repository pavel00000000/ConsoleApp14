
//Написать программу которая будет сохранять историю математических операций калькулятора в Json файл
//и  пользователь по желанию может просмотреть все математические операции которые он проводил
//Калькулятор должен быть полностью реализован 




using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Xml;
using System.Collections.Generic;


class Program
{
    static List<Operation> operationsHistory = new List<Operation>();

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Выберите операцию:");
            Console.WriteLine("1. Сложение");
            Console.WriteLine("2. Вычитание");
            Console.WriteLine("3. Умножение");
            Console.WriteLine("4. Деление");
            Console.WriteLine("5. Показать историю операций");
            Console.WriteLine("6. Выход");

            int choice = GetIntInput();

            if (choice == 6)
                break;

            if (choice == 5)
            {
                ShowOperationsHistory();
                continue;
            }

            Console.WriteLine("Введите первое число:");
            double num1 = GetDoubleInput();

            Console.WriteLine("Введите второе число:");
            double num2 = GetDoubleInput();

            double result = PerformOperation(choice, num1, num2);
            Console.WriteLine($"Результат: {result}");

            string expression = GetExpression(num1, num2, result, choice);
            Operation operation = new Operation(expression);
            operationsHistory.Add(operation);

            SaveOperationsHistoryToJson();
        }
    }

    static int GetIntInput()
    {
        int input;
        while (!int.TryParse(Console.ReadLine(), out input))
        {
            Console.WriteLine("Некорректный ввод. Попробуйте ещё раз.");
        }
        return input;
    }

    static double GetDoubleInput()
    {
        double input;
        while (!double.TryParse(Console.ReadLine(), out input))
        {
            Console.WriteLine("Некорректный ввод. Попробуйте ещё раз.");
        }
        return input;
    }

    static double PerformOperation(int choice, double num1, double num2)
    {
        switch (choice)
        {
            case 1:
                return num1 + num2;
            case 2:
                return num1 - num2;
            case 3:
                return num1 * num2;
            case 4:
                return num1 / num2;
            default:
                throw new ArgumentException("Некорректный выбор операции.");
        }
    }

    static string GetExpression(double num1, double num2, double result, int choice)
    {
        string operationSymbol;
        switch (choice)
        {
            case 1:
                operationSymbol = "+";
                break;
            case 2:
                operationSymbol = "-";
                break;
            case 3:
                operationSymbol = "*";
                break;
            case 4:
                operationSymbol = "/";
                break;
            default:
                throw new ArgumentException("Некорректный выбор операции.");
        }
        return $"{num1} {operationSymbol} {num2} = {result}";
    }

    static void ShowOperationsHistory()
    {
        if (operationsHistory.Count == 0)
        {
            Console.WriteLine("История операций пуста.");
            return;
        }

        Console.WriteLine("История операций:");
        foreach (var operation in operationsHistory)
        {
            Console.WriteLine(operation.Expression);
        }
    }

    static void SaveOperationsHistoryToJson()
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        var expressions = operationsHistory.Select(operation => operation.Expression);
        string json = JsonSerializer.Serialize(expressions, options);
        File.WriteAllText("history.json", json);
    }
}

class Operation
{
    public string Expression { get; set; }

    public Operation(string expression)
    {
        Expression = expression;
    }
}
