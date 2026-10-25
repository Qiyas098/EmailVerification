using System;
using System.Net;
using System.Net.Mail;

namespace EmailVerification
{
    internal class Program
    {
        static string fromMail = "imanoffqiyas098@gmail.com";
        static string appPassword = "dkjq nzwx zsvq xsid";

        static void Main(string[] args)
        {
            Console.Write("Adinizi daxil edin: ");
            string name = Console.ReadLine();

            Console.Write("E-mailinizi daxil edin: ");
            string email = Console.ReadLine();

            Console.Write("Sifrenizi daxil edin: ");
            string password = Console.ReadLine();

            Random random = new Random();
            string verificationCode = random.Next(100000, 1000000).ToString();

            try
            {
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress(fromMail, "Registration");
                mail.To.Add(email);
                mail.Subject = "E-mail Tesdiq Kodu";
                mail.Body = $"Salam {name}!\n\n" +
                            $"Sizin tesdiq kodunuz: {verificationCode}\n" +
                            "Kodun etibarliliq muddeti 3 deqiqedir.";

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com"))
                {
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(fromMail, appPassword);
                    smtp.Timeout = 15000;

                    smtp.Send(mail);
                }

                Console.WriteLine("\nTesdiq kodu e-mail unvaniniza gonderildi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("E-mail gonderilmedi...");
                Console.WriteLine("Xeta: " + ex.Message);             
                return;
            }

            DateTime startTime = DateTime.Now;

            int attempts = 0;
            bool verified = false;

            while (attempts < 3)
            {
                if ((DateTime.Now - startTime).TotalMinutes >= 3)
                {
                    Console.WriteLine("Tesdiq kodunun muddeti bitdi...");
                    break;
                }

                Console.Write("6 reqemli tesdiq kodunu daxil edin: ");
                string enteredCode = Console.ReadLine();

                attempts++;

                if ((DateTime.Now - startTime).TotalMinutes >= 3)
                {
                    Console.WriteLine("Tesdiq kodunun muddeti bitdi...");
                    break;
                }

                if (enteredCode == verificationCode)
                {
                    verified = true;
                    break;
                }
                else
                {
                    Console.WriteLine("Yanlis kod Qalan cehd sayi: " + (3 - attempts));
                }
            }

            if (verified)
            {
                Console.WriteLine("\nQeydiyyat ugurla tamamlandi...");
                Console.WriteLine("Ad: " + name);
                Console.WriteLine("E-mail: " + email);
            }
            else
            {
                Console.WriteLine("\nQeydiyyat ugursuz oldu...");
            }
        }
    }
}
