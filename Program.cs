int selection = 0;
int spotNumber = 0;
string lincensePlate = string.Empty;
ParkingSpot ps = new ParkingSpot();
var garage = new Garage();

do
{
    Console.WriteLine("What do want to do?");
    Console.WriteLine("1) Enter a car entry");
    Console.WriteLine("2) Enter a car exit");
    Console.WriteLine("3) Generate report");
    Console.WriteLine("4) Exit");
    Console.WriteLine();
    Console.Write("Your selection: ");
    selection = int.Parse(Console.ReadLine()!);
    Console.WriteLine();

    switch (selection)
    {
        case 1:
            Console.Write("Enter parking spot number:  ");
            spotNumber = int.Parse(Console.ReadLine()!);
            if (garage.IsOccupied(spotNumber)) { Console.WriteLine("Parking spot is occupied"); continue; }
            Console.Write("entere lincense plate: ");
            ps.LincensePlate = Console.ReadLine()!;
            Console.Write("Enter entry date/time: ");
            ps.EntryDate = DateTime.Parse(Console.ReadLine()!);
            garage.Occupy(spotNumber, ps.LincensePlate, ps.EntryDate);
            break;

        case 2:
            Console.Write("Enter parking spot number:  ");
            spotNumber = int.Parse(Console.ReadLine()!);
            if (!garage.TryExit(spotNumber)) continue;
            Console.WriteLine("Enter exit date/time: ");
            var exitDate = Convert.ToDateTime(Console.ReadLine()!);
            garage.Exit(spotNumber, exitDate, out decimal costs);
            Console.WriteLine($"Costs are {costs}");
            break;

        case 3:
            garage.GenerateReport();
            break;
        case 4: Console.WriteLine("Good bye!"); return;
    }

    Console.WriteLine();

} while (true);

public class ParkingSpot
{
    public string LincensePlate { get; set; } = "";
    public DateTime EntryDate { get; set; }
}

public class Garage
{
    public ParkingSpot[] ParkingSpots { get; } = new ParkingSpot[50];
    public bool IsOccupied(int parkingSpotnumber)
    {
        return ParkingSpots[parkingSpotnumber] != null;
    }
    public void Occupy(int parkingSpotNumber, string licensePlate, DateTime entryTime)
    {
        ParkingSpots[parkingSpotNumber - 1] = new() { LincensePlate = licensePlate, EntryDate = entryTime };
    }
    public bool TryExit(int parkingSpotNumber)
    {
        if (ParkingSpots[parkingSpotNumber - 1] != null) return true;
        Console.WriteLine("Parking spot is not occupied");
        return false;
    }
    public void Exit(int parkingSpotNumber, DateTime exitTime, out decimal costs)
    {
        costs = exitTime.Minute - ParkingSpots[parkingSpotNumber - 1].EntryDate.Minute <= 15 ? 0m : ((exitTime.Minute - ParkingSpots[parkingSpotNumber - 1].EntryDate.Minute) / 30) * 3;
        ParkingSpots[parkingSpotNumber - 1] = null;
    }
    public void GenerateReport()
    {
        ColoredBorder("| ");
        Console.Write("Spot | ");
        Console.Write("License Plate ");
        ColoredBorder("|");
        Console.WriteLine();

        ColoredBorder("| ");
        Console.Write("---- | ");
        Console.Write("------------- ");
        ColoredBorder("|");
        Console.WriteLine();

        for (int i = 0; i < ParkingSpots.Length; i++)
        {
            ColoredBorder("| ");
            Console.Write(+i <= 8 ? $"{i + 1}    | " : $"{i + 1}   | ");
            if (ParkingSpots[i] == null)
            {
                Console.Write("              ");
            }
            else
            {
                Console.Write(ParkingSpots[i].LincensePlate);
                for (int j = 0; j <13- ParkingSpots[i].LincensePlate.Length; j++)
                {
                    Console.Write(" ");
                }
            }
            ColoredBorder(" |");
            Console.WriteLine();
        }
    }

    void ColoredBorder(string border)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(border);
        Console.ResetColor();
    }
}