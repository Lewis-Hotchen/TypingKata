using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Components.DictionaryAdapter;
using KataDataModule;
using KataDataModule.Interfaces;
using KataDataModule.JsonObjects;
using Moq;
using NUnit.Framework;

namespace TypingKataDataUnitTests {

    [TestFixture]
    public class TypingResultsRepositoryUnitTest {

        private Mock<IJsonLoader> _mockJsonLoader;
        private Mock<IDataSerializer> _mockSerializer;
        private string _mockPath = "testPath";

        private WPMJsonObject _mockJsonObject =
            new WPMJsonObject(10, 0, 0.0d, new EditableList<Tuple<string, string>>(), default(DateTime), 1);

        [SetUp]
        public void SetUp() {
            _mockJsonLoader = new Mock<IJsonLoader>();
            _mockSerializer = new Mock<IDataSerializer>();
        }

        [Test]
        public void ShouldInstantiateWithCorrectResultsWhenPopulated() {
            _mockJsonLoader.Setup(x => x.LoadTypeFromJson<List<WPMJsonObject>>(It.IsAny<string>())).Returns(
                new List<WPMJsonObject> {
                    _mockJsonObject
                }
                );

            var target = CreateTarget(_mockJsonLoader.Object, _mockSerializer.Object, _mockPath);

            _mockSerializer.VerifyNoOtherCalls();
            _mockJsonLoader.VerifyAll();
            Assert.AreEqual(1, target.Results.ToArray().Length);
        }

        [Test]
        public void ShouldInstantiateWithEmptyCollectionIfNoResultsLoaded() {
            _mockJsonLoader.Setup(x => x.LoadTypeFromJson<List<WPMJsonObject>>(It.IsAny<string>())).Returns(default(List<WPMJsonObject>));
            _mockSerializer.Setup(x => x.SerializeObject(It.IsAny<object>(), It.IsAny<string>()));
            var target = CreateTarget(_mockJsonLoader.Object, _mockSerializer.Object, _mockPath);

            _mockSerializer.VerifyAll();
            _mockJsonLoader.VerifyAll();
            Assert.AreEqual(0, target.Results.ToArray().Length);
        }

        [Test]
        public void ShouldWriteOutResultsOnResultsUpdate() {
            var eventsRaised = 0;
            _mockSerializer.Setup(x => x.SerializeObject(It.IsAny<object>(), It.IsAny<string>()));
            var target = CreateTarget(_mockJsonLoader.Object, _mockSerializer.Object, _mockPath);
            target.ResultsChangedEvent += (sender, args) => eventsRaised++;
            target.AddResult(_mockJsonObject);
            target.RemoveResult(_mockJsonObject);
            target.ResetResults();

            _mockSerializer.Verify(x => x.SerializeObject(It.IsAny<object>(), It.IsAny<string>()), Times.Exactly(eventsRaised + 1));
        }

        public TypingResultsRepository CreateTarget(IJsonLoader jsonLoader, IDataSerializer serializer, string path) {
            return new TypingResultsRepository(jsonLoader, serializer, path);
        }

    }
}
