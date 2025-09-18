using System;
using System.Collections.Generic;
public class Employee
{
    public string Name { get; }
    public Employee(string name) => Name = name;
    public override string ToString() => Name;
}
public class Item
{
    public string Name { get; }
    public double Price { get; }
    public double Discount { get; }  /

    public Item(string name, double price, double discount)
    {
        Name = name; Price = price; Discount = discount;
    }

    public override string ToString()
        => $"{Name}: price={Price:F2}, disc={Discount:F2}";
}
public class GroceryBill
{
    protected Employee clerk;
    protected List<Item> items;

    public GroceryBill(Employee clerk)
    {
        this.clerk = clerk;
        this.items = new List<Item>();
    }

    public virtual void Add(Item i) => items.Add(i);

    public virtual double GetTotal()
    {
        double sum = 0.0;
        foreach (var it in items) sum += it.Price;
        return sum;
    }

    public virtual void PrintReceipt()
    {
        Console.WriteLine($"Receipt (clerk: {clerk})");
        foreach (var it in items)
            Console.WriteLine($" - {it.Name}: {it.Price:F2}");
        Console.WriteLine($"Total: {GetTotal():F2}");
    }
}
public class DiscountBill : GroceryBill
{
    private bool preferred;
    private int discountCount;
    private double discountAmount;

    public DiscountBill(Employee clerk, bool preferred) : base(clerk)
    {
        this.preferred = preferred;
        discountCount = 0;
        discountAmount = 0.0;
    }

    public override void Add(Item i)
    {
        base.Add(i);
        if (preferred && i.Discount > 0.0)
        {
            discountCount += 1;
            discountAmount += i.Discount;
        }
    }
    public override double GetTotal()
    {
        double baseTotal = base.GetTotal();
        return preferred ? baseTotal - discountAmount : baseTotal;
    }
    public int GetDiscountCount() => preferred ? discountCount : 0;
    public double GetDiscountAmount() => preferred ? discountAmount : 0.0;
    public double GetDiscountPercent()
    {
        double baseTotal = 0.0;
        foreach (var it in items) baseTotal += it.Price;
        if (baseTotal == 0.0) return 0.0;
        return preferred ? (discountAmount / baseTotal * 100.0) : 0.0;
    }
    public override void PrintReceipt()
    {
        Console.WriteLine($"Discount Receipt (preferred={preferred}, clerk: {clerk})");
        foreach (var it in items)
            Console.WriteLine($" - {it.Name}: {it.Price:F2} (discount if preferred: {it.Discount:F2})");
        Console.WriteLine($"Subtotal: {base.GetTotal():F2}");
        Console.WriteLine($"Total discount: {GetDiscountAmount():F2} (applied to {GetDiscountCount()} items)");
        Console.WriteLine($"Total due: {GetTotal():F2}");
        Console.WriteLine($"Discount percent: {GetDiscountPercent():F2}%");
    }
}
public class BillLine
{
    public Item Item { get; private set; }
    public int Quantity { get; private set; }

    public BillLine(Item item, int quantity)
    {
        Item = item;
        Quantity = quantity < 0 ? 0 : quantity;
    }

    public void SetQuantity(int q) => Quantity = q < 0 ? 0 : q;

    public double LineTotal() => Item.Price * Quantity;

    public double LineDiscountTotal(bool preferred) =>
        preferred ? Item.Discount * Quantity : 0.0;

    public override string ToString() =>
        $"{Item.Name} x{Quantity} -> {LineTotal():F2} (disc/each={Item.Discount:F2})";
}
public class GroceryBillLines
{
    protected Employee clerk;
    protected List<BillLine> lines;

    public GroceryBillLines(Employee clerk)
    {
        this.clerk = clerk;
        this.lines = new List<BillLine>();
    }

    public virtual void Add(BillLine bl) => lines.Add(bl);

    public virtual double GetTotal()
    {
        double sum = 0.0;
        foreach (var bl in lines) sum += bl.LineTotal();
        return sum;
    }

    public virtual void PrintReceipt()
    {
        Console.WriteLine($"Receipt (clerk: {clerk})");
        foreach (var bl in lines)
            Console.WriteLine($" - {bl.Item.Name} x{bl.Quantity} : {bl.LineTotal():F2}");
        Console.WriteLine($"Total: {GetTotal():F2}");
    }
}
public class DiscountBillLines : GroceryBillLines
{
    private bool preferred;
    private int discountItemCount; 
    private double discountAmount;

    public DiscountBillLines(Employee clerk, bool preferred) : base(clerk)
    {
        this.preferred = preferred;
        discountItemCount = 0;
        discountAmount = 0.0;
    }
    public override void Add(BillLine bl)
    {
        base.Add(bl);
        if (preferred && bl.Item.Discount > 0.0)
        {
            discountItemCount += bl.Quantity;
            discountAmount += bl.Item.Discount * bl.Quantity;
        }
    }
    public override double GetTotal()
    {
        double baseTotal = base.GetTotal();
        return preferred ? baseTotal - discountAmount : baseTotal;
    }
    public int GetDiscountCount() => preferred ? discountItemCount : 0;
    public double GetDiscountAmount() => preferred ? discountAmount : 0.0;
    public double GetDiscountPercent()
    {
        double baseTotal = base.GetTotal();
        if (baseTotal == 0.0) return 0.0;
        return preferred ? (discountAmount / baseTotal * 100.0) : 0.0;
    }
    public override void PrintReceipt()
    {
        Console.WriteLine($"Discount Receipt (lines) (preferred={preferred}) Clerk: {clerk}");
        foreach (var bl in lines)
            Console.WriteLine($" - {bl.Item.Name} x{bl.Quantity} : {bl.LineTotal():F2} (disc/each={bl.Item.Discount:F2})");
        Console.WriteLine($"Subtotal: {base.GetTotal():F2}");
        Console.WriteLine($"Total discount: {GetDiscountAmount():F2} (units discounted: {GetDiscountCount()})");
        Console.WriteLine($"Total due: {GetTotal():F2}");
        Console.WriteLine($"Discount percent: {GetDiscountPercent():F2}%");
    }
}
class Program
{
    static void Main()
    {
        var clerk = new Employee("Alice");

        Console.WriteLine("=== Demo 1: Item-based ===");
        var candy = new Item("Candy", 1.35, 0.25);
        var milk = new Item("Milk", 3.50, 0.0);
        var cereal = new Item("Cereal", 5.00, 1.00);

        var dbPreferred = new DiscountBill(clerk, true);
        dbPreferred.Add(candy);
        dbPreferred.Add(milk);
        dbPreferred.Add(cereal);
        dbPreferred.PrintReceipt();

        Console.WriteLine();

        var dbNonPreferred = new DiscountBill(clerk, false);
        dbNonPreferred.Add(candy);
        dbNonPreferred.Add(milk);
        dbNonPreferred.Add(cereal);
        dbNonPreferred.PrintReceipt();

        Console.WriteLine("\n=== Demo 2: BillLine-based ===");

        var line1 = new BillLine(candy, 3);
        var line2 = new BillLine(milk, 2);
        var line3 = new BillLine(cereal, 1);

        var dblPreferred = new DiscountBillLines(clerk, true);
        dblPreferred.Add(line1);
        dblPreferred.Add(line2);
        dblPreferred.Add(line3);
        dblPreferred.PrintReceipt();

        Console.WriteLine();

        var dblNonPreferred = new DiscountBillLines(clerk, false);
        dblNonPreferred.Add(line1);
        dblNonPreferred.Add(line2);
        dblNonPreferred.Add(line3);
        dblNonPreferred.PrintReceipt();
    }
}
