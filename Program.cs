using System;
using System.Collections.Generic;
// Always program to an interface
namespace DesignPattens
{
    class Program
    {
        static void Main(string[] args)
        {
            // SINGLETON
            Logger obj1 = Logger.GetLogger();
            Logger obj2 = Logger.GetLogger();
            User obj3 = new User();
            User obj4 = new User();
            Loggerc obj5 = Loggerc.Instance;
            Loggerc obj6 = Loggerc.Instance;

            Console.WriteLine("-- Singleton, generic --");
            Console.WriteLine(obj1.GetHashCode());
            Console.WriteLine(obj2.GetHashCode());
            Console.WriteLine("-- Non Singleton --");
            Console.WriteLine(obj3.GetHashCode());
            Console.WriteLine(obj4.GetHashCode());
            Console.WriteLine("-- Singleton, c# --");
            Console.WriteLine(obj5.GetHashCode());
            Console.WriteLine(obj6.GetHashCode());
            // END SINGLETON

            // BASIC PRINCIPLE - CONSTRUCTOR
            IEmail email = new Outlook();
            Employees alex = new Employees(1, "Alex", email);
            alex.NotifyEmployee();

            email = new Gmail();
            Employees bob = new Employees(2, "Bob", email);
            bob.NotifyEmployee();
            // END BASIC

            // FACTORY
            double num1 = 0;
            double num2 = 1;
            bool result = false;

            Console.WriteLine("Enter first number: ");
            result = Double.TryParse(Console.ReadLine(), out num1);
            if (!result)
            {
                Console.WriteLine("That was not a number !");
                return;
            }

            Console.WriteLine("Enter second number: ");
            result = Double.TryParse(Console.ReadLine(), out num2);
            if (!result)
            {
                Console.WriteLine("That was not a number !");
                return;
            }

            Console.WriteLine("Enter add, substract or divide");
            CalculateFactory factory = new CalculateFactory();
            ICalculate fac = factory.GetCalculate(Console.ReadLine());
            fac.Calculate(num1, num2);
            //END FACTORY

            // TEMPLATE
            ProcessFileData pxf = new ExcelFile();
            pxf.ReadProcessSaveData();

            ProcessFileData ptf = new TextFile();
            ptf.ReadProcessSaveData();

            // REPOSITORY
            var repository = new inMemoryListRepository();
            ContactUI contactUI = new ContactUI(repository);

            Contact contact1 = new Friend { Name = "Ronald", Phone = "123123" };
            Contact contact2 = new Work { Name = "ByOne", Email = "byone@pikachu.com" };

            contactUI.Add(contact1);
            contactUI.Add(contact2);
            contactUI.PrintAllContacts();
        }
    }
    // SINGLETON
    // Singleton class, the generic way
    class Logger
    {
        private static Logger logger;
        private Logger() { }
        public static Logger GetLogger()
        {
            if(logger == null)
            {
                logger = new Logger();
            }
            return logger;
        }
    }
    // Singleton class, the c# way
    class Loggerc
    {
        private static Loggerc instance;
        private Loggerc() { }
        public static Loggerc Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new Loggerc();
                }
                return instance;
            }
        }
    }
    // Not Singleton class
    class User
    {

    }

    // BASICS PRINCIPLES - CONSTRUCTOR
    interface IEmail
    {
        public void sendEmail();
    }

    class Outlook : IEmail
    {
        public void sendEmail()
        {
            Console.WriteLine("Sending email through outlook");
        }
    }

    class Gmail : IEmail
    {
        public void sendEmail()
        {
            Console.WriteLine("Sending email through gmail");
        }
    }

    class Employees
    {
        private int empId;
        private string empName;
        public double salary { get; set; }
        public int grade { get; set; }
        public string company = "Taipay";
        IEmail email;

        public Employees(int empId, string empName, IEmail email)
        {
            this.empId = empId;
            this.empName = empName;
            this.email = email;
        }
        public void NotifyEmployee()
        {
            email.sendEmail();
        }
    }


    // FACTORY
    public interface ICalculate
    {
        void Calculate(double num1, double num2);
    }

    class Divide : ICalculate
    {
        public void Calculate(double a, double b)
        {
            Console.WriteLine("a/b is {0}", a / b);
        }
    }

    class Add: ICalculate
    {
        public void Calculate(double a, double b)
        {
            Console.WriteLine("a + b is {0}", a + b);
        }
    }

    class Substract: ICalculate
    {
        public void Calculate(double a, double b)
        {
            Console.WriteLine("a - b is {0}", a - b);
        }
    }

    public class CalculateFactory
    {
        public ICalculate GetCalculate(string type)
        {
            ICalculate obj = null;

            if (type.ToLower().Equals("add"))
            {
                obj = new Add();
            } else if (type.ToLower().Equals("substract"))
            {
                obj = new Substract();
            } else if (type.ToLower().Equals("divide"))
            {
                obj = new Divide();
            } else
            {
                Console.WriteLine("We dont do that");
            }
            return obj;
        }
    }

    // TEMPLATE
    public abstract class ProcessFileData
    {
        public void ReadProcessSaveData()
        {
            ReadData();
            ProcessData();
            SaveData();
        }
        public abstract void ReadData();
        public abstract void ProcessData();
        public void SaveData()
        {
            Console.WriteLine("Savind data to database");
        }
    }
    public class ExcelFile : ProcessFileData
    {
        public override void ReadData()
        {
            Console.WriteLine("Reading data from a Excel file");
        }

        public override void ProcessData()
        {
            Console.WriteLine("Processing data from a Excel file");
        }
    }

    public class TextFile : ProcessFileData
    {
        public override void ReadData()
        {
            Console.WriteLine("Reading data from a text file");
        }

        public override void ProcessData()
        {
            Console.WriteLine("Processing data from a text file");
        }
    }

    // REPOSITORY
    class ContactUI
    {
        IRepository<Contact> repository;
        public ContactUI(IRepository<Contact> repository)
        {
            this.repository = repository;
        }
        public void Add(Contact obj)
        {
            repository.Add(obj);
        }
        public void Remove(Contact obj)
        {
            repository.Remove(obj);
        }
        public void PrintAllContacts()
        {
            Console.WriteLine(repository.GetAll());
        }
    }

    public interface IRepository<T>
    {
        void Add(T obj);
        void Remove(T obj);
        string GetAll();
    }

    class inMemoryListRepository : IRepository<Contact>
    {
        List<Contact> contacts = new List<Contact>();

        public void Add(Contact obj)
        {
            contacts.Add(obj);
        }

        public string GetAll()
        {
            string temp = "";
            foreach(var contact in contacts)
            {
                temp += contact + "\n";
            }
            return temp;
        }

        public void Remove(Contact obj)
        {
            contacts.Remove(obj);
        }
    }

    public class Contact
    {
        public string Name { get; set; }

        public override string ToString()
        {
            string temp = "";
            if (this is Friend)
            {
                temp = "Friend's name is " + Name + ", phone number is " + ((Friend)this).Phone;
            } else if (this is Contact)
            {
                temp = "Work's contact name is " + Name + ", phone number is " + ((Work)this).Email;
            }
            return temp;
        }

    }

    public class Friend : Contact
    {
        public string Phone { get; set; }
    }

    class Work : Contact
    {
        public string Email { get; set; }
    }
}
