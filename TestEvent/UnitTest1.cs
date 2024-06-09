using System.Collections.ObjectModel;
using GeometrucShapeCarLibrary;
using Events_лаб_13;

namespace TestEvent
{
    [TestClass]
    public class MyObservableCollection
    {
        [TestMethod]
        public void AddItem_CollectionCountChangeEvent()
        {
            // Arrange
            MyObservableCollection<Shape> collection = new MyObservableCollection<Shape>("TestCollection", 0);
            bool eventHappened = false; // событие произошло?

            // Act
            collection.CollectionCountChanged += (sender, args) =>
            {
                eventHappened = true;
                Assert.AreEqual("добавлен элемент", args.ChangeType);
            };
            collection.Add(new Shape()); // метод, в котором генерируется событие

            // Assert
            Assert.IsTrue(eventHappened);
        }

        [TestMethod]
        public void RemoveItem_CollectionCountChangeEvent()
        {
            // Arrange
            MyObservableCollection<Shape> collection = new MyObservableCollection<Shape>("TestCollection", 0);
            var shape = new Shape();
            collection.Add(shape);
            bool eventHappened = false; // событие произошло?

            // Act
            collection.CollectionCountChanged += (sender, args) =>
            {
                eventHappened = true;
                Assert.AreEqual("удален элемент", args.ChangeType);
            };

            // Assert
            collection.Remove(shape);
            Assert.IsTrue(eventHappened);
        }

        [TestMethod]
        public void IndexerSet_ExistIndex_NullCollectionReferenceChangeEvent() // событие Null
        {
            // Arrange
            MyObservableCollection<Shape> collection = new MyObservableCollection<Shape>("TestCollection", 1);
            bool eventHappened = false; // событие произошло?

            // Act
            // не подписываем обработчика на событие

            // Assert
            collection[0] = new Shape();
            // подписываемся после обращения к событию
            collection.CollectionReferenceChanged += (sender, args) =>
            {
                eventHappened = true;
                Assert.AreEqual("изменён элемент на", args.ChangeType);
            };
            Assert.IsFalse(eventHappened);
        }

        [TestMethod]
        public void IndexerSet_ExistIndex_CollectionReferenceChangeEvent()
        {
            // Arrange
            MyObservableCollection<Shape> collection = new MyObservableCollection<Shape>("TestCollection", 1);
            bool eventHappened = false; // событие произошло?

            // Act
            collection.CollectionReferenceChanged += (sender, args) =>
            {
                eventHappened = true;
                Assert.AreEqual("изменён элемент на", args.ChangeType);
            };

            // Assert
            collection[0] = new Shape();
            Assert.IsTrue(eventHappened);
        }

        [TestMethod]
        public void IndexerSet_NotExistIndex_CollectionReferenceChangeEvent() // несуществующий индекс set
        {
            // Arrange
            MyObservableCollection<Shape> collection = new MyObservableCollection<Shape>("TestCollection", 1);
            bool eventHappened = false; // событие произошло?

            // Act
            collection.CollectionReferenceChanged += (sender, args) =>
            {
                eventHappened = true;
                Assert.AreEqual("изменён элемент на", args.ChangeType);
            };

            // Assert
            Assert.ThrowsException<IndexOutOfRangeException>(() => (collection[1000] = new Shape()));
            Assert.IsFalse(eventHappened);
        }

        [TestMethod]
        public void IndexerSet_NegativeIndex_CollectionReferenceChangeEvent() // отрицательный индекс set
        {
            // Arrange
            MyObservableCollection<Shape> collection = new MyObservableCollection<Shape>("TestCollection", 1);
            bool eventHappened = false; // событие произошло?

            // Act
            collection.CollectionReferenceChanged += (sender, args) =>
            {
                eventHappened = true;
                Assert.AreEqual("изменён элемент на", args.ChangeType);
            };

            // Assert
            Assert.ThrowsException<IndexOutOfRangeException>(() => (collection[-1000] = new Shape()));
            Assert.IsFalse(eventHappened);
        }

