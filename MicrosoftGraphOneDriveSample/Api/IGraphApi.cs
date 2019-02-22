using System;
using System.Threading.Tasks;
using MicrosoftGraphOneDriveSample.Models;
using MicrosoftGraphOneDriveSample.Models.Payload;
using MicrosoftGraphOneDriveTest.Models;
using RestEase;

namespace MicrosoftGraphOneDriveTest.Api
{
    public interface IGraphApi
    {
        [Get("me")]
        Task<MeProxy> GetMe([Header("Authorization")] string accessToken);
        
        [Get("me/drive/root/children?select=name,size,webUrl,id")]
        Task<RootFileProxy> GetRootFiles([Header("Authorization")] string accessToken);
        
        [Get("me/memberOf")]
        Task<Groups> GetGroups([Header("Authorization")] string userAccessToken);
        
        [Get("me/drive/sharedWithMe")]
        Task<SharedWithMeProxy> GetSharedWithMeFiles([Header("Authorization")] string accessToken);
            
        [Post("/me/drive/root:/{item-path}:/createUploadSession")]
        Task<CreateUploadSessionProxy> CreateUploadSession([Header("Authorization")] string accessToken, [Path("item-path")]string itemPath, [Body]CreateUploadSession payload);

        [Post("/me/drive/items/{item-id}/invite")]
        Task<ShareToEmailPayload> ShareToEmail(
            [Header("Authorization")]string accessToken, 
            [Path("item-id")] string itemId, 
            [Body] ShareToEmailPayload payload);

      
    }
}