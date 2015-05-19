using System;
using MailChimp;
using MailChimp.Lists;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MailChimp.Campaigns;
using MailChimp.Templates;
using MailChimp.Helper;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.IO;
  
namespace TestSharp
{

    [System.Runtime.Serialization.DataContract]
    public class MyMergeVar : MergeVar
    {
        [System.Runtime.Serialization.DataMember(Name = "FNAME")]
        public string FirstName { get; set; }
        [System.Runtime.Serialization.DataMember(Name = "LNAME")]
        public string LastName { get; set; }
    }

    class Communicate
    {

        /*******************************************Subscribe a user*********************************************/

        public static void Subscribe(string Email,string first_name,string last_name)
        {
            MailChimpManager mc = new MailChimpManager("e143036fe2e3cbb1b11e3df05d0071bd-us10");

            EmailParameter email = new EmailParameter()                     /*creating a email parameter*/
            {
                Email = Email,
            };

            MyMergeVar name = new MyMergeVar();                             /*Adding other variables like FNAME,LNAME etc*/
            name.FirstName = first_name;
            name.LastName = last_name;

            EmailParameter results = mc.Subscribe("b52446a346", email, name);
        }


        /*****************************************creating and Sending Mails************************************************/

        public static void Sendmail() 
        {

            MailChimpManager mc = new MailChimpManager("e143036fe2e3cbb1b11e3df05d0071bd-us10");
            ListResult lists = mc.GetLists();

            /*Open the comments to create a new template*/

            //TemplateAddResult tempResult=mc.AddTemplate("Confirmation","<b>Paste ur HTML code Here for the template</b>",null);
            //Console.WriteLine(tempResult.TemplateId);

            /* Options for creating a mailing campaign . i.e From address,senders name,list of subscribers,Title of the campaign
             * (c.ToName)Visible name to the receiver
             * (c.Subject)Subject of the name*/

            CampaignCreateOptions c = new CampaignCreateOptions();
            c.FromEmail = "yashas.sham@gmail.com";
            c.FromName = "Yashas Sham";
            c.ListId = "b52446a346";
            c.Title = "Have a nice day";
            c.ToName = "Yashas";
            c.Subject = "This is a confirmation mail";

            c.TemplateID = 67717;                                               /*template id as in https://us10.admin.mailchimp.com/templates/design?tid=67717 */

            /******************************************************************************************/

            var json = File.ReadAllText("input.txt");
            var a = new { serverTime = "", data = new object[] { } };
            var serial = new JsonSerializer();
            dynamic jsonObject = serial.Deserialize(new StringReader(json), a.GetType());
            Console.WriteLine(jsonObject.data[0]);


            Dictionary<string, string> dictionary = new Dictionary<string, string>();        /*contains the contents of the email(a dictonary class)*/

            dictionary.Add("preheader", jsonObject.data[0]);
            dictionary.Add("Footer", jsonObject.data[1]);
            dictionary.Add("Content", jsonObject.data[2]);

            CampaignCreateContent b = new CampaignCreateContent();
          //b.Text = "Hello, this is a confirmation mail";                                   /*For sending only the text message*/
            b.Sections = dictionary;

            var campaign = mc.CreateCampaign("regular", c, b, null, null);
            Console.WriteLine("success");

            /*******************************************************************************************/
            //sending the created camaign
            //var send = mc.SendCampaign(campaign.Id);

            Console.WriteLine("campaign id" + campaign.Id);

            Console.ReadLine();
        }

        
   }
    class program
    {
        static void Main(string[] args)
        {
            //Communicate.Subscribe("yashas.sham@gmail.com","Yashas","J S");
            Communicate.Sendmail();
        } 
    }
}




