using System.Collections.Generic;
using KataDataModule;
using KataDataModule.Interfaces;
using KataDataModule.JsonObjects;
using Moq;
using NUnit.Framework;

namespace TypingKataDataUnitTests {

    [TestFixture]
    public class SettingsRepositoryUnitTest {

        private readonly string testPath = "testPath";
        private Mock<IDataSerializer> _dataSerializerMock;
        private Mock<IJSonLoader> _jsonLoaderMock;

        [SetUp]
        public void Setup() {
            _dataSerializerMock = new Mock<IDataSerializer>();
            _jsonLoaderMock = new Mock<IJSonLoader>();
        }

        [Test]
        public void ShouldWriteOutSettings() {
            _dataSerializerMock.Setup(x => x.SerializeObject(It.IsAny<IList<SettingJsonObject>>(), It.IsAny<string>()));
            _jsonLoaderMock.Setup(x => x.RefreshJsonFiles());
            var target = CreateTarget(_dataSerializerMock.Object, _jsonLoaderMock.Object);

            target.WriteOutSettings();
            _dataSerializerMock.VerifyAll();
            _jsonLoaderMock.VerifyAll();
        }

        [Test]
        public void ShouldWriteOutSettingsWhenSettingsUpdated() {
            _dataSerializerMock.Setup(x => x.SerializeObject(It.IsAny<IList<SettingJsonObject>>(), It.IsAny<string>()));
            _jsonLoaderMock.Setup(x => x.RefreshJsonFiles());
            var target = CreateTarget(_dataSerializerMock.Object, _jsonLoaderMock.Object);
            target.Settings.Add(new SettingJsonObject(new object(), testPath));

            _dataSerializerMock.VerifyAll();
            _jsonLoaderMock.VerifyAll();
        }

        public SettingsRepository CreateTarget(IDataSerializer dataSerializer, IJSonLoader loader) {
            return new SettingsRepository(testPath, dataSerializer, loader);
        }

    }
}
