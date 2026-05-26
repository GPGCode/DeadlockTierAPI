using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () => getResponse());

app.Run();  

async Task<string> getResponse()
{
    HttpClient httpClient = new HttpClient();

    string result = "";
    double average = 0;
    var response = await httpClient.GetAsync("https://api.deadlock-api.com/v1/analytics/hero-stats");
    var body = await response.Content.ReadAsStringAsync();
    var heroes = JsonSerializer.Deserialize<List<HeroStats>>(body);
    List<HeroTierResults> resultList = new List<HeroTierResults>();
    Dictionary<int, double> tierRatings =  new Dictionary<int, double>();
    var heroNames = new Dictionary<int, string>
    {
        { 1, "Infernus" },
        { 2, "Seven" },
        { 3, "Vindicta" },
        { 4, "Lady Geist" },
        { 6, "Abrams" },
        { 7, "Warden" },
        { 8, "McGinnis" },
        { 10, "Paradox" },
        { 11, "Dynamo" },
        { 12, "Kelvin" },
        { 13, "Haze" },
        { 14, "Mirage" },
        { 15, "Bebop" },
        { 16, "Grey Talon" },
        { 17, "Mo & Krill" },
        { 18, "Wraith" },
        { 19, "Pocket" },
        { 20, "Ivy" },
        { 25, "Lash" },
        { 27, "Yamato" },
        { 31, "Shiv" },
        { 35, "Holliday" },
        { 50, "Viscous" },
        { 52, "Sinclair" },
        { 58, "Vyper" },
        { 60, "Calico" },
        { 63, "Wrecker" },
        { 64, "Magician" },
        { 65, "Fathom" },
        { 66, "Doorman" },
        { 67, "Drifter" },
        { 69, "Talon" },
        { 72, "Viper" },
        { 76, "Graves" },
        { 77, "Mina" },
        { 79, "Paige" },
        { 80, "Victor" },
        { 81, "Apollo" },
    };
    
    foreach (var hero in heroes)
    {
        if (hero.matches != 0)
        {
            double rating = (double)(hero.wins - hero.losses) / hero.matches;
            tierRatings.Add(hero.hero_id, rating);
            average += rating;
        }
    }

    average = average / (double)tierRatings.Count;
    
    var sorted = tierRatings.OrderByDescending(loX => loX.Value);
    foreach (var rating in sorted)
    {
        double deviation = (rating.Value - average);
        string tier = deviation switch
        {
            > 0.10 => "S",
            > 0.05 => "A",
            > 0.00 => "B",
            > -0.05 => "C",
            > -0.10 => "D",
            _ => "F"
        };

        HeroTierResults thisHero = new HeroTierResults();
        thisHero.hero_name = heroNames[rating.Key];
        thisHero.hero_tier = tier;
        
        resultList.Add(thisHero);
    }

    result = JsonSerializer.Serialize(resultList);
    return result;
}

public class HeroStats
{
    public int hero_id { get; set; }
    public int wins { get; set; }

    public int losses { get; set; }

public int matches { get; set; }
    public long total_net_worth { get; set; }
}

public class HeroTierResults
{
    public string hero_name { get; set; }
    public string hero_tier { get; set; }
}
