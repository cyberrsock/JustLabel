using Xunit;
using JustLabel.Data.Models;
using JustLabel.Repositories;
using UnitTests.Data;
using UnitTests.Factories;
using JustLabel.Models;

namespace UnitTests.Repositories
{
    public class MarkedRepositoryUnitTests
    {
        private readonly MarkedRepository _markedRepository;
        private readonly MockDbContextFactory _mockFactory;

        public MarkedRepositoryUnitTests()
        {
            _mockFactory = new MockDbContextFactory();
            _markedRepository = new MarkedRepository(_mockFactory.MockContext.Object);
        }

        [Fact]
        public void TestCreateMarkedWithoutAreas()
        {
            // Arrange
            var markedModel = MarkedModelFactory.Create(
                1,
                2,
                3,
                4,
                false,
                DateTime.Now,
                [
                    AreaModelFactory.Create(1, 1, [(1,2), (2,3)])
                ]
            );

            List<MarkedDbModel> marks = [];
            _mockFactory.SetMarkedList(marks);

            // Act
            _markedRepository.Create(markedModel);

            // Assert
            Assert.Single(marks);
            Assert.Equal(markedModel.ImageId, marks[0].ImageId);
            Assert.Equal(markedModel.SchemeId, marks[0].SchemeId);
        }

        [Fact]
        public void TestCreateMarkedWithAreas()
        {
            // Arrange
            var markedModel = MarkedModelFactory.Create(
                1,
                2,
                3,
                4,
                false,
                DateTime.Now,
                [
                    AreaModelFactory.Create(1, 1, [(1,2), (2,3)]),
                    AreaModelFactory.Create(2, 2, [(1,2), (2,3)]),
                ]
            );

            List<MarkedDbModel> marks = [];
            List<AreaDbModel> areas = [];
            _mockFactory.SetMarkedList(marks);
            _mockFactory.SetAreaList(areas);

            // Act
            _markedRepository.Create(markedModel);

            // Assert
            Assert.Single(marks);
            Assert.Equal(2, areas.Count);
        }

        [Fact]
        public void TestDeleteExistingMarked()
        {
            // Arrange
            var markedDbModel = MarkedDbModelFactory.Create(
                1,
                2,
                3,
                4,
                false,
                DateTime.Now
            );
            List<MarkedDbModel> marks = [markedDbModel];
            _mockFactory.SetMarkedList(marks);

            // Act
            _markedRepository.Delete(1);

            // Assert
            Assert.Empty(marks);
        }

        [Fact]
        public void TestDeleteNonExistentMarked()
        {
            // Arrange
            var markedDbModel = MarkedDbModelFactory.Create(
                1,
                2,
                3,
                4,
                false,
                DateTime.Now
            );
            List<MarkedDbModel> marks = [markedDbModel];
            _mockFactory.SetMarkedList(marks);

            // Act
            _markedRepository.Delete(2);

            // Assert
            Assert.Single(marks);
        }

        [Fact]
        public void TestGetByDatasetId()
        {
            // Arrange
            var markedDbModel = MarkedDbModelFactory.Create(
                1,
                2,
                3,
                4,
                false,
                DateTime.Now
            );
            List<MarkedDbModel> marks = [markedDbModel];
            _mockFactory.SetMarkedList(marks);

            List<DatasetDbModel> datasets = [
                DatasetDbModelFactory.Create(
                    1,
                    "Test Dataset",
                    "Descr",
                    1,
                    DateTime.Now
                )
            ];
            _mockFactory.SetDatasetList(datasets);

            var imageDbo1 = ImageDbModelFactory.Create(1, 1, "path/to/image1.jpg", 1920, 1080);
            var imageDbo2 = ImageDbModelFactory.Create(2, 1, "path/to/image2.jpg", 1280, 720);
            var imageDbo3 = ImageDbModelFactory.Create(3, 1, "path/to/image2.jpg", 1280, 720);

            List<ImageDbModel> images = [imageDbo1, imageDbo2, imageDbo3];
            _mockFactory.SetImageList(images);

            // Act
            var result = _markedRepository.Get_By_DatasetId(1);

            // Assert
            Assert.Single(result);
            Assert.Equal(markedDbModel.ImageId, result[0].ImageId);
        }

