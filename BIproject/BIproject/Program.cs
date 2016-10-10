using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BIproject
{
    class Program
    {
        static void Main(string[] args)
        {
            getData();

        }

        public static void getData()
        {
            Console.WriteLine("Getting Data");
            //Declare Url for Api retrival
            Uri uri = new Uri(@"http://api.wunderground.com/api/88f2e9a1a85ff5d4/conditions/q/CA/San_Francisco.json");
            WebRequest webRequest = WebRequest.Create(uri); //creates a web request
            WebResponse response = webRequest.GetResponse(); //declare response object
            StreamReader streamReader = new StreamReader(response.GetResponseStream()); //reader object
            string responseData = streamReader.ReadToEnd(); //reads data from URL and stores in local variable
            Console.WriteLine("Data Recieved");
            Console.WriteLine(responseData); //writes the string to console
            
            Console.WriteLine("trying to create client...");
            //initialize the connection to mongoDB server
            var myMongodbClient = new MongoClient();

            //Gets a mongoDB database instance representing a database on this server
            var myMongodbDatabase = myMongodbClient.GetDatabase("Weather");

            //Gets a collection from the database
            var myMongodbCollection = myMongodbDatabase.GetCollection<BsonDocument>("San_Francisco");

            //Passes the string response into a BSON document.
            BsonDocument latestWeather = BsonDocument.Parse(responseData);

            //Inserts the document into the collection
            myMongodbCollection.InsertOne(latestWeather);

            Console.WriteLine("Inserted Document into MongoDB!!!!!!!!");

            //Console.ReadLine();
        }
    }
}
