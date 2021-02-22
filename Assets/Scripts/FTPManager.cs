using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class FTPManager : MonoBehaviour
{
    [SerializeField]
    private string host = "ftp://localhost:21";
    [SerializeField]
    private string UserId = "user";
    [SerializeField]
    private string Password = "pass";
    // Start is called before the first frame update

    private string[] lines;
    
    void Start()
    {
        //var dirs = GetAllFtpFiles($"{host}/");
        lines = GetFileText($"{host}/test.txt");
        foreach(var line in lines)
        {
            Debug.Log(line);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        if(lines != null)
        {
            foreach(var line in lines)
            {
                var lineArr = line.Split(',');

                var label = lineArr[0];
                var x = Convert.ToSingle(lineArr[1]);
                var y = Convert.ToSingle(lineArr[2]);
                var width = Convert.ToSingle(lineArr[3]);
                var height = Convert.ToSingle(lineArr[4]);

                var colorString = lineArr[5];
                var color = Color.red;
                if(colorString.Contains("red"))
                    color = Color.red;
                else if(colorString.Contains("green"))
                    color = Color.green;
                else if(colorString.Contains("blue"))
                    color = Color.blue;
                else if(colorString.Contains("yellow"))
                    color = Color.yellow;
                else if(colorString.Contains("black"))
                    color = Color.black;
                
                var rectPosition = new Rect(x,y,width,height);
                GUIDrawRect(rectPosition, color);
                rectPosition.y += 5;
                GUI.TextField(rectPosition, label);
            }
        }
        
    }

    private static Texture2D _staticRectTexture;
    private static GUIStyle _staticRectStyle;
    
    public static void GUIDrawRect( Rect position, Color color )
    {
        if( _staticRectTexture == null )
        {
            _staticRectTexture = new Texture2D( 1, 1 );
        }
 
        if( _staticRectStyle == null )
        {
            _staticRectStyle = new GUIStyle();
        }
 
        _staticRectTexture.SetPixel( 0, 0, color );
        _staticRectTexture.Apply();
 
        _staticRectStyle.normal.background = _staticRectTexture;
 
        GUI.Box( position, GUIContent.none, _staticRectStyle );
 
 
    }

    private string[] GetFileText(string FilePath)  
    {    
          
        FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(FilePath);  
        ftpRequest.Credentials = new NetworkCredential(UserId, Password);  
        ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;  
        FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();  
        StreamReader streamReader = new StreamReader(response.GetResponseStream());  

        var lines = streamReader.ReadToEnd();
        
        streamReader.Close();  

        var linesArr = lines.Split('\n');
        
        return linesArr;  
    }  

    
    private List<string> GetAllFtpFiles(string ParentFolderpath)  
    {    
        try  
        {  
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(ParentFolderpath);  
            ftpRequest.Credentials = new NetworkCredential(UserId, Password);  
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;  
            FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();  
            StreamReader streamReader = new StreamReader(response.GetResponseStream());  

            List<string> directories = new List<string>();  

            string line = streamReader.ReadLine();  
            while (!string.IsNullOrEmpty(line))  
            {  
                var lineArr = line.Split('/');  
                line = lineArr[lineArr.Length - 1];  
                directories.Add(line);  
                line = streamReader.ReadLine();  
            }  

            streamReader.Close();  

            return directories;  
        }  
        catch (Exception ex)  
        {  
            throw ex;  
        }  
    }  

}
