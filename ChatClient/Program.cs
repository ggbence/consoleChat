using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text;
using System.Net.Sockets ;
using System.Threading;

namespace ChatClient
{
    class Program
    {
        private System.Net.Sockets.TcpClient clientSocket;
        private NetworkStream serverStream;
        private string readData;

        private string ipAddress;
        private string userName;

        public Program()
        {
            ipAddress = "";
            userName = "";

            clientSocket = new System.Net.Sockets.TcpClient();
            serverStream = default(NetworkStream);
            readData = null;
        }
        
        // send button
        private void sendMessage(string sendData)
        {
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(sendData + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }

        // connection to the server
        private void Connection(string ipAddress)
        {
            try
            {
                this.clientSocket.Connect(ipAddress, 8888);

                this.serverStream = clientSocket.GetStream();
                readData = "Conected to Chat Server ...";
                msg(readData);

                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(this.userName + "$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();

                Thread ctThread = new Thread(getMessage);
                ctThread.Start();

            }
            catch (SocketException ex)
            {
                Console.WriteLine("Could not connect to the server.");
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

           
            
        }

        // get messages
        private void getMessage() 
        {
            while (true)
            {
                try
                {
                    serverStream = clientSocket.GetStream();
                    int buffSize = 0;
                    byte[] inStream = new byte[10025];
                    buffSize = clientSocket.ReceiveBufferSize;
                    serverStream.Read(inStream, 0, buffSize);
                    string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                    readData = "" + returndata;
                    msg(readData.Replace("\0", ""));
                }
                catch (Exception ex)
                {
                    
                   Thread.CurrentThread.Abort();
                    throw;
                }
                
            }
        }

        // write the message
        private void msg(string data)
        {
            /*
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(msg));
            else
                textBox1.Text = textBox1.Text + Environment.NewLine + " >> " + readData;
             */
            if ((!data.Equals("")) &&(data != null))
            {
                Console.WriteLine(data);
            }
            
        }

        private void run()
        {
            try
            {
                Connection(this.ipAddress);
                while (true)
                {
                    string message = Console.ReadLine();
                    sendMessage(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Communication error. Program will be terminated.");
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                return;
            }
            

        }

        static void Main(string[] args)
        {
            Program program = new Program();
            Console.Write("server: ");
            program.ipAddress = Console.ReadLine();
            Console.Write("user: ");
            program.userName = Console.ReadLine();

            program.run();
            

        }
    }
}
