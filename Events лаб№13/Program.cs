using GeometrucShapeCarLibrary;
using System.Runtime.InteropServices;

namespace Events_лаб_13
{
    public class Program
    {
        /// <summary>
        /// Формирование коллекции ДСЧ
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static MyObservableCollection<Shape> CreateCollection(MyObservableCollection<Shape> collection)
        {
            collection.Clear();
            int count = EnterNumber.EnterIntNumber("Введите количество записей", 0);
            for (int i = 0; i < count; i++)
            {
                Shape shape = new Shape();
                shape.RandomInit();
                collection.Add(shape);
            }
            return collection;
        }

        /// <summary>
        /// Добавление в коллекцию элемента
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static MyObservableCollection<Shape> AddItemCollection(MyObservableCollection<Shape> collection)
        {
            Shape shape = new Shape();
            Console.WriteLine("1. Добавление случайного элемента");
            Console.WriteLine("2. Ввод элемента с клавиатуры");
            int answer = EnterNumber.EnterIntNumber("Выберите нoмер задания", 0);
            switch (answer)
            {
                case 1:
                    {
                        shape.RandomInit();
                        break;
                    }
                case 2:
                    {
                        InitShape();
                        break;
                    }
            }
            try
            {
                collection.Add(shape);
                Console.WriteLine($"Элемент добавлен");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Элемент не добавлен");
            }
            return collection;
        }

        /// <summary>
        /// Удаление элемента из коллекции
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static MyObservableCollection<Shape> RemoveItemCollection(MyObservableCollection<Shape> collection)
        {
            if (collection.Count == 0)
                Console.WriteLine("Таблица пустая");
            else
            {
                Console.WriteLine("Введите элемент для поиска");
                Shape shape = new Shape();
                shape.Init();
                bool ok = collection.Remove(shape);
                if (ok)
                    Console.WriteLine($"Элемент удален");
                else
                    Console.WriteLine($"Элемент не найден");
            }
            return collection;
        }

