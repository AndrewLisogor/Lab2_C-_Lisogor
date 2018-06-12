using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace Lab2CSharp
{
    abstract class Trigger
    {
        protected string identificator;
        protected bool input1;
        protected bool input2;
        protected bool output;
        public string GetIdentificator
        {
            get { return identificator; }
        }
        sealed class TriggerRS : Trigger
        {
            private bool no;
            public TriggerRS()
            {
                identificator = "RS";
                input1 = false;
                input2 = false;
                output = false;
                no = false;
            }
            public void GiveSignaltoSinput()
            {
                if (!input1){
                    if (!input2){
                        input1 = true;
                        output = true;
                    }
                    else{
                        input1 = true;
                        no = true;
                    }
                }

            }
            public void RemoveSignalfromSinput()
            {
                input1 = false;
                if (no){
                    no = false;
                    output = false;
                }
            }
            public void GiveSignaltoRinput()
            {
                if (!input2){
                    if (!input1){
                        input2 = true;
                        output = false;
                    }
                    else{
                        input2 = true;
                        no = true;
                    }
                }
            }
            public void GetTriggerState()
            {
                Console.WriteLine(this.ToString());
            }
            public override string ToString()
            {
                string result = null;
                result += string.Format("Trigger type: {0}\n", this.GetIdentificator);
                if (no)
                    result += string.Format("Trigger state:\nR input:{0}\nS input:{1}\n Output:this is forbidden trigger state . .",
                        Convert.ToByte(input2), Convert.ToByte(input1));
                else
                    result += string.Format("Trigger state:\nR input:{0}\nS input:{1}\n Output:{2}",
                        Convert.ToByte(input2), Convert.ToByte(input1), Convert.ToByte(output));
                return result;
            }
        }
        sealed class TriggerJK : Trigger
        {
            public TriggerJK()
            {
                identificator = "JK";
                input1 = false;
                input2 = false;
                output = false;
            }
            public void GiveSignaltoJinput()
            {
                if (!input1){
                    if (!input2){
                        input1 = true;
                        output = true;
                    }
                    else{
                        input1 = true;
                        output = !output;
                    }
                }
            }
            public void RemoveSignalfromJinput()
            {
                if (input1){
                    input1 = false;
                    if (input2)
                        output = false;
                }
            }
            public void GiveSignaltoKinput()
            {
                if (!input2){
                    if (!input1){
                        input2 = true;
                        output = false;
                    }
                    else{
                        input2 = true;
                        output = !output;
                    }
                }
            }
            public void RemoveSignalfromKinput()
            {
                if (input2){
                    input2 = false;
                    if (input1)
                        output = true;
                }
            }
            public void GetTriggerState()
            {
                Console.WriteLine(this.ToString());
            }
            public override string ToString()
            {
                string result = null;
                result += string.Format("Trigger type: {0}\n", this.GetIdentificator);
                result += string.Format("Trigger state:\nJ input:{0}\nK input:{1}\n Output:{2}",
                        Convert.ToByte(input1), Convert.ToByte(input2), Convert.ToByte(output));
                return result;
            }
        }
        class Register
        {
            private string name;
            private List<Trigger> reg;
            public Register(string n)
            {
                name = n;
                reg = new List<Trigger>();
            }
            public void AddTrigger(Trigger t)//добавить триггер в регистр
            {
                reg.Add(t);
            }
            public Trigger GetTrigger(int index)//получить конкретный триггер из регистра
            {
                if (index < reg.Count && index >= 0)
                    return reg[index];
                else
                    throw new Exception("Out of range in Register class collection . .");
            }
            public void DeleteTrigger(int index)//удалить триггер из регистра
            {
                if (index < reg.Count && index >= 0)
                    reg.Remove(reg[index]);
                else
                    throw new Exception("Out of range in Register class collection . .");
            }
            public void GetTriggersState()//вывести на экран состояние всех триггеров регистра
            {
                Console.WriteLine("Register {0}:", name);
                foreach (Trigger t in reg)
                    Console.WriteLine(t.ToString());
            }


            public IEnumerator GetEnumerator()//реализуем для возможности использования foreach с классом
            {
                foreach (Trigger t in reg){
                    yield return t;
                }
            }
        }
        class Program
        {
            static void Main()
            {
                try
                {
                    Console.WriteLine("Starting test . .\n");//тестируем . .
                    Register register = new Register("Test register");
                    register.AddTrigger(new TriggerRS());
                    register.AddTrigger(new TriggerJK());
                    register.AddTrigger(new TriggerRS());
                    register.GetTriggersState();
                    foreach (Trigger t in register)
                    {
                        if (t is TriggerRS){
                            TriggerRS temp = (TriggerRS)t;
                            temp.GiveSignaltoSinput();
                        }
                        if (t is TriggerJK){
                            TriggerJK temp = (TriggerJK)t;
                            temp.GiveSignaltoJinput();
                        }
                    }
                    register.GetTriggersState();
                    register.DeleteTrigger(1);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                    return;
                }

                Console.ReadLine();
            }
        }
    }
}