        [TestMethod]
        public void IndexerGet_ExistIndex_CollectionReferenceChangeEvent()
        {
            // Arrange
            MyObservableCollection<Shape> collection = new MyObservableCollection<Shape>("TestCollection", 0);

            // Act
            collection.Add(new Shape());

            // Assert
            Shape shape = collection[0];
            Assert.AreEqual(shape, new Shape());
        }

        [TestMethod]
        public void IndexerGet_NotExistIndex_CollectionReferenceChangeEvent() // несуществующий индекс get
        {
            // Arrange
            MyObservableCollection<Shape> collection = new MyObservableCollection<Shape>("TestCollection", 1);

            // Assert
            Assert.ThrowsException<IndexOutOfRangeException>(() => (collection[1000]));
        }

        [TestMethod]
        public void IndexerGet_NegativeIndex_CollectionReferenceChangeEvent() // отрицательный индекс get
        {
            // Arrange
            MyObservableCollection<Shape> collection = new MyObservableCollection<Shape>("TestCollection", 1);

            // Assert
            Assert.ThrowsException<IndexOutOfRangeException>(() => (collection[-1000]));
        }
    }

    [TestClass]
    public class CollectionHandlerEventArgsTests
    {
        [TestMethod]
        public void Constructor_InitializeProperties()
        {
            Shape item = new Shape();
            CollectionHandlerEventArgs args = new CollectionHandlerEventArgs("TestChange", item);

            Assert.AreEqual("TestChange", args.ChangeType);
            Assert.AreEqual(item, args.Item);
        }
    }

    [TestClass]
    public class JournalEntryTests
    {
        [TestMethod]
        public void Constructor_InitializeProperties()
        {
            JournalEntry entry = new JournalEntry("TestCollection", "TestChange", "TestData");

            Assert.AreEqual("TestCollection", entry.CollectionName);
            Assert.AreEqual("TestChange", entry.ChangeType);
            Assert.AreEqual("TestData", entry.Data);
        }

        [TestMethod]
        public void ToString_ReturnFormattedString()
        {
            JournalEntry entry = new JournalEntry("TestCollection", "TestChange", "TestData");
            string expectedString = "В коллекции TestCollection TestChange. Обьект: TestData";

            Assert.AreEqual(expectedString, entry.ToString());
        }

        [TestMethod]
        public void ToString_ReturnNull()
        {
            JournalEntry entry = new JournalEntry("TestCollection", "TestChange", null);
            string expectedString = "";

            Assert.AreEqual(expectedString, entry.ToString());
        }
    }

    [TestClass]
    public class JournalTests
    {
        [TestMethod]
        public void TestMethod_WriteRecord() //Добавление записи в журнал
        {
            // Arrange
            Journal<Shape> journal = new Journal<Shape>();
            MyObservableCollection<Shape> collection = new MyObservableCollection<Shape>("TestCollection",0);

            // Act
            Shape shape = new Shape();
            journal.WriteRecord(collection, new CollectionHandlerEventArgs("добавлен элемент", shape));

            // Assert
            Assert.AreEqual(1, journal.Count);
        }

        [TestMethod]
        public void TestMethod_AddJournal_CollectionCountChanged()
        {
            // Arrange
            MyObservableCollection<Shape> collection = new MyObservableCollection<Shape>("Numbers", 0);
            Journal<Shape> journal = new Journal<Shape>();
            int count = journal.Count;

            // Act
            collection.CollectionCountChanged += journal.WriteRecord; ;
            collection.Add(new Shape());
            count++;

            // Assert
            Assert.AreEqual(count, journal.Count);
        }
    }













    // ТЕСТЫ ДЛЯ MyCollection
    [TestClass]
    public class UnitTestCollection
    {
        [TestMethod]
        public void Clear_RemovesAllElementsFromCollection() // удаление коллекции
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>();
            Shape element1 = new Shape("елемент1", 1);
            Shape element2 = new Shape("елемент2", 2);
            collection.Add(element1);
            collection.Add(element2);

            // Act
            collection.Clear();

