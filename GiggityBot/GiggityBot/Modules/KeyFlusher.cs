using System;
using System.Collections.Generic;
using System.Text;
using WindowsInput;

namespace GiggityBot.Modules
{
    
    /// <summary>
    /// this is a token of how stupid i am
    /// so dont use it but leave it here
    /// </summary>
    public class KeyFlusher
    {
        char[] ab = new char[26];
        InputSimulator ins;
        
        KeyFlusher()
        {
            ins = new InputSimulator();

            for (int i = 0; i < ab.Length; i++)
                switch (i)
                {
                    case 0:
                        ab[i] = 'A';
                        break;
                    case 1:
                        ab[i] = 'B';
                        break;
                    case 2:
                        ab[i] = 'C';
                        break;
                    case 3:
                        ab[i] = 'D';
                        break;
                    case 4:
                        ab[i] = 'E';
                        break;
                    case 5:
                        ab[i] = 'F';
                        break;
                    case 6:
                        ab[i] = 'G';
                        break;
                    case 7:
                        ab[i] = 'H';
                        break;
                    case 8:
                        ab[i] = 'I';
                        break;
                    case 9:
                        ab[i] = 'J';
                        break;
                    case 10:
                        ab[i] = 'K';
                        break;
                    case 11:
                        ab[i] = 'L';
                        break;
                    case 12:
                        ab[i] = 'M';
                        break;
                    case 13:
                        ab[i] = 'N';
                        break;
                    case 14:
                        ab[i] = 'O';
                        break;
                    case 15:
                        ab[i] = 'P';
                        break;
                    case 16:
                        ab[i] = 'Q';
                        break;
                    case 17:
                        ab[i] = 'R';
                        break;
                    case 18:
                        ab[i] = 'S';
                        break;
                    case 19:
                        ab[i] = 'T';
                        break;
                    case 20:
                        ab[i] = 'U';
                        break;
                    case 21:
                        ab[i] = 'V';
                        break;
                    case 22:
                        ab[i] = 'W';
                        break;
                    case 23:
                        ab[i] = 'X';
                        break;
                    case 24:
                        ab[i] = 'Y';
                        break;
                    case 25:
                        ab[i] = 'Z';
                        break;
                }
        }
        public enum FKS
        {
            none,
            enter,
        }

        public void Flush(string keystrokes, FKS finalKeyStroke = FKS.none)
        {
            // convert string to char array, then to int array
            char[] keys = keystrokes.ToCharArray();
            int curind = 0;
            int[] numKeys = new int[keys.Length];
            foreach (char key in keys)
            {
                numKeys[curind] = ToInt(key);
                curind++;
            }
            // flush int array
            foreach (int keyNum in numKeys)
                ins.Keyboard.KeyPress((WindowsInput.Native.VirtualKeyCode)(keyNum + 65));
            // final keystroke
            switch (finalKeyStroke)
            {
                case FKS.none:
                    break;
                case FKS.enter:
                    ins.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
                    break;
            }

        }

        int ToInt(char c)
        {
            int cur = 0;
            foreach (char abind in ab)
                if (c == abind)
                    break;
                else
                    cur++;
            return cur;
        }
    }
}
