using System;

namespace Monitor
{
    class Program
    {
       
        static void Main(string[] args)
        {
            Console.WriteLine("Press ENTER to start the MONITOR process...");
            Console.ReadLine();
            Comm.Receiver receiver = new Comm.Receiver("127.0.0.1", 3000);
            receiver.DataReceived += ReceivedSomeData;
            receiver.StartListener();
        }

        // acest eveniment de declanseaza atunci cand au fost receptionate date

        private static bool isFull = false;
        private static bool isEmpty = true;
        private static void ReceivedSomeData(object sender, EventArgs e)
        {
            Console.WriteLine("-----------------------------------==============>");          
            if (Convert.ToInt32(sender.ToString()) == 4)
            {
                isFull = true;
            }
           
               
            if(Convert.ToInt32(sender.ToString()) == 5)
            {
                isEmpty = true;
            }
          
            switch (Convert.ToInt32(sender.ToString()))
            {
                case 0:
                    if (isEmpty)
                    {
                        Console.WriteLine("Boilerul este complet gol...");
                        isEmpty = false;
                        isFull = false;
                    }
                    else
                    {
                        Console.WriteLine("Totul este oprit!");
                    }
                    
                    break;
                case 1:
                    if (isFull)
                    {
                        Console.WriteLine("Boilerul este plin...");
                        isFull = false;
                        isEmpty = false;
                    }
                    else
                    {
                        
                        Console.WriteLine("Boilerul se umple...");
                    }
                   
                    break;
                case 2:
                    if (isEmpty)
                    {
                        Console.WriteLine("Boilerul este complet gol...");
                        isEmpty = false;
                        isFull = false;
                    }
                    else
                        Console.WriteLine("Boilerul se goleste...");
                    break;
                case 3:
                    if (isFull)
                    {
                        Console.WriteLine("Boilerul este plin...");
                        isFull = false;
                        isEmpty = false;
                    }
                    else if(isEmpty)
                    {
                       Console.WriteLine("Boilerul este complet gol...");
                        isEmpty = false;
                        isFull = false;
                    }
                    else
                    {
                        Console.WriteLine("Boilerul este pe mentinere...");
                    }
                    
                    break;
              
            }
           

        }
    }
}
