﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
class MyWebServer
{
    public static void Main()
    {
        MyWebServer MWS = new MyWebServer();
    }
    private TcpListener myListener;
    public MyWebServer()
    {
        //开始兼听端口
        myListener = new TcpListener(8080);
        myListener.Start();
        Console.WriteLine("启动服务器");
        Thread th = new Thread(new ThreadStart(StartListen));
        th.Start();

        public void StartListen()
        {
            int iStartPos = 0;
            String sRequest;
            String sDirName;
            String sRequestedFile;
            String sErrorMessage;
            String sLocalDir;
            String sMyWebServerRoot = Directory.GetCurrentDirectory(); //设置你的虚拟目录
            String sPhysicalFilePath = "";
            String sFormattedMessage = "";
            String sResponse = "";
            while (true)
            {
                //接受新连接
                Socket mySocket = myListener.AcceptSocket();
                Console.WriteLine("Socket Type " + mySocket.SocketType);
                if (mySocket.Connected)
                {
                    Console.WriteLine("\nClient Connected!!\n==================\nCLient IP {0}\n", mySocket.RemoteEndPoint);
                    Byte[] bReceive = new Byte[1024];
                    int i = mySocket.Receive(bReceive, bReceive.Length, 0);
                    //转换成字符串类型
                    string sBuffer = Encoding.ASCII.GetString(bReceive);
                    //只处理"get"请求类型
                    if (sBuffer.Substring(0, 3) != "GET")
                    {
                        Console.WriteLine("只处理get请求类型..");
                        mySocket.Close();
                        return;
                    }
                    // 查找 "HTTP" 的位置
                    iStartPos = sBuffer.IndexOf("HTTP", 1);
                    string sHttpVersion = sBuffer.Substring(iStartPos, 8);
                    // 得到请求类型和文件目录文件名
                    sRequest = sBuffer.Substring(0, iStartPos - 1);
                    sRequest.Replace("\\", "/");
                    //如果结尾不是文件名也不是以"/"结尾则加"/"
                    if ((sRequest.IndexOf(".") < 1) && (!sRequest.EndsWith("/")))
                    {
                        sRequest = sRequest + "/";
                    }
                    //得带请求文件名
                    iStartPos = sRequest.LastIndexOf("/") + 1;
                    sRequestedFile = sRequest.Substring(iStartPos);
                    //得到请求文件目录
                    sDirName = sRequest.Substring(sRequest.IndexOf("/"), sRequest.LastIndexOf("/") - 3);
                    //获取虚拟目录物理路径
                    sLocalDir = sMyWebServerRoot;
                    Console.WriteLine("请求文件目录 : " + sLocalDir);
                    if (sLocalDir.Length == 0)
                    {
                        sErrorMessage = "Error!! Requested Directory does not exists";
                        SendHeader(sHttpVersion, "", sErrorMessage.Length, " 404 Not Found", ref mySocket);
                        SendToBrowser(sErrorMessage, ref mySocket);
                        mySocket.Close();
                        continue;
                    }
                    if (sRequestedFile.Length == 0)
                    {
                        // 取得请求文件名
                        sRequestedFile = "index.HTML";
                    }
                    // 取得请求文件类型(设定为text/HTML)
                    String sMimeType = "text/HTML";
                    sPhysicalFilePath = sLocalDir + sRequestedFile;
                    Console.WriteLine("请求文件: " + sPhysicalFilePath);
                    if (File.Exists(sPhysicalFilePath) == false)
                    {
                        sErrorMessage = "404 Error! File Does Not Exists...";
                        SendHeader(sHttpVersion, "", sErrorMessage.Length, " 404 Not Found", ref mySocket);
                        SendToBrowser(sErrorMessage, ref mySocket);
                        Console.WriteLine(sFormattedMessage);
                    }
                    else
                    {
                        int iTotBytes = 0;
                        sResponse = "";
                        FileStream fs = new FileStream(sPhysicalFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        BinaryReader reader = new BinaryReader(fs);
                        byte[] bytes = new byte[fs.Length];
                        int read;
                        while ((read = reader.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            sResponse = sResponse + Encoding.ASCII.GetString(bytes, 0, read);
                            iTotBytes = iTotBytes + read;
                        }
                        reader.Close();
                        fs.Close();
                        SendHeader(sHttpVersion, sMimeType, iTotBytes, " 200 OK", ref mySocket);
                        SendToBrowser(bytes, ref mySocket);
                    }
                    mySocket.Close();
                }
            }
        }
    }
    public void SendHeader(string sHttpVersion, string sMIMEHeader, int iTotBytes, string sStatusCode, ref Socket mySocket)
    {
        String sBuffer = "";
        if (sMIMEHeader.Length == 0)
        {
            sMIMEHeader = "text/HTML"; // 默认 text/HTML
        }
        sBuffer = sBuffer + sHttpVersion + sStatusCode + "\r\n";
        sBuffer = sBuffer + "Server: cx1193719-b\r\n";
        sBuffer = sBuffer + "Content-Type: " + sMIMEHeader + "\r\n";
        sBuffer = sBuffer + "Accept-Ranges: bytes\r\n";
        sBuffer = sBuffer + "Content-Length: " + iTotBytes + "\r\n\r\n";
        Byte[] bSendData = Encoding.ASCII.GetBytes(sBuffer);
        SendToBrowser(bSendData, ref mySocket);
        Console.WriteLine("Total Bytes : " + iTotBytes.ToString());
    }
    public void SendToBrowser(String sData, ref Socket mySocket) => SendToBrowser(Encoding.ASCII.GetBytes(sData), ref mySocket);
    public void SendToBrowser(Byte[] bSendData, ref Socket mySocket)
    {
        int numBytes = 0;
        try
        {
            if (mySocket.Connected)
            {
                if ((numBytes = mySocket.Send(bSendData, bSendData.Length, 0)) == -1)
                    Console.WriteLine("Socket Error cannot Send Packet");
                else
                {
                    Console.WriteLine("No. of bytes send {0}", numBytes);
                }
            }
            else
                Console.WriteLine("连接失败....");
        }
        catch (Exception e)
        {
            Console.WriteLine("发生错误 : {0} ", e);
        }
    }
}