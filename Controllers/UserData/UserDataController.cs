using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShardData;

namespace ASP.Net_MVC_Firebase.Controllers.UserData
{
    [Route("api/[controller]")]
    [Controller()]
    public class UserDataController : Controller
    {

        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "kvXq0Ku5A1GjVsJg6dokP7YN5qjMLcRz0WP68XPw",             //웹 API키
            BasePath = "https://project-servergame-default-rtdb.firebaseio.com/" //경로
        };

        public readonly IFirebaseClient client;

        public  UserDataController()
        {
            try
            {
                client = new FirebaseClient(config);

            } //end try

            catch (Exception)
            {
                Console.WriteLine("Connection 실패!");

            } //end catch
        }


        [HttpPost("CreateUserData")]
        public async Task<IActionResult> CreateUserData([FromQuery(Name = "uid")] string uid)
        {
            // PlayerId를 위한 고유한 랜덤 수 생성
            ulong playerId = GenerateUniquePlayerId();  

            UserServerData data = new UserServerData
            {
                UserName = "unknown",
                Coin = 1000,
                PlayerId = playerId,
                SkinList = new List<string>()
            };

            // Firebase에 데이터 추가 자동으로 역직렬화 해줌
            var response = await client.SetAsync($"UserData/{uid}", data);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonPlayerId = JsonConvert.SerializeObject(playerId);

                // 성공 메시지 반환
                return Ok(jsonPlayerId);
            }

            // 오류 발생 시 상태 코드 반환
            return StatusCode((int)response.StatusCode, "유저 데이터를 생성할 수 없습니다.");
        }

        [HttpGet("GetUserDataByPlayerId")]
        public async Task<IActionResult> GetUserDataByPlayerId([FromQuery(Name = "uid")] string uid)
        {
            FirebaseResponse response = await client.GetAsync($"UserData/{uid}");

            // 데이터가 없는 경우
            if (response.Body == "null") return NotFound();

            UserServerData? userData = response.ResultAs<UserServerData>();
            // 사용자 데이터 반환
            return Ok(userData);
        }

        [HttpPut("UpdateUserData")]
        public async Task<IActionResult> UpdateUserData([FromQuery(Name = "uid")] string uid, [FromBody] UserServerData updatedData)
        {
            UserServerData newData = new()
            {
                UserName = updatedData.UserName,
                Coin = updatedData.Coin,
                PlayerId = updatedData.PlayerId,
                SkinList = updatedData.SkinList,
            };

            // Firebase에 데이터 업데이트
            var setRes = await client.UpdateAsync($"UserData/{uid}", updatedData);

            if (setRes.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok("유저 데이터가 성공적으로 업데이트되었습니다.");

            } //end if

            return StatusCode((int)setRes.StatusCode, "유저 데이터를 업데이트할 수 없습니다.");
        }

        private ulong GenerateUniquePlayerId()
        {
            // 예: 고유한 ID를 위한 랜덤한 ulong 생성
            Random random = new Random();
            return (ulong)(random.NextInt64(1, long.MaxValue)); // long.MaxValue 미만의 랜덤한 숫자 생성
        }

    } //end class
}
