//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading.Tasks;

//namespace HL7Listener
//{
    
//    public class SubscriberNew
//    {
//        //private System.Net.Sockets.Socket listener;
//        private IPEndPoint endPoint;
//        private static Socket serverSocket;
//        private static byte[] buffer = new byte[1024];
//        public SubscriberNew(IPEndPoint endPoint)
//        {
//            this.endPoint = endPoint;
//            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//            serverSocket.Bind(endPoint);
//            Console.WriteLine("Listening to port {0}", endPoint);
//            serverSocket.Listen(3);
//        }

//        public static async Task Main()
//        {
//            // Establecer el puerto en el que el servidor estará escuchando
//            int serverPort = 12345;

//            // Crear el socket del servidor y enlazarlo a la dirección local
//            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//            serverSocket.Bind(new IPEndPoint(IPAddress.Any, serverPort));
//            serverSocket.Listen(1);

//            Console.WriteLine("Servidor esperando conexiones...");

//            while (true)
//            {
//                // Aceptar una conexión entrante
//                Socket clientSocket = serverSocket.Accept();
//                Console.WriteLine("Cliente conectado.");

//                // Recibir el mensaje del cliente
//                int received = clientSocket.Receive(new ArraySegment<byte>(buffer), SocketFlags.None);
//                if (received > 0)
//                {
//                    string mensaje = Encoding.ASCII.GetString(buffer, 0, received);
//                    Console.WriteLine("Mensaje recibido del cliente: " + mensaje);

//                    // Procesar el mensaje HL7 y obtener el contenido útil (por ejemplo, el número de orden)
//                    string numeroOrden = ProcesarMensajeHL7(mensaje);

//                    // Construir el ACK de respuesta
//                    string ackMensaje = $"MSH|^~\\&|YOUR_APP|YOUR_FACILITY|HL7LAB||{DateTime.Now.ToString("yyyyMMddHHmmss")}||ACK^O01|1|P|2.3||||||ASCII\rMSA|AA|{numeroOrden}|||0\r";
//                    byte[] ackBytes = Encoding.ASCII.GetBytes(ackMensaje);

//                    // Enviar el ACK al cliente
//                    await clientSocket.SendAsync(new ArraySegment<byte>(ackBytes), SocketFlags.None);
//                }

//                // Cerrar el socket del cliente
//                clientSocket.Close();
//                Console.WriteLine("Conexión cerrada.");
//            }
//        }

//        private static string ProcesarMensajeHL7(string mensaje)
//        {
//            // Aquí puedes implementar la lógica para procesar el mensaje HL7 y obtener el número de orden
//            // Por simplicidad, este ejemplo solo extraerá el primer campo del mensaje

//            string[] campos = mensaje.Split('|');
//            if (campos.Length >= 4)
//            {
//                return campos[3]; // Retorna el contenido del cuarto campo del mensaje (por ejemplo, el número de orden)
//            }

//            return string.Empty;
//        }

//        private string HandleMessage(string data)
//        {
//            string responseMessage = String.Empty;
//            try
//            {
//                Console.WriteLine("Message received.");

//                Message msg = new Message();
//                msg.DeSerializeMessage(data);

//                // You can do what you want with the message here as per your appliation requirements.
//                // For eg: read patient ID, patient last name, age etc.

//                // Create a response message
//                //
//                responseMessage = CreateRespoonseMessage(msg.MessageControlId());
//            }
//            catch (Exception ex)
//            {
//                // Exception handling
//            }
//            return responseMessage;
//        }

//        private string CreateRespoonseMessage(string messageControlID)
//        {
//            try
//            {
//                Message response = new Message();

//                Segment msh = new Segment("MSH");
//                msh.Field(2, "^~\\&");
//                msh.Field(7, DateTime.Now.ToString("yyyyMMddhhmmsszzz"));
//                msh.Field(9, "ACK");
//                msh.Field(10, Guid.NewGuid().ToString());
//                msh.Field(11, "P");
//                msh.Field(12, "2.5.1");
//                response.Add(msh);

//                Segment msa = new Segment("MSA");
//                msa.Field(1, "AA");
//                msa.Field(2, messageControlID);
//                response.Add(msa);


//                // Create a Minimum Lower Layer Protocol (MLLP) frame.
//                // For this, just wrap the data lik this: <VT> data <FS><CR>
//                StringBuilder frame = new StringBuilder();
//                frame.Append((char)0x0b);
//                frame.Append(response.SerializeMessage());
//                frame.Append((char)0x1c);
//                frame.Append((char)0x0d);

//                return frame.ToString();
//            }
//            catch (Exception ex)
//            {
//                // Exception handling

//                return String.Empty;
//            }
//        }
//    }
//}
