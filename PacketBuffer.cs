using System;
using System.Collections.Generic;
using System.Text;

namespace SocketTutorial
{
    public class PacketBuffer
    {
        List<byte> _bufferlist;
        byte[] _readbuffer;
        int _readpos;
        bool _buffupdate = false;

        public PacketBuffer()
        {
            _bufferlist = new List<byte>();
            _readpos =0;
        }
        public int GetReadPos(){
            return _readpos;
        }
        public byte[] ToArray(){
            return _bufferlist.ToArray();
        }
        public int Count(){
            return _bufferlist.Count;
        }
        public int Length(){
            return Count() - _readpos;
        }
        public void Clear(){
            _bufferlist.Clear();
            _readpos=0;
        }
        //write data
        public void WriteBytes (byte[] input){
            _bufferlist.AddRange(input);
            _buffupdate = true;
        }
         public void WriteByte (byte input){
            _bufferlist.Add(input);
            _buffupdate = true;
        }
         public void WriteInteger (int input){
            _bufferlist.AddRange(BitConverter.GetBytes(input));
            _buffupdate = true;
        }
         public void WriteFloat (float input){
            _bufferlist.AddRange(BitConverter.GetBytes(input));
            _buffupdate = true;
        }
        public void WriteString (string input){
            _bufferlist.AddRange(BitConverter.GetBytes(input.Length));
            _bufferlist.AddRange(Encoding.ASCII.GetBytes(input));
            _buffupdate = true;
        }

        //read data


    }
}