        /// <summary>
        /// Изменение элемента коллекции
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static MyObservableCollection<Shape> ChangeItemCollection(MyObservableCollection<Shape> collection)
        {
            if (collection.Count == 0)
                Console.WriteLine("Таблица пустая");
            else
            {
                // Вводим позицию элемента, который хотим изменить
                int index = EnterNumber.EnterIntNumber("Введите позицию элемента, который хотим изменить", 1); // вводим позицию
                // Вводим фигуру, на которую хотите поменять выбранную
                Shape changed = InitShape("Введите фигуру, на которую хотите поменять выбранную");
                try
                {
                    collection[index - 1] = changed;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return collection;
        }

        /// <summary>
        /// выбор фигуры для добавления/удаления/поиска
        /// </summary>
        /// <param name="obj">фигура</param>
        public static void MenuChoise(ref Shape obj)
        {
            int answer;
            //do
            //{
            Console.WriteLine("1.Неопределённую фигуру");
            Console.WriteLine("2.Прямоугольник");
            Console.WriteLine("3.Параллелепипед");
            Console.WriteLine("4.Окружность");
            //Console.WriteLine("0.Выбор сделан - закрыть меню");
            Console.WriteLine("Выберите фигуру");
            answer = EnterNumber.EnterIntNumber(); // выбираем действие
            switch (answer)
            {
                case 1: // первый выбор
                    {
                        obj = new Shape();
                        break;
                    }
                case 2: // второй выбор
                    {
                        obj = new Rectangle();
                        break;
                    }
                case 3: // третий выбор
                    {
                        obj = new Parallelepiped();
                        break;
                    }
                case 4: // третий выбор
                    {
                        obj = new Circle();
                        break;
                    }
                //case 0: // программа продолжит работу
                //    {
                //        Console.WriteLine("Выбор закрыт");
                //        break;
                //    }
                default: // введённое число не подошло ни к одному пункту
                    {
                        Console.WriteLine("Неправильно задан пункт меню. По умолчанию выбрана - неопределённая фигура.");
                        break;
                    }
            }
            //} while (answer != 0); // не закрываем запросы, пока не введём 0
        }
        /// <summary>
        /// Инициализация фигуры
        /// </summary>
        /// <param name="message">Сообщение о том, для чего инициализируем фигуру</param>
        /// <returns></returns>
        public static Shape InitShape(string message = "Выбор фигуры:")
        {
            // Вводим фигуру
            Console.WriteLine(message);
            Shape shape = new Shape();
            MenuChoise(ref shape); // выбираем фигуру
            Console.WriteLine("Введите данные для объекта:");
            shape.Init(); // задаем параметры для фигуры
            return shape;
        }
        static void Main(string[] args)
        {
            // создаём две коллекции
            MyObservableCollection<Shape> firstCollection = new MyObservableCollection<Shape>("First", 0);
            MyObservableCollection<Shape> secondCollection = new MyObservableCollection<Shape>("Second", 0);

            // создаём два журнала: один - для первой коллекции, другой - для второй
            Journal<Shape> firstJournal = new Journal<Shape>();
            Journal<Shape> secondJournal = new Journal<Shape>();

            // ПОДПИСКИ
            // один объект Journal подписать на события CollectionCountChanged и CollectionReferenceChanged из первой коллекции,
            // другой объект Journal подписать на события CollectionReferenceChanged из обеих коллекций. 
            // Первый журнал
            firstCollection.CollectionCountChanged += firstJournal.WriteRecord; // Подписка обработчика события (запись в первый журнал) на событие первой коллекции (изменение количества элементов)
            firstCollection.CollectionReferenceChanged += firstJournal.WriteRecord; // Подписка обработчика события (запись в первый журнал) на событие первой коллекции (изменение ссылки)
            // Второй журнал
            firstCollection.CollectionReferenceChanged += secondJournal.WriteRecord; // Подписка обработчика события (запись в первый журнал) на событие первой коллекции (изменение ссылки)
            secondCollection.CollectionReferenceChanged += secondJournal.WriteRecord; // Подписка обработчика события (запись во второй журнал) на событие второй коллекции (изменение ссылки)

            int answer;
            do
            {
                Console.WriteLine("1. Создать первую коллекцию ДСЧ");
                Console.WriteLine("2. Создать вторую коллекцию ДСЧ");
                Console.WriteLine("3. Печать первого журнала");
                Console.WriteLine("4. Печать второго журнала");
                Console.WriteLine("5. Добавить элемент в первую коллекцию");
                Console.WriteLine("6. Добавить элемент во вторую коллекцию");
                Console.WriteLine("7. Удалить элемент из первой коллекции");
                Console.WriteLine("8. Удалить элемент из второй коллекции");
                Console.WriteLine("9. Изменить элемент из первой коллекции");
                Console.WriteLine("10. Изменить элемент из второй коллекции");
                Console.WriteLine("0. Выход");
                answer = EnterNumber.EnterIntNumber("Выберите пункт меню", 0);
                switch (answer)
                {
                    case 1: // первый выбор Формирование первой коллекции
                        {
                            firstCollection = CreateCollection(firstCollection);
                            Console.WriteLine("Полученная первая коллекция");
                            firstCollection.Print();
                            break;
                        }
                    case 2: // второй выбор Формирование второй коллекции
                        {
                            secondCollection = CreateCollection(secondCollection);
                            Console.WriteLine("Полученная вторая коллекция");
                            secondCollection.Print();
                            break;
                        }
                    case 3: // третий выбор печать первой
                        {
                            Console.WriteLine("Первый журнал:");
                            firstJournal.PrintJournal();
                            break;
                        }
                    case 4: // четвёртый выбор печать второй
                        {
                            Console.WriteLine("Второй журнал:");
                            secondJournal.PrintJournal();
                            break;
                        }
                    case 5: // пятый выбор добавить в первую            
                        {
                            firstCollection = AddItemCollection(firstCollection);
                            Console.WriteLine("Полученная первая коллекция:");
                            firstCollection.Print();
                            break;
                        }
                    case 6: // шестой выбор добавить во вторую          
                        {
                            secondCollection = AddItemCollection(secondCollection);
                            Console.WriteLine("Полученная вторая коллекция:");
                            secondCollection.Print();
                            break;
                        }
                    case 7: // седьмой выбор удаление элемента в первой коллекции
                        {
                            firstCollection = RemoveItemCollection(firstCollection);
                            Console.WriteLine("Полученная первая коллекция:");
                            firstCollection.Print();
                            break;
                        }
                    case 8: // восьмой выбор удаление элемента во второй коллекции
                        {
                            secondCollection = RemoveItemCollection(secondCollection);
                            Console.WriteLine("Полученная вторая коллекция:");
                            secondCollection.Print();
                            break;
                        }
                    case 9: // девятый выбор изменение элемента в первой коллекции
                        {
                            firstCollection = ChangeItemCollection(firstCollection);
                            Console.WriteLine("Полученная первая коллекция:");
                            firstCollection.Print();
                            break;
                        }
                    case 10: // десятый выбор изменение элемента во второй коллекции
                        {
                            secondCollection = ChangeItemCollection(secondCollection);
                            Console.WriteLine("Полученная вторая коллекция:");
                            secondCollection.Print();
                            break;
                        }
                    case 0: // программа закончит работу
                        {
                            Console.WriteLine("Выбор закрыт");
                            break;
                        }
                    default: // введённое число не подошло ни к одному пункту
                        {
                            Console.WriteLine("Неправильно задан пункт меню");
                            break;
                        }
                }
            } while (answer != 0);
        }
    }
}
