using Newtonsoft.Json;

class Transport
{
    class Flight
    {
        public int number { get; set; }
        public string departure { get; set; }
        public string arrival { get; set; }
        public int capacity { get; set; }
        public int cargo { get; set; }
    }

    class Orders
    {
        public string ID { get; set; }
        public Flight flight { get; set; }
        public int day { get; set; }
    }

    private List<Orders> generateflightitinery(Dictionary<string, Dictionary<string, string>>? json, Dictionary<int, List<Flight>> flightschedule)
    {
        List<Orders> flightItineray = new List<Orders>();
        foreach (var order_id in json.Keys)
        {
            for (int day = 1; day <= flightschedule.Count; day++)
            {
                Flight flight = new Flight();
                flight = flightschedule[day].Find(x => x.arrival == json[order_id]["destination"]);
                if (flight != null)
                {
                    if (flight.capacity < flight.cargo)
                    {
                        continue;
                    }
                    flight.cargo++;
                    flightItineray.Add(new Orders { flight = flight, ID = order_id, day = day });
                    json.Remove(order_id);
                    break;
                }
                else
                {
                    flightItineray.Add(new Orders { flight = null, ID = order_id, day = day });
                    json.Remove(order_id);
                    break;
                }

            }
        }
        return flightItineray;
    }

    static void Main(string[] args)
    {
        /*
           USER STORY 1
           GENERATE FLIGHT ININERARIES
        */
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


        foreach (int day in flightschedule.Keys)
        {
            var flights = flightschedule[day];
            foreach (var flight in flights)
            {
                Console.WriteLine("Flight: {0}, departure: {1}, arrival: {2}, day: {3}", flight.number, flight.departure, flight.arrival, day);
                Console.WriteLine("");
            }
        }


        /*
            USER STORY 2
            GENERATE FLIGHT ININERARIES
         */
        var json = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText("N:\\Code\\Code\\C#\\app\\coding-assigment-orders.json"));
        List<Orders> orders = new List<Orders>();
        Transport inventoryManagemenet = new Transport();
        orders = inventoryManagemenet.generateflightitinery(json, flightschedule);

        //Print desired output
        orders = (from order in orders orderby order.day select order).ToList();
        foreach (var order in orders)
        {
            if (order.flight == null)
                Console.WriteLine("order: {0}, flightNumber: not scheduled", order.ID);
            else
                Console.WriteLine("order: {0}, flightNumber: {1}, departure: {2}, arrival: {3}, day: {4}, cargo: {5}", order.ID, order.flight.number, order.flight.departure, order.flight.arrival, order.day, order.flight.cargo);
        }
    }
}
