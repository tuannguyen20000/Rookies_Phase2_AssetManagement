using RookieOnlineAssetManagement.Entities.Dtos.RequestService;
using RookieOnlineAssetManagement.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.UnitTests.FakeData
{
    public class RequestFakeData
    {
        public static DetailRequestDto CompleteRequest()
        {
            return new DetailRequestDto()
            {
                Id = 1,
                AssetId = 1,
                AssignedDate = DateTime.Now.Date,
                ReturnedDate = DateTime.Now,
                RequestState = RequestState.Completed.ToString(),
            };
        }

        public static DetailRequestDto CancelRequest()
        {
            return new DetailRequestDto()
            {
                Id = 1,
                AssetId = 1,
                AssignedDate = DateTime.Now.Date,
                ReturnedDate = DateTime.Now,
                RequestState = "0",
            };
        }
    }
}
