using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Android.Bluetooth;
using Android.Util;
using Java.Lang;
using Java.Util;
using Java.IO;
using System.IO;

namespace App2.Droid
{
    class bluetoothManager
    {
        // id para el bluetooth
        private static string MY_ID = "fa87c0d0-afac-11de-8a39-0800200c9a66";
        private BluetoothDevice result; //lista de bluetooth
        private BluetoothSocket mySocket; // entrada y salida de mensajes
        private BufferedReader buff; // escritura del mensaje
        private System.IO.Stream mStream; //strem de lectura
        private InputStreamReader mReader; //lectura del mensaje
        private Stream buffOut; //mensaje de salida

        public bluetoothManager()
        {
            buff = null;
        }

        //obtener el identificador
        private UUID getUuid()
        {
            return UUID.FromString(MY_ID);
        }

        //abrir la coneccion
        private void openDeviceConnection(BluetoothDevice btDevice)
        {
            try
            {
                mySocket = btDevice.CreateRfcommSocketToServiceRecord(getUuid());
                mySocket.Connect();
                mStream = mySocket.InputStream;
                mReader = new InputStreamReader(mStream);
                buff = new BufferedReader(mReader);
                buffOut = null;
            }
            catch (InvalidOperationException e)
            {
                close(mySocket);
                close(mStream);
                close(mReader);
                throw e;
            }
        }

        //Cerrar la coneccion
        private void close(IDisposable connectObject)
        {
            if (connectObject == null) return;
            try
            {
                connectObject.Dispose();
            }
            catch (System.Exception)
            {
                throw;
            }
            connectObject = null;
        }

        //obtener los disponitivos a conectar
        public void getDevices()
        {
            BluetoothAdapter btAdapter = BluetoothAdapter.DefaultAdapter;
            var devices = btAdapter.BondedDevices;
            if (devices != null && devices.Count > 0)
            {
                foreach (BluetoothDevice mdevice in devices)
                {
                    openDeviceConnection(mdevice);
                }
            }
        }

        //obtener el mensaje
        public string getMessaje()
        {
            return buff.ReadLine();
        }

        //enviar un mensaje
        private void SendMessage(Java.Lang.String message)
        {
            // mensaje no nulo
            if (message.Length() > 0)
            {
                byte[] send = message.GetBytes();
                Write(send);

                // Reset out string buffer to zero and clear the edit text field
               buffOut.SetLength(0);
            }
        }

        public void Write(byte[] buffer)
        {
            try
            {
                buffOut.Write(buffer, 0, buffer.Length);
            }
            catch (Java.IO.IOException e)
            {
                throw e;
            }
        }
    }
}