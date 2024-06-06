using GeometrucShapeCarLibrary;

namespace Events_лаб_13
{
    public class Program
    {
        static void Main(string[] args)
        {
            // создаём две коллекции
            MyObservableCollection<Shape> collection1 = new MyObservableCollection<Shape>("First");
            MyObservableCollection<Shape> collection2 = new MyObservableCollection<Shape>("Second");

            // создаём два журнала: один - для первой коллекции, другой - для второй
            Journal<Shape> journal1 = new Journal<Shape>();
            Journal<Shape> journal2 = new Journal<Shape>();

            // ПОДПИСКИ
            // На событие первой коллекции
            collection1.CollectionCountChange += journal1.WriteRecord; // Подписка обработчика события (запись в первый журнал) на событие первой коллекции (изменение количества элементов)
            collection1.CollectionReferenceChange += journal1.WriteRecord; // Подписка обработчика события (запись в первый журнал) на событие первой коллекции (изменение ссылки)
            // На событие второй коллекции
            collection2.CollectionCountChange += journal2.WriteRecord; // Подписка обработчика события (запись во второй журнал) на событие второй коллекции (изменение количества элементов)
            collection2.CollectionReferenceChange += journal2.WriteRecord; // Подписка обработчика события (запись во второй журнал) на событие второй коллекции (изменение ссылки)

            int answer;
            do
            {
                Console.WriteLine("1. Создать первую коллекцию");
                Console.WriteLine("2. Создать вторую коллекцию");
                Console.WriteLine("3. Печать журнала первой коллекции");
                Console.WriteLine("4. Печать журнала второй коллекции");
                Console.WriteLine("5. Добавить элемент в первую коллекцию");
                Console.WriteLine("6. Добавить элемент во вторую коллекцию");
                Console.WriteLine("7. Удалить элемент из первой коллекции");
                Console.WriteLine("8. Удалить элемент из второй коллекции");
                Console.WriteLine("0. Выход");
                answer = EnterNumber.EnterIntNumber("Выберите пункт меню", 0);
                switch (answer)
                {
                    case 1: // первый выбор Формирование первой коллекции
                        {
                            int count = EnterNumber.EnterIntNumber("Введите количество записей", 0);
                            for (int i = 0; i < count; i++)
                            {
                                Shape shape = new Shape();
                                shape.RandomInit();
                                collection1.Add(shape);
                            }
                            break;
                        }
                    case 2: // второй выбор Формирование второй коллекции
                        {
                            int count = EnterNumber.EnterIntNumber("Введите количество записей", 0);
                            for (int i = 0; i < count; i++)
                            {
                                Shape shape = new Shape();
                                shape.RandomInit();
                                collection2.Add(shape);
                            }
                            break;
                        }
                    case 3: // третий выбор печать первой
                        {
                            journal1.PrintJournal();
                            break;
                        }
                    case 4: // четвёртый выбор печать второй
                        {
                            journal2.PrintJournal();
                            break;
                        }
                    case 5: // пятый выбор добавить в первую            
                        {
                            Shape shape = new Shape();
                            Console.WriteLine("1. Добавление случайного элемента");
                            Console.WriteLine("2. Ввод элемента с клавиатуры");
                            answer = EnterNumber.EnterIntNumber("Выберите нoмер задания", 0);
                            switch (answer)
                            {
                                case 1:
                                    {
                                        shape.RandomInit();
                                        break;
                                    }
                                case 2:
                                    {
                                        Console.WriteLine("Введите элемент");
                                        shape.Init();
                                        break;
                                    }
                            }
                            try
                            {
                                collection1.Add(shape);
                                Console.WriteLine($"Элемент добавлен");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("Элемент не добавлен");
                            }
                            break;
                        }
                    case 6: // шестой выбор добавить во вторую          
                        {
                            Shape shape = new Shape();
                            Console.WriteLine("1. Добавление случайного элемента");
                            Console.WriteLine("2. Ввод элемента с клавиатуры");
                            answer = EnterNumber.EnterIntNumber("Выберите нoмер задания", 0);
                            switch (answer)
                            {
                                case 1:
                                    {
                                        shape.RandomInit();
                                        break;
                                    }
                                case 2:
                                    {
                                        Console.WriteLine("Введите элемент");
                                        shape.Init();
                                        break;
                                    }
                            }
                            try
                            {
                                collection2.Add(shape);
                                Console.WriteLine($"Элемент добавлен");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("Элемент не добавлен");
                            }
                            break;
                        }
                    case 7: // седьмой выбор удаление элемента в первой коллекции
                        {
                            if (collection1.Count == 0)
                                Console.WriteLine("Таблица пустая");
                            else
                            {
                                Console.WriteLine("Введите элемент для поиска");
                                Shape shape = new Shape();
                                shape.Init();  
                                bool ok = collection1.Remove(shape);
                                if (ok)
                                    Console.WriteLine($"Элемент удален");
                                else
                                    Console.WriteLine($"Элемент не найден");
                            }
                            break;
                        }
                    case 8: // восьмой выбор удаление элемента во второй коллекции
                        {
                            if (collection2.Count == 0)
                                Console.WriteLine("Таблица пустая");
                            else
                            {
                                Console.WriteLine("Введите элемент для поиска");
                                Shape shape = new Shape();
                                shape.Init();
                                bool ok = collection2.Remove(shape);
                                if (ok)
                                    Console.WriteLine($"Элемент удален");
                                else
                                    Console.WriteLine($"Элемент не найден");
                            }
                            break;
                        }
                }
            } while (answer != 0);
        }
    }
}
