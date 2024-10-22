using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Net_MVC_Firebase.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        // FirebaseApp 초기화
        private readonly FirebaseApp _firebaseApp;

        public AuthController() { }

        [HttpGet("verifyToken")]
        public async Task<IActionResult> VerifyToken()
        {
            //토큰을 받아옴
            string authorizationHeader = Request.Headers["Authorization"];
            if (authorizationHeader == null || !authorizationHeader.StartsWith("Bearer "))
            {
                return Unauthorized("No token provided");

            } //end if

            // 토큰에서 "Bearer " 부분을 제거하고 토큰 추출
            string token = authorizationHeader.Substring("Bearer ".Length).Trim();


            try
            {
                // Firebase 토큰 검증
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                // 토큰을 받고 Uid를 뽑아서 전달
                string uid = decodedToken.Uid;

                // 토큰 검증 성공 시 사용자 정보 반환
                return Ok(uid);

            } //edn try

            catch (FirebaseAuthException ex)
            {
                // 검증 실패 시 예외 처리
                return Unauthorized($"Token verification failed: {ex.Message}");

            } //end catch

        } //end method

    } //end class
}
