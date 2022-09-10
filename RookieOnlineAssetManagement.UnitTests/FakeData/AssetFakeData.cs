using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Entities.Dtos.AssetService;
using RookieOnlineAssetManagement.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.UnitTests.FakeData
{
    public class AssetFakeData
    {
        public static List<Asset> ListAssets()
        {
            return new List<Asset>() {
                new Asset()
                {
                    Id = 1,
                    AssetCode = "CA000001",
                    AssetName = "Asset1",
                    CategoryId = 1,
                    InstalledDate = DateTime.Now.Date,
                    Specification = "AssetSpe",
                    State = AssetState.Available,
                    Location = Location.HoChiMinh,
                    Disabled = false,
                },
                new Asset()
                {
                    Id = 2,
                    AssetCode = "CB000001",
                    AssetName = "Asset2",
                    CategoryId = 2,
                    InstalledDate = DateTime.Now.Date,
                    Specification = "",
                    State = AssetState.Assigned,
                    Location = Location.HoChiMinh,
                    Disabled = false,
                },
                new Asset()
                {
                    Id = 3,
                    AssetCode = "CC000001",
                    AssetName = "Asset3",
                    CategoryId = 3,
                    InstalledDate = DateTime.Now.Date,
                    Specification = "",
                    State = AssetState.NotAvailable,
                    Location = Location.HaNoi,
                    Disabled = false,
                },
                new Asset()
                {
                    Id = 4,
                    AssetCode = "CD000001",
                    AssetName = "Asset4",
                    CategoryId = 4,
                    InstalledDate = DateTime.Now.Date,
                    Specification = "AssetSpe",
                    State = AssetState.Available,
                    Location = Location.HaNoi,
                    Disabled = false,
                },
                new Asset()
                {
                    Id = 5,
                    AssetCode = "CD000005",
                    AssetName = "Asset5",
                    CategoryId = 1,
                    InstalledDate = DateTime.Now.Date,
                    Specification = "AssetSpe",
                    State = AssetState.Available,
                    Location = Location.HaNoi,
                    Disabled = false,
                },
                new Asset()
                {
                    Id = 6,
                    AssetCode = "CD000005",
                    AssetName = "Asset6",
                    CategoryId = 1,
                    InstalledDate = DateTime.Now.Date,
                    Specification = "AssetSpe",
                    State = AssetState.Available,
                    Location = Location.HaNoi,
                    Disabled = false,
                },
                new Asset()
                {
                    Id = 7,
                    AssetCode = "CD000007",
                    AssetName = "Asset7",
                    CategoryId = 1,
                    InstalledDate = DateTime.Now.Date,
                    Specification = "AssetSpe",
                    State = AssetState.Available,
                    Location = Location.HaNoi,
                    Disabled = false,
                },
                new Asset()
                {
                    Id = 8,
                    AssetCode = "CD000007",
                    AssetName = "Asset7",
                    CategoryId = 1,
                    InstalledDate = DateTime.Now.Date,
                    Specification = "AssetSpe",
                    State = AssetState.Available,
                    Location = Location.HaNoi,
                    Disabled = false,
                }
            };
        }
        public static List<Category> ListCategories()
        {
            return new List<Category>() {
                new Category()
                {
                    Id = 1,
                    CategoryName = "Laptop",
                    CreatedDate = DateTime.Parse("2022-02-02"),
                },
                new Category()
                {
                    Id = 2,
                    CategoryName = "Phone",
                    CreatedDate = DateTime.Parse("2022-02-02"),
                },
                new Category()
                {
                    Id = 3,
                    CategoryName = "Watch",
                    CreatedDate = DateTime.Parse("2022-02-02"),
                },
            };
        }

        public static CreateAssetDto CreateAsset()
        {
            return new CreateAssetDto()
            {
                AssetCode = "CA000001",
                AssetName = "Asset1",
                CategoryId = 1,
                Specification = "Asset1",
                InstalledDate = DateTime.Now.Date,
                State = AssetState.Available,
            };
        }

        public static DetailsAssetDto GetAssetDetail()
        {
            return new DetailsAssetDto()
            {
                Id = 1,
                AssetCode = "CA000001",
                AssetName = "Asset1",
                InstalledDate = DateTime.Now.Date,
                Specification = "AssetSpe",
                State = AssetState.Available,
                Category = "Laptop",
                Location = Location.HoChiMinh,
            };
        }
        public static UpdateAssetDto UpdateAsset()
        {
            return new UpdateAssetDto()
            {
                AssetName = "Asset1",
                InstalledDate = DateTime.Now.Date,
                Specification = "AssetSpe",
                State = AssetState.Available,
            };
        }
        public static DetailsUpdateAssetDto GetAssetUpdateDetail()
        {
            return new DetailsUpdateAssetDto()
            {
                Id = 1,
                AssetName = "Asset1",
                CategoryId =1,
                CategoryName = "Cate1",
                InstalledDate = DateTime.Now.Date,
                Specification = "AssetSpe",
                State = AssetState.Available,
            };
        }

        public static CategoryDto CreateCategory()
        {
            return new CategoryDto()
            {
                CategoryName = "Laptop",
                CategoryPrefix = "LA",
            };
        }
    }
}
