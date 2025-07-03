using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;

namespace LAB1201
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

    public class LowLevelPlayer
    {
        public string Name { get; set; } = "";
        public int Level { get; set; }
        public int CurrentGold { get; set; }
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

            // Nguoi choi lv < 10
            var lowLevelPlayers = players
                .Where(p => p.Level < 10)
                .Select(p => new
                {
                    p.Name,
                    p.Level,
                    CurrentGold = p.Gold
                })
                .ToList();

            // Hien thi ra Console
            Console.WriteLine("List nguoi choi cap thap (Level < 10)");
            foreach (var p in lowLevelPlayers)
            {
                Console.WriteLine($"Name: {p.Name}, Level: {p.Level}, Gold: {p.CurrentGold}");
            }

            // Post len Firebase
            await firebase
                .Child("final_exam_bai1_low_level_players")
                .PutAsync(lowLevelPlayers);
        }
    }
}