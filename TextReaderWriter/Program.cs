using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextReaderWriter
{
    class Program
    {
        static readonly string MAGIC_NUMBER = "42";
        static readonly string FILE_PATH = @"C:\AdvProg\helloworld.txt";
        static readonly string FILE_PATH2 = @"C:\AdvProg\helloworld2.txt";

        /*
        Note that when it comes to streams we must be aware that they are inherently
        binary--they only understand and work with byte data. The interpretation
        of a given byte must be agreed between a producer and a consumer. When we
        come to writing out text, we are faced with an encoding choice--are we
        encoding ASCII or Unicode and, if the latter, what exact encoding scheme
        (predominately either UTF8 or UTF16). By default, TextWriter/TextReader
        classes make use of UTF8 as the default encoding scheme.
        */
        static void Main(string[] args)
        {
            using (FileStream fStream = File.Create(FILE_PATH))
            /*
            Observe the explicit use of the Encoding enum set to UTF8--it is always
            best to be explicit in your code and not rely on default class behaviours.
            NOTE:  Other streams such as BinaryWriter etc use UTF8 by default.
            */
            using (TextWriter txtWriter = new StreamWriter(fStream, Encoding.UTF8))
            {
                txtWriter.Write('A');
                txtWriter.WriteLine(" short story...");
                txtWriter.Write("Hello ");
                txtWriter.WriteLine("World!");
                // Note the use of the NewLine property below--this is implicitly
                // done for you in WriteLine().
                txtWriter.Write("The end" + txtWriter.NewLine);

                /*
                We can also output numbers, but these will be converted to text 
                values, so we will have to "reconstitute" these back to numbers
                ourselves when we come to reading them--see code below.
                */
                txtWriter.Write(MAGIC_NUMBER);
            }

            using (FileStream fStream = File.OpenRead(FILE_PATH))
            /*
            Again we are explicit about the encoding. As mentioned above, both the
            producer and the consumer must agree on the encoding scheme to use, 
            otherwise you will end up reading gibberish.
            */
            using (TextReader txtReader = new StreamReader(fStream, Encoding.UTF8))
            {
                while(txtReader.Peek() > -1)
                {
                    string line = txtReader.ReadLine();
                    if(line.Equals(MAGIC_NUMBER))
                    {
                        /*
                        Here we "reconstitute" back the numerical value from the
                        string version of the number we wrote out to the stream.
                        */
                        int magicNum;
                        if(int.TryParse(line, out magicNum))
                        {
                            Console.WriteLine($"Magic number, {magicNum}, multipled by 2 = {magicNum *2}");
                        }
                    }
                    else
                    {
                        Console.WriteLine(line);
                    }
                }
            }

            // Shorthand versions of the above code using static File based functions
            CreateSecondFile();

            PrintSecondFile();

            File.Delete(FILE_PATH2);

            CreateSecondFile();

            AppendText();

            PrintSecondFile();
        }

        private static void AppendText()
        {
            if(File.Exists(FILE_PATH2))
            {
                using (TextWriter txtWriter = File.AppendText(FILE_PATH2))
                {
                    txtWriter.WriteLine("Appended some text here");
                }
            }
        }

        private static void CreateSecondFile()
        {
            /*
            Just as an aside the classes File and Directory offer a lot of helper
            functions, such as checking whether a file or directory exists.  You
            should check these functions out in the MSDN
            */
            if (File.Exists(FILE_PATH2) == false)
            {
                using (TextWriter txtWriter = File.CreateText(FILE_PATH2))
                {
                    txtWriter.WriteLine("File number 2");
                    txtWriter.Write('A');
                    txtWriter.WriteLine(" short story...");
                }
            }
        }

        private static void PrintSecondFile()
        {
            using (TextReader txtReader = File.OpenText(FILE_PATH2))
            {
                while(txtReader.Peek() > -1)
                {
                    Console.WriteLine(txtReader.ReadLine());
                }
                /*
                Below code is the same as the Peek() code above.
                string line;
                while ((line = txtReader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
                */
            }
        }

    }
}
