using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometrucShapeCarLibrary;

namespace Events_лаб_13
{
    /// <summary>
    /// хэш-таблица(открытая адресация), реализующая интерфейс ICollection
    /// </summary>
    /// <typeparam name="T">обобщённый тип данных</typeparam>
    public class MyCollection<T> : MyHashTable<T>, ICollection<T>, IEnumerable<T> where T : IInit, ICloneable, IComparable, new()
    {
        /// <summary>
        /// свойство только для чтения
        /// </summary>
        public bool IsReadOnly => false; // Коллекция доступна не только для чтения

        public MyCollection()
        {
            table = new T[10];
            fillRatio = 0.72;
            flags = new int[10]; // в flags столько же ячеек, сколько и в хэш-таблице
        }

        public MyCollection(int length, double fillRatio = 0.72)
        {
            table = new T[length];
            this.fillRatio = fillRatio;
            flags = new int[length]; // в flags столько же ячеек, сколько и в хэш-таблице
        }

        public MyCollection(MyCollection<T> c, double fillRatio = 0.72)
        {
            table = new T[c.Count];
            this.fillRatio = fillRatio;
            flags = new int[c.Count]; // в flags столько же ячеек, сколько и в хэш-таблице
            foreach (T item in c)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Добавление элемента (с проверкой на заполненность хэш-таблицы)
        /// </summary>
        /// <param name="item">добавляемый элемент</param>
        public void Add(T item)
        {
            AddItem(item);
        }

        /// <summary>
        /// Очистка хэш-таблицы
        /// </summary>
        public void Clear()
        {
            table = new T[table.Length];
            flags = new int[flags.Length];
            count = 0;
        }

        /// <summary>
        /// Удаление заданного элемента из хэш-таблицы
        /// </summary>
        /// <param name="data">удаляемый элемент</param>
        public bool Remove(T item)
        {
            return RemoveData(item);
        }

        /// <summary>
        /// Копирование элементов хэш-таблицы в массив
        /// </summary>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) // массив является нулевой ссылкой
                throw new ArgumentNullException();
            if (arrayIndex < 0) // индекс не может быть отрицательным
                throw new ArgumentOutOfRangeException();
            if (array.Length - arrayIndex < Count) // в массиве не хватит места для всех копируемых элементов
                throw new ArgumentException();
            int copiedCount = 0; // будем считать количество скопируемых элементов
            for (int i = 0; i < table.Length; i++) // идём по хэш-таблице
            {
                if (flags[i] == 1) // копируем только занятые ячейки
                {
                    array[arrayIndex + copiedCount] = table[i]; // копируем в массив элемент из хэш-таблицы
                    copiedCount++;
                }
            }
        }

        /// <summary>
        /// Поверхностное копирование коллекции
        /// </summary>
        /// <returns>Новая коллекция с поверхностным копированием элементов</returns>
        public MyCollection<T> ShallowCopy()
        {
            // Создаём новую пустую коллекцию той же размерности и с тем же коэффициентом заполняемости
            MyCollection<T> newCollection = new MyCollection<T>(Capacity, fillRatio);
            newCollection.count = count; // копируем количество записей
            // проходимся по массиву и переписываем, клонируя, все значения
            for (int i = 0; i < table.Length; i++)
            {
                newCollection.table[i] = table[i]; // прикрепляем ссылку на исходный объект
                newCollection.flags[i] = flags[i]; // прикрепляем ссылку на исходный флаг
            }
            return newCollection; // возвращаем поверхностную копию
        }

        /// <summary>
        /// Глубокое копирование коллекции
        /// </summary>
        /// <returns>Новая коллекция с глубоким копированием элементов</returns>
        public MyCollection<T> DeepCopy()
        {
            // Создаём новую пустую коллекцию той же размерности и с тем же коэффициентом заполняемости
            MyCollection<T> newCollection = new MyCollection<T>(Capacity, fillRatio);
            // проходимся по массиву и переписываем, клонируя, все значения
            for (int i = 0; i < table.Length; i++)
            {
                if (flags[i] == 1 && table[i] != null) // если ячейка занята
                {
                    newCollection.table[i] = (T)table[i].Clone(); // клонируем значение ячейки
                    newCollection.flags[i] = 1; // фиксируем занятость ячейки
                }
                else // иначе, если элемента в ячейке нет
                {
                    newCollection.flags[i] = flags[i]; // то просто фиксируем состояние ячейки 
                }
            }
            newCollection.count = count; // копируем количество записей
            return newCollection; // возвращаем глубокую копию
        }

        /// <summary>
        /// Получение перечислителя для элементов хэш-таблицы (обобщённный)
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < table.Length; i++) // движемся по хэш-таблице
            {
                if (flags[i] == 1) // возвращаем только занятые ячейки
                {
                    yield return table[i]; // возвращаем значения объекта
                }
            }
        }

        /// <summary>
        /// Получение перечислителя для элементов хэш-таблицы (необобщённый)
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
