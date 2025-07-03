using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;

namespace LAB12BAITHI
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Level { get; set; }
        public int Gold { get; set; }
        public int Coins { get; set; }
        public bool IsActive { get; set; }
        public int VipLevel { get; set; }
        public string Region { get; set; } = "";
        public DateTime LastLogin { get; set; }
    }

    public class InactivePlayer
    {
        public string Name { get; set; } = "";
        public bool IsActive { get; set; }
        public DateTime LastLogin { get; set; }
    }
    internal class Program
    {
        private static FirebaseClient firebase = new FirebaseClient("https://lab12thi-default-rtdb.firebaseio.com/");

        static async Task Main(string[] args)
        {            
            var url = "https://raw.githubusercontent.com/NTH-VTC/OnlineDemoC-/refs/heads/main/lab12_players.json";
            var client = new HttpClient();
            var json = await client.GetStringAsync(url);            
            var players = JsonConvert.DeserializeObject<List<Player>>(json);

            
            DateTime now = new DateTime(2025, 06, 30, 0, 0, 0, DateTimeKind.Utc);
            DateTime limitDate = now.AddDays(-5); 
           
            var inactivePlayers = players
                .Where(p => !p.IsActive || p.LastLogin < limitDate)
                .Select(p => new
                {
                    p.IsActive,
                    p.LastLogin,
                    p.Name
                })
                .ToList();

            // Hiển thị ra Console
            Console.WriteLine("List nguoi choi khong hoat dong");
            foreach (var p in inactivePlayers)
            {
                Console.WriteLine($"Name: {p.Name}, IsActive: {p.IsActive}, LastLogin: {p.LastLogin}");
            }

            // Post lên Firebase
            await firebase
                .Child("final_exam_bai1_inactive_players")
                .PutAsync(inactivePlayers);

        }
    }
}
