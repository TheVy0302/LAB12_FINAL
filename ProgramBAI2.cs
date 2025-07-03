using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;

namespace LAB12BAI2
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

    public class VipAward
    {
        public string Name { get; set; } = "";
        public int Level { get; set; }
        public int VipLevel { get; set; }
        public int CurrentGold { get; set; }
        public int AwardedGoldAmount { get; set; }
    }

    internal class ProgramBAI2
    {
        private static FirebaseClient firebase = new FirebaseClient("https://lab12thi-default-rtdb.firebaseio.com/");

        static async Task Main(string[] args)
        {
            var url = "https://raw.githubusercontent.com/NTH-VTC/OnlineDemoC-/refs/heads/main/lab12_players.json";
            var client = new HttpClient();
            var json = await client.GetStringAsync(url);
            var players = JsonConvert.DeserializeObject<List<Player>>(json);

            // Tinh Vip cua nguoi choi
            var topVipPlayers = players
                .Where(p => p.VipLevel > 0)
                .OrderByDescending(p => p.Level)
                .Take(3)
                .ToList();

            // Thuong TOP 3
            var awardValues = new[] { 2000, 1500, 1000 };
            var awardedPlayers = topVipPlayers.Select((p, index) => new VipAward
            {
                Name = p.Name,               
                Level = p.Level,
                VipLevel = p.VipLevel,
                CurrentGold = p.Gold,
                AwardedGoldAmount = awardValues[index]
            }).ToList();

            // In ra console
            Console.WriteLine("TOP 3 nguoi choi duoc nhan thuong:");
            foreach (var p in awardedPlayers)
            {
                Console.WriteLine($"Name: {p.Name},Vip: {p.VipLevel}, Level: {p.Level}, Gold: {p.CurrentGold}, Awarded: {p.AwardedGoldAmount}");
            }

            // Post len Firebase
            await firebase
                .Child("final_exam_bai2_top3_vip_awards")
                .PutAsync(awardedPlayers);
        }
    }
}