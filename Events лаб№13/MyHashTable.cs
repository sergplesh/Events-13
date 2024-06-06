using GeometrucShapeCarLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Events_лаб_13
{
    /// <summary>
    /// хэш-таблица по типу хэш-множества с открытой адресацией
    /// </summary>
    /// <typeparam name="T">обобщённый тип</typeparam>
    public class MyHashTable<T> where T : IInit, IComparable, ICloneable, new()
    {
        /// <summary>
        /// Taбличка
        /// </summary>
        protected T[] table;
        /// <summary>
        /// кол-во записей в хэш-таблице
        /// </summary>
        protected int count = 0;
        /// <summary>
        /// коэффициент заполняемости хэш-таблицы
        /// </summary>
        protected double fillRatio; // превысив данный коэффициент - увеличиваем размерность таблицы

        /// <summary>
        /// состояния ячеек хэш-таблицы
        /// </summary>
        public int[] flags; // СДЕЛАЛ PUBLIC ДЛЯ ТЕСТИРОВАНИЯ
                            // с помощью данного массива будем отлавливать состояние ячейки в хэш таблице:
                            // "0" - пустая; "1" - занята; "2" - в ячейке был удалён элемент

        /// <summary>
        /// ёмкость хэш-таблицы
        /// </summary>
        public int Capacity => table.Length; //емкость, кол-во выделенной памяти
        /// <summary>
        /// текущее кол-во элементов в хэш-таблице
        /// </summary>
        public int Count => count; //текущее кол-во элементов

        /// <summary>
        /// конструктор (по умолчанию: размер 10, коэффициент заполненности 72%(рекомендация Microsoft))
        /// </summary>
        /// <param name="size">размер создаваемой хэш-таблицы</param>
        /// <param name="fillRatio">коэффициент заполняемости хэш-таблицы</param>
        public MyHashTable(int size = 10, double fillRatio = 0.72)
        {
            table = new T[size];
            this.fillRatio = fillRatio;
            flags = new int[size]; // в flags столько же ячеек, сколько и в хэш-таблице
        }

        /// <summary>
        /// Печать хэш-таблицы
        /// </summary>
        public void Print()
        {
            if (Count != 0)
            {
                int i = 0; // позиция элемента в хэш-таблице
                           // выводим 
                foreach (T item in table)
                {
                    Console.Write($"{i}: ");
                    if (item != null) //Не пустая ссылка
                    {
                        Console.WriteLine(item); //Вывод элемента
                    }
                    else Console.WriteLine(); // если пустая ссылка - ничего не выводим
                    i++;
                }
            }
            else Console.WriteLine("В хэш-таблице отсутствуют элементы");
        }

        /// <summary>
        /// Получение индекса в хэш-таблице для элемента
        /// </summary>
        /// <param name="data">элемент</param>
        /// <returns>результат хэш-функции</returns>
        public int GetIndex(T data) // private (так как вспомогательный метод в рамках данного класса), но public для тестирования
        {
            return Math.Abs(data.GetHashCode()) % Capacity; // высчитываем хэш-код элемента по модулю размерности хэш-таблицы
        }

        /// <summary>
        /// Проверка наличия элемента в хэш-таблице
        /// </summary>
        /// <param name="data">искомый элемент</param>
        /// <returns></returns>
        public bool Contains(T data)
        {
            return (FindItem(data) >= 0); // элемент есть, если вернулся конкретный индекс(неотрицательное число)
        }

        /// <summary>
        /// Удаление заданного элемента из хэш-таблицы
        /// </summary>
        /// <param name="data">удаляемый элемент</param>
        /// <returns></returns>
        public bool RemoveData(T data)
        {
            // ищем место в таблице, где расположен элемент, который хотим удалить
            int index = FindItem(data);
            // элемента в таблице не оказалось:
            if (index < 0) return false; // удаление не произошло
            // нашли - удалили:
            count--; // уменьшили количество элементов, содержащихся в таблице
            table[index] = default; // обнуляем ссылку
            flags[index] = 2; // состояние ячейки с данным индексом - "в ячейке был удалён элемент"
            return true; // удаление произошло
        }

        /// <summary>
        /// Добавление элемента (с проверкой на заполненность хэш-таблицы)
        /// </summary>
        /// <param name="item">добавляемый элемент</param>
        public bool AddItem(T item)
        {
            if (item == null) return false;
            // случай если размерность равна нулю
            if (Capacity == 0) // если размерность была равна нулю, то устанавливаем размерность равную 1
            {
                table = new T[1]; // пустая хэш-таблица вдвое увеличенной размерности
                flags = new int[1]; // обнуляем состояния всех ячеек (так как сейчас все ячейки "пусты")
                AddData(item); // сразу добавляем заданный элемент
                return true; // и возвращаем true
            }
            // если такой элемент уже есть в хэш-таблице, то не добавляем
            if (Contains(item)) return false;
            // превысили коэффициент заполненности
            if (((double)(Count + 1) / Capacity) > fillRatio)
            {
                // увеличиваем таблицу в 2 раза и переписываем всю информацию
                T[] temp = (T[])table.Clone(); // сохраняем изначальную хэш-таблицу: из неё будем переписывать элементы в новую
                int newSize = temp.Length * 2; // новая размерность
                table = new T[newSize]; // пустая хэш-таблица вдвое увеличенной размерности
                count = 0; // обнуляем количество записей
                flags = new int[newSize]; // обнуляем состояния всех ячеек (так как сейчас все ячейки "пусты")
                // переписываем все элементы, содержавшиеся в изначальной(не увеличенной) таблице
                for (int i = 0; i < temp.Length; i++)
                    AddData(temp[i]);
            }
            //добавляем новый элемент (не дубликат и не null)
            AddData(item);
            return true; // добавление произошло
        }

        /// <summary>
        /// Добаление заданного элемента в хэш-таблицу (вспомогательный метод внутри класса)
        /// </summary>
        /// <param name="data">добавляемый элемент</param>
        void AddData(T data)
        {
            if (data == null) return; // добавляется пустой элемент
            T item = (T)data.Clone(); // выделяем память под элемент
            // ищем место
            int index = GetIndex(item);
            int current = index;
            // если ячейка не "свободна"
            if (flags[index] != 0)
            {
                current = SearchPlace(index);
            }
            // пустое место найдено - добавляем
            table[current] = item;
            count++; // увеличиваем количество элементов в хэш-таблице
            flags[current] = 1; // изменяем состояние ячейки, в которую добавили элемент, на "занято"
        }

        /// <summary>
        /// Поиск места для добавляемого элемента в случае коллизии
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int SearchPlace(int index) // ВЫДЕЛИЛ ДАННЫЙ МЕТОД ДЛЯ ТЕСТИРОВАНИЯ
        {
            int current = index;
            // идём до конца таблицы или до первого пустого места
            while (current < table.Length && table[current] != null)
                current++;
            // если таблица закончилась
            if (current == table.Length)
            {
                // идём от начала до индекса, который запомнили
                current = 0;
                while (current < index && table[current] != null)
                    current++;
            }
            return current;
        }

        /// <summary>
        /// Поиск элемента в хэш-таблице
        /// </summary>
        /// <param name="data">искомый элемент</param>
        /// <returns></returns>
        public int FindItem(T data) // СДЕЛАЛ PUBLIC ДЛЯ ТЕСТИРОВАНИЯ
        {
            // находим индекс:
            int index = GetIndex(data);
            // если в той ячейке, где должен находиться искомый элемент, состояние "пусто",
            // значит не было попыток добавить сюда какие-либо элементы - а значит и искомого элемента в хэш-таблице не может быть
            if (flags[index] == 0) return -1;
            // есть элемент, совпадает - сразу выдаём его индекс в хэш-таблице как результат
            if (flags[index] == 1 && table[index].Equals(data))
                return index;
            else // если не совпадает - линейное пробирование
            {
                // ВАЖНО: если натыкаемся на пустое место, а элемент так и не нашли - значит и не найдём
                int current = index; // идём вниз по таблице
                while (current < table.Length)
                {
                    if (flags[current] == 0) return -1; // наткнулись на "пусто" - искомого элемента нет в хэш-таблице
                    if (flags[current] == 1 && table[current].Equals(data)) // совпадает
                        return current;
                    current++;
                }
                current = 0; // идём сначала таблицы
                while (current < index)
                {
                    if (flags[current] == 0) return -1; // наткнулись на "пусто" - искомого элемента нет в хэш-таблице
                    else if (flags[current] == 1 && table[current].Equals(data)) // совпадает
                        return current;
                    else current++;
                }
            }
            // не нашли
            return -1;
        }
    }
}
