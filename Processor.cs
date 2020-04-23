using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

//Firstly initialized in 2012 by Harry Tian
//Recoded and fix bug on 2017 by Harry Tian

namespace PO_Processor
{
    public class Processor
    {
        public void Process(string infile, string outfile)
        {
            StreamReader sr = new StreamReader(file, Encoding.UTF8, true);
            StreamWriter sw = new StreamWriter(outfile, false, Encoding.UTF8);
            sw.AutoFlush = true;

            int counter = 0;
			const int startLineNumber = 18;
			
            string currentLine = "";
            string nextLine = "";
            string nextStrLine = "";
			
            while (sr.Peek() > 0)
            {
                counter++;
                currentLine = sr.ReadLine();
                if (counter < startLineNumber)
                {
                    sw.WriteLine(currentLine);
                }
                else
                {
                    if (currentLine.StartsWith("#"))
                    {
                        sw.WriteLine(currentLine);
                    }
                    else
                    {
                        if (currentLine.StartsWith("msgid"))
                        {
                            sw.WriteLine(currentLine);

                            nextLine= sr.ReadLine();
                            counter++;

                            if (nextLine.StartsWith("msgstr"))
                            {
                                string cache = nextLine.Substring(8, nextLine.Length-9);
                                if (sr.Peek() > 0)
                                {
                                    nextStrLine = sr.ReadLine();
                                    counter++;

                                    while (nextStrLine != "")
                                    {
                                        //Cut from msgstr
                                        nextStrLine = nextStrLine.Substring(1, nextStrLine.Length - 2);
                                        cache = cache + nextStrLine;
                                        nextStrLine = sr.ReadLine();
                                        counter++;
                                    }

                                    if (cache != "")
                                    {
                                        sw.WriteLine("msgstr \"" + cache + "\"");
                                        sw.WriteLine();
                                    }
                                    else
                                    {
                                        //Cut from msgsid
                                        sw.WriteLine("msgstr \"" + currentLine.Substring(7, currentLine.Length - 8) + "\"");
                                        sw.WriteLine();
                                    }
                                }
                                else
                                {
                                    //end of file
                                    if (nextLine == "msgstr \"\"")
                                    {
                                        sw.WriteLine("msgstr \"" + currentLine.Substring(7, currentLine.Length - 8) + "\"");
                                    }
                                    else
                                    {
                                        sw.WriteLine(nextLine);
                                    }
                                }
                            }
                            else
                            {
                                //
                                sw.WriteLine(currentLine);
                            }

                        }
                        else
                        {
                            //s
                            sw.WriteLine(currentLine);
                        }
                    }
                }
            }

            if (sr != null)
            {
                sr.Close();
            }
            if (sw != null)
            {
                sw.Close();
            }
        }
    }
}
