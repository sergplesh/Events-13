using System.Collections.ObjectModel;
using GeometrucShapeCarLibrary;
using Events_���_13;

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
            bool eventHappened = false; // ������� ���������?

            // Act
            collection.CollectionCountChanged += (sender, args) =>
            {
                eventHappened = true;
                Assert.AreEqual("�������� �������", args.ChangeType);
            };
            collection.Add(new Shape()); // �����, � ������� ������������ �������

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
            bool eventHappened = false; // ������� ���������?

            // Act
            collection.CollectionCountChanged += (sender, args) =>
            {
                eventHappened = true;
                Assert.AreEqual("������ �������", args.ChangeType);
            };

            // Assert
            collection.Remove(shape);
            Assert.IsTrue(eventHappened);
        }

        [TestMethod]
        public void IndexerSet_ExistIndex_NullCollectionReferenceChangeEvent() // ������� Null
        {
            // Arrange
            MyObservableCollection<Shape> collection = new MyObservableCollection<Shape>("TestCollection", 1);
            bool eventHappened = false; // ������� ���������?

            // Act
            // �� ����������� ����������� �� �������

            // Assert
            collection[0] = new Shape();
            // ������������� ����� ��������� � �������
            collection.CollectionReferenceChanged += (sender, args) =>
            {
                eventHappened = true;
                Assert.AreEqual("������ ������� ��", args.ChangeType);
            };
            Assert.IsFalse(eventHappened);
        }

        [TestMethod]
        public void IndexerSet_ExistIndex_CollectionReferenceChangeEvent()
        {
            // Arrange
            MyObservableCollection<Shape> collection = new MyObservableCollection<Shape>("TestCollection", 1);
            bool eventHappened = false; // ������� ���������?

            // Act
            collection.CollectionReferenceChanged += (sender, args) =>
            {
                eventHappened = true;
                Assert.AreEqual("������ ������� ��", args.ChangeType);
            };

            // Assert
            collection[0] = new Shape();
            Assert.IsTrue(eventHappened);
        }

        [TestMethod]
        public void IndexerSet_NotExistIndex_CollectionReferenceChangeEvent() // �������������� ������ set
        {
            // Arrange
            MyObservableCollection<Shape> collection = new MyObservableCollection<Shape>("TestCollection", 1);
            bool eventHappened = false; // ������� ���������?

            // Act
            collection.CollectionReferenceChanged += (sender, args) =>
            {
                eventHappened = true;
                Assert.AreEqual("������ ������� ��", args.ChangeType);
            };

            // Assert
            Assert.ThrowsException<IndexOutOfRangeException>(() => (collection[1000] = new Shape()));
            Assert.IsFalse(eventHappened);
        }

        [TestMethod]
        public void IndexerSet_NegativeIndex_CollectionReferenceChangeEvent() // ������������� ������ set
        {
            // Arrange
            MyObservableCollection<Shape> collection = new MyObservableCollection<Shape>("TestCollection", 1);
            bool eventHappened = false; // ������� ���������?

            // Act
            collection.CollectionReferenceChanged += (sender, args) =>
            {
                eventHappened = true;
                Assert.AreEqual("������ ������� ��", args.ChangeType);
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
        public void IndexerGet_NotExistIndex_CollectionReferenceChangeEvent() // �������������� ������ get
        {
            // Arrange
            MyObservableCollection<Shape> collection = new MyObservableCollection<Shape>("TestCollection", 1);

            // Assert
            Assert.ThrowsException<IndexOutOfRangeException>(() => (collection[1000]));
        }

        [TestMethod]
        public void IndexerGet_NegativeIndex_CollectionReferenceChangeEvent() // ������������� ������ get
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
            string expectedString = "� ��������� TestCollection TestChange. ������: TestData";

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
        public void TestMethod_WriteRecord() //���������� ������ � ������
        {
            // Arrange
            Journal<Shape> journal = new Journal<Shape>();
            MyObservableCollection<Shape> collection = new MyObservableCollection<Shape>("TestCollection",0);

            // Act
            Shape shape = new Shape();
            journal.WriteRecord(collection, new CollectionHandlerEventArgs("�������� �������", shape));

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













    // ����� ��� MyCollection
    [TestClass]
    public class UnitTestCollection
    {
        [TestMethod]
        public void Clear_RemovesAllElementsFromCollection() // �������� ���������
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>();
            Shape element1 = new Shape("�������1", 1);
            Shape element2 = new Shape("�������2", 2);
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
        public void CopyTo_CopiesElementsToArray() // ����������� ��������� ��������� � ������
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>(1);
            collection.Add(new Shape("�������1", 1));
            collection.Add(new Shape("�������2", 2));
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
        public void CopyTo_ThrowsArgumentNullException_ArrayNull() // ���������� ��� null-������� � CopyTo
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>();
            Shape[] array = null;

            // Act and Assert
            Assert.ThrowsException<ArgumentNullException>(() => collection.CopyTo(array, 0));
        }

        [TestMethod]
        public void CopyTo_ThrowsArgumentOutOfRangeException_IndexIsNegative() // ���������� ��� �������������� ������� � CopyTo
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>();
            Shape[] array = new Shape[10];

            // Act and Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => collection.CopyTo(array, -1));
        }

        [TestMethod]
        public void CopyTo_ThrowsArgumentException_ArrayTooSmall() // ���������� � CopyTo, ���� � ������ �� ��������� ��� �������� � ���������
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>();
            Shape element1 = new Shape("�������1", 1);
            Shape element2 = new Shape("�������2", 2);
            collection.Add(element1);
            collection.Add(element2);
            Shape[] array = new Shape[1];

            // Act and Assert
            Assert.ThrowsException<ArgumentException>(() => collection.CopyTo(array, 0));
        }

        [TestMethod]
        public void Constructor_CreatesCollectionWithSameElements() // ����������� �����������
        {
            // Arrange
            MyCollection<Shape> originalCollection = new MyCollection<Shape>();
            Shape element1 = new Shape("�������1", 1);
            Shape element2 = new Shape("�������2", 2);
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
        public void TestRemoveItemCollection_Existent() // �������� ������������� � ���-������� ��������
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>();
            collection.Add(new Shape("�������", 1));
            collection.Add(new Shape("�������", 2));

            // Act
            bool removed = collection.Remove(new Shape("�������", 2));

            // Assert
            Assert.IsTrue(removed);
            Assert.IsFalse(collection.Contains(new Shape("�������", 2))); // ��������, ��� ������� �����
        }

        [TestMethod]
        public void TestRemoveItemCollection_NonExistent() // �������� �� ������������� � ���-������� ��������
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>();
            collection.AddItem(new Shape("�������", 1));
            collection.AddItem(new Shape("�������", 2));

            // Act
            bool notAdded = collection.Remove(new Shape("�������", 100)); // ������� �������� ��������, ������� �� ��� ��������

            // Assert
            Assert.IsFalse(notAdded); // �������� �� ���������
        }

        [TestMethod]
        public void DeepCopyCollection() // ���� ��� �������� ����� ���������
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>();
            for (int i = 0; i < 5; i++) // �������� �������������� �������� ���������
            {
                Shape s = new Shape();
                s.RandomInit();
                collection.Add(s); // ��������� � ���-�������
            }
            MyCollection<Shape> deepCopy = collection.DeepCopy();

            // Act
            foreach (Shape shape in deepCopy) // ������� ��� �������� � ������������� ����� �� <�������>
                shape.Name = "�������";

            // Assert
            // ������ � �������� ����� �������� ������ ���� �������� ��������, � �������� ������ ���� �������� ��� ���������
            foreach (Shape shape in collection)
            {
                Assert.AreNotEqual(shape.Name, "�������");
            }
            foreach (Shape shape in deepCopy)
            {
                Assert.AreEqual(shape.Name, "�������");
            }
        }

        [TestMethod]
        public void ShallowCopyCollection() // ���� ��� ������������� ����� ���������
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>();
            for (int i = 0; i < 5; i++) // �������� �������������� �������� ���������
            {
                Shape s = new Shape();
                s.RandomInit();
                collection.Add(s); // ��������� � ���-�������
            }
            MyCollection<Shape> shallowCopy = collection.ShallowCopy();

            // Act
            foreach (Shape shape in shallowCopy) // ������� ��� �������� � ������������� ����� �� <�������>
                shape.Name = "�������";

            // Assert
            // � � �������� ���������, � � ������������� ����� ��� �������� ������ ����� �������� "�������"
            foreach (Shape shape in collection)
            {
                Assert.AreEqual(shape.Name, "�������");
            }
            foreach (Shape shape in shallowCopy)
            {
                Assert.AreEqual(shape.Name, "�������");
            }
        }

        [TestMethod]
        public void TestEnumerator()
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>(3);
            int exceptedCount = 2;

            // Act
            Shape element1 = new Shape("�������1", 1);
            Shape element2 = new Shape("�������2", 2);
            collection.Add(element1);
            collection.Add(element2);

            // Assert
            int i = 0;
            foreach (Shape shape in collection)
            {
                i++;
                if (shape.id.Number == i) Assert.AreEqual(shape, new Shape("�������" + i.ToString(), i));
            }
            Assert.AreEqual(exceptedCount, i);
        }

        [TestMethod]
        public void IsReadOnly_ReturnsFalse() // ���� ��� �������� isReadOnly
        {
            // Arrange
            MyCollection<Shape> collection = new MyCollection<Shape>();

            // Act
            bool isReadOnly = collection.IsReadOnly;

            // Assert
            Assert.IsFalse(isReadOnly);
        }















        // ����� ��� ��� �������
        [TestClass]
        public class UnitTestHash
        {
            [TestMethod]
            public void TestAddItem_DifferentObjects() // ���������� ������������ ���� �� ����� ��������
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>();
                int expectedCount = 3;

                // Act
                hashTable.AddItem(new Shape("�������", 1));
                hashTable.AddItem(new Shape("�������", 2));
                hashTable.AddItem(new Shape("�������", 3));

                // Assert
                // ��� �������� ������ - ������ ��� ����������
                Assert.AreEqual(expectedCount, hashTable.Count);
            }

            [TestMethod]
            public void TestAddItem_ZeroCapacity() // ���������� �������� � ���-������� ������� �����������
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>(0);
                int expectedCount = 3;

                // Act
                hashTable.AddItem(new Shape("�������", 1));
                hashTable.AddItem(new Shape("�������", 2));
                hashTable.AddItem(new Shape("�������", 3));

                // Assert
                // ��� �������� ������ - ������ ��� ����������
                Assert.AreEqual(expectedCount, hashTable.Count);
            }

            [TestMethod]
            public void TestAddItem_SimilarObjects() // ���������� ���������� ��������
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>();
                int expectedCount = 1;

                // Act
                hashTable.AddItem(new Shape("�������", 0));
                hashTable.AddItem(new Shape("�������", 0));
                hashTable.AddItem(new Shape("�������", 0));

                // Assert
                // ��� �������� ���������� - � ���-������� ������ ���� ���� �������
                Assert.AreEqual(expectedCount, hashTable.Count);

            }

            [TestMethod]
            public void TestRemoveItem_Existent() // �������� ������������� � ���-������� ��������
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>();
                hashTable.AddItem(new Shape("�������", 1));
                hashTable.AddItem(new Shape("�������", 2));

                // Act
                bool removed = hashTable.RemoveData(new Shape("�������", 2));

                // Assert
                Assert.IsTrue(removed);
                Assert.IsFalse(hashTable.Contains(new Shape("�������", 2))); // ��������, ��� ������� �����
            }

            [TestMethod]
            public void TestRemoveItem_NonExistent() // �������� �� ������������� � ���-������� ��������
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>();
                hashTable.AddItem(new Shape("�������", 1));
                hashTable.AddItem(new Shape("�������", 2));

                // Act
                bool notAdded = hashTable.RemoveData(new Shape("�������", 100)); // ������� �������� ��������, ������� �� ��� ��������

                // Assert
                Assert.IsFalse(notAdded); // �������� �� ���������
            }

            [TestMethod]
            public void TestContains_Existent() // �������� ����������� ������������� � ���-������� ��������
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>();
                hashTable.AddItem(new Shape("�������", 1));
                hashTable.AddItem(new Shape("�������", 2));

                // Act
                bool isExist = hashTable.Contains(new Shape("�������", 1));

                // Assert
                Assert.IsTrue(isExist); // ������� ���� � �������
            }

            [TestMethod]
            public void TestContains_NonExistent() // �������� ���������� �� ������������� � ���-������� ��������
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>();
                hashTable.AddItem(new Shape("�������", 1));
                hashTable.AddItem(new Shape("�������", 2));

                // Act
                bool isNotExist = hashTable.Contains(new Shape("�������", 100)); // ������� ����� ������������� �������

                // Assert
                Assert.IsFalse(isNotExist); // �������� ��� � �������
            }

            [TestMethod]
            public void TestFindItem_NonExistent() // ����� �� ������������� � ���-������� ��������
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>();
                hashTable.AddItem(new Shape("�������", 1));
                hashTable.AddItem(new Shape("�������", 2));

                // Act
                int NotExist = hashTable.FindItem(new Shape("�������", 100)); // ������� ����� ������������� �������

                // Assert
                Assert.IsTrue(NotExist == -1); // �������� ��� � �������
            }

            [TestMethod]
            public void TestResize() // ���������� ������������ ������������� � ���������� ����������� ���-�������
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>(2); // ��������� ������ 2
                int expectedSize = 4;

                // Act
                hashTable.AddItem(new Shape("�������", 1));
                hashTable.AddItem(new Shape("�������", 2));

                // Assert
                Assert.AreEqual(expectedSize, hashTable.Capacity); // ������ ������ ����������� � ��� ����, �� 4
            }

            [TestMethod]
            public void TestAddItem_NullObject() // ���������� null-�������
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
            public void TestAddItem_�ollisionAfter() // �������� � ������� �������� ����� ������ �������
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>(10, 1); // ����� ��������� ����������� ��������, �������� ����������� ������������� �������
                int place; // �����, ���� ������� �������, ������������ ��������
                Shape temp = new Shape(); // � ������� ������ ���������� �������� ���� ������������ �������

                // Act
                while (true)
                {
                    // ������ ����� ������� ��� ����������
                    Shape shape = new Shape();
                    shape.RandomInit();
                    // ��������� ��������:
                    if (hashTable.flags[hashTable.GetIndex(shape)] == 1)
                    {
                        // ���� �����, ���� � ����� ������� ����������� �������
                        place = hashTable.SearchPlace(hashTable.GetIndex(shape));
                        // ���� �� ����� ����� ���������������� ��� �����, �� ������� �� �����
                        if (place > hashTable.GetIndex(shape))
                        {
                            temp = shape;
                            break;
                        }
                    }
                    // ��������� ��������� �������
                    hashTable.AddItem(shape);
                }

                // Assert
                Assert.IsTrue(hashTable.flags[hashTable.GetIndex(temp)] == 1); // ����������� ����� ���������� �������� ��� ������ ������������ ������ ��������� ������
                Assert.IsTrue(place > hashTable.GetIndex(temp)); // ����� ����� ������������ �����
            }

            [TestMethod]
            public void TestAddItem_�ollisionBefore() // �������� � ������� �������� �� ������ �������
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>(10, 1); // ����� ��������� ����������� ��������, �������� ����������� ������������� �������
                int place; // �����, ���� ������� �������, ������������ ��������
                Shape temp = new Shape(); // � ������� ������ ���������� �������� ���� ������������ �������

                // Act
                while (true)
                {
                    // ������ ����� ������� ��� ����������
                    Shape shape = new Shape();
                    shape.RandomInit();
                    // ��������� ��������:
                    if (hashTable.flags[hashTable.GetIndex(shape)] == 1)
                    {
                        // ���� �����, ���� � ����� ������� ����������� �������
                        place = hashTable.SearchPlace(hashTable.GetIndex(shape));
                        // ���� �� ����� ����� ���������������� ��� �����, �� ������� �� �����
                        if (place < hashTable.GetIndex(shape))
                        {
                            temp = shape;
                            break;
                        }
                    }
                    // ��������� ��������� �������
                    hashTable.AddItem(shape);
                }

                // Assert
                Assert.IsTrue(hashTable.flags[hashTable.GetIndex(temp)] == 1); // ����������� ����� ���������� �������� ��� ������ ������������ ������ ��������� ������
                Assert.IsTrue(place < hashTable.GetIndex(temp)); // ����� ����� ������������ �����
            }

            [TestMethod]
            public void TestAddItem_�ollisionBefore_NotBegin() // �������� � ������� �������� �� ������ �������� �� �� � ������ �������
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>(10, 1); // ����� ��������� ����������� ��������, �������� ����������� ������������� �������
                int place; // �����, ���� ������� �������, ������������ ��������
                Shape temp = new Shape(); // � ������� ������ ���������� �������� ���� ������������ �������

                // Act
                while (true)
                {
                    // ������ ����� ������� ��� ����������
                    Shape shape = new Shape();
                    shape.RandomInit();
                    // ��������� ��������:
                    if (hashTable.flags[hashTable.GetIndex(shape)] == 1)
                    {
                        // ���� �����, ���� � ����� ������� ����������� �������
                        place = hashTable.SearchPlace(hashTable.GetIndex(shape));
                        // ���� �� ����� ����� ���������������� ��� �����, �� ������� �� �����
                        if (place < hashTable.GetIndex(shape) && place != 0)
                        {
                            temp = shape;
                            break;
                        }
                    }
                    // ��������� ��������� �������
                    hashTable.AddItem(shape);
                }

                // Assert
                Assert.IsTrue(hashTable.flags[hashTable.GetIndex(temp)] == 1); // ����������� ����� ���������� �������� ��� ������ ������������ ������ ��������� ������
                Assert.IsTrue(place < hashTable.GetIndex(temp)); // ����� ����� ������������ �����
            }

            [TestMethod]
            public void TestFindExistItem_�ollisionAfter() // �������� � ����� ��������, ������������ ����� ������ �������
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>(10, 1); // ����� ��������� ����������� ��������, �������� ����������� ������������� �������
                int place; // �����, ���� ������� �������, ������������ ��������
                Shape temp = new Shape(); // � ������� ������ ���������� �������� ���� ������������ �������

                // Act
                while (true)
                {
                    // ������ ����� ������� ��� ����������
                    Shape shape = new Shape();
                    shape.RandomInit();
                    // ��������� ��������:
                    if (hashTable.flags[hashTable.GetIndex(shape)] == 1)
                    {
                        // ���� �����, ���� � ����� ������� ����������� �������
                        place = hashTable.SearchPlace(hashTable.GetIndex(shape));
                        // ���� �� ����� ����� ���������������� ��� �����, �� ������� �� �����
                        if (place > hashTable.GetIndex(shape))
                        {
                            hashTable.AddItem(shape);
                            temp = shape;
                            break;
                        }
                    }
                    // ��������� ��������� �������
                    hashTable.AddItem(shape);
                }

                // Assert
                Assert.IsTrue(hashTable.FindItem(temp) > hashTable.GetIndex(temp)); // ����� ������ ������ ����� ������������ �����
            }

            [TestMethod]
            public void TestFindExistItem_�ollisionBefore() // �������� � ����� ��������, ������������ �� ������ �������
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>(10, 1); // ����� ��������� ����������� ��������, �������� ����������� ������������� �������
                int place; // �����, ���� ������� �������, ������������ ��������
                Shape temp = new Shape(); // � ������� ������ ���������� �������� ���� ������������ �������

                // Act
                while (true)
                {
                    // ������ ����� ������� ��� ����������
                    Shape shape = new Shape();
                    shape.RandomInit();
                    // ��������� ��������:
                    if (hashTable.flags[hashTable.GetIndex(shape)] == 1)
                    {
                        // ���� �����, ���� � ����� ������� ����������� �������
                        place = hashTable.SearchPlace(hashTable.GetIndex(shape));
                        // ���� �� ����� ����� ���������������� ��� �����, �� ������� �� �����
                        if (place < hashTable.GetIndex(shape))
                        {
                            hashTable.AddItem(shape);
                            temp = shape;
                            break;
                        }
                    }
                    // ��������� ��������� �������
                    hashTable.AddItem(shape);
                }

                // Assert
                Assert.IsTrue(hashTable.FindItem(temp) < hashTable.GetIndex(temp)); // ����� ������ ������ �� ������������ �����
            }

            [TestMethod]
            public void TestFindExistItem_�ollisionBefore_NotZeroAndNotFirstPosition() // �������� � ����� ��������, ������������ �� ������ �������, �� �� � ������ ��� ������� �������
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>(1000, 1); // ����� ��������� ����������� ��������, �������� ����������� ������������� �������
                int place; // �����, ���� ������� �������, ������������ ��������
                Shape temp = new Shape(); // � ������� ������ ���������� �������� ���� ������������ �������

                // Act
                while (true)
                {
                    // ������ ����� ������� ��� ����������
                    Shape shape = new Shape();
                    shape.RandomInit();
                    // ��������� ��������:
                    if (hashTable.flags[hashTable.GetIndex(shape)] == 1)
                    {
                        // ���� �����, ���� � ����� ������� ����������� �������
                        place = hashTable.SearchPlace(hashTable.GetIndex(shape));
                        // ���� �� ����� ����� ���������������� ��� �����, �� ������� �� �����
                        if (place < hashTable.GetIndex(shape) && place > 1)
                        {
                            hashTable.AddItem(shape);
                            temp = shape;
                            break;
                        }
                    }
                    // ��������� ��������� �������
                    hashTable.AddItem(shape);
                }
                //hashTable.RemoveData(hashTable.table[hashTable.GetIndex(temp) - 5]);

                // Assert
                Assert.IsTrue(place < hashTable.GetIndex(temp) && place > 1); // ����� ������ ������ �� ������������ ����� (�� �� ������ ��� ������� �������)
            }

            [TestMethod]
            public void TestFindNotExistItem_�ollision() // ����� �� ������������� ��������, ������������� ��������
            {
                // Arrange
                MyHashTable<Shape> hashTable = new MyHashTable<Shape>(100, 1.5); // �������� ����������� ������������� �������, ����� ��������� � ���������, � ����� ������ � ��������� ����������� ������� �� ������������ �������
                Shape notExistShape = new Shape("��������", 777); // �������� �� ������������ �������, ������� ����� ������

                // Act
                // ��������� ��������� �������. ���������� ����, ����� ��� ������ ��������������� �������� ����������� �������� � ��������� ����������� �������
                while (hashTable.Capacity != hashTable.Count)
                {
                    // ������ ����� ������� ��� ����������
                    Shape shape = new Shape();
                    shape.RandomInit();
                    // ���������
                    hashTable.AddItem(shape);
                }

                // Assert
                Assert.IsTrue(hashTable.FindItem(notExistShape) == -1); // ����� ������ -1, ��� ��� �������� � ������� ���
            }
        }
    }
}