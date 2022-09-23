
using Newtonsoft.Json;

class TestClass
{
    private List<Orders> generateflightitinery(Dictionary<string, Dictionary<string, string>>? json, Dictionary<int, List<Flight>> flightschedule)
    {
        List<Orders> fi = new List<Orders>();
        foreach (var order_id in json.Keys)
        {
            for (int i = 1; i <= flightschedule.Count; i++)
            {
                //string sdsdsd = json[order_id]["destination"];
                Flight reqflight = new Flight();
                reqflight = flightschedule[i].Find(x => x.arrival == json[order_id]["destination"]);
                if (reqflight != null)
                {
                    if (json[order_id]["destination"] == reqflight.arrival)
                    {
                        if (reqflight.cargo < reqflight.capacity)
                        {
                            reqflight.cargo++;
                            fi.Add(new Orders { flight = reqflight, ID = order_id, day = i });
                            json.Remove(order_id);
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    fi.Add(new Orders { flight = null, ID = order_id, day = i });
                    json.Remove(order_id);
                    break;
                }

            }
        }
        return fi;
    }
    class Flight
    {

        public string departure { get; set; }
        //public string orderId { get; set; }
        public string arrival { get; set; }

        //public int day { get; set; }
        public int number { get; set; }
        public int capacity { get; set; }
        public int cargo { get; set; }
    }


    class Orders
    {
        public string ID { get; set; }
        public Flight flight { get; set; }
        public int day { get; set; }
    }
    static void Main(string[] args)
    {

        Dictionary<int, List<Flight>> flightschedule = new Dictionary<int, List<Flight>>();
        flightschedule.Add(1, new List<Flight>()
        {
                new Flight(){ arrival="YYZ", departure="YUL", number =1 , capacity=20, cargo=0},
                new Flight(){  arrival="YYC", departure="YUL", number =2  , capacity=20, cargo=0},
                new Flight(){  arrival="YVR", departure="YUL", number =3  , capacity=20, cargo=0},

        });
        flightschedule.Add(2, new List<Flight>()
        {
                new Flight(){  arrival="YYZ", departure="YUL", number =4  , capacity=20, cargo=0},
                new Flight(){  arrival="YYC", departure="YUL", number =5  , capacity=20, cargo=0},
                new Flight(){  arrival="YVR", departure="YUL", number =6  , capacity=20, cargo=0},

        });
        var json = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText("N:\\Code\\Code\\C#\\app\\coding-assigment-orders.json"));
        List<Orders> orders = new List<Orders>();
        TestClass cdd = new TestClass();
        orders = cdd.generateflightitinery(json, flightschedule);
        orders = (from oo in orders orderby oo.day select oo).ToList();
        foreach (var c in orders)
        {
            if (c.flight == null)
                Console.WriteLine("order: {0}, flightNumber: not scheduled", c.ID);
            else
                Console.WriteLine("order: {0}, flightNumber: {1}, departure: {2}, arrival: {3}, day: {4}, cargo: {5}", c.ID, c.flight.number, c.flight.departure, c.flight.arrival, c.day, c.flight.cargo);
        }
    }

    
}