            // Assert
            Assert.AreEqual(0, collection.Count);
            Assert.IsFalse(collection.Contains(element1));
            Assert.IsFalse(collection.Contains(element2));
        }

        [TestMethod]
        public void CopyTo_CopiesElementsToArray() // копирование элементов коллекции в массив
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>(1);
            collection.Add(new Shape("елемент1", 1));
            collection.Add(new Shape("елемент2", 2));
            int index = 0;
            Shape[] array = new Shape[collection.Count + index];

            // Act
            collection.CopyTo(array, index);

            // Assert
            int i = 0;
            foreach (Shape shape in collection)
            {
                Assert.AreEqual(shape, array[i]);
                i++;
            }
        }

        [TestMethod]
        public void CopyTo_ThrowsArgumentNullException_ArrayNull() // Исключение для null-массива в CopyTo
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>();
            Shape[] array = null;

            // Act and Assert
            Assert.ThrowsException<ArgumentNullException>(() => collection.CopyTo(array, 0));
        }

        [TestMethod]
        public void CopyTo_ThrowsArgumentOutOfRangeException_IndexIsNegative() // Исключение для отрицательного индекса в CopyTo
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>();
            Shape[] array = new Shape[10];

            // Act and Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => collection.CopyTo(array, -1));
        }

        [TestMethod]
        public void CopyTo_ThrowsArgumentException_ArrayTooSmall() // Исключение в CopyTo, если в массив не уместятся все элементы в коллекции
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>();
            Shape element1 = new Shape("елемент1", 1);
            Shape element2 = new Shape("елемент2", 2);
            collection.Add(element1);
            collection.Add(element2);
            Shape[] array = new Shape[1];

            // Act and Assert
            Assert.ThrowsException<ArgumentException>(() => collection.CopyTo(array, 0));
        }

        [TestMethod]
        public void Constructor_CreatesCollectionWithSameElements() // конструктор копирования
        {
            // Arrange
            MyCollection<Shape> originalCollection = new MyCollection<Shape>();
            Shape element1 = new Shape("елемент1", 1);
            Shape element2 = new Shape("елемент2", 2);
            originalCollection.Add(element1);
            originalCollection.Add(element2);

            // Act
            MyCollection<Shape> newCollection = new MyCollection<Shape>(originalCollection);

            // Assert
            Assert.AreEqual(originalCollection.Count, newCollection.Count);

            foreach (Shape item in originalCollection)
            {
                Assert.IsTrue(newCollection.Contains(item));
            }
        }

        [TestMethod]
        public void TestRemoveItemCollection_Existent() // Удаление существующего в хэш-таблице элемента
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>();
            collection.Add(new Shape("элемент", 1));
            collection.Add(new Shape("элемент", 2));

            // Act
            bool removed = collection.Remove(new Shape("элемент", 2));

            // Assert
            Assert.IsTrue(removed);
            Assert.IsFalse(collection.Contains(new Shape("элемент", 2))); // убедимся, что элемент удалён
        }

        [TestMethod]
        public void TestRemoveItemCollection_NonExistent() // Удаление НЕ существующего в хэш-таблице элемента
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>();
            collection.AddItem(new Shape("элемент", 1));
            collection.AddItem(new Shape("элемент", 2));

            // Act
            bool notAdded = collection.Remove(new Shape("элемент", 100)); // попытка удаления элемента, который не был добавлен

            // Assert
            Assert.IsFalse(notAdded); // удаление не произошло
        }

        [TestMethod]
        public void DeepCopyCollection() // тест для глубокой копии коллекции
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>();
            for (int i = 0; i < 5; i++) // случайно инициализируем исходную коллекцию
            {
                Shape s = new Shape();
                s.RandomInit();
                collection.Add(s); // добавляем в хэш-таблицу
            }
            MyCollection<Shape> deepCopy = collection.DeepCopy();

            // Act
            foreach (Shape shape in deepCopy) // Изменим все значения в поверхностной копии на <Бабочка>
                shape.Name = "Бабочка";

            // Assert
            // только в глубокой копии элементы должны были поменять название, в исходной должны были остаться без изменений
            foreach (Shape shape in collection)
            {
                Assert.AreNotEqual(shape.Name, "Бабочка");
            }
            foreach (Shape shape in deepCopy)
            {
                Assert.AreEqual(shape.Name, "Бабочка");
            }
        }

        [TestMethod]
        public void ShallowCopyCollection() // тест для поверхностной копии коллекции
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>();
            for (int i = 0; i < 5; i++) // случайно инициализируем исходную коллекцию
            {
                Shape s = new Shape();
                s.RandomInit();
                collection.Add(s); // добавляем в хэш-таблицу
            }
            MyCollection<Shape> shallowCopy = collection.ShallowCopy();

            // Act
            foreach (Shape shape in shallowCopy) // Изменим все значения в поверхностной копии на <Бабочка>
                shape.Name = "Бабочка";

            // Assert
            // и в исходной коллекции, и в поверхностной копии все элементы должны иметь название "Бабочка"
            foreach (Shape shape in collection)
            {
                Assert.AreEqual(shape.Name, "Бабочка");
            }
            foreach (Shape shape in shallowCopy)
            {
                Assert.AreEqual(shape.Name, "Бабочка");
            }
        }

        [TestMethod]
        public void TestEnumerator()
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>(3);
            int exceptedCount = 2;

            // Act
            Shape element1 = new Shape("елемент1", 1);
            Shape element2 = new Shape("елемент2", 2);
            collection.Add(element1);
            collection.Add(element2);

            // Assert
            int i = 0;
            foreach (Shape shape in collection)
            {
                i++;
                if (shape.id.Number == i) Assert.AreEqual(shape, new Shape("елемент" + i.ToString(), i));
            }
            Assert.AreEqual(exceptedCount, i);
        }

        [TestMethod]
        public void IsReadOnly_ReturnsFalse() // тест для свойства isReadOnly
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>();

            // Act
            bool isReadOnly = collection.IsReadOnly;

            // Assert
            Assert.IsFalse(isReadOnly);
        }















        // ТЕСТЫ ДЛЯ ХЭШ ТАБЛИЦЫ
        [TestClass]
        public class UnitTestHash
        {
            [TestMethod]
            public void TestAddItem_DifferentObjects() // Добавление отличающихся друг от друга объектов
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>();
                int expectedCount = 3;

                // Act
                hashTable.AddItem(new Shape("элемент", 1));
                hashTable.AddItem(new Shape("элемент", 2));
                hashTable.AddItem(new Shape("элемент", 3));

                // Assert
                // все элементы разные - должны все добавиться
                Assert.AreEqual(expectedCount, hashTable.Count);
            }

            [TestMethod]
            public void TestAddItem_ZeroCapacity() // Добавление элемента в хэш-таблицу нулевой размерности
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>(0);
                int expectedCount = 3;

                // Act
                hashTable.AddItem(new Shape("элемент", 1));
                hashTable.AddItem(new Shape("элемент", 2));
                hashTable.AddItem(new Shape("элемент", 3));

                // Assert
                // все элементы разные - должны все добавиться
                Assert.AreEqual(expectedCount, hashTable.Count);
            }

            [TestMethod]
            public void TestAddItem_SimilarObjects() // Добавление одинаковых объектов
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>();
                int expectedCount = 1;

                // Act
                hashTable.AddItem(new Shape("элемент", 0));
                hashTable.AddItem(new Shape("элемент", 0));
                hashTable.AddItem(new Shape("элемент", 0));

                // Assert
                // все элементы одинаковые - в хэш-таблице должен быть один элемент
                Assert.AreEqual(expectedCount, hashTable.Count);

            }

            [TestMethod]
            public void TestRemoveItem_Existent() // Удаление существующего в хэш-таблице элемента
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>();
                hashTable.AddItem(new Shape("элемент", 1));
                hashTable.AddItem(new Shape("элемент", 2));

                // Act
                bool removed = hashTable.RemoveData(new Shape("элемент", 2));

                // Assert
                Assert.IsTrue(removed);
                Assert.IsFalse(hashTable.Contains(new Shape("элемент", 2))); // убедимся, что элемент удалён
            }

            [TestMethod]
            public void TestRemoveItem_NonExistent() // Удаление НЕ существующего в хэш-таблице элемента
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>();
                hashTable.AddItem(new Shape("элемент", 1));
                hashTable.AddItem(new Shape("элемент", 2));

                // Act
                bool notAdded = hashTable.RemoveData(new Shape("элемент", 100)); // попытка удаления элемента, который не был добавлен

                // Assert
                Assert.IsFalse(notAdded); // удаление не произошло
            }

            [TestMethod]
            public void TestContains_Existent() // Проверка присутствия существующего в хэш-таблице элемента
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>();
                hashTable.AddItem(new Shape("элемент", 1));
                hashTable.AddItem(new Shape("элемент", 2));

                // Act
                bool isExist = hashTable.Contains(new Shape("элемент", 1));

                // Assert
                Assert.IsTrue(isExist); // элемент есть в таблице
            }

            [TestMethod]
            public void TestContains_NonExistent() // Проверка отсутствия НЕ существующего в хэш-таблице элемента
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>();
                hashTable.AddItem(new Shape("элемент", 1));
                hashTable.AddItem(new Shape("элемент", 2));

                // Act
                bool isNotExist = hashTable.Contains(new Shape("элемент", 100)); // попытка найти отсутствующий элемент

                // Assert
                Assert.IsFalse(isNotExist); // элемента нет в таблице
            }

            [TestMethod]
            public void TestFindItem_NonExistent() // Поиск НЕ существующего в хэш-таблице элемента
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>();
                hashTable.AddItem(new Shape("элемент", 1));
                hashTable.AddItem(new Shape("элемент", 2));

                // Act
                int NotExist = hashTable.FindItem(new Shape("элемент", 100)); // попытка найти отсутствующий элемент

                // Assert
                Assert.IsTrue(NotExist == -1); // элемента нет в таблице
            }

            [TestMethod]
            public void TestResize() // Превышение коэффициента заполненности и увеличение размерности хэш-таблицы
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>(2); // начальный размер 2
                int expectedSize = 4;

                // Act
                hashTable.AddItem(new Shape("элемент", 1));
                hashTable.AddItem(new Shape("элемент", 2));

                // Assert
                Assert.AreEqual(expectedSize, hashTable.Capacity); // размер должен увеличиться в два раза, до 4
            }

            [TestMethod]
            public void TestAddItem_NullObject() // Добавление null-объекта
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>();
                int expectedCount = 0;

                // Act
                hashTable.AddItem(null);

                // Assert
                Assert.AreEqual(expectedCount, hashTable.Count);
            }

            [TestMethod]
            public void TestAddItem_СollisionAfter() // Коллизия и вставка элемента ПОСЛЕ своего индекса
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>(10, 1); // чтобы увеличить возможность коллизии, увеличим коэффициент заполненности таблицы
                int place; // место, куда встанет элемент, претерпевший коллизию
                Shape temp = new Shape(); // с помощью данной переменной запомним этот коллизионный элемент

                // Act
                while (true)
                {
                    // создаём новый элемент для добавления
                    Shape shape = new Shape();
                    shape.RandomInit();
                    // произошла коллизия:
                    if (hashTable.flags[hashTable.GetIndex(shape)] == 1)
                    {
                        // ищем место, куда в итоге встанет добавляемый элемент
                        place = hashTable.SearchPlace(hashTable.GetIndex(shape));
                        // если он встал после предназначенного ему места, то выходим из цикла
                        if (place > hashTable.GetIndex(shape))
                        {
                            temp = shape;
                            break;
                        }
                    }
                    // добавляем очередной элемент
                    hashTable.AddItem(shape);
                }

                // Assert
                Assert.IsTrue(hashTable.flags[hashTable.GetIndex(temp)] == 1); // назначенное место найденного элемента для нашего тестируемого случая оказалось занято
                Assert.IsTrue(place > hashTable.GetIndex(temp)); // встал после назначенного места
            }

            [TestMethod]
            public void TestAddItem_СollisionBefore() // Коллизия и вставка элемента ДО своего индекса
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>(10, 1); // чтобы увеличить возможность коллизии, увеличим коэффициент заполненности таблицы
                int place; // место, куда встанет элемент, претерпевший коллизию
                Shape temp = new Shape(); // с помощью данной переменной запомним этот коллизионный элемент

                // Act
                while (true)
                {
                    // создаём новый элемент для добавления
                    Shape shape = new Shape();
                    shape.RandomInit();
                    // произошла коллизия:
                    if (hashTable.flags[hashTable.GetIndex(shape)] == 1)
                    {
                        // ищем место, куда в итоге встанет добавляемый элемент
                        place = hashTable.SearchPlace(hashTable.GetIndex(shape));
                        // если он встал после предназначенного ему места, то выходим из цикла
                        if (place < hashTable.GetIndex(shape))
                        {
                            temp = shape;
                            break;
                        }
                    }
                    // добавляем очередной элемент
                    hashTable.AddItem(shape);
                }

                // Assert
                Assert.IsTrue(hashTable.flags[hashTable.GetIndex(temp)] == 1); // назначенное место найденного элемента для нашего тестируемого случая оказалось занято
                Assert.IsTrue(place < hashTable.GetIndex(temp)); // встал после назначенного места
            }

            [TestMethod]
            public void TestAddItem_СollisionBefore_NotBegin() // Коллизия и вставка элемента ДО своего индексаб но не в начало таблицы
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>(10, 1); // чтобы увеличить возможность коллизии, увеличим коэффициент заполненности таблицы
                int place; // место, куда встанет элемент, претерпевший коллизию
                Shape temp = new Shape(); // с помощью данной переменной запомним этот коллизионный элемент

                // Act
                while (true)
                {
                    // создаём новый элемент для добавления
                    Shape shape = new Shape();
                    shape.RandomInit();
                    // произошла коллизия:
                    if (hashTable.flags[hashTable.GetIndex(shape)] == 1)
                    {
                        // ищем место, куда в итоге встанет добавляемый элемент
                        place = hashTable.SearchPlace(hashTable.GetIndex(shape));
                        // если он встал после предназначенного ему места, то выходим из цикла
                        if (place < hashTable.GetIndex(shape) && place != 0)
                        {
                            temp = shape;
                            break;
                        }
                    }
                    // добавляем очередной элемент
                    hashTable.AddItem(shape);
                }

                // Assert
                Assert.IsTrue(hashTable.flags[hashTable.GetIndex(temp)] == 1); // назначенное место найденного элемента для нашего тестируемого случая оказалось занято
                Assert.IsTrue(place < hashTable.GetIndex(temp)); // встал после назначенного места
            }

            [TestMethod]
            public void TestFindExistItem_СollisionAfter() // Коллизия и поиск элемента, вставленного ПОСЛЕ своего индекса
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>(10, 1); // чтобы увеличить возможность коллизии, увеличим коэффициент заполненности таблицы
                int place; // место, куда встанет элемент, претерпевший коллизию
                Shape temp = new Shape(); // с помощью данной переменной запомним этот коллизионный элемент

                // Act
                while (true)
                {
                    // создаём новый элемент для добавления
                    Shape shape = new Shape();
                    shape.RandomInit();
                    // произошла коллизия:
                    if (hashTable.flags[hashTable.GetIndex(shape)] == 1)
                    {
                        // ищем место, куда в итоге встанет добавляемый элемент
                        place = hashTable.SearchPlace(hashTable.GetIndex(shape));
                        // если он встал после предназначенного ему места, то выходим из цикла
                        if (place > hashTable.GetIndex(shape))
                        {
                            hashTable.AddItem(shape);
                            temp = shape;
                            break;
                        }
                    }
                    // добавляем очередной элемент
                    hashTable.AddItem(shape);
                }

                // Assert
                Assert.IsTrue(hashTable.FindItem(temp) > hashTable.GetIndex(temp)); // Поиск выдаст индекс после назначенного места
            }

            [TestMethod]
            public void TestFindExistItem_СollisionBefore() // Коллизия и поиск элемента, вставленного ДО своего индекса
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>(10, 1); // чтобы увеличить возможность коллизии, увеличим коэффициент заполненности таблицы
                int place; // место, куда встанет элемент, претерпевший коллизию
                Shape temp = new Shape(); // с помощью данной переменной запомним этот коллизионный элемент

                // Act
                while (true)
                {
                    // создаём новый элемент для добавления
                    Shape shape = new Shape();
                    shape.RandomInit();
                    // произошла коллизия:
                    if (hashTable.flags[hashTable.GetIndex(shape)] == 1)
                    {
                        // ищем место, куда в итоге встанет добавляемый элемент
                        place = hashTable.SearchPlace(hashTable.GetIndex(shape));
                        // если он встал после предназначенного ему места, то выходим из цикла
                        if (place < hashTable.GetIndex(shape))
                        {
                            hashTable.AddItem(shape);
                            temp = shape;
                            break;
                        }
                    }
                    // добавляем очередной элемент
                    hashTable.AddItem(shape);
                }

                // Assert
                Assert.IsTrue(hashTable.FindItem(temp) < hashTable.GetIndex(temp)); // Поиск выдаст индекс до назначенного места
            }

            [TestMethod]
            public void TestFindExistItem_СollisionBefore_NotZeroAndNotFirstPosition() // Коллизия и поиск элемента, вставленного ДО своего индекса, но не в первые две позиции таблицы
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>(1000, 1); // чтобы увеличить возможность коллизии, увеличим коэффициент заполненности таблицы
                int place; // место, куда встанет элемент, претерпевший коллизию
                Shape temp = new Shape(); // с помощью данной переменной запомним этот коллизионный элемент

                // Act
                while (true)
                {
                    // создаём новый элемент для добавления
                    Shape shape = new Shape();
                    shape.RandomInit();
                    // произошла коллизия:
                    if (hashTable.flags[hashTable.GetIndex(shape)] == 1)
                    {
                        // ищем место, куда в итоге встанет добавляемый элемент
                        place = hashTable.SearchPlace(hashTable.GetIndex(shape));
                        // если он встал после предназначенного ему места, то выходим из цикла
                        if (place < hashTable.GetIndex(shape) && place > 1)
                        {
                            hashTable.AddItem(shape);
                            temp = shape;
                            break;
                        }
                    }
                    // добавляем очередной элемент
                    hashTable.AddItem(shape);
                }
                //hashTable.RemoveData(hashTable.table[hashTable.GetIndex(temp) - 5]);

                // Assert
                Assert.IsTrue(place < hashTable.GetIndex(temp) && place > 1); // Поиск выдаст индекс до назначенного места (но не первые две позиции таблицы)
            }

            [TestMethod]
            public void TestFindNotExistItem_Сollision() // Поиск не существующего элемента, претерпевшего коллизию
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>(100, 1.5); // увеличим коэффициент заполненности таблицы, чтобы заполнить её полностью, а затем искать в полностью заполненной таблице не существующий элемент
                Shape notExistShape = new Shape("Звёздочка", 777); // создадим не существующий элемент, который будем искать

                // Act
                // ПОЛНОСТЬЮ заполняем таблицу. Добиваемся того, чтобы при поиске несуществующего элемента происходила коллизия в ПОЛНОСТЬЮ заполненной таблице
                while (hashTable.Capacity != hashTable.Count)
                {
                    // создаём новый элемент для добавления
                    Shape shape = new Shape();
                    shape.RandomInit();
                    // добавляем
                    hashTable.AddItem(shape);
                }

                // Assert
                Assert.IsTrue(hashTable.FindItem(notExistShape) == -1); // Поиск выдаст -1, так как элемента в таблице нет
            }
        }
    }
}