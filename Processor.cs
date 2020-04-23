using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

//Firstly initialized in 2012 by Harry Tian
//Recoded and fix bug on 2017 by Harry Tian
//Added Fix multiline msgid first 2017.11.24

namespace PO_Processor
{
    public class Processor
    {
        public void Process(string infile, string outfile, int startLineNumber)
        {
            string tempfile = infile + ".tmp";
            PreProcess(infile, tempfile, startLineNumber);

            StreamReader sr = null;
            StreamWriter sw = null;
            try
            {
                sr = new StreamReader(tempfile, Encoding.UTF8, true);
                sw = new StreamWriter(outfile, false, Encoding.UTF8);
                sw.AutoFlush = true;

                int counter = 0;
                string currentLine = "";
                string nextLine = "";
                string nextStrLine = "";

                Regex reg = new Regex("^[-a-zA-Z_0-9]+$", RegexOptions.Compiled);


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

                                nextLine = sr.ReadLine();
                                counter++;

                                if (nextLine.StartsWith("msgstr"))
                                {
                                    //Cut from msgstr
                                    string cache = nextLine.Substring(8, nextLine.Length - 9);
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
                                            if (reg.IsMatch(currentLine.Substring(7, currentLine.Length - 8)))
                                            {
                                                sw.WriteLine("msgstr \"\"");
                                            }
                                            else
                                            {
                                                sw.WriteLine("msgstr \"" + currentLine.Substring(7, currentLine.Length - 8) + "\"");
                                            }
                                            sw.WriteLine();
                                        }
                                    }
                                    else
                                    {
                                        //end of file
                                        if (nextLine == "msgstr \"\"")
                                        {
                                            //Cut from msgsid
                                            if (reg.IsMatch(currentLine.Substring(7, currentLine.Length - 8)))
                                            {
                                                sw.WriteLine("msgstr \"\"");
                                            }
                                            else
                                            {
                                                sw.WriteLine("msgstr \"" + currentLine.Substring(7, currentLine.Length - 8) + "\"");
                                            }
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
            }

            catch (FileLoadException e)
            {
                throw e;
            }
            catch (PathTooLongException e)
            {
                throw e;
            }
            catch (FileNotFoundException e)
            {
                throw e;
            }
            catch (IOException e)
            {
                throw e;
            }
            catch (IndexOutOfRangeException e)
            {
                throw e;
            }
            catch (NullReferenceException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    //File.Delete(tempfile);
                }
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }

        private void PreProcess(string infile, string outfile, int startLineNumber)
        {
            StreamReader sr = null;
            StreamWriter sw = null;

            try
            {
                sr = new StreamReader(infile, Encoding.UTF8, true);
                sw = new StreamWriter(outfile, false, Encoding.UTF8);

                sw.AutoFlush = true;

                int counter = 0;
                string currentLine = "";
                string nextLine = "";

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
                                string cache = currentLine.Substring(7, currentLine.Length - 8);

                                nextLine = sr.ReadLine();
                                counter++;

                                while (!nextLine.StartsWith("msgstr"))
                                {
                                    nextLine = nextLine.Substring(1, nextLine.Length - 2);
                                    cache = cache + nextLine;
                                    nextLine = sr.ReadLine();
                                    counter++;
                                }

                                sw.WriteLine("msgid \"" + cache + "\"");
                                //sw.WriteLine("msgid " + cache + "\"");
                                sw.WriteLine(nextLine);
                            }
                            else
                            {
                                sw.WriteLine(currentLine);
                            }
                        }
                    }
                }

            }

            catch (FileLoadException e)
            {
                throw e;
            }
            catch (PathTooLongException e)
            {
                throw e;
            }
            catch (FileNotFoundException e)
            {
                throw e;
            }
            catch (IOException e)
            {
                throw e;
            }
            catch (IndexOutOfRangeException e)
            {
                throw e;
            }
            catch (NullReferenceException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
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
}