        [Fact]
        public void TestGetByNonExistentDatasetId()
        {
            // Arrange
            var markedDbModel = MarkedDbModelFactory.Create(
                1,
                2,
                3,
                4,
                false,
                DateTime.Now
            );
            List<MarkedDbModel> marks = [markedDbModel];
            _mockFactory.SetMarkedList(marks);

            List<DatasetDbModel> datasets = [
                DatasetDbModelFactory.Create(
                    1,
                    "Test Dataset",
                    "Descr",
                    1,
                    DateTime.Now
                )
            ];
            _mockFactory.SetDatasetList(datasets);

            var imageDbo1 = ImageDbModelFactory.Create(1, 1, "path/to/image1.jpg", 1920, 1080);
            var imageDbo2 = ImageDbModelFactory.Create(2, 1, "path/to/image2.jpg", 1280, 720);
            var imageDbo3 = ImageDbModelFactory.Create(3, 1, "path/to/image2.jpg", 1280, 720);

            List<ImageDbModel> images = [imageDbo1, imageDbo2, imageDbo3];
            _mockFactory.SetImageList(images);

            // Act
            var result = _markedRepository.Get_By_DatasetId(2);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void TestGetBySchemeId()
        {
            // Arrange
            var markedDbModel = MarkedDbModelFactory.Create(
                1,
                2,
                3,
                4,
                false,
                DateTime.Now
            );
            List<MarkedDbModel> marks = [markedDbModel];
            _mockFactory.SetMarkedList(marks);

            List<SchemeDbModel> schemes = [
                SchemeDbModelFactory.Create(1, "Scheme 1", "Description 1", 3, DateTime.Now),
                SchemeDbModelFactory.Create(2, "Scheme 2", "Description 2", 4, DateTime.Now)
            ];
            _mockFactory.SetSchemeList(schemes);

            // Act
            var result = _markedRepository.Get_By_SchemeId(2);

            // Assert
            Assert.Single(result);
            Assert.Equal(markedDbModel.SchemeId, result[0].SchemeId);
        }

        [Fact]
        public void TestGetByNonExistentSchemeId()
        {
            // Arrange
            var markedDbModel = MarkedDbModelFactory.Create(
                1,
                2,
                3,
                4,
                false,
                DateTime.Now
            );
            List<MarkedDbModel> marks = [markedDbModel];
            _mockFactory.SetMarkedList(marks);

            List<SchemeDbModel> schemes = [
                SchemeDbModelFactory.Create(1, "Scheme 1", "Description 1", 3, DateTime.Now),
                SchemeDbModelFactory.Create(2, "Scheme 2", "Description 2", 4, DateTime.Now)
            ];
            _mockFactory.SetSchemeList(schemes);

            // Act
            var result = _markedRepository.Get_By_SchemeId(100);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void TestGetByDatasetAndSchemeId()
        {
            // Arrange
            var markedDbModel = MarkedDbModelFactory.Create(
                1,
                2,
                3,
                4,
                false,
                DateTime.Now
            );
            List<MarkedDbModel> marks = [markedDbModel];
            _mockFactory.SetMarkedList(marks);
            
            List<DatasetDbModel> datasets = [
                DatasetDbModelFactory.Create(
                    1,
                    "Test Dataset",
                    "Descr",
                    1,
                    DateTime.Now
                )
            ];
            _mockFactory.SetDatasetList(datasets);

            var imageDbo1 = ImageDbModelFactory.Create(1, 1, "path/to/image1.jpg", 1920, 1080);
            var imageDbo2 = ImageDbModelFactory.Create(2, 1, "path/to/image2.jpg", 1280, 720);
            var imageDbo3 = ImageDbModelFactory.Create(3, 1, "path/to/image2.jpg", 1280, 720);

            List<ImageDbModel> images = [imageDbo1, imageDbo2, imageDbo3];
            _mockFactory.SetImageList(images);

            List<SchemeDbModel> schemes = [
                SchemeDbModelFactory.Create(1, "Scheme 1", "Description 1", 3, DateTime.Now),
                SchemeDbModelFactory.Create(2, "Scheme 2", "Description 2", 4, DateTime.Now)
            ];
            _mockFactory.SetSchemeList(schemes);

            // Act
            var result = _markedRepository.Get_By_Dataset_and_SchemeId(1, 2);

            // Assert
            Assert.Single(result);
            Assert.Equal(markedDbModel.Id, result[0].Id);
        }

        [Fact]
        public void TestGetByNonExistentDatasetAndSchemeId()
        {
            // Arrange
            var markedDbModel = MarkedDbModelFactory.Create(
                1,
                2,
                3,
                4,
                false,
                DateTime.Now
            );
            List<MarkedDbModel> marks = [markedDbModel];
            _mockFactory.SetMarkedList(marks);
            
            List<DatasetDbModel> datasets = [
                DatasetDbModelFactory.Create(
                    1,
                    "Test Dataset",
                    "Descr",
                    1,
                    DateTime.Now
                )
            ];
            _mockFactory.SetDatasetList(datasets);

            var imageDbo1 = ImageDbModelFactory.Create(1, 1, "path/to/image1.jpg", 1920, 1080);
            var imageDbo2 = ImageDbModelFactory.Create(2, 1, "path/to/image2.jpg", 1280, 720);
            var imageDbo3 = ImageDbModelFactory.Create(3, 1, "path/to/image2.jpg", 1280, 720);

            List<ImageDbModel> images = [imageDbo1, imageDbo2, imageDbo3];
            _mockFactory.SetImageList(images);

            List<SchemeDbModel> schemes = [
                SchemeDbModelFactory.Create(1, "Scheme 1", "Description 1", 3, DateTime.Now),
                SchemeDbModelFactory.Create(2, "Scheme 2", "Description 2", 4, DateTime.Now)
            ];
            _mockFactory.SetSchemeList(schemes);

            // Act
            var result = _markedRepository.Get_By_Dataset_and_SchemeId(3, 5);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void TestGetNonExistentMarked()
        {
            // Arrange
            var markedModel = MarkedModelFactory.Create(
                1,
                2,
                3,
                4,
                false,
                DateTime.Now,
                [
                    AreaModelFactory.Create(1, 1, [(1,2), (2,3)]),
                    AreaModelFactory.Create(2, 2, [(1,2), (2,3)]),
                ]
            );
            List<MarkedDbModel> marks = [];
            _mockFactory.SetMarkedList(marks);

            // Act
            var result = _markedRepository.Get(markedModel);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TestGetExistingMarked()
        {
            // Arrange
            var markedDbModel = MarkedDbModelFactory.Create(
                1,
                2,
                3,
                4,
                false,
                DateTime.Now
            );
            List<MarkedDbModel> marks = [markedDbModel];
            List<AreaDbModel> areas = [];
            _mockFactory.SetMarkedList(marks);
            _mockFactory.SetAreaList(areas);

            var markedModel = MarkedModelFactory.Create(markedDbModel);

            // Act
            var result = _markedRepository.Get(markedModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(markedDbModel.Id, result.Id);
            Assert.Equal(markedDbModel.ImageId, result.ImageId);
        }

        [Fact]
        public void TestUpdateRects()
        {
            // Arrange
            var markedModel = MarkedModelFactory.Create(
                1,
                2,
                3,
                4,
                false,
                DateTime.Now,
                [
                    AreaModelFactory.Create(1, 1, [(1,2), (2,3)])
                ]
            );

            List<MarkedDbModel> marks = [MarkedDbModelFactory.Create(markedModel)];
            List<AreaDbModel> areas = [];
            _mockFactory.SetMarkedList(marks);
            _mockFactory.SetAreaList(areas);

            // Act
            _markedRepository.Update_Rects(markedModel);

            // Assert
            Assert.Single(areas);
            Assert.Equal(areas[0].Id, markedModel.AreaModels[0].Id);
            Assert.Equal(areas[0].LabelId, markedModel.AreaModels[0].LabelId);
        }

        [Fact]
        public void TestUpdateNonExistentRects()
        {
            // Arrange
            var markedModel = MarkedModelFactory.Create(
                1,
                2,
                3,
                4,
                false,
                DateTime.Now,
                [
                    AreaModelFactory.Create(1, 1, [(1,2), (2,3)])
                ]
            );
            List<MarkedDbModel> marks = [
                MarkedDbModelFactory.Create(
                    2,
                    3,
                    4,
                    5,
                    false,
                    DateTime.Now
                )
            ];
            _mockFactory.SetMarkedList(marks);

            // Act
            _markedRepository.Update_Rects(markedModel);

            // Assert
            Assert.Single(marks);
        }

        [Fact]
        public void TestUpdateBlock()
        {
            // Arrange
            List<MarkedDbModel> marks = [
                MarkedDbModelFactory.Create(
                    1,
                    3,
                    4,
                    5,
                    false,
                    DateTime.Now
                )
            ];
            _mockFactory.SetMarkedList(marks);

            var markedModel = MarkedModelFactory.Create(marks[0]);
            markedModel.IsBlocked = true;

            // Act
            _markedRepository.Update_Block(markedModel);

            // Assert
            Assert.True(marks[0].IsBlocked);
        }

        [Fact]
        public void TestUpdateBlockWithNonExistentId()
        {
            // Arrange
            List<MarkedDbModel> marks = [
                MarkedDbModelFactory.Create(
                    1,
                    3,
                    4,
                    5,
                    false,
                    DateTime.Now
                )
            ];
            _mockFactory.SetMarkedList(marks);

            var markedModel = MarkedModelFactory.Create(marks[0]);
            markedModel.IsBlocked = true;
            markedModel.Id = 2;

            // Act
            _markedRepository.Update_Block(markedModel);

            // Assert
            Assert.False(marks[0].IsBlocked);
        }

        [Fact]
        public void TestGetAll()
        {
            // Arrange
            var markedDbModel1 = MarkedDbModelFactory.Create(
                1,
                2,
                3,
                4,
                false,
                DateTime.Now
            );
            var markedDbModel2 = MarkedDbModelFactory.Create(
                2,
                3,
                3,
                4,
                false,
                DateTime.Now
            );
            List<AreaDbModel> areas = [];
            List<MarkedDbModel> marks = [markedDbModel1, markedDbModel2];
            _mockFactory.SetMarkedList(marks);
            _mockFactory.SetAreaList(areas);

            // Act
            var result = _markedRepository.GetAll();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, m => m.Id == markedDbModel1.Id);
            Assert.Contains(result, m => m.Id == markedDbModel2.Id);
        }

        [Fact]
        public void TestGetAllNoMarked()
        {
            // Arrange
            List<MarkedDbModel> marks = [];
            _mockFactory.SetMarkedList(marks);

            // Act
            var result = _markedRepository.GetAll();

            // Assert
            Assert.Empty(result);
        }
    }
}

