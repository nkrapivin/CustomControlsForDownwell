using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UndertaleModLib;
using UndertaleModLib.Decompiler;
using UndertaleModLib.Models;

namespace CustomControlsForDownwell
{
    class Program
    {
        const string DefaultIni =
@"[Keyboard]
README_PLEASE = OPEN https://yal.cc/key-code-to-key-name-and-vice-versa/ FOR KEYCODE NAMES.
Left = a
Right = d
Choose = shift
Cancel = escape

[Gamepad]
README_PLEASE = OPEN https://docs2.yoyogames.com/source/_build/3_scripting/4_gml_reference/controls/gamepad%20input/index.html FOR gp_ CONSTANTS.
Left = gp_padl
Right = gp_padr
Up = gp_padu
Down = gp_padd
Choose = gp_face1
Cancel = gp_face2
Deadzone = 0.65
README_SLOT = Xbox 360 pads have slot id from 0 to 3. DualShock 4 pads have slot id from 4 to 11
README_SLOT_TWO = To enable automatic gamepad detection (Async System Event) set Slot to -1, set it to anything lower than -1 to disable gamepad input.
Slot = 0

[Misc]
README_PLEASE = Set RevertCaption to true to revert the new window caption back to original (in case you don't like when mods say that they are present).
RevertCaption = false"; // please do not modify.

        static UndertaleData Data;

        static string mypath = AppDomain.CurrentDomain.BaseDirectory + "Scripts" + Path.DirectorySeparatorChar;

        static void Main(string[] args)
        {
            Console.WriteLine("Main() called!");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Downwell Custom Controls Mod Installer Thing...");
            Console.WriteLine("Open the game folder, what do you see?");
            Console.WriteLine("1 - A single Downwell.exe file, that's it.");
            Console.WriteLine("2 - A lot of files, and data.win is one of them.");
            Console.Write("Input: ");
            var key = Console.ReadKey(false);
            Console.WriteLine();
            bool singleexe = false;
            string datawinpath;
            if (key.Key == ConsoleKey.D2)
            {
                Console.WriteLine("Please drag data.win onto this window and press enter:");
                datawinpath = Console.ReadLine();
            }
            else if (key.Key == ConsoleKey.D1)
            {
                Console.WriteLine("Please start the game, and press any key.");
                Console.ReadKey(true);
                Console.WriteLine("Copying IXP000.TMP...");
                if (!CopyIXPFolder())
                {
                    Console.WriteLine("Cannot detect or copy the game's folder, ask nik on Steam Discussion forums.");
                    Console.ReadKey(true);
                    return;
                }

                // We copied the folder without any issues.
                datawinpath = Environment.GetEnvironmentVariable("TEMP") + Path.DirectorySeparatorChar + "ForPatchTemp" + Path.DirectorySeparatorChar + "data.win";

                singleexe = true;
            }
            else
            {
                Console.WriteLine("Unknown Input, relaunch this program and try again.");
                Console.ReadKey(true);
                return;
            }

            if (!File.Exists(datawinpath))
            {
                Console.WriteLine("What? Somehow data.win is missing, ask nik on Steam Discussion forums.");
                Console.ReadKey(true);
                return;
            }

            Console.WriteLine("Please make sure you've closed the game. If you did, press any key.");
            Console.ReadKey(true);

            // Ok, we can finally patch!!!
            Console.WriteLine("Loading data.win in UndertaleModLib...");
            bool quit = false;
            try
            {
                using (var stream = new FileStream(datawinpath, FileMode.Open, FileAccess.Read))
                {
                    Data = UndertaleIO.Read(stream, warning =>
                    {
                        Console.WriteLine("[MODLIB|WARN]: " + warning);
                        quit = true;
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[MODLIB|ERR ]: " + e.Message);
                quit = true;
            }

            if (quit)
            {
                Console.WriteLine("Warnings or errors occured when loading data.win!");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey(true);
                Environment.Exit(-1); // forcefully quit.
            }

            Console.WriteLine("Executing PatchThing();");
            PatchThing();
            Console.WriteLine();
            Console.WriteLine("Writing new data.win...");
            File.Delete(datawinpath);
            using (var stream = new FileStream(datawinpath, FileMode.Create, FileAccess.Write))
            {
                UndertaleIO.Write(stream, Data);
            }
            Console.WriteLine("Writing default controls.ini...");
            File.WriteAllText(Path.GetDirectoryName(datawinpath) + Path.DirectorySeparatorChar + "controls.ini", DefaultIni);
            if (singleexe)
            {
                FindDownwellExe(new DirectoryInfo(Path.GetDirectoryName(datawinpath)));
                Process.Start("explorer.exe", Path.GetDirectoryName(datawinpath));
                Console.WriteLine("Copy all files from this folder to your game folder.");
                Console.WriteLine("(if it asks to replace, agree, then press any key)");
                Console.ReadKey(true);
            }
            Console.WriteLine("Ok, we're done here, launch the game and press any key to exit.");
            Console.ReadKey(true);
            Console.WriteLine("Cleaning up...");
            if (singleexe) Directory.Delete(Path.GetDirectoryName(datawinpath), true);
        }

        static bool CopyIXPFolder()
        {
            string tempPathToIXP = Environment.GetEnvironmentVariable("TEMP") + Path.DirectorySeparatorChar + "IXP000.TMP" + Path.DirectorySeparatorChar;
            string newPath = Environment.GetEnvironmentVariable("TEMP") + Path.DirectorySeparatorChar + "ForPatchTemp" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(tempPathToIXP)) return false;

            bool failed = false;
            try
            {
                DirectoryCopy(tempPathToIXP, newPath, false);
            }
            catch
            {
                failed = true;
            }
            if (failed || !Directory.Exists(newPath)) return false;

            return true;
        }

        static void ScriptError(string body, string title)
        {
            Console.WriteLine(string.Format("[{0}]: {1}", title, body));
            Console.ReadKey(true);
            Environment.Exit(-1);
        }

        static void ScriptMessage(string body)
        {
            Console.WriteLine("[SCRIPTMSG]: " + body);
        }

        // Basically a copied-over .csx file...
        static void PatchThing()
        {
            Data.GeneralInfo.DisplayName.Content = "Downwell [Custom Controls Mod]";

            // Keyname->Keycode stuff... (thx yal)
            AddScriptFromFile(mypath + "scrYALKeycodderInit.gml");
            AddScriptFromFile(mypath + "scrYALKeycodder.gml");

            // "gp_*" -> gp_* stuff...
            AddScriptFromFile(mypath + "scrParseGpadConst.gml");

            // Load "controls.ini"
            AddScriptFromFile(mypath + "scrCustomControlsInit.gml");

            // :(
            AddScriptFromFile(mypath + "scrCustomKbdPerformEvent.gml");

            // Add our all new cool stylish scrControlInput (wow they line up!!!!)
            ReplaceScriptFromFile(mypath + "scrControlInput.gml", "scrControlInput");

            // Append initialization scripts to the bytecode.
            string bytecodeInitSource = 
@"00900: call.i scrYALKeycodderInit(argc=0)
00902: popz.v
00904: call.i scrCustomControlsInit(argc=0)
00906: popz.v";
            
            var bytecodeInit = CompileBC(bytecodeInitSource);
            Data.Scripts.ByName("scrInitialize").Code.Append(bytecodeInit);

            // Append scrCustomKbdPerformEvent call to objControllerN...
            string bytecodeAppendSource = 
@"00900: call.i scrCustomKbdPerformEvent(argc=0)
00902: popz.v";
            var bytecodeAppend = CompileBC(bytecodeAppendSource);
            Data.Code.ByName("gml_Object_objControlerN_Step_0").Append(bytecodeAppend);

            // Add "Async System" event to objControllerN
            AddAsyncEvent(mypath + "gml_Object_objControlerN_Other_75.gml");

            ScriptMessage("Done!");
        }

        static void AddAsyncEvent(string path)
        {
            string eventSrc = File.ReadAllText(path);
            string name = "gml_Object_objControlerN_Other_75";

            // Make a code entry.
            UndertaleCode codeEntry = new UndertaleCode();
            codeEntry.Name = Data.Strings.MakeString(name);
            Data.Code.Add(codeEntry);

            // Make a code locals entry.
            UndertaleCodeLocals locals = new UndertaleCodeLocals();
            locals.Name = codeEntry.Name;

            // Make a `var arguments;` entry.
            UndertaleCodeLocals.LocalVar argsLocal = new UndertaleCodeLocals.LocalVar();
            argsLocal.Name = Data.Strings.MakeString("arguments");
            argsLocal.Index = 0;

            // Glue everything together.
            locals.Locals.Add(argsLocal);
            Data.CodeLocals.Add(locals);

            // Set code locals entry for the code entry.
            codeEntry.LocalsCount = 1;
            //codeEntry.GenerateLocalVarDefinitions(codeEntry.FindReferencedLocalVars(), locals); // fails here.

            // FINALLY compile our script.
            Data.Code.ByName(name).ReplaceGML(eventSrc, Data);

            // Add this code entry to Async System event in objControllerN...

            var obj = Data.GameObjects.ByName("objControlerN");
            int OtherEventInd = 7;
            uint MethodNumber = 75;

            UndertaleGameObject.EventAction action = new UndertaleGameObject.EventAction();
            action.ActionName = codeEntry.Name;
            action.CodeId = codeEntry;

            UndertaleGameObject.Event evnt = new UndertaleGameObject.Event();
            evnt.EventSubtype = MethodNumber;
            evnt.Actions.Add(action);

            var eventList = obj.Events[OtherEventInd];
            eventList.Add(evnt);
        }

        static void AddScriptFromFile(string path)
        {
            string scriptSource = File.ReadAllText(path);
            string gmlWeirdName = "gml_Script_" + Path.GetFileNameWithoutExtension(path);

            // Make a code entry.
            UndertaleCode codeEntry = new UndertaleCode();
            codeEntry.Name = Data.Strings.MakeString(gmlWeirdName);
            Data.Code.Add(codeEntry);

            // Make a code locals entry.
            UndertaleCodeLocals locals = new UndertaleCodeLocals();
            locals.Name = codeEntry.Name;

            // Make a `var arguments;` entry.
            UndertaleCodeLocals.LocalVar argsLocal = new UndertaleCodeLocals.LocalVar();
            argsLocal.Name = Data.Strings.MakeString("arguments");
            argsLocal.Index = 0;

            // Glue everything together.
            locals.Locals.Add(argsLocal);
            Data.CodeLocals.Add(locals);

            // Set code locals entry for the code entry.
            codeEntry.LocalsCount = 1;
            //codeEntry.GenerateLocalVarDefinitions(codeEntry.FindReferencedLocalVars(), locals); // fails here.

            // FINALLY compile our script.
            Data.Code.ByName(gmlWeirdName).ReplaceGML(scriptSource, Data);

            // ... and actually add it like a script...
            var scr = new UndertaleScript();
            scr.Code = Data.Code.ByName(gmlWeirdName);
            scr.Name = Data.Strings.MakeString(Path.GetFileNameWithoutExtension(path));
            Data.Scripts.Add(scr);

            // ... oh, and don't forget to add a *function* reference.
            var funcentry = new UndertaleFunction();
            funcentry.Name = Data.Strings.MakeString(Path.GetFileNameWithoutExtension(path));
            Data.Functions.Add(funcentry);
        }

        static void FindDownwellExe(DirectoryInfo dir)
        {
            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles("*.exe");
            try
            {
                File.Move(files[0].FullName, files[0].DirectoryName + Path.DirectorySeparatorChar + "Downwell.exe");
            }
            catch (Exception e)
            {
                Console.WriteLine("[PATCHER|ERR]: Error when renaming Downwell.exe! " + e.Message);
            }
        }

        static void ReplaceScriptFromFile(string path, string name)
        {
            string scriptSource = File.ReadAllText(path);
            var scriptCodeEntry = Data.Scripts.ByName(name).Code;
            scriptCodeEntry.ReplaceGML(scriptSource, Data);
        }

        static List<UndertaleInstruction> CompileBC(string src)
        {
            return Assembler.Assemble(src, Data);
        }

        // Taken from https://docs.microsoft.com/dotnet/standard/io/how-to-copy-directories
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
