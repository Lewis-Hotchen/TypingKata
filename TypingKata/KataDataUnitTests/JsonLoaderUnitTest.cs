using System.Collections.Generic;
using System.IO.Abstractions;
using KataDataModule;
using KataDataModule.EventArgs;
using KataDataModule.Interfaces;
using KataIocModule;
using Moq;
using NUnit.Framework;

namespace KataDataUnitTests {
    [TestFixture]
    public class JsonLoaderUnitTest {

        private Mock<ITinyMessengerHub> _messengerMock;
        private Mock<IDataSerializer> _dataSerializerMock;
        private Mock<IFileSystem> _fileSystemMock;
        private readonly string testDirectory = "testDirectory";
        private readonly string testJson = "{\"TestString\" : \"test\"}";
        private TestObj _testObj;
        private List<string> _testFiles;

        [SetUp]
        public void Setup() {
            _messengerMock = new Mock<ITinyMessengerHub>();
            _dataSerializerMock = new Mock<IDataSerializer>();
            _fileSystemMock = new Mock<IFileSystem>();
            _testObj = new TestObj("test");
            _testFiles = new List<string> {"testobj.json"};
        }

        [Test]
        public void ShouldLoadTypeFromJson() {
            _dataSerializerMock.Setup(x => x.DeserializeObject<TestObj>(It.IsAny<string>()))
                .Returns(_testObj);

            _fileSystemMock.Setup(x => x.Directory.EnumerateFiles(testDirectory, It.IsAny<string>()))
                .Returns(_testFiles);

            _fileSystemMock.Setup(x => x.File.ReadAllText(It.IsAny<string>()))
                .Returns(testJson);

            var target = CreateTarget(_messengerMock.Object, _dataSerializerMock.Object, _fileSystemMock.Object);
            
            var res = target.LoadTypeFromJson<TestObj>("testobj.json");
            _dataSerializerMock.VerifyAll();
            _fileSystemMock.VerifyAll();
            Assert.IsInstanceOf<TestObj>(res);
        }

        [Test]
        public void ShouldRereadFilesOnRefresh() {
            _fileSystemMock.Setup(x => x.Directory.EnumerateFiles(testDirectory, It.IsAny<string>()))
                .Returns(_testFiles);

            _fileSystemMock.Setup(x => x.File.ReadAllText(It.IsAny<string>()))
                .Returns(testJson);

            var target = CreateTarget(_messengerMock.Object, _dataSerializerMock.Object, _fileSystemMock.Object);
            target.RefreshJsonFiles();

            _fileSystemMock.VerifyAll();
        }

        [Test]
        public void ShouldSignalJsonUpdatedOnReread() {
            _fileSystemMock.Setup(x => x.Directory.EnumerateFiles(testDirectory, It.IsAny<string>()))
                .Returns(_testFiles);

            _fileSystemMock.Setup(x => x.File.ReadAllText(It.IsAny<string>()))
                .Returns(testJson);
            _messengerMock.Setup(x => x.Publish(It.IsAny<JsonUpdatedMessage>()));

            var target = CreateTarget(_messengerMock.Object, _dataSerializerMock.Object, _fileSystemMock.Object);

            target.RefreshJsonFiles();
            _messengerMock.VerifyAll();

        }

        public JsonLoader CreateTarget(ITinyMessengerHub messengerHub, IDataSerializer serializer, IFileSystem fileSystem) {
            return new JsonLoader(
                testDirectory,
                messengerHub,
                serializer,
                fileSystem);
        }
    }

    
}